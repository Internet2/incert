using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Org.InCommon.InCert.BootstrapperEngine.Enumerations;
using Org.InCommon.InCert.BootstrapperEngine.Extensions;
using Org.InCommon.InCert.BootstrapperEngine.Logging;
using Org.InCommon.InCert.BootstrapperEngine.Models;
using Org.InCommon.InCert.BootstrapperEngine.NamedPipeServer;
using Org.InCommon.InCert.BootstrapperEngine.NamedPipeServer.MessageHandlers;

namespace Org.InCommon.InCert.BootstrapperEngine.Commands
{
    class LaunchEngineCommand:RelayCommand
    {
        public const string WaitMessage = "ready";
        
        public LaunchEngineCommand(PagedViewModel model, bool newInstall) : base(param=>StartUtility(model, newInstall), param=>true)
        {
        }

        private static void StartUtility(PagedViewModel model, bool newInstall)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.BaseModel.InstallFolder))
                {
                    Logger.Error("Cannot launch engine; InstallFolder not set");
                    return;
                }

                var installFolder = model.BaseModel.InstallFolder;
                var enginePath = Path.Combine(installFolder, "engine.exe");
                if (!File.Exists(enginePath))
                {
                    Logger.Error("Cannot launch engine; invalid path {0}", enginePath);
                    return;
                }

                var pipeName = Guid.NewGuid().ToString();
                var info = new ProcessStartInfo(enginePath)
                    {
                        Verb = "runas",
                        Arguments = string.Format("-echo=ready -pipe=\"{0}\"", pipeName)
                    };

                Process.Start(info);
                model.BaseModel.UploadLogEntry("Install", "Launching engine");
                model.InstallationState = newInstall 
                    ? InstallationState.LaunchingNewInstall 
                    : InstallationState.LaunchingAlreadyInstalled;

                var waitTask = new Task(() => WaitForExitMessage(15, pipeName));
                waitTask.Start();
                waitTask.WaitWithPumping();
               
                model.Dispatcher.BeginInvoke((Action)(() => model.View.Close()));
            }
            catch (Exception e)
            {
                model.BaseModel.UploadLogEntry("InstallError", "An exception occurred while attempting to launch the engine: {0}", e.Message);
                Logger.Error("An exception occurred while attempting to launch the engine: {0}", e.Message);
            }
        }

        private static void WaitForExitMessage(int seconds, string pipeName)
        {
            using (var pipeServer = new PipeServer(pipeName))
            {
                var messageHandler = new WaitHandler(pipeServer, WaitMessage);
                var start = DateTime.UtcNow;
                do
                {
                    Thread.Sleep(5);

                    if (messageHandler.Received)
                        break;

                } while (DateTime.UtcNow.Subtract(start).Duration().TotalSeconds < seconds);
            }

        }

       
    }

         
}


