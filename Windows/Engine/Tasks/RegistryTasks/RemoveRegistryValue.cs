using System;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.RegistryTasks
{
    class RemoveValue:AbstractRegistryTask
    {
        private static readonly ILog Log = Logger.Create();
        
        public RemoveValue(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                using (var key = RegistryRoot.OpenRegistryKey(RegistryKey, true))
                {
                    if (key == null)
                    {
                        Log.InfoFormat("The registry key {0} {1} does not exist", RegistryRoot, RegistryKey);
                        return new NextResult();
                    }

                    key.DeleteValue(RegistryValue, false);
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
            return string.Format("Remove registry value ({0})", RegistryValue);
        }
    }
}
