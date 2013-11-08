using System;
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
    class CheckSystemTime : AbstractTimeTask
    {
        private static readonly ILog Log = Logger.Create();

        [PropertyAllowedFromXml]
        public int MaxVariantMinutes { get; set; }

        public CheckSystemTime(IEngine engine)
            : base(engine)
        {
            MaxVariantMinutes = 20;
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var serverTime = GetServerTime();
                if (serverTime == null)
                {
                    Log.Warn("Could not retrieve server time.");
                    throw new Exception("Could not retrieve server time");
                }

                var difference = DateTime.UtcNow.Subtract(serverTime.Value);

                if (Math.Abs(difference.TotalMinutes) > MaxVariantMinutes)
                    return new SystemClockWrong
                        {
                            Issue = GetTimeDifferenceString(Math.Abs(difference.TotalMinutes)),
                            MinutesOff = difference.TotalMinutes,
                            MaxAllowedDifference = MaxVariantMinutes
                        };

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        private static string GetTimeDifferenceString(double totalMinutes)
        {
            if (totalMinutes >= 60 * 24)
                return "at least a day off";

            if (totalMinutes >= 60)
                return "at least an hour off";

            if (totalMinutes <= 1)
                return "about a minute off";

            return string.Format("at least {0:D} minutes off", totalMinutes);
        }

        public override string GetFriendlyName()
        {
            return "Check system time";
        }
    }
}
