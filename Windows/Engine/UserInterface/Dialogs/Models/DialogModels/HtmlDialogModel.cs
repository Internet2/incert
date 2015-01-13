using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using log4net;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Instances;
using Org.InCommon.InCert.Engine.Logging;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels
{
    public class HtmlDialogModel:AbstractDialogModel
    {
        private static readonly ILog Log = Logger.Create();
        private readonly WebBrowser _browserInstance;
        
        public HtmlDialogModel(IHasEngineFields engine) : base(engine, new HtmlWindow())
        {
            ShowInTaskbar = true;
            WindowStyle = WindowStyle.SingleBorderWindow;
            Width = 600;
            Height = 600;

            _browserInstance = ((HtmlWindow) DialogInstance).BrowserControl;
            _browserInstance.Loaded += BrowserLoadedHandler;
            _browserInstance.ObjectForScripting = new ScriptingProxy(engine.SettingsManager, this);
        }

        private Uri Uri
        {
            get
            {
                return _browserInstance.Source;
            }
            set
            {
                _browserInstance.Navigate(value, null, null, "Incert: true\r\n");
                OnPropertyChanged();
            }
        }

        public IResult ShowPage(string url)
        {
            Uri = new Uri(url);
            DialogInstance.Show();
            return WaitForResult();
        }

        private void BrowserLoadedHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                var activeX = _browserInstance.GetType().InvokeMember("ActiveXInstance",
                    BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                    null, _browserInstance, new object[] { }) as SHDocVw.WebBrowser;

                if (activeX == null)
                    return;

                activeX.Silent = true;
                activeX.NavigateError += HandleNavigateIssue;
            }
            catch (Exception ex)
            {
                Log.Warn("An issue occurred while attempting to configure the browser control: {0}", ex);
            }
        }

        private void HandleNavigateIssue(object pdisp, ref object url, ref object frame, ref object statusCode, ref bool cancel)
        {
            try
            {
                var issueCode = (int)statusCode;
                if (!Enum.IsDefined(typeof(HttpStatusCode), issueCode))
                    throw Marshal.GetExceptionForHR(issueCode);

                throw new HttpException(issueCode, "");
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to load content from {0}: {1}", url, e);
                cancel = true;
            }
        }



        [ComVisible(true)]
        public class ScriptingProxy
        {
            private readonly ISettingsManager _settingsManager;
            private readonly HtmlDialogModel _model;

            public ScriptingProxy(ISettingsManager settingsManager, HtmlDialogModel model)
            {
                _settingsManager = settingsManager;
                _model = model;
            }

            public void ReturnNext()
            {
               _model.Result = new NextResult();
            }

            public void ReturnBack()
            {
                _model.Result = new BackResult();
            }


        }
    }

    
}
