using System;
using System.Linq;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.Registration;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Tasks.Authentication;
using Org.InCommon.InCert.Engine.Utilities;
using Org.InCommon.InCert.Engine.WebServices.Contracts;
using Org.InCommon.InCert.Engine.WebServices.DataWrappers;
using Org.InCommon.InCert.Engine.WebServices.Managers;

namespace Org.InCommon.InCert.Engine.Tasks.Registration
{
    class RegisterComputer : AbstractWebServiceAuthenticationTask
    {
        public RegisterComputer(IEngine engine)
            : base(engine)
        {
        
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var wiredAddresses = NetworkUtilities.GetMacAddresses(NetworkUtilities.GetWiredAdapters());
                var wirelessAddresses = NetworkUtilities.GetMacAddresses(NetworkUtilities.GetWirelessAdapters());

                if (!wiredAddresses.Any() && !wirelessAddresses.Any())
                    return new CouldNotRegisterComputer { Issue = "Could not determine mac addresses to register" };

                var request = EndpointManager.GetContract<AbstractRegistrationContract>(EndPointFunctions.RegisterComputer);

                request.LoginId = SettingsManager.GetTemporarySettingString(UsernameKey);
                request.Credential1 = SettingsManager.GetTemporarySettingString(PassphraseKey);
                request.Credential2 = SettingsManager.GetTemporarySettingString(Credential2Key);
                request.Credential3 = SettingsManager.GetTemporarySettingString(Credential3Key);
                request.Credential4 = SettingsManager.GetTemporarySettingString(Credential4Key);
                request.Provider = CertificateProvider;
                request.WiredAddresses = wiredAddresses;
                request.WirelessAddresses = wirelessAddresses;

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
            return "Register computer";
        }
    }
}
