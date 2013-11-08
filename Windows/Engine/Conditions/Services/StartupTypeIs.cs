using System;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Services
{
    class StartupTypeIs:AbstractCondition
    {
        public StartupTypeIs(IEngine engine):base(engine)
        {
        }

        public string ServiceName
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public ServiceUtilities.ServiceStartupValues StartupValue {get; set; }

        public override BooleanReason Evaluate()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ServiceName))
                    return new BooleanReason(false, "Service name cannot be null or empty");

                using (var controller = ServiceUtilities.GetServiceInstance(ServiceName))
                {
                    if (controller == null)
                        return new BooleanReason(false, "Could not retrieve instance of service for name {0}",
                                                 ServiceName);

                    var currentValue = ServiceUtilities.GetServiceStartupValue(controller);
                    return currentValue != StartupValue 
                        ? new BooleanReason(false, "The service {0} has the startup value of {1}", controller.ServiceName, currentValue) 
                        : new BooleanReason(true, "The service {0} has the startup value of {1}", controller.ServiceName, currentValue);
                }
            }
            catch (Exception e)
            {
                return new BooleanReason(false, "An issue occurred while evaluating the condition: {0}", e.Message);
            }

        }

        public override bool IsInitialized()
        {
            return StartupValue != ServiceUtilities.ServiceStartupValues.Unknown && IsPropertySet("ServiceName");
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            ServiceName = XmlUtilities.GetTextFromAttribute(node, "serviceName");
            StartupValue = XmlUtilities.GetEnumValueFromAttribute(node, "type",
                                                                  ServiceUtilities.ServiceStartupValues.Unknown);
        }
    }
}
