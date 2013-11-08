using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.SystemRestore
{
    class EnableSystemRestore:AbstractTask
    {
        private static readonly ILog Log = Logger.Create();
        public EnableSystemRestore(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                using (var task = Task<uint>.Factory.StartNew(() => NativeCode.Wmi.SystemRestore.Enable("", true)))
                {
                    task.WaitUntilExited();

                    if (task.Result !=0)
                        throw new Win32Exception((int)task.Result);
                }
                
                Log.Info("System restore enabled");
                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Enable system restore";
        }
    }
}
