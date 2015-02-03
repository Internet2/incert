using System;
using System.Windows;
using CefSharp;
using CefSharp.Wpf;
using log4net;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels.LifespanHandlers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels.SchemeHandlers;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels
{
    public class BrowserContentModel:AbstractContentModel
    {
        private static readonly ILog Log = Logger.Create();

        public BrowserContentModel(AbstractModel parentModel) : base(parentModel)
        {
            InitializeChromium();
        }

        public string Address
        {
            get { return Browser.Address; }
            set { Browser.Address = value; }
        }

        private ChromiumWebBrowser Browser
        {
            get
            {
                var browser = Content as ChromiumWebBrowser;
                if (browser == null)
                {
                    throw new Exception("Could not convert content to Chromium Web Browser");
                }

                return browser;
            }
        }

        public override T LoadContent<T>(AbstractContentWrapper wrapper)
        {
            var content = new ChromiumWebBrowser();
            var browserWrapper = wrapper as BrowserContentWrapper;
            if (browserWrapper == null)
            {
                throw new InvalidCastException("Could not cast wrapper to valid type");
            }

            content.RegisterJsObject("engine", new ScriptingModel(wrapper.Engine, RootDialogModel));
            content.Width = 600;
            content.Height = 600;
            
            content.RequestHandler = new RequestHandler();
            content.LifeSpanHandler = new LifespanHandler();
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
                PackLoadingDisabled =true,
                LogSeverity =  LogSeverity.Verbose
                
            };
            
            
            settings.RegisterScheme(new CefCustomScheme
            {
                SchemeName = ArchiveSchemeHandlerFactory.SchemeName,
                SchemeHandlerFactory = new ArchiveSchemeHandlerFactory(),
               
            });

            settings.RegisterScheme(new CefCustomScheme
            {
                SchemeName = EmbeddedResourceSchemeHandlerFactory.SchemeName,
                SchemeHandlerFactory = new EmbeddedResourceSchemeHandlerFactory(),

            });

            
            if (!Cef.Initialize(settings))
            {
                throw new Exception("Could not initialize Chromium browser");
            }    
        }
    }
}
