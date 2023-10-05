/*
    Definitions.cs
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
using System.Runtime.InteropServices;

namespace JLibrary.PortableExecutable
{
    //The indexing of the data directories. You'll love me later.
    public enum DATA_DIRECTORIES
    {
        ExportTable,
        ImportTable,
        ResourceTable,
        ExceptionTable,
        CertificateTable,
        BaseRelocTable,
        Debug,
        Architecture,
        GlobalPtr,
        TLSTable,
        LoadConfigTable,
        BoundImport,
        IAT,
        DelayImportDescriptor,
        CLRRuntimeHeader,
        Reserved
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1), Serializable]
    public struct IMAGE_DOS_HEADER
    {
        public UInt16 e_magic;              // Magic number //0
        public UInt16 e_cblp;               // Bytes on last page of file //2
        public UInt16 e_cp;                 // Pages in file //4
        public UInt16 e_crlc;               // Relocations //6
        public UInt16 e_cparhdr;            // Size of header in paragraphs //8
        public UInt16 e_minalloc;           // Minimum extra paragraphs needed //10
        public UInt16 e_maxalloc;           // Maximum extra paragraphs needed //12
        public UInt16 e_ss;                 // Initial (relative) SS value //14
        public UInt16 e_sp;                 // Initial SP value //16
        public UInt16 e_csum;               // Checksum //18
        public UInt16 e_ip;                 // Initial IP value //20
        public UInt16 e_cs;                 // Initial (relative) CS value //22
        public UInt16 e_lfarlc;             // File address of relocation table //24
        public UInt16 e_ovno;               // Overlay number //26
        public UInt16 e_res_0;              // Reserved words //28
        public UInt16 e_res_1;              // Reserved words //30
        public UInt16 e_res_2;              // Reserved words //32
        public UInt16 e_res_3;              // Reserved words //34
        public UInt16 e_oemid;              // OEM identifier (for e_oeminfo) //36
        public UInt16 e_oeminfo;            // OEM information; e_oemid specific //38
        public UInt16 e_res2_0;             // Reserved words //40
        public UInt16 e_res2_1;             // Reserved words //42
        public UInt16 e_res2_2;             // Reserved words //44
        public UInt16 e_res2_3;             // Reserved words //46
        public UInt16 e_res2_4;             // Reserved words //48
        public UInt16 e_res2_5;             // Reserved words //50
        public UInt16 e_res2_6;             // Reserved words //52
        public UInt16 e_res2_7;             // Reserved words //54
        public UInt16 e_res2_8;             // Reserved words //56
        public UInt16 e_res2_9;             // Reserved words //58
        public UInt32 e_lfanew;             // File address of new exe header // 60
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1), Serializable]
    public struct IMAGE_FILE_HEADER
    {
        public UInt16 Machine; //0
        public UInt16 NumberOfSections; //2
        public UInt32 TimeDateStamp; //4
        public UInt32 PointerToSymbolTable; //8
        public UInt32 NumberOfSymbols; //12
        public UInt16 SizeOfOptionalHeader; //16
        public UInt16 Characteristics; //
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1), Serializable]
    public struct IMAGE_OPTIONAL_HEADER32
    {
        public UInt16 Magic; //0
        public Byte MajorLinkerVersion; //2
        public Byte MinorLinkerVersion; //3
        public UInt32 SizeOfCode; //4
        public UInt32 SizeOfInitializedData; //8
        public UInt32 SizeOfUninitializedData; //12
        public UInt32 AddressOfEntryPoint; //16
        public UInt32 BaseOfCode; //20
        public UInt32 BaseOfData; //24
        public UInt32 ImageBase; //28
        public UInt32 SectionAlignment; //32
        public UInt32 FileAlignment; //36
        public UInt16 MajorOperatingSystemVersion; //40
        public UInt16 MinorOperatingSystemVersion; //42
        public UInt16 MajorImageVersion; //44
        public UInt16 MinorImageVersion; //46
        public UInt16 MajorSubsystemVersion; //48
        public UInt16 MinorSubsystemVersion; //50
        public UInt32 Win32VersionValue; //52
        public UInt32 SizeOfImage; //56
        public UInt32 SizeOfHeaders; //60
        public UInt32 CheckSum; //64
        public UInt16 Subsystem; //68
        public UInt16 DllCharacteristics; //70
        public UInt32 SizeOfStackReserve; //72
        public UInt32 SizeOfStackCommit; //76
        public UInt32 SizeOfHeapReserve; //80
        public UInt32 SizeOfHeapCommit; //84
        public UInt32 LoaderFlags; //88
        public UInt32 NumberOfRvaAndSizes; //92
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)] //96
        public IMAGE_DATA_DIRECTORY[] DataDirectory;
    };

    [StructLayout(LayoutKind.Sequential), Serializable]
    public struct IMAGE_DATA_DIRECTORY
    {
        public uint VirtualAddress;
        public uint Size;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1), Serializable]
    public struct IMAGE_NT_HEADER32
    {
        public int Signature;
        public IMAGE_FILE_HEADER FileHeader;
        public IMAGE_OPTIONAL_HEADER32 OptionalHeader;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1), Serializable]
    public struct IMAGE_BASE_RELOCATION
    {
        public uint VirtualAddress;
        public uint SizeOfBlock;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1), Serializable]
    public struct IMAGE_SECTION_HEADER
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] Name; // 0, 8 UTF-8 Encoded string of size 8 bytes and null terminated.
        public UInt32 VirtualSize; //8, 4
        public UInt32 VirtualAddress; //12, 4
        public UInt32 SizeOfRawData; //16, 4
        public UInt32 PointerToRawData; //20, 4
        public UInt32 PointerToRelocations; //24, 4
        public UInt32 PointerToLineNumbers; //28 , 4
        public UInt16 NumberOfRelocations; //32, 2
        public UInt16 NumberOfLineNumbers; //34, 2
        public UInt32 Characteristics; //36, 4

        public override string ToString()
        {
            string temp = System.Text.Encoding.UTF8.GetString(Name);
            if (temp.Contains("\0")) temp = temp.Substring(0, temp.IndexOf("\0"));
            return temp;
        }
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1), Serializable]
    public struct IMAGE_IMPORT_DESCRIPTOR
    {
        public uint OriginalFirstThunk;
        public uint TimeDateStamp;
        public uint ForwarderChain;
        public uint Name;
        public uint FirstThunkPtr;
    }

    [StructLayout(LayoutKind.Explicit), Serializable]
    public struct U1
    {
        [FieldOffset(0)]
        public uint ForwarderString;
        [FieldOffset(0)]
        public uint Function;
        [FieldOffset(0)]
        public uint Ordinal;
        [FieldOffset(0)]
        public uint AddressOfData;
    };

    [StructLayout(LayoutKind.Sequential), Serializable]
    public struct IMAGE_THUNK_DATA
    {
        public U1 u1;
    };

    [StructLayout(LayoutKind.Sequential), Serializable]
    public struct IMAGE_RESOURCE_DIRECTORY
    {
        public uint Characteristics;
        public uint TimeDateStamp;
        public ushort MajorVersion;
        public ushort MinorVersion;
        public ushort NumberOfNamedEntries;
        public ushort NumberOfIdEntries;
    };

    [StructLayout(LayoutKind.Explicit), Serializable]
    public struct IMAGE_RESOURCE_DIRECTORY_ENTRY
    {
        [FieldOffset(0)]
        public uint NameRva;
        [FieldOffset(0)]
        public uint IntegerId;
        [FieldOffset(4)]
        public uint DataEntryRva;
        [FieldOffset(4)]
        public uint SubdirectoryRva;
    };

    [StructLayout(LayoutKind.Sequential), Serializable]
    public struct IMAGE_RESOURCE_DATA_ENTRY
    {
        public uint OffsetToData;
        public uint Size;
        public uint CodePage;
        public uint Reserved;
    };
}
