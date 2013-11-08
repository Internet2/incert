using System;
using System.IO;
using Microsoft.Win32;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Network
{
    class EnableNtlmV2:AbstractTask
    {
        private static readonly ILog Log = Logger.Create();
        
        public EnableNtlmV2(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var path = Path.Combine(new[] {"System", "CurrentControlSet", "Control", "Lsa"});
                
                using (
                    var key =
                        Registry.LocalMachine.OpenSubKey(path, true))
                {
                    if (key == null)
                        throw  new Exception(string.Format("Could not open registry key {0}", path));

                    var level = key.GetValue("lmcompatibilitylevel").ToIntOrDefault(0);
                    
                    // if level==5 nothing to do
                    if (level ==5)
                        return new NextResult();

                    Log.InfoFormat("Ntlm level set from {0} to 5", level);
                    key.SetValue("lmcompatibilitylevel", 5);
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
            return "Enable NtlmV2";
        }
    }
}
