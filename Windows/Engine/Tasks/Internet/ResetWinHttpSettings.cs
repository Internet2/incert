using System;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Tasks.Internet
{
    class ResetWinHttpSettings : AbstractTask
    {
        private const string RegistryKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings";

        public ResetWinHttpSettings(IEngine engine)
            : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                using (
                    var key = RegistryUtilities.RegistryRootValues.LocalMachine.OpenRegistryKey(RegistryKeyPath, true))
                {
                    if (key == null)
                        throw new Exception("Could not open internet settings registry key");

                    key.DeleteSubKey("Connections",false);
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
            return "Reset WinHttp settings";
        }
    }
}
