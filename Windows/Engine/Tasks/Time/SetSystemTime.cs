using System;
using System.ComponentModel;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Time
{
    class SetSystemTime:AbstractTimeTask
    {
        public SetSystemTime(IEngine engine) 
            : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var serverTime = GetServerTime();
                if (!serverTime.HasValue)
                {
                    throw new Exception("Could not retrieve server time");
                }

                var time = new NativeMethods.Systemtime
                    {
                        Day = (ushort)serverTime.Value.Day,
                        DayOfWeek = (ushort)serverTime.Value.DayOfWeek,
                        Hour = (ushort)serverTime.Value.Hour,
                        Milliseconds = (ushort)serverTime.Value.Millisecond,
                        Minute = (ushort)serverTime.Value.Minute,
                        Month = (ushort)serverTime.Value.Month,
                        Second = (ushort)serverTime.Value.Second,
                        Year = (ushort)serverTime.Value.Year
                    };

                if (!NativeMethods.SetSystemTime(ref time))
                    throw  new Win32Exception();

                return new NextResult();
            }
            catch (Exception e)
            {
                return  new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Set computer time";
        }
    }
}
