using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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

        public static string SplitByCapitalLetters(this string value)
        {
            const string regEx = "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))";
            try
            {
                return Regex.Replace(value, regEx, "$1 ");
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to split a string by its capital letters: {0}", e.Message);
                return value;
            }
            
        }

        public static string ToSha1Hash(this string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("input string cannot be null or empty");
                }

                var data = Encoding.UTF8.GetBytes(value);
                var hasher = new SHA1CryptoServiceProvider();
                var result = hasher.ComputeHash(data);
                return BitConverter.ToString(result).Replace("-", string.Empty);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to generate an sha1 hash: {0}", e.Message);
                return string.Empty;
            }
        }
    }
}
