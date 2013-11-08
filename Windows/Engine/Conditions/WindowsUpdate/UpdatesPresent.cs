using System;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using WUApiLib;

namespace Org.InCommon.InCert.Engine.Conditions.WindowsUpdate
{
    class UpdatesPresent:AbstractCondition
    {
        public string ObjectKey
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }
        
        public UpdatesPresent(IEngine engine):base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            try
            {
                var updates = SettingsManager.GetTemporaryObject(ObjectKey) as UpdateCollection;
                if (updates == null)
                    return new BooleanReason(false, "No update collection found in temporary object store matching key {0}", ObjectKey);

                if (updates.Count ==0)
                    return new BooleanReason(false, "No updates found in collection for key {0}", ObjectKey);

                return new BooleanReason(true, "{0} updates found in collection for key {1}", updates.Count, ObjectKey);
            }
            catch (Exception e)
            {
                return new BooleanReason(e);
            }
        }

        public override bool IsInitialized()
        {
            return IsPropertySet("ObjectKey");
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            ObjectKey = XmlUtilities.GetTextFromAttribute(node, "objectKey");
        }
    }
}
