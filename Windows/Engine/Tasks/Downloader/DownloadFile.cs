using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using Org.InCommon.InCert.DataContracts;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.Downloader;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.Path;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Downloader
{
    class DownloadFile : AbstractTask
    {
        private static readonly ILog Log = Logger.Create();

        private bool _completed;
        private bool _cancelRequested;
        private bool _updateInProgress;

        [PropertyAllowedFromXml]
        public string SettingKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string DisplayName
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string ProgressPercentKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string ProgressTextKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string TimeEstimateKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string CancelKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        private class DownloadInfoWrapper
        {
            public DateTime StartTime { get; set; }
            public string DisplayName { get; set; }
            public IResult Result { get; set; }
        }

        public DownloadFile(IEngine engine)
            : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SettingKey))
                    throw new Exception("No settings key specified");

                var info = SettingsManager.GetTemporaryObject(SettingKey) as FileInfoWrapper;
                if (info == null)
                    throw new Exception("Data object is invalid");

                ResetInstance();
                return DownloadFileAsync(info);
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        protected virtual void ResetInstance()
        {
            _cancelRequested = false;
            _completed = false;
            UpdateProgressText("Initializing - please wait");
        }

        protected virtual IResult DownloadFileAsync(FileInfoWrapper info)
        {
            try
            {

                using (var client = new ConfigurableWebClient { KeepAlive = false })
                {
                    var targetPath = PathUtilities.DownloadFolder;
                    if (!Directory.Exists(targetPath))
                        return new DirectoryNotFound { Target = targetPath };

                    var uri = GetDownloadUri(info);
                    if (uri == null)
                        return new CouldNotDownloadFile
                                   {
                                       Issue = "Could not determine host target",
                                       Target = info.FileName
                                   };

                    client.Credentials = GetDownloadCredentials(uri);
                    client.DownloadFileCompleted += DownloadCompletedHandler;
                    client.DownloadProgressChanged += DownloadProgressHandler;

                    _completed = false;

                    if (string.IsNullOrWhiteSpace(info.FileName))
                        return new CouldNotDownloadFile
                                   {
                                       Issue = "Could not determine local target",
                                       Target = info.FileName
                                   };

                    if (string.IsNullOrWhiteSpace(DisplayName))
                        DisplayName = info.FileName;

                    targetPath = Path.Combine(targetPath, info.FileName);

                    var resultWrapper = new DownloadInfoWrapper
                                            {
                                                DisplayName = DisplayName,
                                                Result = new NextResult(),
                                                StartTime = DateTime.UtcNow
                                            };

                    client.Headers.Add(HttpRequestHeader.UserAgent,
                        string.Format("{0} {1}",
                        Application.Current.GetProductName(),
                        Application.Current.GetVersion()));

                    InterveneIfConnectionLost();

                    client.DownloadFileAsync(uri, targetPath, resultWrapper);
                    do
                    {
                        Application.Current.DoEvents(250);

                        // check for cancel
                        _cancelRequested = CancelRequested();
                        if (!_cancelRequested) continue;

                        // handle cancel stuff
                        client.CancelAsync();
                        _cancelRequested = true;
                        SetCancelMessages();
                    } while (_completed == false);

                    UserInterfaceUtilities.WaitForMilliseconds(DateTime.UtcNow, 250);

                    UpdateProgressPercent(100);
                    UpdateTimeEstimate("");

                    UpdateProgressText(resultWrapper.Result.IsOk() ? "Download complete!" : "Download failed!");

                    return resultWrapper.Result;
                }

            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        protected virtual void InterveneIfConnectionLost()
        {
            if (NetworkUtilities.AnyActualAdapterWithValidLease())
                return;

            Log.Warn("No network adapters found with valid lease. Attempting to renew lease.");
            NetworkUtilities.ReleaseAndRenewInterfaces();
        }

        protected virtual void ClearCancelSetting()
        {
            if (string.IsNullOrWhiteSpace(CancelKey))
                return;

            SettingsManager.RemoveTemporarySettingString(CancelKey);
        }

        protected virtual void ResetProgressSetting()
        {
            UpdateProgressPercent(0);
        }

        protected virtual void ClearProgressText()
        {
            UpdateProgressText("");
        }

        protected virtual void ClearTimeEstimate()
        {
            UpdateTimeEstimate("");
        }

        protected virtual bool CancelRequested()
        {
            if (string.IsNullOrWhiteSpace(CancelKey))
                return false;

            bool result;
            bool.TryParse(SettingsManager.GetTemporarySettingString(CancelKey), out result);

            return result;
        }

        protected virtual void SetCancelMessages()
        {
            UpdateProgressText("Cancelling download - please wait");
            UpdateTimeEstimate("");
        }

        protected virtual Uri GetDownloadUri(FileInfoWrapper info)
        {
            if (UriUtilities.IsValidAbsoluteUri(info.FileUrl))
                return UriUtilities.ResolveAbsoluteUri(info.FileUrl);

            return UriUtilities.ResolveUri(info.FileUrl, string.IsNullOrWhiteSpace(info.BaseUrl)
                ? EndpointManager.GetEndpointForFunction(EndPointFunctions.GetFileInfo)
                : info.BaseUrl);
        }

        protected virtual CredentialCache GetDownloadCredentials(Uri uri)
        {
            return null;

            /*var credentials = new NetworkCredential(
                SettingsManager.GetTemporarySettingString("username"), 
                SettingsManager.GetSecureTemporarySettingString("passphrase"));

            return new CredentialCache {{uri, "Basic", credentials}};*/
        }

        protected virtual void DownloadCompletedHandler(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                var wrapper = e.UserState as DownloadInfoWrapper;
                if (wrapper == null)
                    return;

                if (e.Cancelled)
                {
                    wrapper.Result = new CouldNotDownloadFile
                        {
                            Issue = "User cancelled download",
                            Target = wrapper.DisplayName
                        };
                    return;
                }

                if (e.Error != null)
                {
                    wrapper.Result = new CouldNotDownloadFile
                        {
                            Issue = GetIssueString(e.Error),
                            Target = wrapper.DisplayName
                        };
                    return;
                }

                wrapper.Result = new NextResult();
            }
            catch (Exception ex)
            {
                Log.WarnFormat("An issue occurred in the download completed handler: {0}", ex.Message);
            }
            finally
            {
                _completed = true;
            }
        }

        private static string GetIssueString(Exception issue)
        {
            try
            {
                if (issue == null)
                    return "an unexpected issue occurred";

                if (issue.InnerException == null)
                    return issue.Message;

                return string.Format("{0} {1}", issue.Message, issue.InnerException.Message);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to parse a download issue result: {0}", e.Message);
                return "an unexpected issue occurred";
            }


        }

        protected virtual void DownloadProgressHandler(object sender, DownloadProgressChangedEventArgs e)
        {
            try
            {
                if (_cancelRequested)
                    return;

                if (_updateInProgress)
                    return;

                _updateInProgress = true;
                UpdateProgressPercent(e.ProgressPercentage);
                UpdateProgressText(e);
                UpdateTimeEstimate(e.UserState as DownloadInfoWrapper, e.ProgressPercentage);
            }
            catch (Exception ex)
            {
                Log.WarnFormat("An issue occurred while updating download progress text: {0}", ex.Message);
            }
            finally
            {
                _updateInProgress = false;
            }

        }

        private void UpdateProgressPercent(int value)
        {
            if (string.IsNullOrWhiteSpace(ProgressPercentKey))
                return;

            SettingsManager.SetTemporarySettingString(ProgressPercentKey, value.ToString(CultureInfo.InvariantCulture));
        }

        private void UpdateProgressText(DownloadProgressChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProgressTextKey))
                return;

            var multiplier = .001;
            var unitSize = "KB";
            if (e.TotalBytesToReceive > (10 ^ 6))
            {
                multiplier = 0.000001;
                unitSize = "MB";
            }

            var received = e.BytesReceived;
            if (received > 0)
                received = (long)Math.Round(e.BytesReceived * multiplier);

            var totalToReceive = e.TotalBytesToReceive;
            if (totalToReceive > 0)
                totalToReceive = (long)Math.Round(e.TotalBytesToReceive * multiplier);

            var message = string.Format("Received: {0} of {1} {2} ({3}% complete)", received, totalToReceive, unitSize, e.ProgressPercentage);
            UpdateProgressText(message);
        }

        private void UpdateTimeEstimate(DownloadInfoWrapper wrapper, int percent)
        {
            if (wrapper == null)
                return;

            if (percent <= 0)
                return;

            var elapsed = DateTime.UtcNow.Subtract(wrapper.StartTime);
            var ticksPerPercent = elapsed.Ticks / percent;
            var totalTicks = ticksPerPercent * 100;
            var remainingTicks = totalTicks - elapsed.Ticks;
            var timeRemaining = new TimeSpan(remainingTicks);

            string message;

            if (timeRemaining.TotalMinutes <= 60)
                message = timeRemaining.TotalMinutes <= 1 ? "About 1 minute remains" : string.Format("About {0} minutes remain", timeRemaining.TotalMinutes);
            else if (timeRemaining.TotalMinutes > 60)
                message = "Over an hour remains";
            else
                message = "About 1 hour remains";

            UpdateTimeEstimate(message);
        }

        private void UpdateProgressText(string message)
        {
            if (string.IsNullOrWhiteSpace(ProgressTextKey))
                return;

            if (message.Equals(SettingsManager.GetTemporarySettingString(ProgressTextKey),
                               StringComparison.InvariantCulture))
                return;

            SettingsManager.SetTemporarySettingString(ProgressTextKey, message);

        }

        private void UpdateTimeEstimate(string message)
        {
            if (string.IsNullOrWhiteSpace(TimeEstimateKey))
                return;

            if (message.Equals(SettingsManager.GetTemporarySettingString(TimeEstimateKey),
                               StringComparison.InvariantCulture))
                return;

            SettingsManager.SetTemporarySettingString(TimeEstimateKey, message);

        }


        public override string GetFriendlyName()
        {
            return "Download file";
        }

        internal class ConfigurableWebClient : WebClient
        {
            public bool KeepAlive { get; set; }

            protected override WebRequest GetWebRequest(Uri address)
            {
                var request = base.GetWebRequest(address);
                var webRequest = request as HttpWebRequest;
                if (webRequest == null)
                {
                    return request;
                }

                webRequest.KeepAlive = KeepAlive;
                webRequest.ProtocolVersion = HttpVersion.Version10;
                return request;
            }
        }
    }
}
