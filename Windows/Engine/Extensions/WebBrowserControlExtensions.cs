using System;
using System.Runtime.InteropServices;
using SHDocVw;

namespace Org.InCommon.InCert.Engine.Extensions
{
    public static class WebBrowserControlExtensions
    {
        internal static class NativeMethods
        {
            [ComImport, Guid("6D5140C1-7436-11CE-8034-00AA006009FA"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            internal interface IOleServiceProvider
            {
                [PreserveSig]
                int QueryService([In] ref Guid guidService, [In] ref Guid riid, [MarshalAs(UnmanagedType.IDispatch)] out object ppvObject);
            }

         
        }

        public static WebBrowser ToComInterface(this System.Windows.Controls.WebBrowser browser)
        {
            var sp = browser.Document as NativeMethods.IOleServiceProvider;
            if (sp == null) return null;

            var iidIWebBrowserApp = new Guid("0002DF05-0000-0000-C000-000000000046");
            var iidIWebBrowser2 = new Guid("D30C1661-CDAF-11d0-8A3E-00C04FC9E26E");

            object webBrowser;
            sp.QueryService(ref iidIWebBrowserApp, ref iidIWebBrowser2, out webBrowser);
            return webBrowser as  WebBrowser;
        }
    }
}
