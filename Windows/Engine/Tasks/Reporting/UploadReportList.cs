using System;
using System.Collections.Generic;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.WebServices.Contracts;
using Org.InCommon.InCert.Engine.WebServices.DataWrappers;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Reporting
{
    class UploadReportList : AbstractTask
    {
        private static readonly ILog Log = Logger.Create();

        [PropertyAllowedFromXml]
        public string Entry
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string ListObjectKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public bool Synchronous { get; set; }

        public UploadReportList(IEngine engine)
            : base(engine)
        {

        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var function = EndPointFunctions.UploadAsyncReport;
                if (Synchronous)
                    function = EndPointFunctions.UploadReport;

                if (string.IsNullOrWhiteSpace(Entry))
                {
                    Log.Warn("Cannot updload report list. Entry is not set.");
                    return new NextResult();
                }

                if (string.IsNullOrWhiteSpace(ListObjectKey))
                {
                    Log.Warn("Cannot updload report list. List key is not set.");
                    return new NextResult();
                }

                var valueList = SettingsManager.GetTemporaryObject(ListObjectKey) as List<string>;
                if (valueList == null)
                {
                    Log.Warn("Cannot updload report list. List object not found.");
                    return new NextResult();
                }

                foreach (var value in valueList)
                {
                    var request = EndpointManager.GetContract<AbstractReportingContract>(function);
                    request.Name = Entry;
                    request.Value = value;

                    request.MakeRequest<NoContentWrapper>();
                }

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return string.Format("Upload report list (entry = {0})", Entry);
        }
    }
}
