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
using Org.InCommon.InCert.Engine.AdvancedMenu;
using Org.InCommon.InCert.Engine.Dynamics;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Help;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Instances;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels
{
    public class HtmlDialogModel : AbstractDialogModel
    {
        private static readonly ILog Log = Logger.Create();
        private readonly WebBrowser _browserInstance;

        public HtmlDialogModel(IHasEngineFields engine)
            : base(engine, new HtmlWindow())
        {
            ShowInTaskbar = true;
            WindowStyle = WindowStyle.SingleBorderWindow;
            Width = 600;
            Height = 600;

            _browserInstance = ((HtmlWindow)DialogInstance).BrowserControl;
            _browserInstance.Loaded += BrowserLoadedHandler;
            _browserInstance.ObjectForScripting = new ScriptingProxy(engine, this);
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

        public void EnableDisableBrowser(bool enable)
        {
            _browserInstance.IsEnabled = enable;
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
            private readonly IHelpManager _helpManager;
            private readonly IAdvancedMenuManager _advancedMenuManager;
            private readonly IHasEngineFields _engine;
            private readonly HtmlDialogModel _model;

            public ScriptingProxy(IHasEngineFields engine, HtmlDialogModel model)
            {
                _settingsManager = engine.SettingsManager;
                _helpManager = engine.HelpManager;
                _advancedMenuManager = engine.AdvancedMenuManager;
                _engine = engine;
                _model = model;
            }

            public string GetValue(string key)
            {
                return _settingsManager.GetTemporarySettingString(key);
            }

            public void SetValue(string key, string value)
            {
                _settingsManager.SetTemporarySettingString(key, value);
            }

            public void ReturnNext()
            {
                _model.Result = new NextResult();
            }

            public void ReturnBack()
            {
                _model.Result = new BackResult();
            }

            public void ShowAdvancedMenu(string group = "")
            {
                try
                {
                    var left = _model.DialogInstance.Left;
                    var top = _model.DialogInstance.Top;

                    _model.EnableDisableAllControls(false);
                    _model.EnableDisableBrowser(false);
                    var advancedMenuModel = new AdvancedMenuModel(_engine, _model);
                    advancedMenuModel.ShowDialog(
                        left,
                        top,
                        group.Resolve(_engine.SettingsManager, true));

                    if (advancedMenuModel.Result != null)
                    {
                        if (advancedMenuModel.Result is RestartComputerResult ||
                            advancedMenuModel.Result is SilentRestartComputerResult ||
                            advancedMenuModel.Result is ExitUtilityResult)
                        {
                            _model.Result = advancedMenuModel.Result;
                            _model.SuppressCloseQuestion = true;
                            _model.DialogInstance.Close();
                        }
                    }
                }
                finally
                {
                    _model.EnableDisableAllControls(true);
                    _model.EnableDisableBrowser(true);
                }
            }


            public void ShowHelpTopic(string value)
            {
                _helpManager.ShowHelpTopic(value, _model);
            }

        }
    }


}
