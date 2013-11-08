using System;
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
    class GetCampusForUser : AbstractTask
    {
        [PropertyAllowedFromXml]
        public string Username
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string SettingKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public GetCampusForUser(IEngine engine)
            : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var request = EndpointManager.GetContract<AbstractUserCampusContract>(EndPointFunctions.GetUserCampus);
                request.User = Username;

                var result = request.MakeRequest<Campus>();
                if (result == null)
                    return request.GetErrorResult();

                SettingsManager.SetTemporarySettingString(SettingKey, result.ShortName);
                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return string.Format("Get campus for user ({0})", Username);
        }
    }
}
