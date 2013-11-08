using System;
using Org.InCommon.InCert.DataContracts;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.WebServices.Contracts;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Location
{
    class GetCurrentLocation:AbstractTask
    {
        private static readonly ILog Log = Logger.Create();
       
        public GetCurrentLocation(IEngine engine)
            : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string SettingKey { get; set; }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SettingKey))
                    throw new Exception("Setting key not specified");
                
                var request =
                    EndpointManager.GetContract<AbstractLocationQueryContract>(EndPointFunctions.LocationQuery);
                request.IgnoreCertificateErrors = true;
                var result = request.MakeRequest<LocationQueryResult>();
                if (result == null)
                {
                    Log.WarnFormat("An issue occurred while querying server for location data: {0}",
                        request.GetErrorResult().GetDetails());
                    return null;
                }

                SettingsManager.SetTemporarySettingString(SettingKey, result.Location);

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Get computer's current location";
        }
    }
}
