using System;
using System.ServiceProcess;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Services
{
    class ServiceStarted : AbstractCondition
    {
        public ServiceStarted(IEngine engine)
            : base (engine)
        {
        }

        public string ServiceName
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public override BooleanReason Evaluate()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ServiceName))
                    return new BooleanReason(false, "Service name cannot be null or empty");

                using (var controller = ServiceUtilities.GetServiceInstance(ServiceName))
                {
                    if (controller == null)
                        return new BooleanReason(false, "Could not retrieve instance of service for name {0}", ServiceName);

                    if (controller.Status != ServiceControllerStatus.Running)
                        return new BooleanReason(false, "The {0} reports that its state is {1}", controller.DisplayName,
                                                 controller.Status);

                    return new BooleanReason(true, "The {0} reports that its state is {1}", controller.DisplayName,
                                             controller.Status);
                }
            }

            catch (Exception e)
            {
                return new BooleanReason(false, "An issue occurred while evaluating the condition: {0}", e.Message);
            }
        }

        public override bool IsInitialized()
        {
            return IsPropertySet("ServiceName");
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            ServiceName = XmlUtilities.GetTextFromAttribute(node, "serviceName");
        }
    }
}
