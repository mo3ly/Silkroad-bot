/*
    ManualMap.cs
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
using System.Collections.Generic;
using System.Text;
using JLibrary.PortableExecutable;
using JLibrary.Win32;
using JLibrary.Tools;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

namespace InjectionLibrary
{
    internal class ManualMap : InjectionMethod
    {
        #region Path Injection Implementations
        public override IntPtr Inject(string dllPath, IntPtr hProcess)
        {
            ClearErrors();
            try
            {
                using (PortableExecutable img = new PortableExecutable(dllPath))
                    return Inject(img, hProcess);
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
            return Array.ConvertAll(dllPaths, dp => Inject(dp, hProcess));
        }
        #endregion

        public override IntPtr Inject(PortableExecutable image, IntPtr hProcess)
        {
            ClearErrors();
            try
            {
                return MapModule(Utils.DeepClone(image), hProcess, true);
            }
            catch (Exception e)
            {
                SetLastError(e);
                return IntPtr.Zero;
            }
        }

        public override IntPtr[] InjectAll(PortableExecutable[] images, IntPtr hProcess)
        {
            ClearErrors();
            return Array.ConvertAll(images, pe => Inject(pe, hProcess));
        }

        public override bool Unload(IntPtr hModule, IntPtr hProcess)
        {
            // Unloading a manually mapped file is fairly straightforward. There is no need to call FreeLibrary or anything
            // because the file was never actually injected using LoadLibrary and doesn't need to clear any PEB entries etc.
            // basically just call the Entry Point again with DLL_PROCESS_DETACH flag and not-null for the lpReserved parameter
            // and then VirtualFree the remote memory.
            ClearErrors();

            if (hModule.IsNull())
                throw new ArgumentNullException("hModule", "Invalid module handle");

            if (hProcess.IsNull() || hProcess.Compare(-1))
                throw new ArgumentException("Invalid process handle.", "hProcess");

            IntPtr pStub = IntPtr.Zero;
            uint nBytes = 0;
            try
            {
                uint entry = FindEntryPoint(hProcess, hModule);
                if (entry != 0)
                {
                    var stub = (byte[])DLLMAIN_STUB.Clone();
                    BitConverter.GetBytes(hModule.ToInt32()).CopyTo(stub, 0x0B);
                    BitConverter.GetBytes((uint)0).CopyTo(stub, 0x06);
                    BitConverter.GetBytes((uint)1000).CopyTo(stub, 0x01);

                    pStub = WinAPI.VirtualAllocEx(hProcess, IntPtr.Zero, (uint)DLLMAIN_STUB.Length, 0x1000 | 0x2000, 0x40);
                    if (pStub.IsNull() || (!WinAPI.WriteProcessMemory(hProcess, pStub, stub, stub.Length, out nBytes) || nBytes != (uint)stub.Length))
                        throw new InvalidOperationException("Unable to write stub to the remote process.");

                    IntPtr hStubThread = WinAPI.CreateRemoteThread(hProcess, 0, 0, pStub, (uint)hModule.Add(entry).ToInt32(), 0, 0);
                    if (WinAPI.WaitForSingleObject(hStubThread, 5000) == 0x0L)
                    {
                        WinAPI.VirtualFreeEx(hProcess, pStub, 0, 0x8000);
                        WinAPI.CloseHandle(hStubThread);
                        return WinAPI.VirtualFreeEx(hProcess, hModule, 0, 0x8000);
                    }
                    return false;
                }
                else
                {
                    return WinAPI.VirtualFreeEx(hProcess, hModule, 0, 0x8000);
                }
            }
            catch (Exception e)
            {
                SetLastError(e);
                return false;
            }
        }

        public override bool[] UnloadAll(IntPtr[] hModules, IntPtr hProcess)
        {
            // No fancy optimized method here, UnloadAll simply represents a series of direct calls to Unload for each
            // module. Pretty simple stuff.
            ClearErrors();
            if (hModules == null)
                throw new ArgumentNullException("hModules", "Parameter cannot be null.");
            if (hProcess.IsNull() || hProcess.Compare(-1))
                throw new ArgumentOutOfRangeException("hProcess", "Invalid process handle specified.");
            try
            {
                var results = new bool[hModules.Length];
                for (int i = 0; i < hModules.Length; i++)
                    results[i] = Unload(hModules[i], hProcess);
                return results;
            }
            catch (Exception e)
            {
                SetLastError(e);
                return null;
            }
        }

        #region Codebase
        // Most efficient version, this will be the work horse.
        private static IntPtr MapModule(PortableExecutable image, IntPtr hProcess, bool preserveHeaders = false)
        {
            if (hProcess.IsNull() || hProcess.Compare(-1))
                throw new ArgumentException("Invalid process handle.", "hProcess");

            if (image == null)
                throw new ArgumentException("Cannot map a non-existant PE Image.", "image");

            int processId = WinAPI.GetProcessId(hProcess);

            if (processId == 0)
                throw new ArgumentException("Provided handle doesn't have sufficient permissions to inject", "hProcess");

            IntPtr hModule = IntPtr.Zero;
            IntPtr pStub = IntPtr.Zero;
            uint nBytes = 0;

            try
            {
                //allocate memory for the image to load into the remote process.
                hModule = WinAPI.VirtualAllocEx(hProcess, IntPtr.Zero, image.NTHeader.OptionalHeader.SizeOfImage, 0x1000 | 0x2000, 0x04);  
                if (hModule.IsNull())
                    throw new InvalidOperationException("Unable to allocate memory in the remote process.");

                PatchRelocations(image, hModule);
                LoadDependencies(image, hProcess, processId);
                PatchImports(image, hProcess, processId);

                if (preserveHeaders)
                {
                    long szHeader = (image.DOSHeader.e_lfanew + Marshal.SizeOf(typeof(IMAGE_FILE_HEADER)) + sizeof(uint) + image.NTHeader.FileHeader.SizeOfOptionalHeader);
                    byte[] header = new byte[szHeader];
                    if (image.Read(0, SeekOrigin.Begin, header))
                        WinAPI.WriteProcessMemory(hProcess, hModule, header, header.Length, out nBytes);
                }

                MapSections(image, hProcess, hModule);

                // some modules don't have an entry point and are purely libraries, mapping them and keeping the handle is just fine
                // an unlikely scenario with forced injection, but you never know.
                if (image.NTHeader.OptionalHeader.AddressOfEntryPoint > 0) 
                {
                    var stub = (byte[])DLLMAIN_STUB.Clone();
                    BitConverter.GetBytes(hModule.ToInt32()).CopyTo(stub, 0x0B);

                    pStub = WinAPI.VirtualAllocEx(hProcess, IntPtr.Zero, (uint)DLLMAIN_STUB.Length, 0x1000 | 0x2000, 0x40);
                    if (pStub.IsNull() || (!WinAPI.WriteProcessMemory(hProcess, pStub, stub, stub.Length, out nBytes) || nBytes != (uint)stub.Length))
                        throw new InvalidOperationException("Unable to write stub to the remote process.");

                    IntPtr hStubThread = WinAPI.CreateRemoteThread(hProcess, 0, 0, pStub, (uint)(hModule.Add(image.NTHeader.OptionalHeader.AddressOfEntryPoint).ToInt32()), 0, 0);
                    if (WinAPI.WaitForSingleObject(hStubThread, 5000) == 0x0L)
                    {
                        WinAPI.GetExitCodeThread(hStubThread, out nBytes);
                        if (nBytes == 0)
                        {
                            WinAPI.VirtualFreeEx(hProcess, hModule, 0, 0x8000);
                            throw new Exception("Entry method of module reported a failure " + Marshal.GetLastWin32Error().ToString());
                        }
                        WinAPI.VirtualFreeEx(hProcess, pStub, 0, 0x8000);
                        WinAPI.CloseHandle(hStubThread);
                    }
                }
            }
            catch (Exception e)
            {
                if (!hModule.IsNull())
                    WinAPI.VirtualFreeEx(hProcess, hModule, 0, 0x8000);
                if (!pStub.IsNull())
                    WinAPI.VirtualFreeEx(hProcess, hModule, 0, 0x8000);

                hModule = IntPtr.Zero;
                throw e;
            }
            return hModule; 
        }

        /**
         * Find the entry point of a loaded module
         * based on its Base Address. Reverses the PE
         * structure to find the entry point
         */
        private static uint FindEntryPoint(IntPtr hProcess, IntPtr hModule)
        {
            if (hProcess.IsNull() || hProcess.Compare(-1))
                throw new ArgumentException("Invalid process handle.", "hProcess");
            if (hModule.IsNull())
                throw new ArgumentException("Invalid module handle.", "hModule");

            byte[] bDosHeader = WinAPI.ReadRemoteMemory(hProcess, hModule, (uint)Marshal.SizeOf(typeof(IMAGE_DOS_HEADER)));
            if (bDosHeader != null)
            {
                ushort e_magic = BitConverter.ToUInt16(bDosHeader, 0);
                uint e_lfanew = BitConverter.ToUInt32(bDosHeader, 0x3C);
                if (e_magic == 23117)
                {
                    byte[] bNtHeader = WinAPI.ReadRemoteMemory(hProcess, hModule.Add(e_lfanew), (uint)Marshal.SizeOf(typeof(IMAGE_NT_HEADER32)));
                    if (bNtHeader != null && BitConverter.ToUInt32(bNtHeader, 0) == 17744)
                    {
                        IMAGE_NT_HEADER32 ntHd = default(IMAGE_NT_HEADER32);
                        using (var buffer = new UnmanagedBuffer(256))
                            if (buffer.Translate<IMAGE_NT_HEADER32>(bNtHeader, out ntHd))
                                return ntHd.OptionalHeader.AddressOfEntryPoint;
                    }
                }
            }
            return 0;
        }

        private static void MapSections(PortableExecutable image, IntPtr hProcess, IntPtr pModule)
        {
            //very straightforward really. Just iterate through all the sections and map them to their desired virtual addresses in the remote process.
            //I'm not 100% sure about how well masking the section header characteristics and passing them off as memory protection constants goes. But
            //so far I haven't hit any issues. (i.e a section header with characteristics "IMAGE_SCN_TYPE_NO_PAD" will set "PAGE_WRITECOPY" memory protection.
            byte[] databuffer;
            uint n;

            foreach (var pSecHd in image.EnumSectionHeaders())
            {
                databuffer = new byte[pSecHd.SizeOfRawData];
                if (image.Read(pSecHd.PointerToRawData, SeekOrigin.Begin, databuffer))
                {
                    if ((pSecHd.Characteristics & 0x02000000) == 0) //can actually ignore this section (usually the reloc section)
                    {
                        WinAPI.WriteProcessMemory(hProcess, pModule.Add(pSecHd.VirtualAddress), databuffer, databuffer.Length, out n);
                        WinAPI.VirtualProtectEx(hProcess, pModule.Add(pSecHd.VirtualAddress), pSecHd.SizeOfRawData, pSecHd.Characteristics & 0x00FFFFFF, out n);
                    }
                }
                else
                {
                    throw image.GetLastError();
                }
            }
        }

        private static void PatchRelocations(PortableExecutable image, IntPtr pAlloc)
        {
            // Base relocations are essentially Microsofts ingenious way of preserving portability in images.
            // for all absolute address calls/jmps/references...etc, an entry is made into the base relocation
            // table telling the loader exactly where an "absolute" address is being used. This allows the loader
            // to iterate through the relocations and patch these absolute values to ensure they are correct when
            // the image is loaded somewhere that isn't its preferred base address.
            IMAGE_DATA_DIRECTORY relocDir = image.NTHeader.OptionalHeader.DataDirectory[(int)DATA_DIRECTORIES.BaseRelocTable];
            if (relocDir.Size > 0) //check if there are in fact any relocations.
            {
                uint n = 0;
                uint delta = (uint)(pAlloc.ToInt32() - image.NTHeader.OptionalHeader.ImageBase); //The difference in loaded/preferred addresses.
                uint pReloc = image.GetPtrFromRVA(relocDir.VirtualAddress);
                uint szReloc = (uint)Marshal.SizeOf(typeof(IMAGE_BASE_RELOCATION));
                IMAGE_BASE_RELOCATION reloc;

                while (n < relocDir.Size && image.Read(pReloc, SeekOrigin.Begin, out reloc))
                {
                    // A relocation block consists of an IMAGE_BASE_RELOCATION, and an array of WORDs.
                    // To calculate the number of relocations (represented by WORDs), just do some simple math.
                    int nrelocs = (int)((reloc.SizeOfBlock - szReloc) / sizeof(ushort));
                    uint pageVa = image.GetPtrFromRVA(reloc.VirtualAddress); //The Page RVA for this set of relocations (usually a 4K boundary).
                    ushort vreloc;
                    uint old;

                    for (int i = 0; i < nrelocs; i++)
                    {
                        // There are only 2 types of relocations on Intel machines: ABSOLUTE (padding, nothing needs to be done) and HIGHLOW (0x03)
                        // Highlow means that all 32 bits of the "delta" value need to be added to the relocation value.
                        if (image.Read(pReloc + szReloc + (i << 1), SeekOrigin.Begin, out vreloc) && (vreloc >> 12 & 3) != 0)
                        {
                            uint vp = (uint)(pageVa + (vreloc & 0x0FFF));
                            if (image.Read<uint>(vp, SeekOrigin.Begin, out old))
                                image.Write<uint>(-4, SeekOrigin.Current, (uint)(old + delta));
                            else
                                throw image.GetLastError(); //unlikely, but I hate crashing targets because something in the PE was messed up.
                        }
                    }
                    n += reloc.SizeOfBlock;
                    pReloc += reloc.SizeOfBlock;
                }
            }
        }

        private static void PatchImports(PortableExecutable image, IntPtr hProcess, int processId)
        {
            string module = string.Empty;
            string fname = string.Empty;

            foreach (var desc in image.EnumImports())
            {
                if (image.ReadString(image.GetPtrFromRVA(desc.Name), SeekOrigin.Begin, out module))
                {
                    IntPtr pModule = IntPtr.Zero;
                    IntPtr tModule = IntPtr.Zero;
                    //Thanks to FastLoadDependencies, all dependent modules *should* be loaded into the remote process already.
                    pModule = GetRemoteModuleHandle(module, processId);

                    if (pModule.IsNull())
                        throw new FileNotFoundException(string.Format("Unable to load dependent module '{0}'.", module));

                    //now have a supposedly valid module handle remote process, all that remains to be done is patch the info.
                    uint pThunk = image.GetPtrFromRVA(desc.FirstThunkPtr); //despite the fact FirstThunk and OriginalFirstThunk are identical within an unmapped PE, only FirstThunk is looked up after mapping.
                    uint szThunk = (uint)Marshal.SizeOf(typeof(IMAGE_THUNK_DATA));
                    IMAGE_THUNK_DATA thunk;

                    while (image.Read(pThunk, SeekOrigin.Begin, out thunk) && thunk.u1.AddressOfData > 0) //final thunk is signified by a null-filled IMAGE_THUNK_DATA structure.
                    {
                        IntPtr remote = IntPtr.Zero;
                        object procVal = null;
                        if ((thunk.u1.Ordinal & 0x80000000) == 0) //import by name
                        {
                            if (image.ReadString(image.GetPtrFromRVA(thunk.u1.AddressOfData) + 2, SeekOrigin.Begin, out fname)) //get the function name.
                                procVal = fname;
                            else
                                throw image.GetLastError(); //error occurred during memory iteration, this is only a safeguard and shouldn't really ever occur, but the universe loves proving me wrong.
                        }
                        else //import by ordinal.
                        {
                            procVal = (ushort)(thunk.u1.Ordinal & 0xFFFF);
                        }

                        // the following section of code simply aims to reduce overhead. A check is first performed to see if the current module
                        // is loaded in the current process first, if so it simply uses relative addresses to calculate the function address in the remote
                        // process. If the module isn't found in our process, a call to GetProcAddressEx is used to find the remote address. Of course, you
                        // could simply guarantee the first case by calling LoadLibrary internally, but I find that can have unwanted side effects.
                        if (!(remote = WinAPI.GetModuleHandleA(module)).IsNull())
                        {
                            IntPtr local = procVal.GetType().Equals(typeof(string)) 
                                            ? WinAPI.GetProcAddress(remote, (string)procVal) 
                                            : WinAPI.GetProcAddress(remote, (uint)((ushort)procVal) & 0x0000FFFF);
                            if (!local.IsNull())
                                remote = pModule.Add(local.Subtract(remote.ToInt32()).ToInt32());
                        }
                        else
                        {
                            remote = WinAPI.GetProcAddressEx(hProcess, pModule, procVal);
                        }
                        
                        if (remote.IsNull()) //alas, couldn't find the function.
                            throw new EntryPointNotFoundException(string.Format("Unable to locate imported function '{0}' from module '{1}' in the remote process.", fname, module));

                        image.Write<int>(pThunk, SeekOrigin.Begin, remote.ToInt32()); //overwrite the thunk and continue on our merry way.
                        pThunk += szThunk;
                    }
                }
            }
        }

        /*
         * Handles loading of all dependent modules. Iterates the IAT entries and attempts to load (using LoadLibrary) all 
         * of the necessary modules for the main module to function. The manifest is extracted and activation contexts used to
         * ensure correct loading of Side-By-Side dependencies.
         */
        private static bool LoadDependencies(PortableExecutable image, IntPtr hProcess, int processId)
        {
            List<string> neededDependencies = new List<string>();
            string curdep = string.Empty;
            bool success = false;

            foreach (var desc in image.EnumImports())
            {
                if (image.ReadString(image.GetPtrFromRVA(desc.Name), SeekOrigin.Begin, out curdep) && !string.IsNullOrEmpty(curdep))
                {
                    if (GetRemoteModuleHandle(curdep, processId).IsNull())
                        neededDependencies.Add(curdep);
                }
            }

            if (neededDependencies.Count > 0) //do we actually need to load any new modules?
            {
                byte[] bManifest = ExtractManifest(image);
                string pathManifest = string.Empty;

                if (bManifest == null) // no internal manifest, may be an external manifest or none at all?
                {
                    if (!string.IsNullOrEmpty(image.FileLocation) && File.Exists(Path.Combine(Path.GetDirectoryName(image.FileLocation), Path.GetFileName(image.FileLocation) + ".manifest")))
                    {
                        pathManifest = Path.Combine(Path.GetDirectoryName(image.FileLocation), Path.GetFileName(image.FileLocation) + ".manifest");
                    }
                    else // no internal or external manifest, presume no side-by-side dependencies.
                    {
                        var standard = InjectionMethod.Create(InjectionMethodType.Standard);
                        var results = standard.InjectAll(neededDependencies.ToArray(), hProcess);

                        foreach (var result in results)
                            if (result.IsNull())
                                return false; // failed to inject a dependecy, abort mission.

                        return true; // done loading dependencies.
                    }
                }
                else
                {
                    pathManifest = Utils.WriteTempData(bManifest);
                }

                if (string.IsNullOrEmpty(pathManifest))
                    return false;

                IntPtr pResolverStub = WinAPI.VirtualAllocEx(hProcess, IntPtr.Zero, (uint)RESOLVER_STUB.Length, 0x1000 | 0x2000, 0x40);
                IntPtr pManifest = WinAPI.CreateRemotePointer(hProcess, Encoding.ASCII.GetBytes(pathManifest + "\0"), 0x04);
                IntPtr pModules = WinAPI.CreateRemotePointer(hProcess, Encoding.ASCII.GetBytes(string.Join("\0", neededDependencies.ToArray()) + "\0"), 0x04);

                if (!pResolverStub.IsNull())
                {
                    var resolverStub = (byte[])RESOLVER_STUB.Clone();
                    uint nBytes = 0;

                    // Call patching. Patch the empty function addresses with the runtime addresses.
                    BitConverter.GetBytes(FN_CREATEACTCTXA.Subtract(pResolverStub.Add(0x3F)).ToInt32()).CopyTo(resolverStub, 0x3B);
                    BitConverter.GetBytes(FN_ACTIVATEACTCTX.Subtract(pResolverStub.Add(0x58)).ToInt32()).CopyTo(resolverStub, 0x54);
                    BitConverter.GetBytes(FN_GETMODULEHANDLEA.Subtract(pResolverStub.Add(0x84)).ToInt32()).CopyTo(resolverStub, 0x80);
                    BitConverter.GetBytes(FN_LOADLIBRARYA.Subtract(pResolverStub.Add(0x92)).ToInt32()).CopyTo(resolverStub, 0x8E);
                    BitConverter.GetBytes(FN_DEACTIVATEACTCTX.Subtract(pResolverStub.Add(0xC8)).ToInt32()).CopyTo(resolverStub, 0xC4);
                    BitConverter.GetBytes(FN_RELEASEACTCTX.Subtract(pResolverStub.Add(0xD1)).ToInt32()).CopyTo(resolverStub, 0xCD);

                    // Parameter patching
                    BitConverter.GetBytes(pManifest.ToInt32()).CopyTo(resolverStub, 0x1F);
                    BitConverter.GetBytes(neededDependencies.Count).CopyTo(resolverStub, 0x28);
                    BitConverter.GetBytes(pModules.ToInt32()).CopyTo(resolverStub, 0x31);

                    if (WinAPI.WriteProcessMemory(hProcess, pResolverStub, resolverStub, resolverStub.Length, out nBytes) && nBytes == (uint)resolverStub.Length)
                    {
                        uint result = WinAPI.RunThread(hProcess, pResolverStub, 0, 5000);
                        success = (result != uint.MaxValue && result != 0);
                    }

                    // Cleanup
                    WinAPI.VirtualFreeEx(hProcess, pModules, 0, 0x8000);
                    WinAPI.VirtualFreeEx(hProcess, pManifest, 0, 0x8000);
                    WinAPI.VirtualFreeEx(hProcess, pResolverStub, 0, 0x8000);
                }
            }

            return success;
        }

        /**
         * Extract an application manifest from
         * an executable file's resources.
         * Returns null if no manifest was found in the executable.
         */
        private static byte[] ExtractManifest(PortableExecutable image)
        {
            byte[] manifest = null;
            ResourceWalker walker = new ResourceWalker(image);
            ResourceWalker.ResourceDirectory manifestDir = null;
            for (int i = 0; i < walker.Root.Directories.Length && manifestDir == null; i++)
                if (walker.Root.Directories[i].Id == Constants.RT_MANIFEST)
                    manifestDir = walker.Root.Directories[i];

            if (manifestDir != null && manifestDir.Directories.Length > 0)
                if (IsManifestResource(manifestDir.Directories[0].Id) && manifestDir.Directories[0].Files.Length == 1)
                    manifest = manifestDir.Directories[0].Files[0].GetData();

            return manifest;
        }

        private static bool IsManifestResource(int id)
        {
            switch ((uint)id)
            {
                case Constants.CREATEPROCESS_MANIFEST_RESOURCE_ID:
                case Constants.ISOLATIONAWARE_MANIFEST_RESOURCE_ID:
                case Constants.ISOLATIONAWARE_NOSTATICIMPORT_MANIFEST_RESOURCE_ID:
                    return true;
                default:
                    return false;
            }
        }

        private static IntPtr GetRemoteModuleHandle(string module, int processId)
        {
            IntPtr hModule = IntPtr.Zero;
            Process target = Process.GetProcessById(processId);
            for (int i = 0; i < target.Modules.Count && hModule.IsNull(); i++)
                if (target.Modules[i].ModuleName.ToLower() == module.ToLower())
                    hModule = target.Modules[i].BaseAddress;
            return hModule;
        }
        #endregion

        #region Gigantic Assembly Bytecode, do not open for fear of losing eyesight.
        /* patch values */
        private static readonly IntPtr H_KERNEL32 = WinAPI.GetModuleHandleA("KERNEL32.dll");
        private static readonly IntPtr FN_CREATEACTCTXA = WinAPI.GetProcAddress(H_KERNEL32, "CreateActCtxA");
        private static readonly IntPtr FN_ACTIVATEACTCTX = WinAPI.GetProcAddress(H_KERNEL32, "ActivateActCtx");
        private static readonly IntPtr FN_LOADLIBRARYA = WinAPI.GetProcAddress(H_KERNEL32, "LoadLibraryA");
        private static readonly IntPtr FN_GETMODULEHANDLEA = WinAPI.GetProcAddress(H_KERNEL32, "GetModuleHandleA");
        private static readonly IntPtr FN_DEACTIVATEACTCTX = WinAPI.GetProcAddress(H_KERNEL32, "DeactivateActCtx");
        private static readonly IntPtr FN_RELEASEACTCTX = WinAPI.GetProcAddress(H_KERNEL32, "ReleaseActCtx");

        // DllMain function call stub.
        private static readonly byte[] DLLMAIN_STUB = 
        {
            0x68, 0x00, 0x00, 0x00, 0x00, //push lpReserved
            0x68, 0x01, 0x00, 0x00, 0x00, //push dwReason
            0x68, 0x00, 0x00, 0x00, 0x00, //push hModule
            0xFF, 0x54, 0x24, 0x10, //call [esp + 10h]
            0xC3//2, 0x0C, 0x00 
        };

        // Resolve a list of dlls.
        private static readonly byte[] RESOLVER_STUB = 
        {
            0x55,                              //push ebp
            0x8B, 0xEC,                        //mov ebp, esp
            0x83, 0xEC, 0x3C,                  //sub esp, 3Ch
            0x8B, 0xCC,                        //mov ecx, esp
            0x8B, 0xD1,                        //mov edx, ecx
            0x83, 0xC2, 0x3C,                  //add edx, 3Ch
            0xC7, 0x01, 0x00, 0x00, 0x00, 0x00,//mov [ecx], 0 <--- memset
            0x83, 0xC1, 0x04,                  //add ecx, 4h            ^
            0x3B, 0xCA,                        //cmp ecx, edx           |
            0x7E, 0xF3,                        //jle memset   -----------
            0xC6, 0x04, 0x24, 0x20,            //mov [esp], 20h
            0xB9, 0x00, 0x00, 0x00, 0x00,      //mov ecx, 0x00000000 //manifest char*
            0x89, 0x4C, 0x24, 0x08,            //mov [esp + 8], ecx
            0xB9, 0x00, 0x00, 0x00, 0x00,      //mov ecx, 0x00000000 //nFiles
            0x89, 0x4C, 0x24, 0x28,            //mov [esp + 28h], ecx
            0xB9, 0x00, 0x00, 0x00, 0x00,      //mov ecx, 0x00000000 //files char*
            0x89, 0x4C, 0x24, 0x2C,            //mov [esp + 2Ch], ecx
            0x54,                              //push esp
            0xE8, 0x00, 0x00, 0x00, 0x00,      //call CreateActCtxA
            0x83, 0x38, 0xFF,                  //cmp eax, -1
            0x0F, 0x84, 0x89, 0x00, 0x00, 0x00,//je finish -----------------------------------v
            0x89, 0x44, 0x24, 0x30,            //mov [esp + 30h], eax                         |
            0x8B, 0xCC,                        //mov ecx, esp                                 |
            0x83, 0xC1, 0x20,                  //add ecx, 20h                                 |
            0x51,                              //push ecx                                     |
            0x50,                              //push eax                                     |
            0xE8, 0x00, 0x00, 0x00, 0x00,      //call ActivateActCtxA                         |
            0x83, 0xF8, 0x00,                  //cmp eax, 0                                   |
            0x74, 0x6B,                        //je cleanupfinish -------------------------v  |
            0xC6, 0x44, 0x24, 0x24, 0x01,      //mov [esp + 24h], 1                        |  |
            0x8B, 0x4C, 0x24, 0x28,            //mov ecx, [esp + 28h] <--- loadloop <+     |  |
            0x83, 0xF9, 0x00,                  //cmp ecx, 0                          |     |  |
            0x7E, 0x3E,                        //jle endloop ------------------------|--v  |  |
            0x83, 0xE9, 0x01,                  //sub ecx, 1                          |  |  |  |
            0x89, 0x4C, 0x24, 0x28,            //mov [esp + 28h], ecx                |  |  |  |
            0x8B, 0x4C, 0x24, 0x24,            //mov ecx, [esp + 24h]                |  |  |  |
            0x83, 0xF9, 0x00,                  //cmp ecx, 0                          |  |  |  |
            0x74, 0x2E,                        //je endloop -------------------------|--v  |  |
            0xFF, 0x74, 0x24, 0x2C,            //push [esp + 2Ch]                    |  |  |  |
            0xE8, 0x00, 0x00, 0x00, 0x00,      //call GetModuleHandleA               |  |  |  |
            0x83, 0xF8, 0x00,                  //cmp eax, 0                          |  |  |  |
            0x75, 0x09,                        //jnz initnext -------------------+   |  |  |  |
            0xFF, 0x74, 0x24, 0x2C,            //push [esp + 2Ch]                |   |  |  |  |
            0xE8, 0x00, 0x00, 0x00, 0x00,      //call LoadLibraryA               v   |  |  |  |
            0x89, 0x44, 0x24, 0x24,            //mov [esp + 24h], eax <--- initnext  |  |  |  |
            0x8B, 0x4C, 0x24, 0x2C,            //mov ecx, [esp + 2Ch]                |  |  |  |
            0x8A, 0x01,                        //mov al, [ecx] <--- findnull         |  |  |  |
            0x83, 0xC1, 0x01,                  //add ecx, 1                 ^        |  |  |  |
            0x3C, 0x00,                        //cmp al, 0                  |        |  |  |  |
            0x75, 0xF7,                        //jnz findnull --------------+        |  |  |  |
            0x89, 0x4C, 0x24, 0x2C,            //mov [esp + 2Ch], ecx                |  |  |  |
            0xEB, 0xB9,                        //jmp loadloop -----------------------+  |  |  |
            0x8B, 0x44, 0x24, 0x24,            //mov eax, [esp + 24h]   <---endloop-----+  |  |
            0xB9, 0x01, 0x00, 0x00, 0x00,      //mov ecx, 1                                |  |
            0x23, 0xC1,                        //and eax, ecx                              |  |
            0x89, 0x4C, 0x24, 0x24,            //mov [esp + 24h], ecx                      |  |
            0x83, 0xF9, 0x00,                  //cmp ecx, 0                                |  |
            0x75, 0x14,                        //jne finish -------------------------------|--v
            0xFF, 0x74, 0x24, 0x20,            //push [esp + 20h]                          |  |
            0x6A, 0x00,                        //push 0                                    |  |
            0xE8, 0x00, 0x00, 0x00, 0x00,      //call DeactivateActCtx                     |  |
            0xFF, 0x74, 0x24, 0x30,            //push [esp + 30h] <--- cleanupfinish ------+  |
            0xE8, 0x00, 0x00, 0x00, 0x00,      //call ReleaseActCtx                           |
            0x8B, 0x44, 0x24, 0x24,            //mov eax, [esp + 24h] <--- finish <-----------+
            0x8B, 0xE5,                        //mov esp, ebp
            0x5D,                              //pop ebp
            0xC3				               //ret
        };
        #endregion
    }
}
