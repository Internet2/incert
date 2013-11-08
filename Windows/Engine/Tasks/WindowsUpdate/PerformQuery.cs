using System;
using System.Threading;
using System.Windows;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.WindowsUpdate;
using Org.InCommon.InCert.Engine.Engines;
using WUApiLib;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.WindowsUpdate
{
    class PerformQuery : AbstractWuApiTask
    {
        private static readonly ILog Log = Logger.Create();

        public PerformQuery(IEngine engine)
            : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string Query
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string ResultsObjectKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ResultsObjectKey))
                    throw new Exception("Results object key cannot be null");

                if (string.IsNullOrWhiteSpace(Query))
                    throw new Exception("Query cannot be null");
                
                SettingsManager.RemoveTemporaryObject(ResultsObjectKey);

                
                var result = RetryGetUpdates(Query);
                if (result == null)
                    throw new Exception("Could not retrieve update results");

                if (result.ResultCode == OperationResultCode.orcAborted)
                    return new Cancelled();

                if (result.ResultCode != OperationResultCode.orcSucceeded &&
                    result.ResultCode != OperationResultCode.orcSucceededWithErrors)
                    return new UpdateQueryFailed
                        {
                            Issue = String.Format("{0} ({1})",  
                            ConvertResultToString((uint)result.ResultCode, "unknown issue"),
                            result.ResultCode)
                        };

                if (result.Updates.Count==0)
                    Log.Info("Update query found no updates");
                else
                    Log.WarnFormat("Update query found {0} updates", result.Updates.Count);
                
                SettingsManager.SetTemporaryObject(ResultsObjectKey, result.Updates);

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        private ISearchResult RetryGetUpdates(string query)
        {
            var count = 0;
            ISearchResult result;
            do
            {
                if (count >0)
                    Log.Warn("Update query failed, retrying");

                result = GetAvailableUpdatesList(query);
                if (result != null)
                {
                    if (result.ResultCode == OperationResultCode.orcSucceeded)
                        break;

                    if (result.ResultCode == OperationResultCode.orcSucceededWithErrors)
                    {
                        Log.Warn("Update query succeeded but with issues");
                        break;
                    }

                    if (result.ResultCode == OperationResultCode.orcAborted)
                    {
                        Log.Warn("Update query aborted");
                        break;
                    }    
                }
                
                DialogsManager.WaitForDurationOrCancel(DateTime.UtcNow, new TimeSpan(0,0,0,2));
                count++;
            } while (count < 4);

            return result;
        }

        private ISearchResult GetAvailableUpdatesList(string query)
        {
            ISearchJob job = null;
            try
            {
                var session = GetUpdateSession();
                var searcher = session.CreateUpdateSearcher();
                job = searcher.BeginSearch(query, new object(), null);

                WaitForSearchJobComplete(job);

                return searcher.EndSearch(job);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while searching for windows updates: {0}",e.Message);
                return null;
            }
            finally
            {
                if (job !=null)
                    job.CleanUp();

                
            }
        }

        private void WaitForSearchJobComplete(ISearchJob job)
        {
            if (job == null)
                return;

            do
            {
                if (DialogsManager.CancelRequested)
                    job.RequestAbort();

                Thread.Sleep(5);
                Application.Current.DoEvents();
             } while (!job.IsCompleted);
        }

        public override string GetFriendlyName()
        {
            return "Scan for missing updates";
        }
    }
}
