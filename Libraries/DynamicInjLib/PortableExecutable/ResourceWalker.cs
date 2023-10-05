/*
    ResourceWalker.cs
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

using System.Collections.Generic;
using System.Text;

namespace JLibrary.PortableExecutable
{
    /**
     * Helper class to walk a Portable Executable's resource table,
     * Very, very basic, probably not overly efficient either but it gets the job done.
     * This is probably the messiest and hardest-to-follow file in the solution
     * but the amount of documentation needed to properly explain the concept
     * is way out of proportion to the use of this class in the project so I haven't really
     * bothered. If you really want a good explanation about the Resource Table structure,
     * see Matt Pietrek's work, as well as the Microsoft PE/COFF specification (v8)
     */
    public class ResourceWalker
    {
        public ResourceDirectory Root { get; private set; }

        public ResourceWalker(PortableExecutable image)
        {
            IMAGE_DATA_DIRECTORY rsrcDir = image.NTHeader.OptionalHeader.DataDirectory[(int)DATA_DIRECTORIES.ResourceTable];
            IMAGE_RESOURCE_DIRECTORY rootDir;
            uint rootAddr = 0;

            if (rsrcDir.VirtualAddress > 0 && rsrcDir.Size > 0)
            {
                if (image.Read((rootAddr = image.GetPtrFromRVA(rsrcDir.VirtualAddress)), System.IO.SeekOrigin.Begin, out rootDir))
                    this.Root = new ResourceDirectory(image, new IMAGE_RESOURCE_DIRECTORY_ENTRY() { SubdirectoryRva = 0x80000000 }, false, rootAddr);
                else
                    throw image.GetLastError();
            }
        }

        public abstract class ResourceObject
        {
            private string _name;

            protected uint _root;
            protected PortableExecutable _owner;
            protected IMAGE_RESOURCE_DIRECTORY_ENTRY _entry;

            public string Name { get { return _name; } }
            public int Id { get {  return (this.IsNamedResource ? -1 : (int)this._entry.IntegerId); }}
            public bool IsNamedResource { get; protected set; }

            public ResourceObject(PortableExecutable owner, IMAGE_RESOURCE_DIRECTORY_ENTRY entry, bool named, uint root)
            {
                this._owner = owner;
                this._entry = entry;
                this.IsNamedResource = named;
                if (named)
                {
                    ushort len = 0;
                    if (owner.Read(root + (entry.NameRva & 0x7FFFFFFF), System.IO.SeekOrigin.Begin, out len))
                    {
                        byte[] unicodeBuffer = new byte[len << 1]; //each unicode character is 2 bytes wide
                        if (owner.Read(0, System.IO.SeekOrigin.Current, unicodeBuffer))
                            this._name = Encoding.Unicode.GetString(unicodeBuffer);
                    }

                    if (_name == null)
                        throw owner.GetLastError();
                }
                this._root = root;
            }
        }

        public class ResourceFile : ResourceObject
        {
            private IMAGE_RESOURCE_DATA_ENTRY _base;

            public ResourceFile(PortableExecutable owner, IMAGE_RESOURCE_DIRECTORY_ENTRY entry, bool named, uint root)
                : base(owner, entry, named, root)
            {
                if (!owner.Read(_root + entry.DataEntryRva, System.IO.SeekOrigin.Begin, out this._base))
                    throw owner.GetLastError();
            }

            public byte[] GetData()
            {
                byte[] buffer = new byte[_base.Size];
                if (!_owner.Read(_owner.GetPtrFromRVA(_base.OffsetToData), System.IO.SeekOrigin.Begin, buffer))
                    throw _owner.GetLastError();
                return buffer;
            }
        }

        public class ResourceDirectory : ResourceObject
        {
            private const uint SZ_ENTRY = 8;
            private const uint SZ_DIRECTORY = 16;
            private IMAGE_RESOURCE_DIRECTORY _base;

            private ResourceFile[] _files;
            private ResourceDirectory[] _dirs;

            public ResourceFile[] Files 
            {
                get
                {
                    if (_files == null)
                        Initialize();
                    return _files;
                }
            }

            public ResourceDirectory[] Directories
            {
                get
                {
                    if (_dirs == null)
                        Initialize();
                    return _dirs;
                }
            }

            private void Initialize()
            {
                IMAGE_RESOURCE_DIRECTORY_ENTRY curEnt;
                List<ResourceDirectory> directories = new List<ResourceDirectory>();
                List<ResourceFile> files = new List<ResourceFile>();

                int namedCount = _base.NumberOfNamedEntries;

                for (int i = 0; i < namedCount + _base.NumberOfIdEntries; i++)
                {
                    if (_owner.Read(_root + SZ_DIRECTORY + (_entry.SubdirectoryRva ^ 0x80000000) + (i * SZ_ENTRY), System.IO.SeekOrigin.Begin, out curEnt))
                    {
                        if ((curEnt.SubdirectoryRva & 0x80000000) != 0)
                            directories.Add(new ResourceDirectory(this._owner, curEnt, i < namedCount, _root));
                        else
                            files.Add(new ResourceFile(this._owner, curEnt, i < namedCount, _root));
                    }
                }
                this._files = files.ToArray();
                this._dirs = directories.ToArray();
            }

            public ResourceDirectory(PortableExecutable owner, IMAGE_RESOURCE_DIRECTORY_ENTRY entry, bool named, uint root)
                : base(owner, entry, named, root)
            {
                if (!owner.Read(root + (entry.SubdirectoryRva ^ 0x80000000), System.IO.SeekOrigin.Begin, out this._base))
                    throw owner.GetLastError();
            }
        }
    }
}
