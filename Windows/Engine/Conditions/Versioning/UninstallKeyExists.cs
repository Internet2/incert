using System;
using System.IO;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Versioning
{
    class UninstallKeyExists : AbstractCondition
    {
        public UninstallKeyExists(IEngine engine)
            : base (engine)
        {
        }

        public string KeyName
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public override BooleanReason Evaluate()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(KeyName))
                    return new BooleanReason(false, "No keyname is specified");

                var normalPath = Path.Combine(new[] { "SOFTWARE", "Microsoft", "Windows", "CurrentVersion", "Uninstall", KeyName });
                if (RegistryUtilities.RegistryRootValues.LocalMachine.KeyExists(normalPath))
                        return new BooleanReason(true, "The registry key {0} exists", normalPath);
                
                var wow6432Path = Path.Combine(new[] { "SOFTWARE", "Wow6432Node", "Microsoft", "Windows", "CurrentVersion", "Uninstall", KeyName });
                if (RegistryUtilities.RegistryRootValues.LocalMachine.KeyExists(wow6432Path))
                        return new BooleanReason(true, "The registry key {0} exists",  wow6432Path);
                
                return new BooleanReason(false, "No uninstall key exists for the keyname {0}", KeyName);
            }
            catch (Exception e)
            {
                return new BooleanReason(false, "An issue occurred while evaluating the condition: {0}", e.Message);
            }
        }


        public override bool IsInitialized()
        {
            return IsPropertySet("KeyName");
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            KeyName = XmlUtilities.GetTextFromAttribute(node, "key");
        }
    }
}
