using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Tasks.Utility
{
    class ArchiveFolderContents:AbstractTask
    {
        [PropertyAllowedFromXml]
        public string TargetPath
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }

        [PropertyAllowedFromXml]
        public string ArchivePath
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }
        
        public ArchiveFolderContents(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TargetPath))
                    throw new Exception("Target path cannot be empty");

                if (!Directory.Exists(TargetPath))
                    throw new Exception(string.Format("Target folder {0} does not exist.", TargetPath));

                if (Directory.GetFiles(TargetPath).Length<=0)
                    throw new Exception(string.Format("Target folder {0} contains no files.", TargetPath));

                if (string.IsNullOrWhiteSpace(ArchivePath))
                    throw new Exception("Destination path cannot be empty");

                var destinationFolder = new FileInfo(ArchivePath).DirectoryName;
                if (string.IsNullOrWhiteSpace(destinationFolder))
                    throw new Exception("Could not retrieve valid destination path.");
                
                if (!Directory.Exists(destinationFolder))
                    throw new Exception(string.Format("Destination path {0} does not exist", ArchivePath));

                var directivesFilePath = Path.GetTempFileName();
                File.WriteAllText(
                    directivesFilePath, 
                    GenerateDirectivesFile(TargetPath, ArchivePath).ToString());

                var info = new ProcessStartInfo(PathUtilities.GetSystemUtilityPath("makecab.exe"))
                    {
                        WorkingDirectory = destinationFolder,
                        Arguments = string.Format("/F \"{0}\"", directivesFilePath),
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        UseShellExecute = false
                    };

                using (var process = Process.Start(info))
                {
                    do
                    {
                        Application.Current.DoEvents(250);
                        process.WaitForExit(5);
                    } while (!process.HasExited);

                    if (process.ExitCode != 0)
                    {
                        throw new Exception(string.Format("Creating cabinet file failed with issue code {0}", process.ExitCode));
                    }
                }

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        private static StringBuilder GenerateDirectivesFile(string targetPath, string destinationPath)
        {
            var cabname = Path.GetFileName(destinationPath);
            var buffer = new StringBuilder();
            buffer.AppendLine(".OPTION EXPLICIT");
            buffer.AppendFormat(".Set CabinetNameTemplate={0}", cabname);
            buffer.AppendLine();
            buffer.AppendLine(".Set DiskDirectoryTemplate=CDROM");
            buffer.AppendLine(".Set CompressionType=MSZIP");
            buffer.AppendLine(".Set  UniqueFiles=\"OFF\"");
            buffer.AppendLine(".Set Cabinet=on");
            buffer.AppendFormat(".Set DiskDirectory1=\"\"");
            buffer.AppendLine();

            foreach (var filename in Directory.GetFiles(targetPath))
            {
                buffer.AppendLine("\""+filename+"\"");
            }

            return buffer;
        }

        public override string GetFriendlyName()
        {
            return "Archive folder";
        }


    }
}
