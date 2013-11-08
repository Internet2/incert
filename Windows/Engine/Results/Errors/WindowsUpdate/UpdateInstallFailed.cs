using WUApiLib;

namespace Org.InCommon.InCert.Engine.Results.Errors.WindowsUpdate
{
    class UpdateInstallFailed:AbstractWindowsUpdateIssue
    {
        public UpdateInstallFailed(IInstallationResult result)
        {
            var message = GetMessageForResult(result.HResult);
            Issue = string.Format("One or more updates failed to install ({0}): {1}", result.ResultCode,
                                  message);
        }
        
        public UpdateInstallFailed(string baseMessage, IUpdateInstallationResult result)
        {
            var message = GetMessageForResult(result.HResult);
            Issue = string.Format("{0} ({1}): {2}", baseMessage, result.ResultCode, message);
        }

        public UpdateInstallFailed(){}
    }
}
