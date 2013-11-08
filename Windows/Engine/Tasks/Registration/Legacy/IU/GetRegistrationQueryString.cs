using System;
using System.Linq;
using Org.InCommon.InCert.DataContracts;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.Registration;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Tasks.Authentication;
using Org.InCommon.InCert.Engine.Utilities;
using Org.InCommon.InCert.Engine.WebServices.Contracts;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using RestSharp.Contrib;

namespace Org.InCommon.InCert.Engine.Tasks.Registration.Legacy.IU
{
    class GetRegistrationQueryString : AbstractWebServiceAuthenticationTask
    {

        public GetRegistrationQueryString(IEngine engine) : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string ResultKey { get; set; }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var wiredAddresses = NetworkUtilities.GetMacAddresses(NetworkUtilities.GetWiredAdapters());
                var wirelessAddresses = NetworkUtilities.GetMacAddresses(NetworkUtilities.GetWirelessAdapters());

                if (!wiredAddresses.Any() && !wirelessAddresses.Any())
                    return new CouldNotRegisterComputer { Issue = "Could not determine mac addresses to register" };

                var request = EndpointManager.GetContract<AbstractRegistrationContract>(EndPointFunctions.GetRegistrationQueryString);

                request.LoginId = SettingsManager.GetTemporarySettingString(UsernameKey);
                request.Credential1 = SettingsManager.GetTemporarySettingString(PassphraseKey);
                request.Credential2 = SettingsManager.GetTemporarySettingString(Credential2Key);
                request.Credential3 = SettingsManager.GetTemporarySettingString(Credential3Key);
                request.Credential4 = SettingsManager.GetTemporarySettingString(Credential4Key);
                request.Provider = CertificateProvider;
                request.WiredAddresses = wiredAddresses;
                request.WirelessAddresses = wirelessAddresses;

                var result = request.MakeRequest<LegacyRegistrationQueryResult>();
                if (result == null)
                    return request.GetErrorResult();

                if (string.IsNullOrWhiteSpace(result.Result))
                    return new CouldNotRegisterComputer { Issue = "Could not retrieve valid query string" };

                SettingsManager.SetTemporarySettingString(ResultKey, HttpUtility.UrlEncode(result.Result));

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Get legacy registration query string";
        }
    }
}
