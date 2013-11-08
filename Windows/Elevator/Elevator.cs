using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Org.InCommon.InCert.Elevator
{
    class Elevator
    {
        
        private static class NativeMethods
        {
            [return: MarshalAs(UnmanagedType.Bool)]
            [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
            internal static extern bool SetForegroundWindow(IntPtr hwnd);
        }
        
        [STAThread]
        static int Main(string[] args)
        {
            try
            {
                var path = GetEnginePath();
                if (!File.Exists(path))
                    return -1;

                var info = new ProcessStartInfo(path) {Arguments = String.Join(" ", args), WindowStyle = ProcessWindowStyle.Normal, UseShellExecute = true, Verb = "RunAs"};
                var process = Process.Start(info);

                if (process.MainWindowHandle !=IntPtr.Zero)
                    NativeMethods.SetForegroundWindow(process.MainWindowHandle);

                return 0;
            }
            catch
            {
                return -1;
            }
        }
        
        private static string GetEnginePath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "engine.exe");
        }
        
    }
}

