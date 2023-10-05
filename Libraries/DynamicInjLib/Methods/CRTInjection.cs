/*
    CRTInjection.cs
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
using System.Text;
using JLibrary.Win32;

namespace InjectionLibrary
{
    /// <summary>
    /// InjectionMethod implementation that utilizes the CreateRemoteThread API to
    /// execute code in the remote process. CRT is leveraged to call the LoadLibrary API
    /// in a remote process
    /// </summary>
    internal class CRTInjection : StandardInjectionMethod
    {
        // Core injection method that actually does all the work.
        public override IntPtr Inject(string dllPath, IntPtr hProcess)
        {
            ClearErrors();
            if (hProcess.IsNull() || hProcess.Compare(-1))
                throw new ArgumentOutOfRangeException("hProcess", "Invalid process handle specified.");

            try
            {
                IntPtr hModule = IntPtr.Zero;

                // Find the LoadLibraryW function address in the remote process
                IntPtr fnLoadLibraryW = WinAPI.GetProcAddress(WinAPI.GetModuleHandleA("kernel32.dll"), "LoadLibraryW");
                if (fnLoadLibraryW.IsNull())
                    throw new Exception("Unable to locate the LoadLibraryW entry point");

                // Create a wchar_t * in the remote process which points to the unicode version of the dll path.
                IntPtr pLib = WinAPI.CreateRemotePointer(hProcess, Encoding.Unicode.GetBytes(dllPath + "\0"), 0x04);
                if (pLib.IsNull())
                    throw new InvalidOperationException("Failed to allocate memory in the remote process");

                try
                {
                    // Call LoadLibraryW in the remote process by using CreateRemoteThread.
                    uint hMod = WinAPI.RunThread(hProcess, fnLoadLibraryW, (uint)pLib.ToInt32(), 10000);
                    if (hMod == uint.MaxValue)
                        throw new Exception("Error occurred when calling function in the remote process");
                    else if (hMod == 0)
                        throw new Exception("Failed to load module into remote process. Error code: " + WinAPI.GetLastErrorEx(hProcess).ToString());
                    else
                        hModule = Win32Ptr.Create(hMod);
                }
                finally
                {
                    // Cleanup in all cases.
                    WinAPI.VirtualFreeEx(hProcess, pLib, 0, 0x8000);
                }
                return hModule;
            }
            catch (Exception e)
            {
                SetLastError(e);
                return IntPtr.Zero;
            }
        }

        public override IntPtr[] InjectAll(string[] dllPaths, IntPtr hProcess)
        {
            ClearErrors();
            if (hProcess.IsNull() || hProcess.Compare(-1))
                throw new ArgumentOutOfRangeException("hProcess", "Invalid process handle specified.");

            try
            {
                // Use the optimized assembly "multiload" stub to load all the dlls in one go.
                IntPtr pModules = IntPtr.Zero;
                IntPtr pStub = CreateMultiLoadStub(dllPaths, hProcess, out pModules);
                IntPtr[] modules = null;

                if (!pStub.IsNull())
                {
                    try
                    {
                        // Run the stub
                        uint threadval = WinAPI.RunThread(hProcess, pStub, 0, 10000);
                        if (threadval != uint.MaxValue)
                        {
                            // Read the module handle buffer into memory
                            byte[] rawHandles = WinAPI.ReadRemoteMemory(hProcess, pModules, (uint)dllPaths.Length << 2);
                            if (rawHandles != null)
                            {
                                // convert the raw binary data into usable module handles.
                                modules = new IntPtr[dllPaths.Length];
                                for (int i = 0; i < modules.Length; i++)
                                    modules[i] = new IntPtr(BitConverter.ToInt32(rawHandles, i << 2));
                            }
                            else
                            {
                                throw new InvalidOperationException("Unable to read from the remote process.");
                            }
                        }
                        else
                        {
                            throw new Exception("Error occurred while executing remote thread.");
                        }
                    }
                    finally
                    {
                        // cleanup the memory
                        WinAPI.VirtualFreeEx(hProcess, pModules, 0, 0x8000);
                        WinAPI.VirtualFreeEx(hProcess, pStub, 0, 0x8000);
                    }
                }
                return modules;
            }
            catch (Exception e)
            {
                SetLastError(e);
                return null;
            }
        }
    }
}
