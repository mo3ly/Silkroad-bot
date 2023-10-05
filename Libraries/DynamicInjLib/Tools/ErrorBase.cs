/*
    ErrorBase.cs
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

namespace JLibrary.Tools
{
    /// <summary>
    /// Simple error reporting, reminiscent of the WinAPI error scheme
    /// </summary>
    [Serializable]
    public abstract class ErrorBase
    {
        // last error to occur within the class
        protected Exception _lasterror = null;

        /// <summary>Gets the last error encountered by the class.</summary>
        /// <returns>An Exception containing information about the last error, or null if no error has been encountered since construction or the last call to ClearErrors()</returns>
        public virtual Exception GetLastError()
        {
            return this._lasterror;
        }

        /// <summary>Clear any errors encountered by the class</summary>
        public virtual void ClearErrors()
        {
            this._lasterror = null;
        }

        /// <summary>Updates the last error encountered by the class</summary>
        /// <param name="e">Exception containing pertinent information about the last error</param>
        /// <returns>false, always. The return value is purely for syntactical sugar, allowing functions to both return a negative result and set an error in a single line</returns>
        protected virtual bool SetLastError(Exception e)
        {
            this._lasterror = e;
            return false;
        }

        /// <summary>Set the last error with a simple error message</summary>
        /// <param name="message">Information about the last error</param>
        /// <returns>false, always. <see cref="SetLastError(Exception)"/></returns>
        protected virtual bool SetLastError(string message)
        {
            return this.SetLastError(new Exception(message));
        }
    }
}
