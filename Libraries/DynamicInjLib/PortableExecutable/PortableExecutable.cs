/*
    PortableExecutable.cs
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
using System.IO;
using JLibrary.Tools;

namespace JLibrary.PortableExecutable
{
    [Serializable]
    public class PortableExecutable :  MemoryIterator
    {
        public IMAGE_NT_HEADER32 NTHeader { get; private set; }
        public IMAGE_DOS_HEADER DOSHeader { get; private set; }
        public string FileLocation { get; private set; }

        public PortableExecutable(string path) 
            : this(File.ReadAllBytes(path))
        {
            this.FileLocation = path;
        }

        /// <summary>Construct a new instance of the PortableExecutable class using a raw data buffer</summary>
        /// <param name="data">An array of bytes containing a Portable Executable Image file</param>
        public PortableExecutable(byte[] data) 
            : base(data)
        {
            string error = string.Empty;
            IMAGE_NT_HEADER32 tempNtHd = default(IMAGE_NT_HEADER32);
            IMAGE_DOS_HEADER tempDosHd = default(IMAGE_DOS_HEADER);

            if (Read(out tempDosHd) && tempDosHd.e_magic == Constants.DOS_SIGNATURE) //first check, DOS header ('MZ')
            {
                if (Read(tempDosHd.e_lfanew, System.IO.SeekOrigin.Begin, out tempNtHd) && tempNtHd.Signature == Constants.NT_SIGNATURE) //Next, the NT header ("PE\0\0" sig)
                {
                    if (tempNtHd.OptionalHeader.Magic == Constants.PE32_FORMAT) //check to make sure only x86 images are allowed.
                    {
                        if (tempNtHd.OptionalHeader.DataDirectory[(int)DATA_DIRECTORIES.CLRRuntimeHeader].Size > 0) //check for a CLR header, .NET dependencies require the .NET framework to be loaded, and cbf doing that.
                            error = "Image contains a CLR runtime header. Currently only native binaries are supported; no .NET dependent libraries.";
                    }
                    else { error = "File is of the PE32+ format. Currently support only extends to PE32 images. Either recompile the binary as x86, or choose a different target."; }
                }
                else { error = "Invalid NT header found in image."; }
            }
            else { error = "Invalid DOS Header found in image"; }

            if (string.IsNullOrEmpty(error))
            {
                this.NTHeader = tempNtHd;
                this.DOSHeader = tempDosHd;
            }
            else
            {
                Dispose();
                throw new ArgumentException(error);
            }
        }

        /// <summary>Enumerate all of the current PortableExecutable's section headers</summary>
        /// <returns>An IEnumerable of IMAGE_SECTION_HEADER structures, each representing one of the headers in the PE image</returns>
        public IEnumerable<IMAGE_SECTION_HEADER> EnumSectionHeaders()
        {
            IMAGE_SECTION_HEADER pSecHd;
            uint nSecs = this.NTHeader.FileHeader.NumberOfSections;
            long pSections = this.NTHeader.FileHeader.SizeOfOptionalHeader + typeof(IMAGE_FILE_HEADER).SizeOf() + sizeof(uint) + this.DOSHeader.e_lfanew;
            uint szSection = typeof(IMAGE_SECTION_HEADER).SizeOf();

            for (uint i = 0; i < nSecs; i++)
            {
                if (base.Read(pSections + (i * szSection), SeekOrigin.Begin, out pSecHd))
                    yield return pSecHd;
            }
        }

        /// <summary>Enumerate the import descriptors of the current PortableExecutable</summary>
        /// <returns>An IEnumerable of IMAGE_IMPORT_DESCRIPTOR structures, each of which represents a single module to import</returns>
        public IEnumerable<IMAGE_IMPORT_DESCRIPTOR> EnumImports()
        {
            IMAGE_DATA_DIRECTORY impDir = this.NTHeader.OptionalHeader.DataDirectory[(int)DATA_DIRECTORIES.ImportTable];
            if (impDir.Size > 0)
            {
                uint pDesc = GetPtrFromRVA(impDir.VirtualAddress);
                uint szDesc = typeof(IMAGE_IMPORT_DESCRIPTOR).SizeOf();
                IMAGE_IMPORT_DESCRIPTOR desc;
                while (base.Read(pDesc, SeekOrigin.Begin, out desc) && desc.OriginalFirstThunk > 0 && desc.Name > 0)
                {
                    yield return desc;
                    pDesc += szDesc;
                }
            }
        }

        /// <summary>Get the raw underlying image data of the current PortableExecutable</summary>
        /// <returns>A byte array containing the contents of the current image</returns>
        public byte[] ToArray()
        {
            return base.GetUnderlyingData();
        }

        private IMAGE_SECTION_HEADER GetEnclosingSectionHeader(uint rva)
        {
            // Find which section encloses a particular RVA.
            foreach (var pSecHd in EnumSectionHeaders())
                if (rva >= pSecHd.VirtualAddress && (rva < (pSecHd.VirtualAddress + (pSecHd.VirtualSize > 0 ? pSecHd.VirtualSize : pSecHd.SizeOfRawData))))
                    return pSecHd;
            throw new EntryPointNotFoundException("RVA does not exist within any of the current sections.");
        }

        /// <summary>Get the file-address of an RVA (Relative Virtual Address). This function is only needed when dealing with unmapped images</summary>
        /// <param name="rva">RVA to get a file-address for</param>
        /// <returns>The appropriate file address that corresponds to the RVA, or throws an EntryPointException if no sections contain this RVA</returns>
        public uint GetPtrFromRVA(uint rva)
        {
            // Essentially, this function is to account for the differing alignments between files and memory pages.
            // File alignment is usually around 512, and page alignment about 4K (4096). This means that any RVA values
            // we obtain while reading the disk-image will not necessarily point to valid locations within the file.
            // fortunately, this difference is easily accounted for thanks to Matt Pietrek's genius. RawData pointers are
            // file aligned, and virtual-addresses are memory page aligned. Simply find the enclosing section and the delta 
            // between files/memory and you've got a valid file-pointer.
            IMAGE_SECTION_HEADER pSecHd = GetEnclosingSectionHeader(rva);
            return (rva - (pSecHd.VirtualAddress - pSecHd.PointerToRawData));
        }
    }
}
