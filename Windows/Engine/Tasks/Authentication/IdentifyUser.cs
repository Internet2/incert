using System;
using System.Collections.Generic;
using Org.InCommon.InCert.DataContracts;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.WebServices.Contracts;
using Org.InCommon.InCert.Engine.WebServices.Managers;

namespace Org.InCommon.InCert.Engine.Tasks.Authentication
{
    class IdentifyUser : AbstractWebServiceAuthenticationTask
    {
        private readonly List<string> _properties = new List<string>();
        private readonly List<string> _groupPaths = new List<string>();

        public IdentifyUser(IEngine engine)
            : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string Property
        {
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    return;

                _properties.Add(value);
            }
        }

        [PropertyAllowedFromXml]
        public string GroupPath
        {
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    return;

                _groupPaths.Add(value);
            }
        }

        [PropertyAllowedFromXml]
        public string UserPropertiesKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string GroupPathsKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var request =
                    EndpointManager.GetContract<AbstractIdentityQueryContract>(EndPointFunctions.IdentityQuery);

                request.LoginId = SettingsManager.GetTemporarySettingString(UsernameKey);
                request.Credential1 = SettingsManager.GetTemporarySettingString(PassphraseKey);
                request.Credential2 = SettingsManager.GetTemporarySettingString(Credential2Key);
                request.Credential3 = SettingsManager.GetTemporarySettingString(Credential3Key);
                request.Credential4 = SettingsManager.GetTemporarySettingString(Credential4Key);
                request.Provider = CertificateProvider;
                request.Properties = _properties;
                request.GroupPaths = _groupPaths;

                var result = request.MakeRequest<IdentityQueryResult>();
                if (result == null)
                    return request.GetErrorResult();
                
                SettingsManager.SetTemporaryObject(UserPropertiesKey, result.Properties);
                SettingsManager.SetTemporaryObject(GroupPathsKey, result.Groups);

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }

        }

        public override string GetFriendlyName()
        {
            return "Identify user";
        }
    }
}
