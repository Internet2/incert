using System;
using System.Net;
using CefSharp;
using CefSharp.Wpf;
using log4net;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results.Errors.UserInterface;
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
        }

        public bool IsLoaded { get; private set; }
      
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

            SubscribeToEngineEvents(wrapper.Engine as IHasEngineEvents);

            content.RegisterJsObject("engine", new ScriptingModel(wrapper.Engine, RootDialogModel));
            content.Width = browserWrapper.Width;
            content.Height = browserWrapper.Height;

            content.RequestHandler = new RequestHandler();
            content.LifeSpanHandler = new LifespanHandler();
            content.ConsoleMessage += ConsoleMessageHandler;
            content.LoadError += OnContentLoadError;
            content.FrameLoadEnd += OnFrameLoadEnd;
            
            Content = content;

            return content as T;

        }
        
        private void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (sender.InvokeIfRequired(() => OnFrameLoadEnd(sender, e)))
            {
                return;
            }
            
            if (!e.IsMainFrame)
            {
                return;
            }

            if (e.HttpStatusCode != 200 && e.HttpStatusCode != 0)
            {
                SetError(e);
            }

            IsLoaded = true;
            
        }

        private void OnContentLoadError(object sender, LoadErrorEventArgs e)
        {
            if (sender.InvokeIfRequired(() => OnContentLoadError(sender, e)))
            {
                return;
            }
            
            if (e.ErrorCode == CefErrorCode.Aborted)
            {
                return;
            }

            SetError(e);
        }

        private void SetError(FrameLoadEndEventArgs e)
        {
            var statusCode = Enum.IsDefined(typeof (HttpStatusCode), e.HttpStatusCode)
                ? ((HttpStatusCode) e.HttpStatusCode).ToString().SplitByCapitalLetters()
                : "Unknown Issue";

            statusCode = string.Format("{0} ({1})", statusCode, e.HttpStatusCode);

            SetError(e.Url, statusCode);
        }

        private void SetError(LoadErrorEventArgs e)
        {
            SetError(e.FailedUrl, e.ErrorCode.ToString().SplitByCapitalLetters());
        }

        private void SetError(string url, string issue)
        {
            RootDialogModel.Result = new CouldNotLoadHtmlContent
            {
                Issue = issue,
                Url = url,
                IsExternalUrl = !IsInternalUrl(url)
            };
        }

        private static bool IsInternalUrl(string url)
        {
            return false;
            if (string.IsNullOrWhiteSpace(url))
            {
                return false;
            }

            return EmbeddedResourceSchemeHandlerFactory.IsSchemeUrl(url)
                   || ArchiveSchemeHandlerFactory.IsSchemeUrl(url);
        }

        private static void ConsoleMessageHandler(object sender, ConsoleMessageEventArgs e)
        {
            if (sender.InvokeIfRequired(() => ConsoleMessageHandler(sender, e)))
            {
                return;
            }
            
            Log.DebugFormat("Javascript Console: {0}", e.Message);
        }

        
        private void SubscribeToEngineEvents(IHasEngineEvents engine)
        {
            if (engine == null)
            {
                throw new Exception("Could not subscribe to engine events.");
            }
            
            engine.IssueOccurred += OnIssueOccurred;
            engine.TaskStarted += OnTaskStarted;
            engine.TaskCompleted += OnTaskCompleted;
        }

        private const string EventScriptFormat = "if (typeof document.raiseEngineEvent!='undefined'){{document.raiseEngineEvent('{0}',{1});}}";

        private void OnTaskCompleted(object sender, TaskEventData e)
        {
            if (!e.HasContent())
            {
                return;
            }

            var script = string.Format(EventScriptFormat, "engine_task_start", e.ToJson());
            Browser.EvaluateScriptAsync(script);
        }

        private void OnTaskStarted(object sender, TaskEventData e)
        {
            if (!e.HasContent())
            {
                return;
            }

            var script = string.Format(EventScriptFormat, "engine_task_finish", e.ToJson());
            Browser.EvaluateScriptAsync(script);
        }

        private void OnIssueOccurred(object sender, IssueEventData e)
        {
            var script = string.Format(EventScriptFormat, "issue_occurred", e.ToJson());
            Browser.EvaluateScriptAsync(script);
        }
    }
}
