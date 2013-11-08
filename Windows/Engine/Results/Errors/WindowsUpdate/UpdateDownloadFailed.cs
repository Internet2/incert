using WUApiLib;

namespace Org.InCommon.InCert.Engine.Results.Errors.WindowsUpdate
{
    class UpdateDownloadFailed:AbstractWindowsUpdateIssue
    {
        public UpdateDownloadFailed(IDownloadResult result)
        {
            var message = GetMessageForResult(result.HResult);
            Issue = string.Format("One or more updates failed to download ({0}): {1}", result.ResultCode,
                                  message);
        }
        
        public UpdateDownloadFailed(string baseMessage, IUpdateDownloadResult result)
        {
            var message = GetMessageForResult(result.HResult);
            Issue = string.Format("{0} ({1}): {2}", baseMessage, result.ResultCode, message);
        }

        public UpdateDownloadFailed(){}
    }
}
