using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using CefSharp;
using CefSharp.Wpf;
using log4net;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.EventWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels.LifespanHandlers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels.SchemeHandlers;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels
{
    public class BrowserContentModel : AbstractContentModel
    {
        private static readonly ILog Log = Logger.Create();

        public BrowserContentModel(AbstractModel parentModel)
            : base(parentModel)
        {
            InitializeChromium();

        }

        public bool IsLoaded { get; private set; }
        public bool LoadErrorOccurred { get; private set; }
        public string LoadError { get; private set; }

        public string Address
        {
            get { return Browser.Address; }
            set {Browser.Address = value; }
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

            SubscribeToEngineEvents(wrapper.Engine as IHasEngineEvents);

            content.RegisterJsObject("engine", new ScriptingModel(wrapper.Engine, RootDialogModel));
            content.Width = 600;
            content.Height = 600;

            content.RequestHandler = new RequestHandler();
            content.LifeSpanHandler = new LifespanHandler();
            content.ConsoleMessage += ConsoleMessageHandler;
            content.LoadError += OnContentLoadError;
            content.FrameLoadEnd += OnFrameLoadEnd;
            content.FrameLoadStart += OnFrameLoadStart;
          
            Content = content;

            return content as T;

        }

        private void OnFrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            var test = 0;
        }

        private void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (!e.IsMainFrame)
            {
                return;
            };

            IsLoaded = true;
            
        }

        private void OnContentLoadError(object sender, LoadErrorEventArgs e)
        {
            LoadErrorOccurred = true;
            LoadError = e.ErrorText;
        }

        private static void ConsoleMessageHandler(object sender, ConsoleMessageEventArgs e)
        {
            Log.DebugFormat("Javascript Console: {0}", e.Message);
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
                LogSeverity = LogSeverity.Verbose
                 

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

        private void SubscribeToEngineEvents(IHasEngineEvents engine)
        {
            if (engine == null)
            {
                throw new Exception("Could not subscribe to engine events.");
            }

            engine.BranchCompleted += OnBranchCompleted;
            engine.BranchStarted += OnBranchStated;
            engine.IssueOccurred += OnIssueOccurred;
            engine.TaskStarted += OnTaskStarted;
            engine.TaskCompleted += OnTaskCompleted;
        }

        private const string EventScriptFormat = "if (typeof raiseEvent!='undefined'){{raiseEvent('{0}',{1});}}";

        private void OnTaskCompleted(object sender, TaskEventData e)
        {
            var script = string.Format(EventScriptFormat, "task_completed", e.ToJson());
            Browser.EvaluateScriptAsync(script);
        }

        private void OnTaskStarted(object sender, TaskEventData e)
        {
            var script = string.Format(EventScriptFormat, "task_started", e.ToJson());
            Browser.EvaluateScriptAsync(script);
        }

        private void OnIssueOccurred(object sender, IssueEventData e)
        {
            var script = string.Format(EventScriptFormat, "issue_occurred", e.ToJson());
            Browser.EvaluateScriptAsync(script);
        }

        private void OnBranchStated(object sender, BranchEventData e)
        {
            var script = string.Format(EventScriptFormat, "branch_started", e.ToJson());
            Browser.EvaluateScriptAsync(script);
        }

        private void OnBranchCompleted(object sender, BranchEventData e)
        {
            var script = string.Format(EventScriptFormat, "branch_completed", e.ToJson());
            Browser.EvaluateScriptAsync(script);
        }
    }
}
