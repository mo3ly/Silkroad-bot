/*
    UnmanagedBuffer.cs
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

namespace JLibrary.Tools
{
    /*
     * Essentially this class is designed to provide a flexible and 
     * easily maintainted buffer of unmanaged memory.
     * It will resize as needed for various writing operations, and
     * when explicitly requested by the user. The class itself consists
     * of a combination of Read/Write/Delete operations with Generics
     * support to make it as easy as possible to read/write between managed
     * and unmanaged memory.
     */
    [Serializable]
    public class UnmanagedBuffer : ErrorBase, IDisposable
    {
        public IntPtr Pointer { get; private set; }
        public int Size { get; private set; }

        public UnmanagedBuffer(int cbneeded)
        {
            if (cbneeded > 0)
            {
                this.Pointer = Marshal.AllocHGlobal(cbneeded);
                this.Size = cbneeded;
            }
            else
            {
                this.Pointer = IntPtr.Zero;
                this.Size = 0;
            }
        }

        public bool Commit(byte[] data, int index, int count)
        {
            if (data != null && this.Alloc(count))
            {
                Marshal.Copy(data, index, this.Pointer, count);
                return true;
            }
            else
            {
                if (data == null)
                    SetLastError(new ArgumentException("Attempting to commit a null reference", "data"));
                return false;
            }
        }

        public bool Commit<T>(T data) where T : struct
        {
            try
            {
                if (this.Alloc(Marshal.SizeOf(typeof(T))))
                {
                    // You may note the fact that fDeleteOld is set to false here, despite the fact that Microsoft recommends 
                    // this to be set to true, due to the generic nature of this class, absently setting it to true every time 
                    // could break more stuff than it will help (as DestroyStructure is called with the current type of <T>, if 
                    // the last commit wasn't the same struct type, there's no point calling DestroyStructure. If you're allocating 
                    // referential structs, call SafeDecommit<T> to deallocate them safetly.
                    Marshal.StructureToPtr(data, this.Pointer, false);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return SetLastError(e);
            }
        }

        public bool SafeDecommit<T>() where T : struct
        {
            try
            {
                if (this.Size >= Marshal.SizeOf(typeof(T)))
                {
                    Marshal.DestroyStructure(this.Pointer, typeof(T));
                    return true;
                }
                else
                {
                    throw new InvalidCastException("Not enough unmanaged memory is allocated to contain this structure type.");
                }
            }
            catch (Exception e)
            {
                return SetLastError(e);
            }
        }

        public bool Read<TResult>(out TResult data) where TResult : struct
        {
            data = default(TResult);
            try
            {
                if (this.Size >= Marshal.SizeOf(typeof(TResult)))
                {
                    data = (TResult)Marshal.PtrToStructure(this.Pointer, typeof(TResult));
                    return true;
                }
                else
                {
                    throw new InvalidCastException("Not enough unmanaged memory is allocated to contain this structure type.");
                }
            }
            catch (Exception e)
            {
                return SetLastError(e);
            }
        }

        public byte[] Read(int count)
        {
            try
            {
                if (count <= this.Size && count > 0)
                {
                    byte[] buffer = new byte[count];
                    Marshal.Copy(this.Pointer, buffer, 0, count);
                    return buffer;
                }
                else
                {
                    throw new ArgumentException("There is either not enough memory allocated to read 'count' bytes, or 'count' is negative (" + count.ToString() + ")", "count");
                }
            }
            catch (Exception e)
            {
                SetLastError(e);
                return null;
            }
        }

        public bool Translate<TSource>(TSource data, out byte[] buffer) 
            where TSource : struct
        {
            buffer = null;
            if (this.Commit<TSource>(data))
            {
                buffer = this.Read(Marshal.SizeOf(typeof(TSource)));
                this.SafeDecommit<TSource>();
            }
            return buffer != null;
        }

        public bool Translate<TResult>(byte[] buffer, out TResult result) 
            where TResult : struct
        {
            result = default(TResult);
            if (buffer == null)
            {
                return SetLastError(new ArgumentException("Attempted to translate a null reference to a structure.", "buffer"));
            }

            return (this.Commit(buffer, 0, buffer.Length) && this.Read<TResult>(out result));
        }

        public bool Translate<TSource, TResult>(TSource data, out TResult result)
            where TSource : struct
            where TResult : struct
        {
            result = default(TResult);
            return (this.Commit<TSource>(data) && this.Read<TResult>(out result) && this.SafeDecommit<TSource>());
        }

        public bool Resize(int size)
        {
            if (size < 0)
            {
                return SetLastError( new ArgumentException("Attempting to resize to less than zero bytes of memory", "size") );
            }
            else if (size == this.Size)
            {
                return true; //already have this much memory allocated, no point wasting resources.
            }
            else if (size > this.Size)
            {
                //increasing memory, already have internal method to handle this.
                return this.Alloc(size);
            }
            else //downsize, possible free?
            {
                try
                {
                    if (size == 0) //Resizing to zero, essentially a free.
                    {
                        Marshal.FreeHGlobal(this.Pointer);
                        this.Pointer = IntPtr.Zero;
                    }
                    else if (size > 0) //just downsizing the memory. May come after a particularly large "commit" call.
                    {
                        this.Pointer = Marshal.ReAllocHGlobal(this.Pointer, new IntPtr(size));
                    }
                    this.Size = size; //reflect size change.
                    return true;
                }
                catch (Exception e)
                {
                    return SetLastError(e);
                }
            }
        }

        private bool Alloc(int cb)
        {
            try
            {
                if (cb > this.Size)
                {
                    this.Pointer = this.Pointer == IntPtr.Zero ? Marshal.AllocHGlobal(cb) : Marshal.ReAllocHGlobal(this.Pointer, new IntPtr(cb));
                    this.Size = cb;
                }
                return true;
            }
            catch (Exception e)
            {
                return SetLastError(e);
            }
        }

        #region IDisposable Implementation
        private bool _disposed = false;

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    this.Resize(0);
                }
                this._disposed = true;
            }
        }
        #endregion
    }
}
