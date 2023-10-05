/*
    InjectionMethod.cs
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
using JLibrary.PortableExecutable;
using JLibrary.Tools;
using JLibrary.Win32;

namespace InjectionLibrary
{
    /*
      Core class that every injection method must conform to. Also acts
      as a factory class to avoid exposing each different injection method
      publically.
     */
    /// <summary>
    /// An DLL-injection method capable of executing user code in a remote process.
    /// </summary>
    public abstract class InjectionMethod : ErrorBase
    {
        // Maintain knowledge about the current injection method.
        public InjectionMethodType Type { get; protected set; }

        /// <summary>
        /// Inject a file into a process using a valid existing process handle
        /// </summary>
        /// <param name="dllPath">Path to the file to inject</param>
        /// <param name="hProcess">Handle to the remote process</param>
        /// <returns>A valid module handle if the function is successful, or IntPtr.Zero otherwise</returns>
        public abstract IntPtr Inject(string dllPath, IntPtr hProcess);

        /// <summary>
        /// Inject a file into a process using a unique process id as an identifier
        /// </summary>
        /// <param name="dllPath">Path to the file to inject</param>
        /// <param name="processId">Unique process identifier</param>
        /// <returns>A valid module handle if the function is successful, or IntPtr.Zero otherwise</returns>
        public virtual IntPtr Inject(string dllPath, int processId)
        {
            ClearErrors();
            IntPtr hProcess = WinAPI.OpenProcess(0x043A, false, processId);
            IntPtr hModule = Inject(dllPath, hProcess);
            WinAPI.CloseHandle(hProcess);
            return hModule;
        }

        /// <summary>
        /// Inject a collection of files into a process using a valid existing process handle
        /// </summary>
        /// <param name="dllPaths">An array listing which files to inject</param>
        /// <param name="hProcess">Handle to the remote process</param>
        /// <returns>An array of the same length as the 'dllPaths' parameter containing the module handles for each file</returns>
        public abstract IntPtr[] InjectAll(string[] dllPaths, IntPtr hProcess);

        /// <summary>
        /// Inject a collection of files into a process using a unique process id as an identifier
        /// </summary>
        /// <param name="dllPaths">An array listing which files to inject</param>
        /// <param name="processId">Unique process identifier</param>
        /// <returns>An array of the same length as the 'dllPaths' parameter containing the module handles for each file</returns>
        public virtual IntPtr[] InjectAll(string[] dllPaths, int processId)
        {
            ClearErrors();
            IntPtr hProcess = WinAPI.OpenProcess(0x043A, false, processId);
            IntPtr[] hModules = InjectAll(dllPaths, hProcess);
            WinAPI.CloseHandle(hProcess);
            return hModules;
        }

        /// <summary>
        /// Inject an existing in-memory PortableExecutable image into a process using a valid existing process handle
        /// </summary>
        /// <param name="image">Any valid existing PortableExecutable instance</param>
        /// <param name="hProcess">Handle to the remote process</param>
        /// <returns>A valid module handle if the function is successful, or IntPtr.Zero otherwise</returns>
        public abstract IntPtr Inject(PortableExecutable image, IntPtr hProcess);

        /// <summary>
        /// Inject an existing in-memory PortableExecutable image into a process using a unique process id as an identifier
        /// </summary>
        /// <param name="image">Any valid existing PortableExecutable instance</param>
        /// <param name="processId">Unique process identifier</param>
        /// <returns>A valid module handle if the function is successful, or IntPtr.Zero otherwise</returns>
        public virtual IntPtr Inject(PortableExecutable image, int processId)
        {
            ClearErrors();
            IntPtr hProcess = WinAPI.OpenProcess(0x043A, false, processId);
            IntPtr hModule = Inject(image, hProcess);
            WinAPI.CloseHandle(hProcess);
            return hModule;
        }

        /// <summary>
        /// Inject a collection of PortableExecutable images into a process using a valid existing process handle
        /// </summary>
        /// <param name="images">An array listing which PortableExecutable instances to inject</param>
        /// <param name="hProcess">Handle to the remote process</param>
        /// <returns>An array of the same length as the 'images' parameter containing the module handles for each image</returns>
        public abstract IntPtr[] InjectAll(PortableExecutable[] images, IntPtr hProcess);

        /// <summary>
        /// Inject a collection of PortableExecutable images into a process using a unique process id as an identifier
        /// </summary>
        /// <param name="images">An array listing which PortableExecutable instances to inject</param>
        /// <param name="processId">Unique process identifier</param>
        /// <returns>An array of the same length as the 'images' parameter containing the module handles for each image, or null if the method failed. Call <see cref="GetLastError"/> for more information</returns>
        public virtual IntPtr[] InjectAll(PortableExecutable[] images, int processId)
        {
            ClearErrors();
            IntPtr hProcess = WinAPI.OpenProcess(0x043A, false, processId);
            IntPtr[] hModules = InjectAll(images, hProcess);
            WinAPI.CloseHandle(hProcess);
            return hModules;
        }

        /// <summary>
        /// Attempt to unload a module from a process using a valid existing process handle
        /// </summary>
        /// <param name="hModule">Handle the module to unload (Base Address)</param>
        /// <param name="hProcess">Handle to the remote process</param>
        /// <returns>true if the module was successfully unloaded, false otherwise</returns>
        public abstract bool Unload(IntPtr hModule, IntPtr hProcess);

        /// <summary>
        /// Attempt to unload a module from a process using a unique process id as an identifier
        /// </summary>
        /// <param name="hModule">Handle the module to unload (Base Address)</param>
        /// <param name="processId">Unique process identifier</param>
        /// <returns>true if the module was successfully unloaded, false otherwise</returns>
        public virtual bool Unload(IntPtr hModule, int processId)
        {
            ClearErrors();
            IntPtr hProcess = WinAPI.OpenProcess(0x043A, false, processId);
            bool unloaded = Unload(hModule, hProcess);
            WinAPI.CloseHandle(hProcess);
            return unloaded;
        }

        /// <summary>
        /// Attempt to unload a collection of modules from the remote process
        /// </summary>
        /// <param name="hModules">An array containing the handles (base address) of the modules to unload</param>
        /// <param name="hProcess">Handle to the remote process</param>
        /// <returns>An array of the same length as the 'hModules' parameter containing the result for each <seealso cref="Unload(IntPtr,IntrPtr)"/>, or null if an error occured. Call <see cref="GetLastError"/> for more information</returns>
        public abstract bool[] UnloadAll(IntPtr[] hModules, IntPtr hProcess);

        /// <summary>
        /// Attempt to unload a collection of modules from the remote process
        /// </summary>
        /// <param name="hModules">An array containing the handles (base address) of the modules to unload</param>
        /// <param name="processId">Unique process identifier</param>
        /// <returns>An array of the same length as the 'hModules' parameter containing the result for each <seealso cref="Unload(IntPtr,IntrPtr)"/>, or null if an error occured. Call <see cref="GetLastError"/> for more information</returns>
        public virtual bool[] UnloadAll(IntPtr[] hModules, int processId)
        {
            ClearErrors();
            IntPtr hProcess = WinAPI.OpenProcess(0x043A, false, processId);
            bool[] unloaded = UnloadAll(hModules, hProcess);
            WinAPI.CloseHandle(hProcess);
            return unloaded;
        }

        /// <summary>
        /// Create a new InjectionMethod based on the specified MethodType
        /// </summary>
        /// <param name="type">Type of injection method to be created</param>
        /// <returns>A valid InjectionMethod instance if the 'type' parameter was valid, null otherwise</returns>
        public static InjectionMethod Create(InjectionMethodType type)
        {
            /** FACTORY CREATION **/
            // So that I never have to expose individual classes to the library user.
            // the Create method will serve up the most appropriate InjectionMethod implementation,
            // and this makes it easier to hand out new versions of the library without worrying about
            // changing internal class names or adding new classes, etc.
            InjectionMethod method;
            switch (type)
            {
                case InjectionMethodType.ManualMap:
                    method = new ManualMap(); break;
                case InjectionMethodType.Standard:
                    method = new CRTInjection(); break;
                case InjectionMethodType.ThreadHijack:
                    method = new ThreadHijack(); break;
                default:
                    return null;
            }
            if (method != null)
                method.Type = type;
            return method;
        }
    }

    public enum InjectionMethodType
    {
        Standard,
        ThreadHijack,
        ManualMap
    };
}
