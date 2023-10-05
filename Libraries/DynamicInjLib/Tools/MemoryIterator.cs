/*
    MemoryIterator.cs
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
using System.IO;
using System.Runtime.InteropServices;

namespace JLibrary.Tools
{
    /// <summary>A basic IO class to manage the reading/writing over a dynamic memory buffer</summary>
    [Serializable]
    public class MemoryIterator : ErrorBase, IDisposable
    {
        private MemoryStream _base;
        private UnmanagedBuffer _ubuffer;

        /// <summary>
        /// Initialize the class instance with a new data base.
        /// </summary>
        /// <param name="iterable">Data buffer to read/write over</param>
        public MemoryIterator(byte[] iterable)
        {
            if (iterable == null)
                throw new ArgumentException("Unable to iterate a null reference", "iterable");

            this._base = new MemoryStream(iterable, 0, iterable.Length, true);
            this._ubuffer = new UnmanagedBuffer(0x100);
        }

        /// <summary>
        /// Get the current state of the memory buffer
        /// </summary>
        /// <returns>byte array containing the raw data of the iterator</returns>
        protected byte[] GetUnderlyingData()
        {
            return this._base.ToArray();
        }

        /// <summary>
        /// Read a value-type of type TResult from the current position in the buffer
        /// </summary>
        /// <typeparam name="TResult">Value-type, must be compatible with <see cref="Marshal.SizeOf(Type)"/></typeparam>
        /// <param name="result">A prefined instance of type TResult which will hold the read data</param>
        /// <returns>true if the read was successful, false otherwise</returns>
        public bool Read<TResult>(out TResult result) where TResult : struct
        {
            return this.Read<TResult>(0, SeekOrigin.Current, out result);
        }

        /// <summary>
        /// Seek to a specified position in the buffer
        /// </summary>
        /// <param name="offset">The offset from 'param' of where to seek to</param>
        /// <param name="origin">The seek origin (Current/End/Beginning)</param>
        /// <returns></returns>
        public long Seek(long offset, SeekOrigin origin)
        {
            return this._base.Seek(offset, origin);
        }

        /// <summary>
        /// Read a value-type of type TResult from the specified position in the buffer
        /// </summary>
        /// <typeparam name="TResult">Value-type, must be compatible with <see cref="Marshal.SizeOf(Type)"/></typeparam>
        /// <param name="offset">The offset from 'param' of where to read from</param>
        /// <param name="origin">The seek origin (Current/End/Beginning)</param>
        /// <param name="result">A prefined instance of type TResult which will hold the read data</param>
        /// <returns>true if the read was successful, false otherwise</returns>
        public bool Read<TResult>(long offset, SeekOrigin origin, out TResult result) where TResult : struct
        {
            result = default(TResult);
            try
            {
                this._base.Seek(offset, origin);
                byte[] buffer = new byte[Marshal.SizeOf(typeof(TResult))];
                this._base.Read(buffer, 0, buffer.Length);

                if (this._ubuffer.Translate<TResult>(buffer, out result))
                    return true;
                else
                    throw this._ubuffer.GetLastError();
            }
            catch (Exception e)
            {
                return base.SetLastError(e);
            }
        }

        /// <summary>
        /// Read a string from the underlying buffer.
        /// </summary>
        /// <param name="offset">The offset from 'param' of where to begin reading from</param>
        /// <param name="origin">The seek origin (Current/End/Beginning)</param>
        /// <param name="lpBuffer">Output string to save the content to</param>
        /// <param name="len">
        /// The length of the string to read. This parameter is ignored if a null-terminating character is found before 'len' bytes have been read.
        /// If the underlying string is known to be of a certain length and contain multiple null-terminating characters, consider using a combination of <see cref="Read(long,SeekOrigin,out byte[])" /> and <see cref="Encoding.GetString(byte[])" />
        /// Specifying '-1' for this parameter will make the function determine the length of the string by looking for the first null-terminating byte in the string</param>
        /// <param name="stringEncoding">The encoding of the string to read. If null is specified, ASCII encoding will be used by default</param>
        /// <returns></returns>
        public bool ReadString(long offset, SeekOrigin origin, out string lpBuffer, int len = -1, Encoding stringEncoding = null)
        {
            lpBuffer = null;
            byte[] buffer = new byte[(len > 0 ? len : 64)];
            if (stringEncoding == null)
                stringEncoding = Encoding.ASCII;

            try
            {
                this._base.Seek(offset, origin);
                StringBuilder sb = new StringBuilder((len > 0 ? len : 260));
                int terminator = -1;
                int n = 0;
                int total = 0;

                while (terminator == -1 && (n = this._base.Read(buffer, 0, buffer.Length)) > 0)
                {
                    sb.Append(stringEncoding.GetString(buffer));
                    terminator = sb.ToString().IndexOf('\0', total);
                    total += n;
                    if (len > 0 && total >= len)
                        break;
                }

                if (terminator > -1)
                    lpBuffer = sb.ToString().Substring(0, terminator);
                else if (total >= len && len > 0)
                    lpBuffer = sb.ToString().Substring(0, len);
                return lpBuffer != null;
            }
            catch (Exception e)
            {
                return SetLastError(e);
            }
        }

        /// <summary>
        /// Basic read operation. Read data into a predefined buffer from the underlying buffer at a certain position
        /// </summary>
        /// <param name="offset">The offset from 'param' of where to begin reading from</param>
        /// <param name="origin">The seek origin (Current/End/Beginning)</param>
        /// <param name="buffer">Predefined buffer to hold the data</param>
        /// <returns>true if the read was successful, false otherwise</returns>
        /// <exception cref="ArgumentNullException">Thrown in the <see cref="buffer"/> parameter is null</exception>
        public bool Read(long offset, SeekOrigin origin, byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer", "Parameter cannot be null");

            try
            {
                this._base.Seek(offset, origin); ;
                this._base.Read(buffer, 0, buffer.Length);
            }
            catch (Exception e)
            {
                SetLastError(e);
                buffer = null;
            }
            return buffer != null;
        }

        /// <summary>Write a value-type to the underling buffer at a certain position.</summary>
        /// <typeparam name="TSource">Value-type, must be compatible with <see cref="Marshal.SizeOf(Type)"/></typeparam>
        /// <param name="offset">The offset from 'param' of where to write to</param>
        /// <param name="origin">The seek origin (Current/End/Beginning)</param>
        /// <param name="data">A prefined instance of type TSource which holds the data to write to memory</param>
        /// <returns>true if the write was successful, false otherwise</returns>
        public bool Write<TSource>(long offset, SeekOrigin origin, TSource data) where TSource : struct
        {
            try
            {
                this._base.Seek(offset, origin);
                byte[] buffer = null;
                if (this._ubuffer.Translate<TSource>(data, out buffer))
                {
                    this._base.Write(buffer, 0, buffer.Length);
                    return true;
                }
                else
                {
                    throw this._ubuffer.GetLastError();
                }
            }
            catch (Exception e)
            {
                return SetLastError(e);
            }
        }

        /// <summary>Write an array of bytes to the underlying buffer at a certain position</summary>
        /// <param name="offset">The offset from 'param' of where to begin writing to</param>
        /// <param name="origin">The seek origin (Current/End/Beginning)</param>
        /// <param name="buffer">Predefined buffer that holds the data to write</param>
        /// <returns>true if the write was successful, false otherwise</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <see cref="data"/> parameter is null</exception>
        public bool Write(long offset, SeekOrigin origin, byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("Parameter 'data' cannot be null");

            try
            {
                this._base.Seek(offset, origin);
                this._base.Write(data, 0, data.Length);
                return true;
            }
            catch (Exception e)
            {
                return SetLastError(e);
            }
        }

        #region IDisposable Implementation
        private bool _disposed;

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    this._ubuffer.Dispose();
                    this._base.Dispose();
                }
                this._disposed = true;
            }
        }
        #endregion
    }
}
