using System;
using System.Reflection;
using System.Windows.Input;
using Org.InCommon.InCert.Engine.Logging;
using log4net;

namespace Org.InCommon.InCert.Engine.Extensions
{
    public static class CursorExtensions
    {
        private static readonly ILog Log = Logger.Create();

        public static Cursor ConvertToCursor(this string value)
        {
            try
            {
                return ConvertToCursor(value, null);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to retrieve the cursor {0}: {1}", value, e);
                return null;
            }
        }

        public static Cursor ConvertToCursor(this string value, Cursor defaultValue)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                    return defaultValue;

                var info = typeof(Cursors).GetProperty(value, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.Public);
                if (info == null)
                    return defaultValue;

                return info.GetValue(null) as Cursor;

            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to retrieve the cursor {0}: {1}", value, e);
                return defaultValue;
            }
        }
    }
}
