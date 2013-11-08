using System;
using Org.InCommon.InCert.BootstrapperEngine.Extensions;
using Org.InCommon.InCert.BootstrapperEngine.Models;

namespace Org.InCommon.InCert.BootstrapperEngine.WebServices
{
    static class LogUploaderFactory
    {
        public static AbstractLogUploader GetLogUploader(BaseModel model)
        {
            if (!model.StringVariables.Contains("LogUploader"))
                return null;

            if (!model.StringVariables.Contains("LoggingUrl"))
                return null;

            AbstractLogUploader result = null;
            var uploaderType = model.StringVariables["LogUploader"];
            if (uploaderType.Equals("InCertLogUploader", StringComparison.InvariantCulture))
                result = new GetBasedLogUploader();
            else if (uploaderType.Equals("WebApiLogUploader", StringComparison.InvariantCulture))
                result = new PostBasedLogUploader();

            if (result == null)
                return null;

            result.EndpointUrl = model.StringVariables["LoggingUrl"];
            result.MachineId = model.GetIdentifier();
            result.Session = IdentifierExtension.GetSessionId();
            result.UserId = "[unknown]";
            result.Timeout = 30000;

            return result;
        }
    }
}
