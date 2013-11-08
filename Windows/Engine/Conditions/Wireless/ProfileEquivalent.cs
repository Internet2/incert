using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using Org.InCommon.InCert.Engine.Extensions;
using log4net;

namespace Org.InCommon.InCert.Engine.Conditions.Wireless
{
    public class ProfileEquivalent:AbstractCondition
    {
        private static readonly ILog Log = Logger.Create();
        
        public string ProfilePath
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }

        public string ProfileName
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }
        
        public ProfileEquivalent(IEngine engine):base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ProfileName))
                    return new BooleanReason(false, "Profile name not specified");

                var instances = NetworkUtilities.GetWirelessAdapters();
                if (!instances.Any())
                    return new BooleanReason(false, "No wireless adapters are present");

                foreach (var instance in instances)
                {
                    var currentText = instance.GetTextForProfile(ProfileName);
                    if (string.IsNullOrWhiteSpace(currentText))
                        return new BooleanReason(false, "The profile {0} is not present on this computer", ProfileName);

                    var currentXml = XDocument.Parse(currentText);

                    var task = Task<XDocument>.Factory.StartNew(()=>GetRemoteProfileText(ProfilePath));
                    task.WaitUntilExited();

                    var compareXml = task.Result;
                    if (compareXml == null)
                        return new BooleanReason(false, "Could not retrieve compare profile content");

                    var comparer = new XNodeEqualityComparer();
                    if (!comparer.Equals(currentXml, compareXml))
                        return new BooleanReason(false, "The computer's {0} profile differs from the target profile {1}", ProfileName, Path.GetFileName(ProfilePath));
                }

                return new BooleanReason(true, "The computer's {0} profile is the same as the target profile {1}", ProfileName, Path.GetFileName(ProfilePath));
            }
            catch (Exception e)
            {
                return new BooleanReason(e);
            }
        }

        public override bool IsInitialized()
        {
            return IsPropertySet("ProfileName") && IsPropertySet("ProfileKey");
        }

        public override void ConfigureFromNode(XElement node)
        {
            base.ConfigureFromNode(node);
            ProfilePath = XmlUtilities.GetTextFromAttribute(node, "profilePath");
            ProfileName = XmlUtilities.GetTextFromAttribute(node, "profileName");
        }

        private static XDocument GetRemoteProfileText(string uri)
        {
            try
            {
                return XDocument.Load(uri);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to retrieve profile content from uri {0}: {1}", uri, e.Message);
                return null;
            }
        }
    }
}
