using System;
using System.Windows.Threading;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using Org.InCommon.InCert.BootstrapperEngine.Logging;
using Org.InCommon.InCert.BootstrapperEngine.Models;

namespace Org.InCommon.InCert.BootstrapperEngine
{
    class BootstrapperEngine : BootstrapperApplication
    {

        protected override void Run()
        {
            try
            {
                Logger.Initialize(Engine);
                Logger.Verbose("Launching bootstrapper engine");

                Logger.Verbose("Initializing base model");
                var baseModel = new BaseModel(this);

                Logger.Verbose("Initializing view model");
                var viewModel = new PagedViewModel(baseModel);

                //baseModel.UploadLogEntry("Install", "Starting install; version={0}", string.Format("{0}", Assembly.GetExecutingAssembly().GetName().Version));

                Logger.Verbose("Starting Detect");
                Engine.Detect();

                baseModel.Result = 1602; // assume result = cancel
                if (baseModel.Command.Display == Display.Passive || baseModel.Command.Display == Display.Full)
                {
                    Logger.Verbose("Showing view");
                    viewModel.View.Show();
                }

                Dispatcher.Run();
                
                Engine.Quit(baseModel.Result);
            }
            catch (Exception e)
            {
                Engine.Log(LogLevel.Error, string.Format("An exception occurred while attempting to initialize the engine: {0}", e.StackTrace));
                Engine.Quit(-1);
            }
        }

    }
}
