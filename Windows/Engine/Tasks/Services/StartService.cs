using System;
using System.Threading.Tasks;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Tasks.Services
{
    class StartService : AbstractTask
    {
        public StartService(IEngine engine)
            : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string ServiceName
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var task = Task<IResult>.Factory.StartNew(()=>ServiceUtilities.EnableService(ServiceName));
                task.WaitUntilExited();

                return task.Result;
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return string.Format("Start {0} service", ServiceName);
        }
    }
}
