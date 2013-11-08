using System;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using Org.InCommon.InCert.Engine.WebServices.Contracts;
using Org.InCommon.InCert.Engine.WebServices.DataWrappers;
using Org.InCommon.InCert.Engine.WebServices.Managers;

namespace Org.InCommon.InCert.Engine.Tasks.Reporting
{
    internal class UploadMacAddressReport : AbstractTask
    {
        public UploadMacAddressReport(IEngine engine)
            : base (engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var request =
                    EndpointManager.GetContract<AbstractMacAddressReportContract>(EndPointFunctions.MacAddressReport);
                request.Addresses = NetworkUtilities.GetMacAddresses(NetworkUtilities.GetActualAdapters());

                var result = request.MakeRequest<NoContentWrapper>();
                return result == null ? request.GetErrorResult() : new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return string.Format("Upload Mac address report");
        }
    }
}
