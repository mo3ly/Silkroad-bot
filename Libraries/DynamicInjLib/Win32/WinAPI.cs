/*
    WinAPI.cs
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
using System.Runtime.InteropServices;

namespace JLibrary.Win32
{
    public static class WinAPI
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenThread(uint dwDesiredAccess, bool bInheritHandle, int dwThreadId);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, int flAllocationType, int flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, int dwFreeType);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flNewProtect, out uint flOldProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpAddress, byte[] lpBuffer, int dwSize, out uint lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out uint lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess, int lpThreadAttributes, int dwStackSize, IntPtr lpStartAddress, uint lpParameter, int dwCreationFlags, int lpThreadId);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetModuleHandleA(string lpModuleName);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, uint lpProcName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint WaitForSingleObject(IntPtr hObject, int dwTimeout);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetExitCodeThread(IntPtr hThread, out uint lpExitCode);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetThreadContext(IntPtr hThread, ref CONTEXT pContext);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetThreadContext(IntPtr hThread, ref CONTEXT pContext);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint SuspendThread(IntPtr hThread);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint ResumeThread(IntPtr hThread);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int GetProcessId(IntPtr hProcess);

        #region Supporting Structs
        public struct FLOATING_SAVE_AREA
        {
            public uint ControlWord;
            public uint StatusWord;
            public uint TagWord;
            public uint ErrorOffset;
            public uint ErrorSelector;
            public uint DataOffset;
            public uint DataSelector;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
            public byte[] RegisterArea;
            public uint Cr0NpxState;
        }

        public struct CONTEXT
        {
            public uint ContextFlags; //set this to an appropriate value
            // Retrieved by CONTEXT_DEBUG_REGISTERS
            public uint Dr0;
            public uint Dr1;
            public uint Dr2;
            public uint Dr3;
            public uint Dr6;
            public uint Dr7;
            // Retrieved by CONTEXT_FLOATING_POINT
            public FLOATING_SAVE_AREA FloatSave;
            // Retrieved by CONTEXT_SEGMENTS
            public uint SegGs;
            public uint SegFs;
            public uint SegEs;
            public uint SegDs;
            // Retrieved by CONTEXT_INTEGER
            public uint Edi;
            public uint Esi;
            public uint Ebx;
            public uint Edx;
            public uint Ecx;
            public uint Eax;
            // Retrieved by CONTEXT_CONTROL
            public uint Ebp;
            public uint Eip;
            public uint SegCs;
            public uint EFlags;
            public uint Esp;
            public uint SegSs;
            // Retrieved by CONTEXT_EXTENDED_REGISTERS
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
            public byte[] ExtendedRegisters;
        };
        #endregion

        public static uint GetLastErrorEx(IntPtr hProcess)
        {
            IntPtr fnGetLastError = WinAPI.GetProcAddress(WinAPI.GetModuleHandleA("kernel32.dll"), "GetLastError");
            return RunThread(hProcess, fnGetLastError, 0);
        }

        public static byte[] ReadRemoteMemory(IntPtr hProc, IntPtr address, uint len)
        {
            byte[] buffer = new byte[len];
            uint bytes = 0;
            if (!ReadProcessMemory(hProc, address, buffer, buffer.Length, out bytes) || bytes != len)
                buffer = null;
            return buffer;
        }

        //very straightforward way to run a thread and capture the return.
        //will return -1 (uint.MaxValue == -1 as a signed integer) if it fails.
        public static uint RunThread(IntPtr hProcess, IntPtr lpStartAddress, uint lpParam, int timeout = 1000)
        {
            uint dwThreadRet = uint.MaxValue; //-1 as a signed integer.
            IntPtr hThread = CreateRemoteThread(hProcess, 0, 0, lpStartAddress, lpParam, 0, 0);
            if (hThread != IntPtr.Zero)
            {
                if (WaitForSingleObject(hThread, timeout) == 0x0L) //wait for a response
                    GetExitCodeThread(hThread, out dwThreadRet);
            }
            return dwThreadRet;
        }

        public static IntPtr ReadRemotePointer(IntPtr hProcess, IntPtr pData)
        {
            IntPtr ptr = IntPtr.Zero;
            if (!hProcess.IsNull() && !pData.IsNull())
            {
                byte[] mem = null;
                if ((mem = ReadRemoteMemory(hProcess, pData, (uint)IntPtr.Size)) != null)
                    ptr = new IntPtr(BitConverter.ToInt32(mem, 0));
            }
            return ptr;
        }

        public static IntPtr GetModuleHandleEx(IntPtr hProcess, string lpModuleName)
        {
            IntPtr hGMW = GetProcAddress(GetModuleHandleA("kernel32.dll"), "GetModuleHandleW");
            IntPtr hModule = IntPtr.Zero;

            if (!hGMW.IsNull())
            {
                IntPtr hParam = CreateRemotePointer(hProcess, System.Text.Encoding.Unicode.GetBytes(lpModuleName + "\0"), 0x04);
                if (!hParam.IsNull())
                {
                    hModule = Win32Ptr.Create(RunThread(hProcess, hGMW, (uint)hParam.ToInt32()));
                    VirtualFreeEx(hProcess, hParam, 0, 0x8000);
                }
            }
            return hModule;
        }

        //Create a pointer to memory in the remote process
        public static IntPtr CreateRemotePointer(IntPtr hProcess, byte[] pData, int flProtect)
        {
            IntPtr pAlloc = IntPtr.Zero;
            if (pData != null && hProcess != IntPtr.Zero)
            {
                pAlloc = VirtualAllocEx(hProcess, IntPtr.Zero, (uint)pData.Length, 0x1000 | 0x2000, flProtect);
                uint nBytes = 0;
                if (pAlloc == IntPtr.Zero || !WriteProcessMemory(hProcess, pAlloc, pData, pData.Length, out nBytes) || nBytes != pData.Length)
                {
                    if (pAlloc != IntPtr.Zero)
                    {
                        VirtualFreeEx(hProcess, pAlloc, 0, 0x8000);
                        pAlloc = IntPtr.Zero;
                    }
                }
            }
            return pAlloc;
        }

        /*
         * This is my GetProcAddressEx function. Basically, it does exactly what GetProcAddress does,
         * but for a module in a remote process. It looks up the module, and from there parses the lexicographically
         * ordered export function table (commonly known as the EAT). It calculates based on ordinal offset.
         * However, if you find this method doesn't work for you for whatever reason (i rushed the binary search algorithm)
         * i've provided another GetProcAddressEx method below with the same parameters which basically just calls the 'official'
         * GetProcAddress function from within the remote process and captures the return. Up to you -Jason
         */
        public static IntPtr GetProcAddressEx(IntPtr hProc, IntPtr hModule, object lpProcName)
        {
            IntPtr procAddress = IntPtr.Zero;
            byte[] pDosHd = ReadRemoteMemory(hProc, hModule, 0x40); //attempt to read the DOS header from memory
            if (pDosHd != null && BitConverter.ToUInt16(pDosHd, 0) == 0x5A4D) //compare the expected DOS "MZ" signature to whatever we just read
            {
                uint e_lfanew = BitConverter.ToUInt32(pDosHd, 0x3C); //read the e_lfanew number
                if (e_lfanew > 0)
                {
                    byte[] pNtHd = ReadRemoteMemory(hProc, hModule.Add(e_lfanew), 0x108); //read the NT_HEADERS.
                    if (pNtHd != null && BitConverter.ToUInt32(pNtHd, 0) == 0x4550) //check the NT_HEADERS signature (PE\0\0)
                    {
                        uint expDirPtr = BitConverter.ToUInt32(pNtHd, 0x78); //get the pointer to the export directory (first data directory)
                        uint expDirSize = BitConverter.ToUInt32(pNtHd, 0x7C);
                        if (expDirPtr > 0 && expDirSize > 0) //does this module even export functions?
                        {
                            byte[] pExpDir = ReadRemoteMemory(hProc, hModule.Add(expDirPtr), 0x28); //Read the export directory from the process
                            uint pEat = BitConverter.ToUInt32(pExpDir, 0x1C); //pointer to the export address table
                            uint pOrd = BitConverter.ToUInt32(pExpDir, 0x24); //pointer to the ordinal table.
                            uint nFunc = BitConverter.ToUInt32(pExpDir, 0x14);
                            int ord = -1;

                            if (pEat > 0 && pOrd > 0)
                            {
                                if (lpProcName.GetType().Equals(typeof(string)))
                                {
                                    int index = SearchExports(hProc, hModule, pExpDir, (string)lpProcName); //search the exported names table for the specified function
                                    if (index > -1) //check the function was found
                                    {
                                        byte[] bOrd = ReadRemoteMemory(hProc, hModule.Add(pOrd + (index << 1)), 0x2); //read the ordinal number for the function from the process
                                        ord = (int)(bOrd == null ? -1 : BitConverter.ToUInt16(bOrd, 0)); //get the ordinal number for this function
                                    }
                                }
                                else if (lpProcName.GetType().Equals(typeof(short)) || lpProcName.GetType().Equals(typeof(ushort)))
                                {
                                    ord = int.Parse(lpProcName.ToString());
                                }
                                if (ord > -1 && ord < nFunc) //just a final check to make sure we have a valid ordinal
                                {
                                    //reference the Export Address Table to find the function address for our ordinal (don't forget to factor in the ordinal base)
                                    //Unlike zero-based indexing, the 'ordinal number' indexing starts at 1, so subtract 1 from the ordbase to get zero-based index
                                    byte[] addr = ReadRemoteMemory(hProc, hModule.Add(pEat + (ord << 2)), 0x4);
                                    if (addr != null)
                                    {
                                        uint pFunction = BitConverter.ToUInt32(addr, 0);
                                        if (pFunction >= expDirPtr && pFunction < (expDirPtr + expDirSize)) //forwarded.
                                        {
                                            string forward = ReadRemoteString(hProc, hModule.Add(pFunction));
                                            if (!string.IsNullOrEmpty(forward) && forward.Contains("."))
                                                procAddress = GetProcAddressEx(hProc, GetModuleHandleEx(hProc, forward.Split('.')[0]), forward.Split('.')[1]);
                                        }
                                        else
                                        {
                                            procAddress = hModule.Add(pFunction);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return procAddress;
        }


        private static int SearchExports(IntPtr hProcess, IntPtr hModule, byte[] exports, string name)
        {
            uint cntExports = BitConverter.ToUInt32(exports, 0x18); //number of named exported functions
            uint ptrNameTable = BitConverter.ToUInt32(exports, 0x20); //pointer to the export name table
            int rva = -1;

            if (cntExports > 0 && ptrNameTable > 0)
            {
                byte[] rawPtrs = ReadRemoteMemory(hProcess, hModule.Add(ptrNameTable), cntExports << 2); //be lazy and read all the name pointers at once.
                if (rawPtrs != null)
                {
                    //quickly convert that series of bytes into pointer values that make sense. 
                    uint[] namePtrs = new uint[cntExports];
                    for (int i = 0; i < namePtrs.Length; i++)
                        namePtrs[i] = BitConverter.ToUInt32(rawPtrs, i << 2);

                    //binary search, huzzah! Part of the PE specification is that all exported functions are ordered lexicographically in a PE file.
                    int start = 0, end = namePtrs.Length - 1, middle = 0;
                    string curvalue = string.Empty;
                    //basically just search through all the exports looking for the specified function
                    while (start >= 0 && start <= end && rva == -1)
                    {
                        middle = (start + end) / 2;
                        curvalue = ReadRemoteString(hProcess, hModule.Add(namePtrs[middle]));
                        if (curvalue.Equals(name))
                            rva = middle;
                        else if (string.CompareOrdinal(curvalue, name) < 0)
                            start = middle - 1;
                        else
                            end = middle + 1;
                    }
                }
            }
            return rva;
        }

        public static string ReadRemoteString(IntPtr hProcess, IntPtr lpAddress, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.ASCII;

            var builder = new StringBuilder(); //easiest and cleanest way to build an unknown-length string.
            byte[] buffer = new byte[256];
            uint nbytes = 0;
            int terminator = -1, index = 0;

            while (terminator < 0 && ReadProcessMemory(hProcess, lpAddress, buffer, buffer.Length, out nbytes) && nbytes > 0)
            {
                lpAddress = lpAddress.Add(nbytes); //advance where we're reading from.
                index = builder.Length; //micro-optimization for .IndexOf
                builder.Append(encoding.GetString(buffer, 0, (int)nbytes)); //append the data to the StringBuilder
                terminator = builder.ToString().IndexOf('\0', index); //check if there's a null-byte in the string yet (strings are null-terminated)
            }

            return builder.ToString().Substring(0, terminator);  //return the data up til the null terminator.
        }
    }
}
