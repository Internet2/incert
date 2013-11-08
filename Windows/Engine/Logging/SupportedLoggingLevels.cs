using System.Reflection;
using log4net.Core;

namespace Org.InCommon.InCert.Engine.Logging
{
    public static class SupportedLoggingLevels
    {
        public static readonly Level Task = new Level(51000, "Task");
        public static readonly Level Branch = new Level(52000, "Branch");
        public static readonly Level Start = new Level(53000, "Start");
        public static readonly Level Exit = new Level(54000, "Exit");
        public static readonly Level Register = new Level(55000, "Register");
        public static readonly Level Finish = new Level(56000, "Finish");
        public static readonly Level Track = new Level(57000, "Tracking");
        public static readonly Level Monitor = new Level(58000, "Monitor");
        public static readonly Level Waypoint = new Level(59000, "Waypoint");
        public static readonly Level Unconfigure = new Level(60000, "Unconfigure");

        // mirrors suppored pre-defined levels
        public static readonly Level Alert = Level.Alert;
        public static readonly Level All = Level.All;
        public static readonly Level Critical = Level.Critical;
        public static readonly Level Debug = Level.Debug;
        public static readonly Level Emergency = Level.Emergency;
        public static readonly Level Error = Level.Error;
        public static readonly Level Fatal = Level.Fatal;
        public static readonly Level Info = Level.Info;
        public static readonly Level Notice = Level.Notice;
        public static readonly Level Severe = Level.Severe;
        public static readonly Level Trace = Level.Trace;
        public static readonly Level Verbose = Level.Verbose;
        public static readonly Level Warn = Level.Warn;
      
        public static Level GetAssociatedLevel(this Values level)
        {
            var info = typeof(SupportedLoggingLevels).GetField(level.ToString(), BindingFlags.Public | BindingFlags.Static);
            if (info == null)
                return null;

            return info.GetValue(null) as Level;
        }

        public enum Values
        {
            Task,
            Branch,
            Start,
            Exit,
            Register,
            Finish,
            Track,
            Monitor,
            Alert,
            All,
            Critical,
            Debug,
            Emergency,
            Error,
            Fatal,
            Info,
            Notice,
            Severe,
            Trace,
            Verbose,
            Warn,
            Waypoint,
            Unconfigure,
            None
        }
    }



    
}
