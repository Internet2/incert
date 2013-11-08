using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.Conditions.Encryption.Pgp
{
    class DrivesEncrypted : AbstractPgpCondition
    {
        private static readonly ILog Log = Logger.Create();

        public DrivesEncrypted(IEngine engine)
            : base (engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            try
            {
                var installPath = GetInstallPath();
                if (string.IsNullOrWhiteSpace(installPath))
                    return new BooleanReason(false, "Pgp install path not present in registry");

                var utilityPath = Path.Combine(installPath, "pgpwde.exe");
                if (!File.Exists(utilityPath))
                    return new BooleanReason(false, "Could not locate pgpwde.exe");

                var diskNodes = GetDiskNodes(utilityPath);
                if (diskNodes == null || !diskNodes.Any())
                    return new BooleanReason(false, "Could not enumerate drives");

                var result = new BooleanReason(true, "All fixed drives are encrypted.");
                foreach (var diskNode in diskNodes)
                {
                    if (!IsRemovableDrive(diskNode))
                        continue;

                    if (!IsDiskEncrypted(utilityPath, diskNode))
                    {
                        Log.WarnFormat("Disk {0} ({1}) is fixed and not encrypted", GetDiskId(diskNode), GetDiskUuid(diskNode));
                        result = new BooleanReason(false, "Disk {0} ({1}) is fixed and not encrypted", GetDiskId(diskNode), GetDiskUuid(diskNode));
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to determine whether this computer's drives are encrypted with PGP Desktop encryption: {0}", e.Message);
                return new BooleanReason(e);
            }
        }

        private static List<XElement> GetDiskNodes(string utilityPath)
        {
            try
            {
                var info = GetStartInfo(utilityPath, "--enum --xml --aa");

                using (var process = Process.Start(info))
                {
                    process.WaitUntilExited();
                    if (process.ExitCode != 0)
                    {
                        Log.WarnFormat("Enum disk call returned {0}", process.ExitCode);
                        /*return new List<XElement>();*/
                    }

                    var node = GetXElementFromOutput(process);
                    return node.Descendants("disk").ToList();
                }
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while retrieving disk info from pgpwde: {0}", e.Message);
                return new List<XElement>();
            }
        }

        private static XElement GetXElementFromOutput(Process process)
        {
            var output = "";
            try
            {
                output = process.StandardOutput.ReadToEnd();

                using (var reader = new StringReader(output))
                {
                    return XElement.Load(reader);    
                }
                
            }
            catch (Exception e)
            {
                var message =
                    string.Format(
                        "An issue occurred while attempting to convert output to XElement: issue = {0}, output={1}",
                        e.Message, output);
                throw new Exception(message, e);
            }
        }

        private static string GetDiskId(XElement diskNode)
        {
            return XmlUtilities.GetTextFromAttribute(diskNode, "id");
        }

        private static string GetDiskUuid(XElement diskNode)
        {
            return XmlUtilities.GetTextFromAttribute(diskNode, "uuid", "[unknown]");
        }

        private static string GetVolumeName(XElement volumeNode)
        {
            return XmlUtilities.GetTextFromChildNode(volumeNode, "name");
        }

        private static string GetVolumeLabel(XElement volumeNode)
        {
            return XmlUtilities.GetTextFromChildNode(volumeNode, "label");
        }

        private static bool IsRemovableDrive(XElement diskNode)
        {
            try
            {
                var volumeNodes = diskNode.Descendants("volume").ToList();
                if (!volumeNodes.Any())
                {
                    Log.WarnFormat("Disk {0} has no associated volumes", GetDiskUuid(diskNode));
                    return false;
                }

                var isFixed = true;
                foreach (var volumeNode in volumeNodes.Where(volumeNode => !IsDriveFixed(GetVolumeName(volumeNode))))
                {
                    Log.InfoFormat("Volume {0} ({1}) is not on fixed-drive.", GetVolumeName(volumeNode),
                                   GetVolumeLabel(volumeNode));
                    isFixed = false;
                }

                return isFixed;
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to determine if a disk is fixed: {0}", e.Message);
                return false;
            }
        }


        

        private static bool IsDiskEncrypted(string utilityPath, XElement diskNode)
        {
            try
            {
                var statusNode = GetDiskStatus(utilityPath, GetDiskId(diskNode));
                if (statusNode == null)
                    throw new Exception("Could not get disk status");

                if (!XmlUtilities.GetBooleanFromChildNode(statusNode, "instrumented", false))
                {
                    Log.WarnFormat("Disk {0} ({1}) is not instrumented", GetDiskId(diskNode), GetDiskUuid(diskNode));
                    return false;
                }

                var scheme = XmlUtilities.GetTextFromChildNode(statusNode, "scheme");
                if (string.IsNullOrWhiteSpace(scheme))
                {
                    Log.WarnFormat("Disk {0} ({1}) has no scheme attribute set", GetDiskId(diskNode), GetDiskUuid(diskNode));
                    return false;
                }

                if (!scheme.Equals("wholedisk", StringComparison.InvariantCultureIgnoreCase))
                {
                    Log.WarnFormat("scheme for Disk {0} ({1}) ({2}) is not 'WholeDisk'", GetDiskId(diskNode), GetDiskUuid(diskNode), scheme);
                    return false;
                }

                return true;

            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to determine whether a disk is encrypted: {0}", e.Message);
                return false;
            }

        }

        private static XElement GetDiskStatus(string utilityPath, string diskId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(diskId))
                    throw new Exception("Disk id cannot be null");

                var info = GetStartInfo(
                    utilityPath,
                    string.Format("--status --disk {0} --xml --aa", diskId));

                using (var process = Process.Start(info))
                {
                    process.WaitUntilExited();
                    if (process.ExitCode != 0)
                    {
                        Log.WarnFormat("Enum disk call returned {0}", process.ExitCode);
                       /* throw new Win32Exception(process.ExitCode);*/
                    }

                    var node = GetXElementFromOutput(process);
                    return node.Descendants("diskstatus").FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while retrieving disk info from pgpwde: {0}", e.Message);
                return null;
            }
        }

        private static ProcessStartInfo GetStartInfo(string utilityPath, string arguments)
        {
            return new ProcessStartInfo(utilityPath)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                Arguments = arguments,
                CreateNoWindow = true,
                RedirectStandardError = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };
        }

        public override bool IsInitialized()
        {
            return true;
        }
    }
}
