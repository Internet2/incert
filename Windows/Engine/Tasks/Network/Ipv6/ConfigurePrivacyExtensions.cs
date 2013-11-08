using System;
using System.Diagnostics;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Network.Ipv6
{
    class ConfigurePrivacyExtensions:AbstractTask
    {
        private static readonly ILog Log = Logger.Create();

        [PropertyAllowedFromXml]
        public bool Enabled { get; set; }
        
        public ConfigurePrivacyExtensions(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
           try
            {
                var command = "interface ipv6 set global randomizeidentifiers=disabled";
                if (Enabled)
                    command = "interface ipv6 set global randomizeidentifiers=enabled";

                var info = new ProcessStartInfo(PathUtilities.GetSystemUtilityPath("netsh.exe"))
                    {
                        Arguments = command,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true
                    };

                var process = Process.Start(info);
                process.WaitUntilExited();

                using (var reader = process.StandardOutput)
                {
                    var logResult = reader.ReadToEnd().RemoveTrailingLineFeeds();
                    Log.InfoFormat("Netsh command ({0}) returned: {1}", command, logResult);
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
            return string.Format("Configure private extensions (Enabled = {0})", Enabled);
        }
    }
}
