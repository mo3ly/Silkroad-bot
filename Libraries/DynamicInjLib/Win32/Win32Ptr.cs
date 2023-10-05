/*
    Win32Ptr.cs
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

/* .NET 2.0 extension sorcery :3 */
namespace System.Runtime.CompilerServices
{

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ExtensionAttribute : Attribute
    {
        public ExtensionAttribute() { }
    }

}

namespace JLibrary.Win32
{
    /// <summary>
    /// Seevice class to distribute 32-bit pointers. .NETs internal
    /// IntPtr structure may refer to either 32 or 64 bit pointers, 
    /// this class simply normalizes all pointers to guarantee 32-bitness
    /// </summary>
    public static class Win32Ptr
    {
        public static IntPtr Create(long value)
        {
            return new IntPtr((int)value);
        }

        public static IntPtr Add(this IntPtr ptr, long val)
        {
            return new IntPtr((int)(ptr.ToInt32() + val));
        }

        public static IntPtr Add(this IntPtr ptr, IntPtr val)
        {
            return new IntPtr((int)(ptr.ToInt32() + val.ToInt32()));
        }

        public static IntPtr Subtract(this IntPtr ptr, long val)
        {
            return new IntPtr((int)(ptr.ToInt64() - val));
        }

        public static IntPtr Subtract(this IntPtr ptr, IntPtr val)
        {
            return new IntPtr((int)(ptr.ToInt64() - val.ToInt64()));
        }

        public static bool IsNull(this IntPtr ptr)
        {
            return ptr == IntPtr.Zero;
        }

        public static bool IsNull(this UIntPtr ptr)
        {
            return ptr == UIntPtr.Zero;
        }

        public static bool Compare(this IntPtr ptr, long value)
        {
            return (ptr.ToInt64() == value);
        }
    }
}
