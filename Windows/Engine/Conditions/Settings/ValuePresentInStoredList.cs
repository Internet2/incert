using System;
using System.Collections.Generic;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using System.Linq;

namespace Org.InCommon.InCert.Engine.Conditions.Settings
{
    class ValuePresentInStoredList:AbstractCondition
    {
        private bool _ignoreCase;

        public string ObjectKey
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }

        public string Value
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }
        
        public ValuePresentInStoredList(IEngine engine):base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Value))
                    return new BooleanReason(false, "specified value is null or empty");

                var list = SettingsManager.GetTemporaryObject(ObjectKey) as List<string>;
                if (list == null)
                    return new BooleanReason(false, "No list is present for the key {0}", ObjectKey);

                var comparer = StringComparer.InvariantCulture;
                if (_ignoreCase)
                    comparer = StringComparer.InvariantCultureIgnoreCase;
                
                return !list.Contains(Value, comparer) 
                    ? new BooleanReason(false, "The list for the key {0} does not contain '{1}'", ObjectKey, Value) 
                    : new BooleanReason(true, "The list for the key {0} contains '{1}'", ObjectKey, Value);
            }
            catch (Exception e)
            {
                return new BooleanReason(e);
            }
        }

        public override bool IsInitialized()
        {
            return IsPropertySet("ObjectKey") && IsPropertySet("Value");
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            ObjectKey = XmlUtilities.GetTextFromAttribute(node, "objectKey");
            Value = XmlUtilities.GetTextFromAttribute(node, "value");
            _ignoreCase = XmlUtilities.GetBooleanFromAttribute(node, "ignoreCase", false);
        }
    }
}
