using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using CefSharp;
using CefSharp.Wpf;
using log4net;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results.Errors.UserInterface;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.BannerWrappers;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels.KeyboardHandlers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels.LifespanHandlers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels.SchemeHandlers;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContainerModels
{
    public class BrowserContentModel : AbstractContainerModel
    {
        private readonly string _url;
        private readonly Dictionary<string, string> _redirects;
        private readonly LinkPolicy _linkPolicy;
        private static readonly ILog Log = Logger.Create();
        private IScriptingModel _scriptingModel;

        public BrowserContentModel(AbstractDialogModel parentModel, string url, Dictionary<string, string> redirects,LinkPolicy linkPolicy)
            : base(parentModel)
        {
            _url = url;
            _redirects = redirects;
           _linkPolicy = linkPolicy;
        }

        public bool IsLoaded { get; private set; }

        private ChromiumWebBrowser Browser { get; set; }

        public string GetAddress()
        {
            return Browser.Dispatcher.InvokeRequired() 
                ? Browser.Dispatcher.Invoke(() => GetAddress()) 
                : Browser.Address;
        }

        public void SetAddress(string value)
        {
            if (Browser.Dispatcher.InvokeIfRequired(() => SetAddress(value)))
            {
                return;
            }

            Browser.Address = value;
        }

        public void Reload()
        {
            if (Browser.Dispatcher.InvokeIfRequired(Reload))
            {
                return;
            }

            Browser.Reload();
        }

        public override T LoadContent<T>(AbstractContentWrapper wrapper)
        {
            throw new NotImplementedException();
        }

        public override T LoadContent<T>(AbstractBanner banner)
        {
            Content = CreateInstance(banner);
            Browser = Content as ChromiumWebBrowser;
            return Content as T;
        }

        protected override DependencyObject CreateInstance(AbstractBanner banner)
        {
            var content = new ChromiumWebBrowser { DataContext = this };
            _scriptingModel = new ScriptingModel(banner.Engine, RootDialogModel, content);
            content.RegisterJsObject("engine", _scriptingModel);
            content.RequestHandler = new RequestHandler(_url, _redirects, _linkPolicy);
            content.LifeSpanHandler = new LifespanHandler();
            content.KeyboardHandler = new KeyboardHandler(_scriptingModel);

            content.ConsoleMessage += ConsoleMessageHandler;
            content.LoadError += OnContentLoadError;
            content.FrameLoadEnd += OnFrameLoadEnd;
            content.FrameLoadStart += OnFrameLoadStart;

            return content;
        }


        private void OnFrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            if (sender.InvokeIfRequired(() => OnFrameLoadStart(sender, e)))
            {
                return;
            }

            if (!e.IsMainFrame)
            {
                return;
            }

            Browser.Focus();
            IsLoaded = false;
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
            var statusCode = Enum.IsDefined(typeof(HttpStatusCode), e.HttpStatusCode)
                ? ((HttpStatusCode)e.HttpStatusCode).ToString().SplitByCapitalLetters()
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

        public override void Dispose()
        {
            if (_scriptingModel != null)
            {
                _scriptingModel.Dispose();
            }

            if (Browser != null)
            {
                Browser.KeyboardHandler = null;
                Browser.RequestHandler = null;
                Browser.LifeSpanHandler = null;
                Browser.Dispose();
            }
        }
    }
}