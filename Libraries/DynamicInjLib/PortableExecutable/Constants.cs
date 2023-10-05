/*
    Constants.cs
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

namespace JLibrary.PortableExecutable
{
    /**
     * Some of the more common constants to be found
     * in a portable executable image. I honestly 
     * couldn't be bothered building a comprehensive
     * list, so I really only added the ones I was going to
     * be using. Someone else can provide the rest if they
     * are that way inclined.
     */
    public static class Constants
    {
        public const ushort DOS_SIGNATURE = 0x5A4D;
        public const uint NT_SIGNATURE = 0x4550;
        public const ushort PE32_FORMAT = 0x10B; //PE32
        public const ushort PE32P_FORMAT = 0x20B; //PE32+
        public const uint RT_MANIFEST = 0x18;
        public const uint CREATEPROCESS_MANIFEST_RESOURCE_ID = 0x01;
        public const uint ISOLATIONAWARE_MANIFEST_RESOURCE_ID = 0x02;
        public const uint ISOLATIONAWARE_NOSTATICIMPORT_MANIFEST_RESOURCE_ID = 0x03;
    }
}
