using System;
using System.IO;
using System.Linq;
using log4net;
using Org.InCommon.InCert.Engine.Logging;

namespace Org.InCommon.InCert.Engine.ClientIdentifier
{
    class StoredClientIdentifier:AbstractClientIdentifier
    {
        private const string IdentifierFileName = "client_guid";
        private const string HermesDirectory = "INCERT";
        private static readonly ILog Log = Logger.Create();

        public override string GetIdentifier()
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
                return GetBackupIdentifier();
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
