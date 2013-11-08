using System;
using System.IO;
using System.Linq;
using System.Windows;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.NativeCode.Wmi;
using log4net;

namespace Org.InCommon.InCert.Engine.Extensions
{
    public static class IdentifierExtension
    {
        private static readonly ILog Log = Logger.Create();
        
        private const string IdentifierFileName = "client_guid";
        private const string HermesDirectory = "INCERT";

        private static string _identifier;

        private static Guid? _sessionId;

        public static string GetIdentifier(this Application application)
        {
            if (string.IsNullOrWhiteSpace(_identifier))
                _identifier = GetBiosIdentifier();

            if (string.IsNullOrWhiteSpace(_identifier))
                _identifier = GetIdentifierValue();

            return _identifier;
        }

        public static Guid GetSessionId(this Application application)
        {
            if (_sessionId == null)
                _sessionId = Guid.NewGuid();

            return _sessionId.Value;
        }
        
        private static string GetBiosIdentifier()
        {
            try
            {
                var biosInfo = Bios.GetInstances().OfType<Bios>().FirstOrDefault();
                if (biosInfo == null)
                    return "";

                if (string.IsNullOrWhiteSpace(biosInfo.SerialNumber))
                    return "";

                if (biosInfo.SerialNumber.Length < 5)
                {
                    return "";
                }
                if (!biosInfo.SerialNumber.Any(char.IsDigit))
                {
                    return "";
                }
                if (biosInfo.SerialNumber.Contains(" "))
                {
                    return "";
                }

                return biosInfo.SerialNumber;

            }
            catch (Exception e)
            {
                Log.Warn(e);
                return "";
            }

        }

        private static string GetIdentifierValue()
        {
            try
            {
                var identifierPath = GetIdentifierPath();
                var result = RetreiveIdentifier(identifierPath);
                if (string.IsNullOrWhiteSpace(result))
                {
                    result = Guid.NewGuid().ToString();
                    StoreIdentifier(identifierPath, result);
                }

                return result;
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to determine this computer's legacy identifier value: {0}", e.Message);
                return Guid.NewGuid().ToString();
            }
        }

        private static string GetIdentifierPath()
        {
            try
            {
                var result = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                if (!Directory.Exists(result))
                    throw new DirectoryNotFoundException(result);

                result = Path.Combine(result, HermesDirectory);
                if (!Directory.Exists(result))
                    Directory.CreateDirectory(result);

                return Path.Combine(result, IdentifierFileName);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to determine the path to the identifier folder: {0}", e.Message);
                return "";
            }
        }

        private static string RetreiveIdentifier(string targetPath)
        {
            return !File.Exists(targetPath) ? "" : File.ReadLines(targetPath).FirstOrDefault();
        }

        private static void StoreIdentifier(string targetPath, string value)
        {
            try
            {
                if (File.Exists(targetPath))
                    File.SetAttributes(targetPath, FileAttributes.Normal);
                
                File.WriteAllText(targetPath, value);
                File.SetAttributes(targetPath, FileAttributes.Hidden | FileAttributes.ReadOnly | FileAttributes.Archive);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to save this computer's identifier: {0}", e.Message);
            }
            
        }

    }
}
