using System;
using System.Runtime.InteropServices;
using NetFwTypeLib;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Firewall
{
    class EnableFirewall:AbstractFirewallTask
    {
        private static readonly ILog Log = Logger.Create();

        public EnableFirewall(IEngine engine) : base(engine)
        {
        }
        
        public override IResult Execute(IResult previousResults)
        {
            INetFwPolicy2 policy = null;
            try
            {
                var profiles = Profiles.All;
                if (Profile != Profiles.None)
                    profiles = Profile;
                
                policy = GetFirewallPolicy();
                if (policy == null)
                    throw new Exception("Could not initialize firewall dialogsManager.");

                // we need to see if each value in the NET_FW_PROFILE_TYPE2_ enumeration is present
                // and if it is, set it individually; the 'All' value is not accepted, not are
                // bitwise combinations of the other values
                foreach (NET_FW_PROFILE_TYPE2_ enumValue in Enum.GetValues(typeof(NET_FW_PROFILE_TYPE2_)))
                {
                    // ignore the all value
                    if (enumValue == NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL)
                        continue;

                    // here we need to convert to integers to compare Profile enum value
                    // to NET_FW_PROFILE_TYPE2_ equivalent
                    // if the value is not present in the profiles value, continue
                    if (((int)profiles & (int)enumValue) != (int)enumValue)
                        continue;

                    // if value is present, process
                    if (policy.FirewallEnabled[enumValue])
                        continue;

                    policy.FirewallEnabled[enumValue] = true;
                    Log.InfoFormat("Firewall enabled for {0} profile", enumValue);
                }
                
                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
            finally
            {
                if (policy != null)
                {
                    Marshal.ReleaseComObject(policy);
                }
            }
        }

        public override string GetFriendlyName()
        {
            return "Enable Windows Firewall";
        }
    }
}
