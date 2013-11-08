using Org.InCommon.InCert.DataContracts;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.WebServices;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using Org.InCommon.InCert.Engine.WebServices.Contracts;
using Org.InCommon.InCert.Engine.WebServices.Managers;

namespace Org.InCommon.InCert.Engine.Tasks.Downloader
{
    class GetFileInfoFromUrl : AbstractTask
    {
        [PropertyAllowedFromXml]
        public string Url
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

        public GetFileInfoFromUrl(IEngine engine)
            : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            if (string.IsNullOrWhiteSpace(Url))
                return new CouldNotRetrieveFileInfo { Issue = "No url specified" };

            if (string.IsNullOrWhiteSpace(SettingKey))
                return new CouldNotRetrieveFileInfo { Issue = "No settings key specified" };


            var request = EndpointManager.GetContract<AbstractUrlContract>(EndPointFunctions.GetFileInfo);
            request.Url = Url;

            var result = request.MakeRequest<FileInfoWrapper>();
            if (result == null)
                return request.GetErrorResult();

            result.BaseUrl = UriUtilities.GetUriWithoutTarget(request.GetEndpointUrl());

            SettingsManager.SetTemporaryObject(SettingKey, result);
            return new NextResult();
        }

        public override string GetFriendlyName()
        {
            return "Get file info from url";
        }
    }
}
