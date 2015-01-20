using System;
using System.Collections.Generic;
using CefSharp;
using CefSharp.Wpf;
using log4net;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Instances.CustomControls;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels
{
    public class BrowserContentModel:AbstractContentModel
    {
        private static readonly ILog Log = Logger.Create();

        public BrowserContentModel(AbstractModel parentModel) : base(parentModel)
        {
            InitializeChromium();
            
        }

        public override T LoadContent<T>(AbstractContentWrapper wrapper)
        {
            var content = new ChromiumWebBrowser();
           /* InitializeBindings(content);
            InitializeValues(wrapper);*/

            var browserWrapper = wrapper as BrowserContentWrapper;
            if (browserWrapper == null)
            {
                throw new InvalidCastException("Could not cast wrapper to valid type");
            }

           /* content.SilentMode = browserWrapper.SilentMode;
            content.Browser.ObjectForScripting = new ScriptingModel(wrapper.Engine, RootDialogModel);*/
            content.RegisterJsObject("engine", new ScriptingModel(wrapper.Engine, RootDialogModel));
            content.Address = browserWrapper.Uri.AbsoluteUri;
            //content.Browser.Navigate(browserWrapper.Uri, null, null, "Incert: true\r\n");
            content.Width = 600;
            content.Height = 600;

            content.ConsoleMessage += ConsoleMessageHandler;

            Content = content;

            return content as T;

        }

        private static void ConsoleMessageHandler(object sender, ConsoleMessageEventArgs e)
        {
            Log.DebugFormat("Javascript Console: {0}",e.Message);
        }

        private static void InitializeChromium()
        {
            if (Cef.IsInitialized)
            {
                return;
            }

            var settings = new CefSettings
            {
                PackLoadingDisabled = true,
                LogSeverity =  LogSeverity.Disable
            };

            if (!Cef.Initialize(settings))
            {
                throw new Exception("Could not initialize Chromium browser");
            }    
        }


        
        
    }
}
