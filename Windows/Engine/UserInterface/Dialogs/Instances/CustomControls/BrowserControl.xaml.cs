using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using log4net;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Logging;
using SHDocVw;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Instances.CustomControls
{
    /// <summary>
    /// Interaction logic for BrowserControl.xaml
    /// </summary>
    public partial class BrowserControl : UserControl
    {
        private static readonly ILog Log = Logger.Create();

        public BrowserControl()
        {
            InitializeComponent();
            SilentMode = true;

            Browser.Navigated += OnNavigatedHandler;

        }

        public bool SilentMode { get; set; }

        private void OnNavigatedHandler(object sender, NavigationEventArgs e)
        {
            var comInterface= Browser.ToComInterface();
            if (comInterface == null)
            {
                return;
            }

            SetSilent(comInterface);
            AddErrorHandler(comInterface);
        }

        private void SetSilent(SHDocVw.WebBrowser comInterface)
        {
            comInterface.Silent = SilentMode;
        }

        private void AddErrorHandler(SHDocVw.WebBrowser comInterface)
        {
            comInterface.NavigateError += OnNavigateErrorHandler;
            
        }

        private void OnNavigateErrorHandler(object pdisp, ref object url, ref object frame, ref object statuscode, ref bool cancel)
        {
            Log.WarnFormat("an issue occurred while attempting to load a web banner from {0}: {1}", url, statuscode);
        }
    }
}
