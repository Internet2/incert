using System;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.SystemInfo
{
    class ServicePackAtLeast:AbstractCondition
    {
        private int _value;
        
        public ServicePackAtLeast(IEngine engine):base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            try
            {
                var version = SystemUtilities.GetServicePackVersion();
                return version < _value 
                    ? new BooleanReason(false, "This computer does not have service pack {0} installed. Computer's version = {1}", _value, version) 
                    : new BooleanReason(true, "This computer has service pack {0} or higher installed", _value);
            }
            catch (Exception e)
            {
                return new BooleanReason(false, "An exception occurred while trying to evaluate the condition: {0}", e.Message);
            }
        }

        public override bool IsInitialized()
        {
            return _value > 0;
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            _value = XmlUtilities.GetIntegerFromAttribute(node, "value", 0);
        }
    }
}
