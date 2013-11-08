using System;
using System.Runtime.InteropServices;
using NetFwTypeLib;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Firewall
{
    class IsFirewallEnabled:AbstractFirewallCondition
    {
        public IsFirewallEnabled(IEngine engine):base(engine)
        {
        }

        private NET_FW_PROFILE_TYPE2_ _profile = NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL;

        public override BooleanReason Evaluate()
        {
            INetFwPolicy2 policy = null;
            try
            {
                policy = GetFirewallPolicy();
                if (policy == null)
                    throw new Exception("Could not instantiate firewall dialogsManager.");

                foreach (NET_FW_PROFILE_TYPE2_ enumValue in Enum.GetValues(typeof(NET_FW_PROFILE_TYPE2_)))
                {
                    if (enumValue == NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL)
                        continue;

                    // ReSharper disable BitwiseOperatorOnEnumWithoutFlags
                    // This enumeration is defined by microsoft, so the flags attribute is not set
                    if ((_profile & enumValue) != enumValue)
                    // ReSharper restore BitwiseOperatorOnEnumWithoutFlags
                        continue;
                    
                    if (!policy.FirewallEnabled[enumValue])
                        return new BooleanReason(false, "The firewall is not enabled for the profile {0}", enumValue);
                }

                return new BooleanReason(true, "The Windows Firewall is enabled.");
            }
            catch (Exception e)
            {
                return new BooleanReason(false, "An issue occurred while querying firewall status ({0}); assuming firewall not enabled.",e.Message);
            }
            finally
            {
                if (policy != null)
                    Marshal.ReleaseComObject(policy);
            }
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            _profile = XmlUtilities.GetEnumValueFromAttribute(
                node, "profile", NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL);
        } 

        public override bool IsInitialized()
        {
            return true;
        }
    }
}
