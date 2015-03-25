using System;
using System.Net;
using CefSharp;
using CefSharp.Wpf;
using log4net;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results.Errors.UserInterface;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers;
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

        public string GetAddress()
        {
            if (!Browser.Dispatcher.CheckAccess())
            {
                return Browser.Dispatcher.Invoke(() => GetAddress());
            }

            return Browser.Address;
        }

        public void SetAddress(string value)
        {
            if (Browser.Dispatcher.InvokeIfRequired(() => SetAddress(value)))
            {
                return;
            }
            ;

            Browser.Address = value;
        }

        public void Reload()
        {
            if (Browser.Dispatcher.InvokeIfRequired(Reload))
            {
                return;
            }
            ;

            Browser.Reload();
        }

        public override T LoadContent<T>(AbstractContentWrapper wrapper)
        {
            var content = new ChromiumWebBrowser();
            var browserWrapper = wrapper as BrowserContentWrapper;
            if (browserWrapper == null)
            {
                throw new InvalidCastException("Could not cast wrapper to valid type");
            }

            content.RegisterJsObject("engine", new ScriptingModel(wrapper.Engine, RootDialogModel, content));
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
    }
}