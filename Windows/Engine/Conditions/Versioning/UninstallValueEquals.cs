using System;
using System.IO;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Versioning
{
    class UninstallValueEquals:AbstractCondition
    {
        public UninstallValueEquals(IEngine engine):base(engine)
        {
        }

        public string KeyName
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public string ValueName
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public string Value
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public bool IgnoreCase { get; set; }

        public override BooleanReason Evaluate()
        {
            try
            {
                var comparison = StringComparison.InvariantCulture;
                if (IgnoreCase)
                    comparison = StringComparison.InvariantCultureIgnoreCase;

                var normalPath = Path.Combine(new[] { "SOFTWARE", "Microsoft", "Windows", "CurrentVersion", "Uninstall", KeyName });
                var value = GetValueFromKey(normalPath, ValueName);
                if (value.Equals(Value, comparison))
                    return new BooleanReason(true, "The value of {0} ({1}) equals {2}", ValueName, value, Value);

                var wow6432Path = Path.Combine(new[] { "SOFTWARE", "Wow6432Node", "Microsoft", "Windows", "CurrentVersion", "Uninstall", KeyName });
                value = GetValueFromKey(wow6432Path, ValueName);
                return value.Equals(Value, comparison) 
                    ? new BooleanReason(true, "The value of {0} ({1}) equals {2}", ValueName, value, Value) 
                    : new BooleanReason(false, "The value of {0} ({1}) is not equal to {2}", ValueName, value, Value);
            }
            catch (Exception e)
            {
                return new BooleanReason(false, "An exception occurred while evaluating the condition: {0}", e.Message);
            }
        }

        public override bool IsInitialized()
        {
            if (!IsPropertySet("KeyName"))
                return false;

            if (!IsPropertySet("ValueName"))
                return false;

            if (!IsPropertySet("Value"))
                return false;

            return true;
        }

        private static string GetValueFromKey(string keyPath, string valueName)
        {
            using (var key = RegistryUtilities.RegistryRootValues.LocalMachine.GetExistingKey(keyPath))
            {
                return key == null ? "" : key.GetValue(valueName).ToStringOrDefault("");
            }
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            KeyName = XmlUtilities.GetTextFromAttribute(node, "uninstallKey");
            ValueName = XmlUtilities.GetTextFromAttribute(node, "valueName");
            Value = XmlUtilities.GetTextFromAttribute(node, "value");
            IgnoreCase = XmlUtilities.GetBooleanFromAttribute(node, "ignoreCase", false);
        }
    }
}
