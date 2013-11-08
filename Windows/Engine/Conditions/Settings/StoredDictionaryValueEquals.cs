using System;
using System.Collections.Generic;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Settings
{
    class StoredDictionaryValueEquals : AbstractCondition
    {
        public string ObjectKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public string ValueKey
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

        public StoredDictionaryValueEquals(IEngine engine)
            : base (engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            try
            {
                var comparison = StringComparison.Ordinal;
                if (IgnoreCase)
                    comparison = StringComparison.OrdinalIgnoreCase;

                var dictionary = SettingsManager.GetTemporaryObject(ObjectKey) as Dictionary<string, string>;
                if (dictionary == null)
                    return new BooleanReason(false, "No valid dictionary exists for key {0}", ObjectKey);

                if (!dictionary.ContainsKey(ValueKey))
                    return new BooleanReason(false, "The key {0} does not exist in the dictionary", ValueKey);

                if (!dictionary[ValueKey].Equals(Value, comparison))
                    return new BooleanReason(false, "The dictionary value ('{0}') is not equal to '{1}'", dictionary[ValueKey], Value);

                return new BooleanReason(true, "The dictionary value ('{0}') is equal to '{1}'", dictionary[ValueKey], Value);
            }
            catch (Exception e)
            {
                return new BooleanReason(false, "An exception occurred while evaluating the condition: {0}", e.Message);
            }
        }

        public override bool IsInitialized()
        {
            return (IsPropertySet("ObjectKey") 
                && IsPropertySet("ValueKey") 
                && IsPropertySet("Value"));
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            ObjectKey = XmlUtilities.GetTextFromAttribute(node, "objectkey");
            ValueKey = XmlUtilities.GetTextFromAttribute(node, "valuekey");
            Value = XmlUtilities.GetTextFromAttribute(node, "value");
            IgnoreCase = XmlUtilities.GetBooleanFromAttribute(node, "ignoreCase",false);
        }
    }
}
