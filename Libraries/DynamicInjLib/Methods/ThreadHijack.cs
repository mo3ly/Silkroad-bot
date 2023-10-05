/*
    ThreadHijack.cs
    Copyright (C) 2012 Jason Larke

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.
 
    In addition to the above disclaimers, I am also not responsible for how
    you decide to use software resulting from this library.

    For a full specification of the GNU GPL license, see <http://www.gnu.org/copyleft/gpl.html>
 
    This license notice should be left in tact in all future works
*/

using System;
using System.Diagnostics;
using JLibrary.Win32;

namespace InjectionLibrary
{
    /// <summary>
    /// InjectionMethod implementation which injects a dll to a remote process by hijacking a 
    /// thread in the remote process, redirecting its instruction pointer to a custom executable stub, 
    /// and resuming the thread afterwards. Low detection possibilities, but harder to guarantee injection
    /// as even after resuming the thread, the operating system may not schedule the thread for an indefinite 
    /// period of time.
    /// </summary>
    internal class ThreadHijack : StandardInjectionMethod
    {
        public override IntPtr Inject(string dllPath, IntPtr hProcess)
        {
            ClearErrors();
            IntPtr[] hModules = InjectAll(new string[] { dllPath }, hProcess);
            if (hModules != null && hModules[0].IsNull() && GetLastError() == null)
                SetLastError(new Exception("Module's entry point function reported a failure"));
            return hModules != null && hModules.Length > 0 
                    ? hModules[0]
                    : IntPtr.Zero;
        }

        public override IntPtr[] InjectAll(string[] dllPaths, IntPtr hProcess)
        {
            ClearErrors();
            try
            {
                if (hProcess.IsNull() || hProcess.Compare(-1))
                    throw new ArgumentException("Invalid process handle.", "hProcess");

                int processId = WinAPI.GetProcessId(hProcess);
                if (processId == 0)
                    throw new ArgumentException("Provided handle doesn't have sufficient permissions to inject", "hProcess");

                Process target = Process.GetProcessById(processId);
                if (target.Threads.Count == 0)
                    throw new Exception("Target process has no targetable threads to hijack.");

                //open a handle to the remote thread to allow for thread operations.
                ProcessThread thread = SelectOptimalThread(target);
                IntPtr hThread = WinAPI.OpenThread(0x001A, false, thread.Id);

                if (hThread.IsNull() || hThread.Compare(-1))
                    throw new Exception("Unable to obtain a handle for the remote thread.");

                IntPtr pModules = IntPtr.Zero;
                IntPtr pRedirect = IntPtr.Zero;
                // use the generic multiload stub to load the paths.
                // call this stub from the REDIRECT_STUB.
                IntPtr pStub = CreateMultiLoadStub(dllPaths, hProcess, out pModules, 1);
                IntPtr[] modules = null;

                if (!pStub.IsNull())
                {
                    if (WinAPI.SuspendThread(hThread) == uint.MaxValue)
                        throw new Exception("Unable to suspend the remote thread");
                    //enter a new try/catch block to ensure the suspended thread is resumed, no matter if we fail somewhere else.
                    try
                    {
                        uint nbytes = 0;
                        WinAPI.CONTEXT ctx = default(WinAPI.CONTEXT);
                        ctx.ContextFlags = 0x10001U; // CONTEXT_CONTROL flag. So that we get back the EIP value of the suspended thread.
                        if (!WinAPI.GetThreadContext(hThread, ref ctx))
                            throw new InvalidOperationException("Cannot get the remote thread's context");

                        byte[] stub = REDIRECT_STUB;
                        IntPtr pAlloc = WinAPI.VirtualAllocEx(hProcess, IntPtr.Zero, (uint)stub.Length, 0x1000 | 0x2000, 0x40);
                        if (pAlloc.IsNull())
                            throw new InvalidOperationException("Unable to allocate memory in the remote process.");

                        //patch the EIP value and the Stub address for the redirection stub before commiting it to the remote process.
                        BitConverter.GetBytes(pStub.Subtract(pAlloc.Add(7)).ToInt32()).CopyTo(stub, 3);
                        BitConverter.GetBytes((uint)(ctx.Eip - pAlloc.Add(stub.Length).ToInt32())).CopyTo(stub, stub.Length - 4);

                        if (!WinAPI.WriteProcessMemory(hProcess, pAlloc, stub, stub.Length, out nbytes) || nbytes != (uint)stub.Length)
                            throw new InvalidOperationException("Unable to write stub to the remote process.");

                        ctx.Eip = (uint)pAlloc.ToInt32(); // Set the entry point for the thread to the redirection stub and resume the thread.
                        WinAPI.SetThreadContext(hThread, ref ctx);                  
                    }
                    catch (Exception e)
                    {
                        SetLastError(e);
                        modules = null;
                        WinAPI.VirtualFreeEx(hProcess, pModules, 0, 0x8000);
                        WinAPI.VirtualFreeEx(hProcess, pStub, 0, 0x8000);
                        WinAPI.VirtualFreeEx(hProcess, pRedirect, 0, 0x8000);
                    }

                    WinAPI.ResumeThread(hThread);
                    if (GetLastError() == null)
                    {
                        System.Threading.Thread.Sleep(100);
                        modules = new IntPtr[dllPaths.Length];
                        byte[] rawHandles = WinAPI.ReadRemoteMemory(hProcess, pModules, (uint)dllPaths.Length << 2);

                        if (rawHandles != null)
                        {
                            for (int i = 0; i < modules.Length; i++)
                                modules[i] = Win32Ptr.Create(BitConverter.ToInt32(rawHandles, i << 2));
                        }
                    }
                    // deliberately didn't clean up the remote memory here. Unfortunately there's no solid way
                    // to ensure the function stub has been scheduled by the operating system. It may not even happen for
                    // ages, or something else may block the thread. When the stub eventually is executes and returns, the reset 
                    // of the stub needs to still be there in order for the function to successfully return, otherwise the process just crashes
                    // and we lose the distinct possibility that the injection was successful.
                    WinAPI.CloseHandle(hThread);
                }
                return modules;
            }
            catch (Exception e)
            {
                SetLastError(e);
                return null;
            }
        }

        private static ProcessThread SelectOptimalThread(Process target)
        {
            ProcessThread thread = target.Threads[0];
            /* todo, actually figure out the optimal thread to target. */
            return thread;
        }

        // Custom executable stub. The remote thread will be redirected to this 
        // code, which essentially just calls LoadLibrary
        private static readonly byte[] REDIRECT_STUB =
        {
            0x9C, 0x60, //pushfd, pushad
            0xE8, 0x00, 0x00, 0x00, 0x00, //call stub
            0x61, 0x9D, //popad, popfd
            0xE9, 0x00, 0x00, 0x00, 0x00 //jmp eip.
        };
    }
}
