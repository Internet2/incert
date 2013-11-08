using System;
using System.Collections.Generic;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results.Errors.Mapping;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.LinkWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers
{
    class ErrorParagraph:SimpleParagraph
    {
        private static readonly ILog Log = Logger.Create();

        public ErrorParagraph(IEngine engine)
            : base(engine)
        {
        }


        public string ErrorInfoKey { get; set; }

        public override Type GetSupportingModelType()
        {
            return typeof(TextContentModel);
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            ErrorInfoKey = XmlUtilities.GetTextFromAttribute(node, "errorInfoKey");
        }

        public override string GetText()
        {
            var info = SettingsManager.GetTemporaryObject(ErrorInfoKey) as AbstractErrorEntry;
            if (info == null)
            {
                Log.WarnFormat("Could not retrieve error info for key {0}", ErrorInfoKey);
                return "";
            }

            return info.Text;
        }

        public override List<AbstractLink> GetLinks()
        {
            var info = SettingsManager.GetTemporaryObject(ErrorInfoKey) as AbstractErrorEntry;
            if (info == null)
            {
                Log.WarnFormat("Could not retrieve error info for key {0}", ErrorInfoKey);
                return new List<AbstractLink>();
            }

            return info.Links;

        }
        
        public override bool Initialized()
        {
            return !String.IsNullOrWhiteSpace(ErrorInfoKey);
        }
    }
}
