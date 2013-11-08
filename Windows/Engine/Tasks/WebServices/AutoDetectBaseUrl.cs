using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.Network;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Tasks.WebServices
{
    class AutoDetectBaseUrl : AbstractTask
    {
       

        public AutoDetectBaseUrl(IEngine engine) : base(engine)
        {
        }
        
        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var activeAdapter = NetworkUtilities.GetPrimaryAdapter("www.microsoft.com");
                if (activeAdapter == null)
                    return new CouldNotDetermineDefaultServer { Issue = "could not determine active adapter" };

                var suffix = activeAdapter.GetIPProperties().DnsSuffix;
                if (string.IsNullOrWhiteSpace(suffix))
                    return new CouldNotDetermineDefaultServer { Issue = "could not determine dns suffix" };

                var url = "https://getconnected." + suffix + "/test";
                SettingsManager.BaseWebServiceUrl = url;
                
                return new NextResult();
            }
            catch (System.Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Auto-detect web-service endpoint";
        }
    }
}
