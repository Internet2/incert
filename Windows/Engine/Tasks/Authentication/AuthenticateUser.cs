using System;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.WebServices.Contracts;
using Org.InCommon.InCert.Engine.WebServices.DataWrappers;
using Org.InCommon.InCert.Engine.WebServices.Managers;

namespace Org.InCommon.InCert.Engine.Tasks.Authentication
{
    class AuthenticateUser : AbstractWebServiceAuthenticationTask
    {
        public AuthenticateUser(IEngine engine)
            : base(engine)
        {
        
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var request =
                    EndpointManager.GetContract<AbstractAuthenticationContract>(
                        EndPointFunctions.AuthenticateUser);
                
                request.LoginId = SettingsManager.GetTemporarySettingString(UsernameKey);
                request.Credential1 = SettingsManager.GetTemporarySettingString(PassphraseKey);
                request.Credential2 = SettingsManager.GetTemporarySettingString(Credential2Key);
                request.Credential3 = SettingsManager.GetTemporarySettingString(Credential3Key);
                request.Credential4 = SettingsManager.GetTemporarySettingString(Credential4Key);
                request.Provider = CertificateProvider;

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
            return "Authenticate user";
        }
    }
}
