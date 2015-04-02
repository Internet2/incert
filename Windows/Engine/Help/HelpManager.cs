﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using log4net;
using Org.InCommon.InCert.Engine.Dynamics;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Managers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Properties;
using Org.InCommon.InCert.Engine.WebServices.Contracts;
using Org.InCommon.InCert.Engine.WebServices.DataWrappers;
using Org.InCommon.InCert.Engine.WebServices.Managers;

namespace Org.InCommon.InCert.Engine.Help
{
    public class HelpManager : PropertyNotifyBase, IHelpManager
    {
        
        private readonly IEndpointManager _endpointManager;
        private readonly IValueResolver _valueResolver;
        private readonly List<IHelpTopic> _helpTopics = new List<IHelpTopic>();
        private static readonly ILog Log = Logger.Create();
        private readonly HelpDialogModel _model;

        public HelpManager(IEndpointManager endpointManager, IAppearanceManager appearanceManager, IValueResolver valueResolver)
        {
            _endpointManager = endpointManager;
            _valueResolver = valueResolver;

            _model = new HelpDialogModel(this, appearanceManager);
           
          
            PreserveContentText = "Preserve this content in a new window when the utility exits.";
            DialogTitle = "Help Resources";
            InitialLeftOffset = -75;
            InitialTopOffset = -75;
        }

        public void ShowHelpTopic(string value, AbstractDialogModel model)
        {
            try
            {
                var resolvedValue = _valueResolver.Resolve(value, true);
                UploadTopicReportingEntry(resolvedValue);
                
                var topics =
                    _helpTopics.FindAll(e => e.Identifier.Equals(
                        resolvedValue,
                        StringComparison.InvariantCultureIgnoreCase));

                if (!topics.Any())
                    throw new Exception("topic does not exist");

                var topic = topics.FirstOrDefault(e => e.IsValid);
                if (topic == null)
                    throw new Exception("topic exists, but not valid entries found");

                if (model == null)
                    _model.ShowUri(ResolveUri(topic.Url));
                else
                    _model.ShowUri(ResolveUri(topic.Url), model.Left, model.Top);

            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to open the help topic {0}: {1}", value, e.Message);
            }
        }

        private Uri ResolveUri(string url)
        {
            try
            {
                var uri = new Uri(url, UriKind.RelativeOrAbsolute);
                if (uri.IsAbsoluteUri)
                    return uri;

                var baseUri = new Uri(BaseHelpUrl);
                return new Uri(baseUri, uri);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to resolve a topic url ({0}): {1}", url, e.Message);
                return null;
            }

        }

        private void UploadTopicReportingEntry(string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ReportingEntry))
                    return;

                if (string.IsNullOrWhiteSpace(value))
                    return;

                var request =
                    _endpointManager.GetContract<AbstractReportingContract>(EndPointFunctions.UploadAsyncReport);
                request.Name = ReportingEntry;
                request.Value = value;

                request.MakeRequest<NoContentWrapper>();
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to upload a topic reporting entry: {0}", e.Message);
            }
            
        }
        
        public bool TopicExists(string value)
        {
            var topics =
                _helpTopics.FindAll(e => e.Identifier.Equals(
                    _valueResolver.Resolve(value,true),
                    StringComparison.InvariantCultureIgnoreCase));

            return topics.Any(e => e.IsValid);
        }

        public void Initialize()
        {

        }

        public bool ImportTopicsFromXml(XElement node)
        {
            try
            {
                if (node == null)
                {
                    Log.Warn("empty xml document passed to ImportTopicsFromXml; cannot import topics");
                    return false;
                }

                var branchesNode = node.Element("HelpTopics");
                if (branchesNode == null)
                    return false;

                foreach (var topicNode in branchesNode.Elements())
                {
                    var topic = AbstractImportable.GetInstanceFromNode<HelpTopic>(topicNode);
                    if (topic == null || !topic.Initialized())
                        continue;

                    _helpTopics.Add(topic);
                }

                return true;
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while importing help topics from xml: {0}", e.Message);
                return false;
            }
        }

        public bool ImportTopics(List<HelpTopic> topics)
        {
            try
            {

                return true;
            }
            catch (Exception e)
            {

                Log.WarnFormat("An exception occurred while importing help topics from list: {0}", e.Message);
                return false;
            }
        }

        public string TopBannerText { get; set; }
        public string HomeUrl { get; set; }
        public string DialogTitle { get; set; }

        public string PreserveContentText { get; set; }
        public string AppendToExternalUris { get; set; }
        public string BaseHelpUrl { get; set; }
        public string ReportingEntry { get; set; }

        public bool PreserveContentOnExit
        {
            get { return Properties.Settings.Default.PreserveHelpContentOnExit; }
            set
            {
                Properties.Settings.Default.PreserveHelpContentOnExit = value;
                Properties.Settings.Default.Save();
            }
        }

        public double InitialLeftOffset { get; set; }
        public double InitialTopOffset { get; set; }
        
        public void OpenCurrentViewInExternalWindow()
        {
            try
            {
                if (_model == null)
                    return;

                if (!_model.ShowingContent)
                    return;

                var uri = GetExternalUri(_model.CurrentContentUrl);
                if (uri == null)
                    return;

                var info = new ProcessStartInfo
                    {
                        UseShellExecute = true,
                        Verb = "open",
                        FileName = uri.AbsoluteUri
                    };

                using (Process.Start(info))
                {
                }

            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to open a help topic in an external window: {0}", e.Message);
            }
        }

        private Uri GetExternalUri(string url)
        {
            return GetExternalUri(new Uri(url, UriKind.RelativeOrAbsolute));
        }

        private Uri GetExternalUri(Uri uri)
        {
            if (uri == null)
                return null;

            if (string.IsNullOrWhiteSpace(AppendToExternalUris))
                return uri;

            var delimiter = "?";
            if (!string.IsNullOrWhiteSpace(uri.Query))
                delimiter = "&";

            return new Uri(uri, string.Format("{0}{1}", delimiter, AppendToExternalUris));
        }
    }
}
