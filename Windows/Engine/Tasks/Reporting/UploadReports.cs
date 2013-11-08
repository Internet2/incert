using System;
using System.Collections.Generic;
using System.Linq;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Importables.PropertySetters;
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
    class UploadReports:AbstractTask
    {
        private static readonly ILog Log = Logger.Create();
        private readonly List<KeyedDynamicStringPropertyEntry> _entries = new List<KeyedDynamicStringPropertyEntry>();

        [PropertyAllowedFromXml]
        public KeyedDynamicStringPropertyEntry Entry
        {
            set
            {
                if (value == null)
                    return;

                _entries.Add(value);
            }
        }

        [PropertyAllowedFromXml]
        public bool Synchronous { get; set; }

        public UploadReports(IEngine engine)
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
                
                if (!_entries.Any())
                {
                    Log.WarnFormat("No report entries present!");
                    return new NextResult();
                }

                foreach (var entry in _entries)
                {
                    if (string.IsNullOrWhiteSpace(entry.Key))
                        continue;

                    if (string.IsNullOrWhiteSpace(entry.Value))
                        continue;
                    
                    var request = EndpointManager.GetContract<AbstractReportingContract>(function);
                    request.Name = entry.Key;
                    request.Value = entry.Value;

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
            return "Upload reports";
        }
    }
}
