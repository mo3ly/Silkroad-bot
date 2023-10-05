/*
    StandardInjectionMethod.cs
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
using JLibrary.PortableExecutable;
using JLibrary.Tools;
using JLibrary.Win32;

namespace InjectionLibrary
{
    /**
     * This class is purely designed to reduce the amount of duplicate code needed for InjectionMethods
     * that use LoadLibrary at some level. Because LoadLibrary needs to be pointed to a file, the overloads
     * with PEImage parameters pretty much just write the data to a file, then call the string counterparts.
     * I also wrote a utility asm stub to avoid spamming CreateRemoteThread (MULTILOAD_STUB).
     */
    internal abstract class StandardInjectionMethod : InjectionMethod
    {
        public override IntPtr Inject(PortableExecutable dll, IntPtr hProcess)
        {
            //same as above, write the temp file and defer to the other Inject methods.
            ClearErrors();
            string path = Utils.WriteTempData(dll.ToArray());
            IntPtr hModule = IntPtr.Zero;
            if (!string.IsNullOrEmpty(path))
            {
                hModule = Inject(path, hProcess);
                try
                {
                    System.IO.File.Delete(path);
                }
                catch { /* nom nom nom */ }

            }
            return hModule;
        }

        public override IntPtr[] InjectAll(PortableExecutable[] dlls, IntPtr hProcess)
        {
            ClearErrors();
            return InjectAll(Array.ConvertAll<PortableExecutable, string>(dlls, pe => Utils.WriteTempData(pe.ToArray())), hProcess);
        }

        public override bool Unload(IntPtr hModule, IntPtr hProcess)
        {
            ClearErrors();

            if (hProcess.IsNull() || hProcess.Compare(-1))
                throw new ArgumentOutOfRangeException("hProcess", "Invalid process handle specified.");
            if (hModule.IsNull())
                throw new ArgumentNullException("hModule", "Invalid module handle");

            try
            {
                bool[] results = UnloadAll(new IntPtr[] { hModule }, hProcess);
                return results != null && results.Length > 0
                        ? results[0]
                        : false;
            }
            catch (Exception e)
            {
                SetLastError(e);
                return false;
            }
        }

        // Unload an array of modules from the remote process.
        public override bool[] UnloadAll(IntPtr[] hModules, IntPtr hProcess)
        {
            ClearErrors();
            IntPtr pBuffer = IntPtr.Zero;
            IntPtr pModules = IntPtr.Zero;
            IntPtr pStub = IntPtr.Zero;
            try
            {
                uint nbytes = 0;
                IntPtr fnFreeLibrary = WinAPI.GetProcAddress(WinAPI.GetModuleHandleA("kernel32.dll"), "FreeLibrary");

                if (fnFreeLibrary.IsNull())
                    throw new Exception("Unable to find necessary function entry points in the remote process");

                // Allocate memory to contain the modules, the FreeLibrary results and the asm stub
                pBuffer = WinAPI.VirtualAllocEx(hProcess, IntPtr.Zero, (uint)(hModules.Length << 2), 0x1000 | 0x2000, 0x04);
                pModules = WinAPI.VirtualAllocEx(hProcess, IntPtr.Zero, (uint)((hModules.Length + 1) << 2), 0x1000 | 0x2000, 0x04);
                pStub = WinAPI.VirtualAllocEx(hProcess, IntPtr.Zero, (uint)MULTIUNLOAD_STUB.Length, 0x1000 | 0x2000, 0x40);

                // sanity check
                if (pBuffer.IsNull() || pModules.IsNull() || pStub.IsNull())
                    throw new InvalidOperationException("Unable to allocate memory in the remote process");

                // Create the modules buffer. Allocated hModules.Length + 1 modules because the ASM stub looks for a NULL module
                // to signify the end of the module list. (C# new byte[] initializes all members to 0)
                byte[] bModules = new byte[(hModules.Length + 1) << 2];
                for (int i = 0; i < hModules.Length; i++)
                    BitConverter.GetBytes(hModules[i].ToInt32()).CopyTo(bModules, i << 2);

                WinAPI.WriteProcessMemory(hProcess, pModules, bModules, bModules.Length, out nbytes);
                var stub = (byte[])MULTIUNLOAD_STUB.Clone();
                // Patch the assembly stub with the dynamic values.
                BitConverter.GetBytes(pModules.ToInt32()).CopyTo(stub, 0x07);
                BitConverter.GetBytes(pBuffer.ToInt32()).CopyTo(stub, 0x0F);
                BitConverter.GetBytes(fnFreeLibrary.Subtract(pStub.Add(0x38)).ToInt32()).CopyTo(stub, 0x34);

                // Write the assembly stub to the remote process
                if (!WinAPI.WriteProcessMemory(hProcess, pStub, stub, stub.Length, out nbytes) || nbytes != (uint)stub.Length)
                    throw new InvalidOperationException("Unable to write the function stub to the remote process.");
                // execute the stub and get the result
                if (WinAPI.RunThread(hProcess, pStub, 0) == uint.MaxValue)
                    throw new InvalidOperationException("Error occurred when running remote function stub.");

                // read the BOOL return array into our own process
                byte[] bResults = WinAPI.ReadRemoteMemory(hProcess, pBuffer, (uint)(hModules.Length << 2));
                if (bResults == null)
                    throw new Exception("Unable to read results from the remote process.");

                // Convert the raw results data into a C# bool datatype (I used a C++ BOOL (typedef'd to a 4 byte value type) in the 
                // assembler function as it's far easier to write, which is why I convert ToInt32())
                bool[] results = new bool[hModules.Length];
                for (int i = 0; i < results.Length; i++)
                    results[i] = BitConverter.ToInt32(bResults, i << 2) != 0;

                return results;
            }
            catch (Exception e)
            {
                SetLastError(e);
                return null;
            }
            finally
            {
                // Always need to free the memory, regardless of the control flow.
                WinAPI.VirtualFreeEx(hProcess, pStub, 0, 0x8000);
                WinAPI.VirtualFreeEx(hProcess, pBuffer, 0, 0x8000);
                WinAPI.VirtualFreeEx(hProcess, pModules, 0, 0x8000);
            }
        }

        protected virtual IntPtr CreateMultiLoadStub(string[] paths, IntPtr hProcess, out IntPtr pModuleBuffer, uint nullmodule = 0)
        {
            // This function creates a multi-loading stub which essentially iterates a list of required modules and writes the results
            // of the LoadLibraryA / GetModuleHandleA call to a preallocated buffer (out IntPtr pModuleBuffer) in the remote process.
            pModuleBuffer = IntPtr.Zero;
            IntPtr pStub = IntPtr.Zero;
            
            try
            {
                // get function addresses.
                IntPtr hKernel32 = WinAPI.GetModuleHandleA("kernel32.dll");
                IntPtr fnLoadLibraryA = WinAPI.GetProcAddress(hKernel32, "LoadLibraryA");
                IntPtr fnGetModuleHandleA = WinAPI.GetProcAddress(hKernel32, "GetModuleHandleA");

                // sanity check
                if (fnLoadLibraryA.IsNull() || fnGetModuleHandleA.IsNull())
                    throw new Exception("Unable to find necessary function entry points in the remote process");

                // Create the necessary remote buffers and values for the assembler call
                pModuleBuffer = WinAPI.VirtualAllocEx(hProcess, IntPtr.Zero, (uint)paths.Length << 2, 0x1000 | 0x2000, 0x04);
                IntPtr pLibs = WinAPI.CreateRemotePointer(hProcess, System.Text.Encoding.ASCII.GetBytes(string.Join("\0", paths) + "\0"), 0x04);
                if (pModuleBuffer.IsNull() || pLibs.IsNull())
                    throw new InvalidOperationException("Unable to allocate memory in the remote process");

                try
                {
                    uint nbytes = 0;
                    byte[] nullset = new byte[paths.Length << 2];
                    for (int i = 0; i < nullset.Length >> 2; i++) 
                        BitConverter.GetBytes(nullmodule).CopyTo(nullset, i << 2);
                    WinAPI.WriteProcessMemory(hProcess, pModuleBuffer, nullset, nullset.Length, out nbytes);

                    var stub = (byte[])MULTILOAD_STUB.Clone();
                    pStub = WinAPI.VirtualAllocEx(hProcess, IntPtr.Zero, (uint)stub.Length, 0x1000 | 0x2000, 0x40);

                    if (pStub.IsNull())
                        throw new InvalidOperationException("Unable to allocate memory in the remote process");

                    BitConverter.GetBytes(pLibs.ToInt32()).CopyTo(stub, 0x07);
                    BitConverter.GetBytes(paths.Length).CopyTo(stub, 0x0F);
                    BitConverter.GetBytes(pModuleBuffer.ToInt32()).CopyTo(stub, 0x18);

                    BitConverter.GetBytes(fnGetModuleHandleA.Subtract(pStub.Add(0x38)).ToInt32()).CopyTo(stub, 0x34);
                    BitConverter.GetBytes(fnLoadLibraryA.Subtract(pStub.Add(0x45)).ToInt32()).CopyTo(stub, 0x41);

                    if (!WinAPI.WriteProcessMemory(hProcess, pStub, stub, stub.Length, out nbytes) || nbytes != (uint)stub.Length)
                        throw new Exception("Error creating the remote function stub.");

                    return pStub;
                }
                finally // Don't actually handle the exception, just clean up some of the allocated memory.
                {
                    WinAPI.VirtualFreeEx(hProcess, pModuleBuffer, 0, 0x8000);
                    WinAPI.VirtualFreeEx(hProcess, pLibs, 0, 0x8000);
                    if (!pStub.IsNull())
                        WinAPI.VirtualFreeEx(hProcess, pStub, 0, 0x8000);
                    pModuleBuffer = IntPtr.Zero;

                }
            }
            catch (Exception e)
            {
                SetLastError(e);
                return IntPtr.Zero;
            }
        }

        protected static readonly byte[] MULTILOAD_STUB =
        {
            0x55, 							//push ebp
            0x8B, 0xEC, 					//mov ebp, esp
            0x83, 0xEC, 0x0C, 				//sub esp, 0x0C,
            0xB9, 0x00, 0x00, 0x00, 0x00,	//mov ecx, pLibs
            0x89, 0x0C, 0x24, 				//mov [esp], ecx
            0xB9, 0x00, 0x00, 0x00, 0x00,	//mov ecx, nLibs
            0x89, 0x4C, 0x24, 0x04, 		//mov [esp + 4], ecx
            0xB9, 0x00, 0x00, 0x00, 0x00,	//mov ecx, hBuffer
            0x89, 0x4C, 0x24, 0x08, 		//mov [esp + 8], ecx
            0x8B, 0x4C, 0x24, 0x04, 		//mov ecx, [esp + 4] <-- mainloop --------+
            0x83, 0xF9, 0x00, 				//cmp ecx, 0							  |
            0x74, 0x3A, 					//jz finish								  |
            0x83, 0xE9, 0x01, 				//sub ecx, 1							  |
            0x89, 0x4C, 0x24, 0x04, 		//mov [esp + 4], ecx					  |
            0xFF, 0x34, 0x24, 				//push [esp]							  |
            0xE8, 0x00, 0x00, 0x00, 0x00,	//call GetModuleHandleA 				  |
            0x83, 0xF8, 0x00, 				//cmp eax, 0							  |
            0x75, 0x08, 					//jz sethandle --------------------+	  |
            0xFF, 0x34, 0x24, 				//push [esp]					   |	  |
            0xE8, 0x00, 0x00, 0x00, 0x00,	//call LoadLibraryA 			   |	  |
            0x8B, 0x4C, 0x24, 0x08, 		//mov ecx, [esp + 8h] <--sethandle-+	  |
            0x89, 0x01, 					//mov [ecx], eax 						  |
            0x83, 0xC1, 0x04, 				//add ecx, 4							  |
            0x89, 0x4C, 0x24, 0x08, 		//mov [esp + 8], ecx					  |
            0x8B, 0x0C, 0x24, 				//mov ecx, [esp]                          |
            0x8A, 0x01, 					//mov al, BYTE PTR [ecx] <-- findnull--+  |
            0x83, 0xC1, 0x01, 				//add ecx, 1                           |  |
            0x3C, 0x00, 					//cmp al 0							   |  |
            0x75, 0xF7, 					//jnz findnull ------------------------+  |
            0x89, 0x0C, 0x24, 				//mov [esp], ecx						  |
            0xEB, 0xBD, 					//jmp mainloop ---------------------------+
            0x8B, 0xE5, 					//mov esp, ebp <-- finish
            0x5D, 							//pop ebp
            0xC3				            //ret
        };

        protected static readonly byte[] MULTIUNLOAD_STUB =
        {
            0x55,
            0x8B, 0xEC,
            0x83, 0xEC, 0x0C,
            0xB9, 0x00, 0x00, 0x00, 0x00,
            0x89, 0x0C, 0x24,
            0xB9, 0x00, 0x00, 0x00, 0x00,
            0x89, 0x4C, 0x24, 0x04,
            0x8B, 0x0C, 0x24,
            0x8B, 0x09,
            0x83, 0xF9, 0x00,
            0x74, 0x3A,
            0x89, 0x4C, 0x24, 0x08,
            0x8B, 0x4C, 0x24, 0x04,
            0xC7, 0x01, 0x00, 0x00, 0x00, 0x00,
            0xFF, 0x74, 0x24, 0x08,
            0xE8, 0x00, 0x00, 0x00, 0x00,
            0x83, 0xF8, 0x00,
            0x74, 0x08,
            0x8B, 0x4C, 0x24, 0x04,
            0x89, 0x01,
            0xEB, 0xEA,
            0x8B, 0x0C, 0x24,
            0x83, 0xC1, 0x04,
            0x89, 0x0C, 0x24,
            0x8B, 0x4C, 0x24, 0x04,
            0x83, 0xC1, 0x04,
            0x89, 0x4C, 0x24, 0x04,
            0xEB, 0xBC,
            0x8B, 0xE5,
            0x5D,
            0xC3
        };
    }
}
