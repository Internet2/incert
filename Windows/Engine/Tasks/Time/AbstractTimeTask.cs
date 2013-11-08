using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Org.InCommon.InCert.DataContracts;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.WebServices.Contracts;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Time
{
    abstract class AbstractTimeTask:AbstractTask
    {
        private static readonly ILog Log = Logger.Create();
        
        protected AbstractTimeTask(IEngine engine)
            : base(engine)
        {
        }

        internal DateTime? GetServerTime()
        {
            try
            {
                var request =
                    EndpointManager.GetContract<AbstractStatusInfoContract>(EndPointFunctions.GetStatusInfo);
                request.IgnoreCertificateErrors = true;
                var result = request.MakeRequest<StatusInfo>();
                if (result == null)
                {
                    Log.WarnFormat("An issue occurred while querying server for time data: {0}", request.GetErrorResult().GetDetails());
                    return null;
                }
                   
                return DateTime.ParseExact(result.GmtTime, "ddd MMM d HH:mm:ss yyyy",
                                           CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while querying server for time data: {0}", e.Message);
                return null;
            }
        }

        internal class NativeMethods
        {
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            public struct DynamicTimeZoneInformation
            {
                public int Bias;

                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
                public string StandardName;

                public Systemtime StandardDate;
                public int StandardBias;

                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
                public string DaylightName;

                public Systemtime DaylightDate;

                public int DaylightBias;

                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
                public string TimeZoneKeyName;

                public byte DynamicDaylightTimeDisabled;

                public void SetBytes(byte[] info)
                {
                    if (info.Length != 44)
                        throw new ArgumentException(@"Information size is incorrect", "info");

                    Bias = BitConverter.ToInt32(info, 0);
                    StandardBias = BitConverter.ToInt32(info, 4);
                    DaylightBias = BitConverter.ToInt32(info, 8);
                    var helper = new byte[16];
                    Array.Copy(info, 12, helper, 0, 16);
                    StandardDate.SetInfo(helper);
                    Array.Copy(info, 28, helper, 0, 16);
                    DaylightDate.SetInfo(helper);
                }
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct Systemtime
            {
                public ushort Year;
                public ushort Month;
                public ushort DayOfWeek;
                public ushort Day;
                public ushort Hour;
                public ushort Minute;
                public ushort Second;
                public ushort Milliseconds;

                public void SetInfo(byte[] info)
                {
                    if (info.Length != Marshal.SizeOf(this))
                        throw new ArgumentException(@"Information size is incorrect", "info");

                    Year = BitConverter.ToUInt16(info, 0);
                    Month = BitConverter.ToUInt16(info, 2);
                    DayOfWeek = BitConverter.ToUInt16(info, 4);
                    Day = BitConverter.ToUInt16(info, 6);
                    Hour = BitConverter.ToUInt16(info, 8);
                    Minute = BitConverter.ToUInt16(info, 10);
                    Second = BitConverter.ToUInt16(info, 12);
                    Milliseconds = BitConverter.ToUInt16(info, 14);
                }
            }

            [DllImport("kernel32.dll", EntryPoint = "SetDynamicTimeZoneInformation", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SetDynamicTimeZoneInformation([In]ref DynamicTimeZoneInformation timeZoneInformation);

            [DllImport("kernel32.dll", EntryPoint = "SetSystemTime", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SetSystemTime([In] ref Systemtime systemTime);

        }
    }


    
}
