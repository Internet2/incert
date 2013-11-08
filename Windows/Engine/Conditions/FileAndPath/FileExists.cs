using System;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.FileAndPath
{
    class FileExists : AbstractCondition
    {
        public FileExists(IEngine engine):base(engine)
        {
        }

        public string Key
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }
        
        public override BooleanReason Evaluate()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Key))
                    return new BooleanReason(false, "The provided path is empty or invalid");

                if (!System.IO.File.Exists(Key))
                    return new BooleanReason(false, "The file {0} does not exist", Key);

                return new BooleanReason(true, "The file {0} exists", Key);
            }
            catch (Exception e)
            {
                return new BooleanReason(false, "An exception occurred while evaluating the condition: " + e.Message);
            }
        }

        public override bool IsInitialized()
        {
            return IsPropertySet("Key");
        }

        public override void ConfigureFromNode(XElement node)
        {
            Key = XmlUtilities.GetTextFromAttribute(node, "key");
        }
    }
}
