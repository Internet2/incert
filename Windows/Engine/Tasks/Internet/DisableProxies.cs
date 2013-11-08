using System;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Tasks.Internet
{
    class DisableProxies : AbstractTask
    {
        private const string RegistryKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings";
        
        public DisableProxies(IEngine engine)
            : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                using (
                    var key = RegistryUtilities.RegistryRootValues.CurrentUser.OpenRegistryKey(RegistryKeyPath, true))
                {
                    if (key == null)
                        throw new Exception("Could not open internet settings registry key");

                    key.SetValue("ProxyEnable", 0);
                }

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Disable proxies";
        }
    }
}
