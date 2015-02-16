using System;
using System.Collections.Generic;
using System.Linq;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.Time;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Time
{
    class CheckTimeZone : AbstractTimeTask
    {
        private static readonly ILog Log = Logger.Create();

        private readonly List<string> _timeZones = new List<string>();

        public CheckTimeZone(IEngine engine) 
            : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string Timezone
        {
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    return;

                _timeZones.Add(value);
            }
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (!_timeZones.Any())
                    return new NextResult();

                var matchFound = false;
                foreach (var value in _timeZones)
                {
                    var timeZone = ValueResolver.Resolve(value,true);
                    if (!VerifyZimeZone(timeZone))
                        continue;

                    matchFound = true;
                }

                if (!matchFound)
                    return new TimezoneWrong();

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        private static bool VerifyZimeZone(string targetZone)
        {
            TimeZoneInfo.ClearCachedData();
            var info = GetTimeZoneByIdentifier(targetZone);
            if (info == null)
            {
                Log.WarnFormat("No information found for time zone {0} on this computer", targetZone);
                return true;
            }

            if (info.Equals(TimeZoneInfo.Local))
                return true ;

            if (info.Id.Equals(TimeZoneInfo.Local.Id, StringComparison.InvariantCulture))
                return true;

            return false;
        }
        
        private static TimeZoneInfo GetTimeZoneByIdentifier(string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                    return null;

                return TimeZoneInfo.FindSystemTimeZoneById(value);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to retrieve time zone info for the value {0}: {1}", value, e.Message);
                return null;
            }
        }

        public override string GetFriendlyName()
        {
            return "Check computer time zone";
        }
    }
}
