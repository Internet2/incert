using System;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    public class SetHtmlBannerLinkPolicy : AbstractTask
    {
        public SetHtmlBannerLinkPolicy(IEngine engine) : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public LinkPolicy LinkPolicy { get; set; }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                BannerManager.LinkPolicy = LinkPolicy;
                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override void ConfigureFromNode(XElement node)
        {
            base.ConfigureFromNode(node);
            LinkPolicy = (LinkPolicy) XmlUtilities.GetEnumValueFromChildNode(node, "LinkPolicy", LinkPolicy.Internal);
        }

        public override string GetFriendlyName()
        {
            return string.Format("Set html banner link policy to {0}", LinkPolicy);
        }
    }
}