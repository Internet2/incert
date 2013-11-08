using Org.InCommon.InCert.DataContracts;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using Org.InCommon.InCert.Engine.WebServices.DataWrappers;

namespace Org.InCommon.InCert.Engine.Conditions.Versioning
{
    class FileInfoVital:AbstractCondition
    {
        public string Key
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }
        
        public FileInfoVital(IEngine engine):base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            var info = SettingsManager.GetTemporaryObject(Key) as FileInfoWrapper;
            if (info == null)
                return new BooleanReason(false, "cannot retrieve info wrapper for key {0}", Key);

            return !info.Vital ? 
                new BooleanReason(false, "The vital flag is not set") : 
                new BooleanReason(true, "The vital flag is set");
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);

            Key = XmlUtilities.GetTextFromAttribute(node, "key");
        }

        public override bool IsInitialized()
        {
            return !string.IsNullOrWhiteSpace(Key);
        }
    }
}
