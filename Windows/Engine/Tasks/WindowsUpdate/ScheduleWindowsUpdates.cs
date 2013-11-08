using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using WUApiLib;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.WindowsUpdate
{
    class ScheduleWindowsUpdates:AbstractTask
    {
        private static readonly ILog Log = Logger.Create();

        public ScheduleWindowsUpdates(IEngine engine) : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public AutomaticUpdatesScheduledInstallationDay InstallDay { get; set; }

        [PropertyAllowedFromXml]
        public AutomaticUpdatesNotificationLevel NotificationLevel { get; set; }

        [PropertyAllowedFromXml]
        public int EarliestHour { get; set; }

        [PropertyAllowedFromXml]
        public int HourRange { get; set; }
        
        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var automaticUpdates = new AutomaticUpdates();
                var settings = automaticUpdates.Settings;
                settings.Refresh();

                if (settings.ReadOnly)
                {
                    Log.Warn("Automatic updates are scheduled by group policy and cannot be modified.");
                    return new NextResult();
                }

                settings.ScheduledInstallationDay = InstallDay;
                settings.NotificationLevel = NotificationLevel;
                settings.ScheduledInstallationTime = ComputeInstallationTime();
                settings.Save();

                return new NextResult();

            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        private int ComputeInstallationTime()
        {
            var generator = new Random();
            return (int) ((generator.NextDouble()*HourRange) + EarliestHour);
        }

        public override string GetFriendlyName()
        {
            return "Schedule windows updates";
        }
    }
}
