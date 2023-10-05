using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SRO_INGAME.Common
{
    class ExternalDLL
    {
        internal const int GWL_EXSTYLE = -20;
        internal const int WS_EX_NOACTIVATE = 0x08000000;

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteObject([In] IntPtr hObject);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        Rectangle myRect = new Rectangle();

        public Rectangle SRDimensions()
        {
            RECT rct;

            IntPtr handle = ExternalDLL.GetForegroundWindow();
            if (!GetWindowRect(new HandleRef(this, GetSRIntPtr()), out rct))//Process.GetProcessById(SRCommon.SRClientID).Handle
            {
                Console.WriteLine("Error while reading the client dimensions handle is: " + GetSRIntPtr());
                return default;
            }

            myRect.X = rct.Left;
            myRect.Y = rct.Top;
            myRect.Width = rct.Right - rct.Left;
            myRect.Height = rct.Bottom - rct.Top;
            return myRect;
        }

        /// <summary>
        /// get IntPtr by its name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IntPtr GetIntPtr(string name)
        {
            Process[] processes = Process.GetProcessesByName(name);
            IntPtr windowHandle = default;

            foreach (Process p in processes)
            {
                windowHandle = p.MainWindowHandle;
            }

            return windowHandle;
        }

        /// <summary>
        /// get the current active window
        /// </summary>
        /// <returns></returns>
        public static bool isGameActive()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();
            //Console.WriteLine($"Handles are { GetForegroundWindow()} {GetSRIntPtr()}" );

            if (GetWindowText(handle, Buff, nChars) > 0 && handle == GetSRIntPtr() && Buff.ToString() == SRCommon.SRClientProcess.MainWindowTitle)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// get sro_client IntPtr
        /// </summary>
        /// <returns></returns>
        public static IntPtr GetSRIntPtr()
        {
            SRCommon.SRClientProcess.Refresh();
            if (SRCommon.SRClientProcess.MainWindowHandle != null)
                return SRCommon.SRClientProcess.MainWindowHandle;
            else
                Environment.Exit(0);// exit if the handle is not found maybe 
            return default;
        }

        /// <summary>
        /// Transform a bitmap image to ImageSource
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static ImageSource ImageSourceFromBitmap(System.Drawing.Bitmap bmp)
        {
            if (bmp != null)
            {
                var handle = bmp.GetHbitmap();
                try
                {
                    return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
                finally { DeleteObject(handle); }
            }
            return default;
        }
    }
}
