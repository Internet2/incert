using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Org.InCommon.InCert.DataContracts;

namespace Org.InCommon.InCert.Utilities.GenerateFileWrapper
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                var source = GetArgument("-source=", args);
                if (string.IsNullOrWhiteSpace(source))
                    throw new Exception("No source specified");

                var regularExpression = GetArgument("-exp=", args);

                if (!IsValidUri(source))
                    throw new Exception("Source is not a valid uri");

                var target = GetArgument("-target=", args);
                if (string.IsNullOrWhiteSpace(target))
                    target = GetPathForUri(source);

                var temporaryPath = DownloadSource(source, target, regularExpression);
                if (string.IsNullOrWhiteSpace(temporaryPath))
                    throw new Exception("Could not transfer source to temporary path");

                if (!File.Exists(temporaryPath))
                    throw new Exception("Temporary file does not exist");

                var temporaryFileName = Path.GetFileName(temporaryPath);
                if (string.IsNullOrWhiteSpace(temporaryFileName))
                    throw new Exception("Could not determine temporary file name");

                var temporaryFolder = Path.GetDirectoryName(temporaryPath);
                if (string.IsNullOrWhiteSpace(temporaryFolder))
                    throw new Exception("Could not determine temporary folder name");

                bool verifySignature;
                if (!bool.TryParse(GetArgument("-requireSigned=", args), out verifySignature))
                    verifySignature = true;

                var info = new FileInfo(temporaryPath);
                if (verifySignature && !info.IsSigned())
                {
                    Console.WriteLine("File is not signed!");
                    return -1;
                }

                var hash = HashFile(temporaryPath);

                bool vital;
                bool.TryParse(GetArgument("-vital=", args), out vital);

                var wrapper = new FileInfoWrapper
                    {
                        FileName = info.Name,
                        FileUrl = info.Name,
                        Version = GetVersionForFile(temporaryPath, GetArgument("-version=", args)).ToString(),
                        Vital = vital,
                        FileSize = info.Length,
                        FileSha1 = hash,
                        MsiProductCode = GetMsiPropertyFromFile(temporaryPath, "ProductCode"),
                        MsiUpgradeCode = GetMsiPropertyFromFile(temporaryPath, "UpgradeCode"),
                    };

                var serializedWrapper = wrapper.ToXElement<FileInfoWrapper>();
                // remove attributes from root
                serializedWrapper.Attributes().Remove();
                var result = new XElement("InCommonProvision", new XAttribute("version", "1.0"));
                result.Add(serializedWrapper);

                var infoName = GetArgument("-info=", args);
                if (string.IsNullOrWhiteSpace(infoName))
                    infoName = GetInfoFilename(temporaryPath);

                var infoPath = Path.Combine(temporaryFolder, infoName);
                File.WriteAllText(
                    infoPath,
                    result.ToString()
                    , new ASCIIEncoding());

                //copy outputfiles to target dir
                var outputFolder = GetOutputFolderPath(target);
                if (string.IsNullOrWhiteSpace(outputFolder))
                    throw new Exception("Could not determine output folder");

                File.Copy(temporaryPath, Path.Combine(outputFolder, temporaryFileName), true);
                File.Copy(infoPath, Path.Combine(outputFolder, infoName), true);

                RemoveTemporaryFolder(temporaryFolder);

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine("An issue occurred while processing the file: {0}", e.Message);
                return -1;
            }
        }

        private static string GetOutputFolderPath(string target)
        {
            var outputFolder = Path.GetDirectoryName(target);
            if (string.IsNullOrWhiteSpace(outputFolder))
                outputFolder = Environment.CurrentDirectory;

            return outputFolder;
        }

        private static void RemoveTemporaryFolder(string path)
        {
            if (!Directory.Exists(path))
                return;

            Directory.Delete(path,true);
        }

        private static string GetPathForUri(string target)
        {
            if (!IsValidUri(target))
                return target;

            return GetPathForUri(new Uri(target));
        }

        private static string GetPathForUri(Uri uri)
        {
            if (!uri.IsFile)
                return uri.AbsoluteUri;

            return uri.LocalPath.TrimStart('/');
        }

        private static string GetInfoFilename(string target)
        {
            var fileName = Path.GetFileName(target);

            return string.Format("{0}.info.xml", fileName);
        }

        private static Version GetVersionForFile(string path, string version)
        {
            var result = new Version(1, 0, 0, 0);
            if (string.IsNullOrWhiteSpace(version))
            {
                var info = FileVersionInfo.GetVersionInfo(path);
                return new Version(info.FileMajorPart, info.FileMinorPart, info.FileBuildPart, info.FilePrivatePart);
            }

            if (string.IsNullOrWhiteSpace(version))
                version = GetMsiPropertyFromFile(path, "ProductVersion");

            if (string.IsNullOrWhiteSpace(version))
                return result;

            if (!Version.TryParse(version, out result))
            {
                Console.WriteLine("Cannot convert version to valid value. Will use 1.0.0.0");
                result = new Version(1, 0, 0, 0);
            }


            return result;
        }

        /// <summary>
        /// Gets a property value from a .msi file
        /// </summary>
        /// <param name="path">Path to .msi</param>
        /// <param name="propertyName">Name of property in question</param>
        /// <returns></returns>
        /// <remarks>Adapted from http://stackoverflow.com/questions/4347325/checking-productversion-of-an-msi-programatically</remarks>
        private static string GetMsiPropertyFromFile(string path, string propertyName)
        {
            var databaseHandle = 0;
            var viewHandle = 0;
            var recordHandle = 0;
            try
            {
                if (!path.ToLowerInvariant().EndsWith(".msi"))
                    return "";

                var result = NativeMethods.MsiOpenDatabase(path, IntPtr.Zero, out databaseHandle);
                if (0 != result)
                    return "";

                var query = string.Format("SELECT * FROM Property WHERE Property = '{0}'", propertyName);
                recordHandle = NativeMethods.MsiCreateRecord(0);

                result = NativeMethods.MsiDatabaseOpenViewW(databaseHandle, query, out viewHandle);
                if (0 != result)
                    return "";

                result = NativeMethods.MsiViewExecute(viewHandle, recordHandle);
                if (0 != result)
                    return "";

                result = NativeMethods.MsiViewFetch(viewHandle, out recordHandle);
                if (0 != result)
                    return "";

                var length = 512;
                var buffer = new StringBuilder(length);
                result = NativeMethods.MsiRecordGetString(recordHandle, 2, buffer, ref length);
                return 0 != result ? "" : buffer.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine("An issue occurred while attempting to extract the product code from a file: {0}", e.Message);
                return "";
            }
            finally
            {
                if (recordHandle != 0)
                    NativeMethods.MsiCloseHandle(recordHandle);

                if (viewHandle != 0)
                    NativeMethods.MsiCloseHandle(viewHandle);

                if (databaseHandle != 0)
                    NativeMethods.MsiCloseHandle(databaseHandle);
            }
        }

        private static string ResolveSource(string source, string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                return source;

            var uri = new Uri(source);

            string sourceContent;
            using (var client = new WebClient())
                sourceContent = client.DownloadString(uri);

            if (string.IsNullOrWhiteSpace(sourceContent))
                throw new Exception(string.Format("Could not get source html for url {0}", source));

            var match = Regex.Match(sourceContent, expression);
            if (!match.Success)
                throw new Exception(string.Format("Could not extract source url from {0} content using regular expression matching", source));

            if (!IsValidUri(match.Value))
                throw new Exception(string.Format("{0} does not resolve to valid uri", match.Value));

            return match.Value;

        }

        private static string MakeTemporaryFolder()
        {
            var result = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(result);
            return result;
        }

        private static string DownloadSource(string source, string target, string expression)
        {
            try
            {
                source = ResolveSource(source, expression);
                var uri = new Uri(source);

                var targetFileName = Path.GetFileName(target);
                if (string.IsNullOrWhiteSpace(targetFileName))
                {
                    Console.WriteLine("Could not determine target file name");
                    return "";
                }
                var targetPath = Path.Combine(MakeTemporaryFolder(), targetFileName);

                if (uri.IsFile)
                {
                    var localPath = GetPathForUri(uri);
                    if (string.IsNullOrWhiteSpace(Path.GetDirectoryName(localPath)))
                        localPath = Path.Combine(Environment.CurrentDirectory, localPath);

                    File.Copy(localPath, targetPath, true);
                    return targetPath;
                }

                using (var client = new WebClient())
                    client.DownloadFile(uri, targetPath);

                return targetPath;
            }
            catch (Exception e)
            {
                Console.WriteLine("An exception occurred while attempting to access source: {0}", e.Message);
                return "";
            }

        }

        private static string GetArgument(string prefix, IEnumerable<string> args)
        {
            foreach (var argument in args.Where(argument => argument.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase)))
                return argument.Substring(prefix.Length, argument.Length - prefix.Length);

            return "";
        }


        private static bool IsValidUri(string value)
        {
            try
            {
                return Uri.IsWellFormedUriString(value, UriKind.Absolute);
            }
            catch (Exception)
            {
                return false;
            }

        }

        private static string HashFile(string targetPath)
        {
            if (string.IsNullOrWhiteSpace(targetPath))
                return "";

            using (var inputstream = File.OpenRead(targetPath))
            {
                var hasher = new SHA1CryptoServiceProvider();
                var value = hasher.ComputeHash(inputstream);
                return value.Length == 0 ? "" : Convert.ToBase64String(value);
            }
        }
    }
}
