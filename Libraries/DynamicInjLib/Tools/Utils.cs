/*
    Utils.cs
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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;

namespace JLibrary.Tools
{
    public static class Utils
    {
        /// <summary>
        /// Attempts to write some data to a temp file on disk.
        /// </summary>
        /// <param name="data">Data to write to disk</param>
        /// <returns>The path to the temporary disk location if successful, null otherwise.</returns>
        /// <exception cref="ArgumentNullException(string)">The 'data' parameter is null</exception>
        /// <remarks>
        /// First, the function attempts to obtain a temp file name
        /// from the system, but if that fails a randomly-named file
        /// will be created in the same folder as the application
        /// </remarks>
        public static string WriteTempData(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            string path = null;
            try
            {
                path = Path.GetTempFileName();
            }
            catch (IOException)
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), Path.GetRandomFileName());
            }

            try { File.WriteAllBytes(path, data); }
            catch { path = null; }

            return path;
        }

        /// <summary>
        /// Deep clone a managed object to create an exact replica.
        /// </summary>
        /// <typeparam name="T">A formattable type (must be compatible with a BinaryFormatter)</typeparam>
        /// <param name="obj">The object to clone</param>
        /// <returns>A clone of the Object 'obj'.</returns>
        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }

        public static uint SizeOf(this Type t)
        {
            //I'm super lazy, and I hate looking at ugly casts everywhere.
            return (uint)Marshal.SizeOf(t);
        }
    }
}
