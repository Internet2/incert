using System;
using System.Text;
using Org.InCommon.InCert.Engine.Logging;
using log4net;

namespace Org.InCommon.InCert.Engine.Extensions
{
    public static class StringExtensions
    {
        private static readonly ILog Log = Logger.Create();
        
        public static byte[] GetRawBytes(this string value)
        {
            var bytes = new byte[value.Length * sizeof(char)];
            Buffer.BlockCopy(value.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string ToStringOrDefault(this object value, string defaultValue)
        {
            if (value == null)
                return defaultValue;

            var result = value.ToString();
            return string.IsNullOrWhiteSpace(result) ? defaultValue : result;
        }

        public static int ToIntOrDefault(this object value, int defaultValue)
        {
            int result;
            return !int.TryParse(value.ToStringOrDefault(""), out result) 
                ? defaultValue : result;
        }

        public static string ToQuoted(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            return string.Format("\"{0}\"", value);
        }

        public static string RemoveTrailingLineFeeds(this string value)
        {
            try
            {
                return string.IsNullOrWhiteSpace(value) ? value : value.TrimEnd(new[] {'\r', '\n'});
            }
            catch (Exception)
            {
                return value;
            }
        }

        public static string SplitByCaptialLetters(this string value)
        {
            try
            {
                var output = new StringBuilder();
                foreach (var letter in value)
                {
                    if (Char.IsUpper(letter))
                        output.Append(" ");

                    if (Char.IsNumber(letter))
                        output.Append(" ");

                    output.Append(letter);
                }

                return output.ToString();
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to split a string by its capital letters", e.Message);
                return value;
            }
            
        }
    }
}
