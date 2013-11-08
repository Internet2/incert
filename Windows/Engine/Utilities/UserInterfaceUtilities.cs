using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Logging;
using log4net;

namespace Org.InCommon.InCert.Engine.Utilities
{
    public static class UserInterfaceUtilities
    {
        /// <summary>
        /// Specifies options for the size of the stock icon.
        /// </summary>
        public enum StockIconSize
        {
            /// <summary>
            /// Retrieve the small version of the icon, as specified by SM_CXSMICON and SM_CYSMICON system metrics.
            /// </summary>
            Small,

            /// <summary>
            /// Retrieve the large version of the icon, as specified by SM_CXICON and SM_CYICON system metrics.
            /// </summary>
            Large,

            /// <summary>
            /// Retrieve the shell-sized icons (instead of the size specified by the system metrics).
            /// </summary>
            ShellSize,
        }

        /// <summary>
        /// Provides values used to specify which standard icon to retrieve. 
        /// </summary>
        public enum StockIconIdentifier
        {
            /// <summary>
            /// Icon for a document (blank page), no associated program.
            /// </summary>
            DocumentNotAssociated = 0,
            /// <summary>
            /// Icon for a document with an associated program.
            /// </summary>
            DocumentAssociated = 1,
            /// <summary>
            ///  Icon for a generic application with no custom icon.
            /// </summary>
            Application = 2,
            /// <summary>
            ///  Icon for a closed folder.
            /// </summary>
            Folder = 3,
            /// <summary>
            /// Icon for an open folder. 
            /// </summary>
            FolderOpen = 4,
            /// <summary>
            /// Icon for a 5.25" floppy disk drive.
            /// </summary>
            Drive525 = 5,
            /// <summary>
            ///  Icon for a 3.5" floppy disk drive. 
            /// </summary>
            Drive35 = 6,
            /// <summary>
            ///  Icon for a removable drive.
            /// </summary>
            DriveRemove = 7,
            /// <summary>
            ///  Icon for a fixed (hard disk) drive.
            /// </summary>
            DriveFixed = 8,
            /// <summary>
            ///  Icon for a network drive.
            /// </summary>
            DriveNetwork = 9,
            /// <summary>
            ///  Icon for a disconnected network drive.
            /// </summary>
            DriveNetworkDisabled = 10,
            /// <summary>
            ///  Icon for a CD drive.
            /// </summary>
            DriveCd = 11,
            /// <summary>
            ///  Icon for a RAM disk drive. 
            /// </summary>
            DriveRam = 12,
            /// <summary>
            ///  Icon for an entire network. 
            /// </summary>
            World = 13,
            /// <summary>
            ///  Icon for a computer on the network.
            /// </summary>
            Server = 15,
            /// <summary>
            ///  Icon for a printer. 
            /// </summary>
            Printer = 16,
            /// <summary>
            /// Icon for My Network places.
            /// </summary>
            MyNetwork = 17,
            /// <summary>
            /// Icon for search (magnifying glass).
            /// </summary>
            Find = 22,
            /// <summary>
            ///  Icon for help.     
            /// </summary>
            Help = 23,
            /// <summary>
            ///  Icon for an overlay indicating shared items.        
            /// </summary>
            Share = 28,
            /// <summary>
            ///  Icon for an overlay indicating shortcuts to items.
            /// </summary>
            Link = 29,
            /// <summary>
            /// Icon for an overlay for slow items.
            /// </summary>
            SlowFile = 30,
            /// <summary>
            ///  Icon for a empty recycle bin.
            /// </summary>
            Recycler = 31,
            /// <summary>
            ///  Icon for a full recycle bin.
            /// </summary>
            RecyclerFull = 32,
            /// <summary>
            ///  Icon for audio CD media.
            /// </summary>
            MediaCdAudio = 40,
            /// <summary>
            ///  Icon for a security lock.
            /// </summary>
            Lock = 47,
            /// <summary>
            ///  Icon for a auto list.
            /// </summary>
            AutoList = 49,
            /// <summary>
            /// Icon for a network printer.
            /// </summary>
            PrinterNet = 50,
            /// <summary>
            ///  Icon for a server share.
            /// </summary>
            ServerShare = 51,
            /// <summary>
            ///  Icon for a Fax printer.
            /// </summary>
            PrinterFax = 52,
            /// <summary>
            /// Icon for a networked Fax printer.
            /// </summary>
            PrinterFaxNet = 53,
            /// <summary>
            ///  Icon for print to file.
            /// </summary>
            PrinterFile = 54,
            /// <summary>
            /// Icon for a stack.
            /// </summary>
            Stack = 55,
            /// <summary>
            ///  Icon for a SVCD media.
            /// </summary>
            MediaSvcd = 56,
            /// <summary>
            ///  Icon for a folder containing other items.
            /// </summary>
            StuffedFolder = 57,
            /// <summary>
            ///  Icon for an unknown drive.
            /// </summary>
            DriveUnknown = 58,
            /// <summary>
            ///  Icon for a DVD drive. 
            /// </summary>
            DriveDvd = 59,
            /// <summary>
            /// Icon for DVD media.
            /// </summary>
            MediaDvd = 60,
            /// <summary>
            ///  Icon for DVD-RAM media.   
            /// </summary>
            MediaDvdRam = 61,
            /// <summary>
            /// Icon for DVD-RW media.
            /// </summary>
            MediaDvdRw = 62,
            /// <summary>
            ///  Icon for DVD-R media.
            /// </summary>
            MediaDvdR = 63,
            /// <summary>
            ///  Icon for a DVD-ROM media.
            /// </summary>
            MediaDvdRom = 64,
            /// <summary>
            ///  Icon for CD+ (Enhanced CD) media.
            /// </summary>
            MediaCdAudioPlus = 65,
            /// <summary>
            ///  Icon for CD-RW media.
            /// </summary>
            MediaCdrw = 66,
            /// <summary>
            ///  Icon for a CD-R media.
            /// </summary>
            MediaCdr = 67,
            /// <summary>
            ///  Icon burning a CD.
            /// </summary>
            MediaCdBurn = 68,
            /// <summary>
            ///  Icon for blank CD media.
            /// </summary>
            MediaBlankCd = 69,
            /// <summary>
            ///  Icon for CD-ROM media.
            /// </summary>
            MediaCdRom = 70,
            /// <summary>
            ///  Icon for audio files.
            /// </summary>
            AudioFiles = 71,
            /// <summary>
            ///  Icon for image files.
            /// </summary>
            ImageFiles = 72,
            /// <summary>
            ///  Icon for video files.
            /// </summary>
            VideoFiles = 73,
            /// <summary>
            ///  Icon for mixed Files.
            /// </summary>
            MixedFiles = 74,
            /// <summary>
            /// Icon for a folder back.
            /// </summary>
            FolderBack = 75,
            /// <summary>
            ///  Icon for a folder front.
            /// </summary>
            FolderFront = 76,
            /// <summary>
            ///  Icon for a security shield. Use for UAC prompts only.
            /// </summary>
            Shield = 77,
            /// <summary>
            ///  Icon for a warning.
            /// </summary>
            Warning = 78,
            /// <summary>
            ///  Icon for an informational message.
            /// </summary>
            Info = 79,
            /// <summary>
            ///  Icon for an error message.
            /// </summary>
            Error = 80,
            /// <summary>
            ///  Icon for a key.
            /// </summary>
            Key = 81,
            /// <summary>
            ///  Icon for software.
            /// </summary>
            Software = 82,
            /// <summary>
            ///  Icon for a rename.
            /// </summary>
            Rename = 83,
            /// <summary>
            ///  Icon for delete.
            /// </summary>
            Delete = 84,
            /// <summary>
            ///  Icon for audio DVD media.
            /// </summary>
            MediaAudioDvd = 85,
            /// <summary>
            ///  Icon for movie DVD media.
            /// </summary>
            MediaMovieDvd = 86,
            /// <summary>
            ///  Icon for enhanced CD media.
            /// </summary>
            MediaEnhancedCd = 87,
            /// <summary>
            ///  Icon for enhanced DVD media.
            /// </summary>
            MediaEnhancedDvd = 88,
            /// <summary>
            ///  Icon for HD-DVD media.
            /// </summary>
            MediaHdDvd = 89,
            /// <summary>
            ///  Icon for BluRay media.
            /// </summary>
            MediaBluRay = 90,
            /// <summary>
            ///  Icon for VCD media.
            /// </summary>
            MediaVcd = 91,
            /// <summary>
            ///  Icon for DVD+R media.
            /// </summary>
            MediaDvdPlusR = 92,
            /// <summary>
            ///  Icon for DVD+RW media.
            /// </summary>
            MediaDvdPlusRw = 93,
            /// <summary>
            ///  Icon for desktop computer.
            /// </summary>
            DesktopPc = 94,
            /// <summary>
            ///  Icon for mobile computer (laptop/notebook).
            /// </summary>
            MobilePc = 95,
            /// <summary>
            ///  Icon for users.
            /// </summary>
            Users = 96,
            /// <summary>
            ///  Icon for smart media.
            /// </summary>
            MediaSmartMedia = 97,
            /// <summary>
            ///  Icon for compact flash.
            /// </summary>
            MediaCompactFlash = 98,
            /// <summary>
            ///  Icon for a cell phone.
            /// </summary>
            DeviceCellPhone = 99,
            /// <summary>
            ///  Icon for a camera.
            /// </summary>
            DeviceCamera = 100,
            /// <summary>
            ///  Icon for video camera.
            /// </summary>
            DeviceVideoCamera = 101,
            /// <summary>
            ///  Icon for audio player.
            /// </summary>
            DeviceAudioPlayer = 102,
            /// <summary>
            ///  Icon for connecting to network.
            /// </summary>
            NetworkConnect = 103,
            /// <summary>
            ///  Icon for the Internet.
            /// </summary>
            Internet = 104,
            /// <summary>
            ///  Icon for a ZIP file.
            /// </summary>
            ZipFile = 105,
            /// <summary>
            /// Icon for settings.
            /// </summary>
            Settings = 106,

            // 107-131 are internal Vista RTM icons
            // 132-159 for SP1 icons

            /// <summary>
            /// HDDVD Drive (all types)
            /// </summary>
            DriveHddvd = 132,

            /// <summary>
            /// Icon for BluRay Drive (all types)
            /// </summary>
            DriveBluRay = 133,

            /// <summary>
            /// Icon for HDDVD-ROM Media
            /// </summary>
            MediaHddvdrom = 134,

            /// <summary>
            /// Icon for HDDVD-R Media
            /// </summary>
            MediaHddvdr = 135,

            /// <summary>
            /// Icon for HDDVD-RAM Media
            /// </summary>
            MediaHddvdram = 136,

            /// <summary>
            /// Icon for BluRay ROM Media
            /// </summary>
            MediaBluRayRom = 137,

            /// <summary>
            /// Icon for BluRay R Media
            /// </summary>
            MediaBluRayR = 138,

            /// <summary>
            /// Icon for BluRay RE Media (Rewriable and RAM)
            /// </summary>
            MediaBluRayRe = 139,

            /// <summary>
            /// Icon for Clustered disk
            /// </summary>
            ClusteredDisk = 140

        }

        private static class NativeMethods
        {
            internal const uint ByCommand = 0x00000000;
            internal const uint Grayed = 0x00000001;
            internal const uint Enabled = 0;

            internal const uint WindowCloseButton = 0xF060;

            /// <summary>
            /// Specifies options for the appearance of the 
            /// stock icon.
            /// </summary>
            [Flags]
            internal enum StockIconOptions
            {
                /// <summary>
                /// Retrieve the small version of the icon, as specified by  
                /// SM_CXICON and SM_CYICON system metrics.
                /// </summary>
                Large = 0x000000000,

                /// <summary>
                /// Retrieve the small version of the icon, as specified by  
                /// SM_CXSMICON and SM_CYSMICON system metrics.
                /// </summary>
                Small = 0x000000001,

                /// <summary>
                /// Retrieve the shell-sized icons (instead of the 
                /// size specified by the system metrics). 
                /// </summary>
                ShellSize = 0x000000004,

                /// <summary>
                /// Specified that the hIcon member of the SHSTOCKICONINFO 
                /// structure receives a handle to the specified icon.
                /// </summary>
                Handle = 0x000000100,

                /// <summary>
                /// Specifies that the iSysImageImage member of the SHSTOCKICONINFO 
                /// structure receives the index of the specified 
                /// icon in the system imagelist.
                /// </summary>
                SystemIndex = 0x000004000,

                /// <summary>
                /// Adds the link overlay to the icon.
                /// </summary>
                LinkOverlay = 0x000008000,

                ///<summary>
                /// Adds the system highlight color to the icon.
                /// </summary>
                Selected = 0x000010000
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            internal struct StockIconInfo
            {
                internal UInt32 StuctureSize;
                internal IntPtr Handle;
                internal Int32 ImageIndex;
                internal Int32 Identifier;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
                internal readonly string Path;
            }

            [PreserveSig]
            [DllImport("Shell32.dll", CharSet = CharSet.Unicode,
                ExactSpelling = true, SetLastError = false)]
            internal static extern int SHGetStockIconInfo(
                StockIconIdentifier identifier,
                StockIconOptions flags,
                ref StockIconInfo info);

            [DllImport("user32.dll", SetLastError = true)]
            internal static extern IntPtr GetSystemMenu(IntPtr handle, bool revert);

            [DllImport("user32.dll", SetLastError = true)]
            internal static extern bool EnableMenuItem(IntPtr handle, uint identifer, uint enable);
        }

        private static readonly ILog Log = Logger.Create();

        public static void WaitForMilliseconds(DateTime start, int milliseconds)
        {
            WaitForInterval(start, new TimeSpan(0, 0, 0, 0, milliseconds));
        }

        public static void WaitForSeconds(DateTime start, int seconds)
        {
            WaitForInterval(start, new TimeSpan(0, 0, seconds));
        }

        public static void WaitForInterval(DateTime start, TimeSpan interval)
        {
            if (interval.Duration().Ticks == 0)
                return;

            TimeSpan elapsed;
            do
            {
                Thread.Sleep(5);
                Application.Current.DoEvents();
                elapsed = DateTime.UtcNow.Subtract(start);
            } while (elapsed.Duration().TotalSeconds <= interval.Duration().TotalSeconds);
        }
        
        public static void RemoveClickHandlers(Button target)
        {
            try
            {
                var fieldInfo = typeof(Control).GetField("EventClick",
                                                          BindingFlags.Static |
                                                          BindingFlags.NonPublic);
                if (fieldInfo == null)
                    return;

                var value = fieldInfo.GetValue(target);
                if (value == null)
                    return;

                var propertyInfo = target.GetType().GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance);
                if (propertyInfo == null)
                    return;

                var list = (EventHandlerList)propertyInfo.GetValue(target, null);
                list.RemoveHandler(value, list[value]);
            }
            catch (Exception e)
            {
                Log.Warn(e);
            }
        }

        public static void RemoveHandlers(Control target, string handlerName)
        {
            try
            {
                var fieldInfo = typeof(Control).GetField("handlerName",
                                                          BindingFlags.Static |
                                                          BindingFlags.NonPublic);
                if (fieldInfo == null)
                    return;

                var value = fieldInfo.GetValue(target);
                if (value == null)
                    return;

                var propertyInfo = target.GetType().GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance);
                if (propertyInfo == null)
                    return;

                var list = (EventHandlerList)propertyInfo.GetValue(target, null);
                list.RemoveHandler(value, list[value]);
            }
            catch (Exception e)
            {
                Log.Warn(e);
            }
        }

        public static bool OpenBrowser(Uri url)
        {
            try
            {
                var info = new ProcessStartInfo(url.AbsoluteUri) { UseShellExecute = true };
                Process.Start(info);
                return true;
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return false;
            }
        }

        public static bool OpenBrowser(string url)
        {
            try
            {
                if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    Log.WarnFormat("Cannot open url {0}: url is not well-formed", url);
                    return false;
                }

                return OpenBrowser(new Uri(url));
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to open the url {0}: {1}", url, e.Message);
                return false;
            }
        }

        /// <summary>
        /// Finds a Child of a given item in the visual tree. 
        /// </summary>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="childName">x:Name or Name of child. </param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, 
        /// a null parent is being returned.</returns>
        /// <remarks>Taken from http://stackoverflow.com/questions/636383/wpf-ways-to-find-controls</remarks>
        public static T FindChild<T>(DependencyObject parent, string childName)
           where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                var childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!String.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        /// <summary>
        /// Gets the top level control for a wpf control
        /// </summary>
        /// <param name="control">The control in question</param>
        /// <returns></returns>
        /// <remarks>Taken from http://stackoverflow.com/questions/838825/how-to-get-a-top-object-window-or-page-in-wpf</remarks>
        public static DependencyObject GetTopLevelControl(DependencyObject control)
        {
            var instance = control;
            var parent = control;
            while ((instance = VisualTreeHelper.GetParent(instance)) != null)
            {
                parent = instance;
            }
            return parent;
        }

        public static T GetControlByName<T>(DependencyObject target, string name) where T : class
        {
            foreach (var instance in LogicalTreeHelper.GetChildren(target))
            {
                if (instance as DependencyObject == null)
                    continue;

                var result = GetControlByName<T>(instance as DependencyObject, name);
                if (result != null)
                    return result;

                var element = instance as FrameworkElement;
                if (element == null)
                    continue;

                if (element.Name.Equals(name, StringComparison.Ordinal))
                    return instance as T;

            }

            return null;
        }


       

        /// <summary>
        /// WPF windows don't get a valid window handle until they are shown. This method shows 
        /// the window minimized and not in the taskbar briefly and then hides it.  This will 
        /// generate a window handle for the window.
        /// </summary>
        /// <param name="instance"></param>
        public static void InitializeWindowHandle(Window instance)
        {
            if (instance == null)
                return;

            var helper = new WindowInteropHelper(instance);
            helper.EnsureHandle();

        }

        /// <summary>
        /// Disables the close button for a wpf window
        /// </summary>
        /// <param name="instance">window in question</param>
        /// <remarks>adapted from http://stackoverflow.com/questions/743906/how-to-hide-close-button-in-wpf-window </remarks>
        public static void DisableCloseButton(Window instance)
        {
            try
            {
                if (instance == null)
                    return;

                var helper = new WindowInteropHelper(instance);

                var windowHandle = helper.Handle;
                if (windowHandle == IntPtr.Zero)
                    InitializeWindowHandle(instance);

                windowHandle = helper.Handle;
                if (windowHandle == IntPtr.Zero)
                {
                    return;
                }

                var menuHandle = NativeMethods.GetSystemMenu(windowHandle, false);
                if (menuHandle == IntPtr.Zero)
                    throw new Win32Exception();

                NativeMethods.EnableMenuItem(menuHandle, NativeMethods.WindowCloseButton,
                                             NativeMethods.Grayed | NativeMethods.ByCommand);


            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to suppress a window's close button: {0}", e);
            }
        }

        /// <summary>
        /// Shows the close button for a wpf window
        /// </summary>
        /// <param name="instance">window in question</param>
        /// <remarks>adapted from http://stackoverflow.com/questions/743906/how-to-hide-close-button-in-wpf-window </remarks>
        public static void EnableCloseButton(Window instance)
        {
            try
            {
                if (instance == null)
                    return;

                var windowHandle = new WindowInteropHelper(instance).Handle;
                if (windowHandle == IntPtr.Zero)
                    return; 
                
                var menuHandle = NativeMethods.GetSystemMenu(windowHandle, false);
                if (menuHandle == IntPtr.Zero)
                    throw new Win32Exception();



                NativeMethods.EnableMenuItem(menuHandle, NativeMethods.WindowCloseButton,
                                             NativeMethods.Enabled | NativeMethods.ByCommand);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to enable a window's close button: {0}", e);
            }
        }


        public static ImageSource GetSystemIconImageSourceFromApi(StockIconIdentifier identifier, StockIconSize size,
                                                                  bool overlay, bool selected)
        {
            try
            {
                var flags = NativeMethods.StockIconOptions.Handle;
                switch (size)
                {
                    case StockIconSize.Small:
                        flags |= NativeMethods.StockIconOptions.Small;
                        break;
                    case StockIconSize.ShellSize:
                        flags |= NativeMethods.StockIconOptions.ShellSize;
                        break;
                    case StockIconSize.Large:
                        flags |= NativeMethods.StockIconOptions.Large;
                        break;
                }

                if (selected)
                    flags |= NativeMethods.StockIconOptions.Selected;

                if (overlay)
                    flags |= NativeMethods.StockIconOptions.LinkOverlay;

                var info = new NativeMethods.StockIconInfo { StuctureSize = (UInt32)Marshal.SizeOf(typeof(NativeMethods.StockIconInfo)) };

                var result = NativeMethods.SHGetStockIconInfo(identifier, flags, ref info);
                if (result != 0)
                {
                    Log.WarnFormat("Could not get icon {0} from Windows API: {1}", identifier, result);
                    return null;
                }

                return Imaging.CreateBitmapSourceFromHIcon(
                    info.Handle,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return null;
            }
        }

        public static System.Windows.Controls.Image GetSystemIconFromApi(StockIconIdentifier identifier, StockIconSize size, bool overlay, bool selected)
        {
            try
            {
                var source = GetSystemIconImageSourceFromApi(identifier, size, overlay, selected);
                    
                return new System.Windows.Controls.Image
                {
                    Source = source,
                    Height = source.Height,
                    Width = source.Width
                };
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return null;
            }
        }

        public static System.Windows.Controls.Image GetSystemIcon(string name)
        {
            try
            {
                var type = typeof(SystemIcons);
                var property = type.GetProperty(name);
                if (property == null)
                    return null;

                var icon = property.GetValue(null, null) as Icon;
                if (icon == null)
                    return null;

                var source = Imaging.CreateBitmapSourceFromHIcon(
                    icon.Handle,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

                return new System.Windows.Controls.Image
                {
                    Source = source,
                    Height = source.Height,
                    Width = source.Width
                };
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return null;
            }

        }

        public static void GetChildControls<T>(DependencyObject target, ref List<T> results) where T : DependencyObject
        {
            if (target == null)
                return;

            foreach (var instance in LogicalTreeHelper.GetChildren(target))
            {
                if (instance as T != null)
                    results.Add(instance as T);

                GetChildControls(instance as DependencyObject, ref results);
            }
        }

        public static void MouseDragDownHandler(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.LeftButton != MouseButtonState.Pressed)
                    return;

                if (sender as DependencyObject == null)
                    return;

                var window = GetTopLevelControl(sender as DependencyObject) as Window;
                if (window == null)
                    return;

                window.DragMove();
            }
            catch (Exception ex)
            {
                Log.Warn(ex);
            }

        }

        public static Thickness GetTotalOffset(FrameworkElement target)
        {
            var result = target.Margin;

            var propertyInfo = target.GetType().GetProperty("Padding");
            if (propertyInfo == null)
                return result;

            if (propertyInfo.PropertyType != typeof(Thickness))
                return result;

            var padding = (Thickness)propertyInfo.GetValue(target, null);

            return result.Add(padding);
        }

        public static void SetPaddingGeneric(FrameworkElement target, Thickness value)
        {
            var propertyInfo = target.GetType().GetProperty("Padding");
            if (propertyInfo == null)
                return;

            if (propertyInfo.PropertyType != typeof(Thickness))
                return;

            propertyInfo.SetValue(target, value, null);
        }

        public class WindowsHandleWrapper: System.Windows.Forms.IWin32Window
        {
            public WindowsHandleWrapper(IntPtr handle)
            {
                Handle = handle;
            }

            public WindowsHandleWrapper(Window window)
            {
                InitializeWindowHandle(window);
                var helper = new WindowInteropHelper(window);
                Handle = helper.Handle;
            }

            public IntPtr Handle { get; private set; }
        }
    }





}
