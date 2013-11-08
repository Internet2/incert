using System;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.FileAndPath
{
    class DirectoryExists:AbstractCondition
    {
        public DirectoryExists(IEngine engine):base(engine)
        {
        }

        public string FolderPath
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public override BooleanReason Evaluate()
        {
            try
            {
                
                if (string.IsNullOrWhiteSpace(FolderPath))
                    return new BooleanReason(false, "The provided path is empty or invalid");

                if (!System.IO.Directory.Exists(FolderPath))
                    return new BooleanReason(false, "The directory {0} does not exist", FolderPath);

                return new BooleanReason(true, "The directoy {0} exists", FolderPath);
            }
            catch (Exception e)
            {
                return new BooleanReason(false, "An exception occurred while evaluating the condition: " + e.Message);
            }
        }

        public override bool IsInitialized()
        {
            return IsPropertySet("FolderPath");
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            FolderPath = XmlUtilities.GetTextFromAttribute(node, "path");
        } 
    }
}
