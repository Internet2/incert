using System;
using System.Windows;
using System.Windows.Threading;
using Ninject;
using Ninject.Modules;
using Org.InCommon.InCert.Engine.AdvancedMenu;
using Org.InCommon.InCert.Engine.ClientIdentifier;
using Org.InCommon.InCert.Engine.CommandLineProcessors;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Help;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.NativeCode.Wmi;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.Mapping;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.TaskBranches;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.BannerWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Managers;
using Org.InCommon.InCert.Engine.Utilities;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using log4net;

namespace Org.InCommon.InCert.Engine
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private static readonly ILog Log = Logger.Create();

        public App()
        {
            Log.InitializeCustomLevels();
            Kernel = new StandardKernel(new EngineModule());
        }

        public IKernel Kernel { get; private set; }

        private void StartupHandler(object sender, StartupEventArgs e)
        {
            Log.InfoFormat("Client started - {0}", DateTime.UtcNow);

            var engine = Kernel.Get<IEngine>();
            var result = engine.Execute();
            if (result is RestartComputerResult)
                SystemUtilities.RestartComputer();

            Current.Shutdown(result.ResultCode);

        }

        private void ExceptionHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Log.WarnFormat("An unhandled exception has occurred: {0}", e.Exception);
            e.Handled = false;
        }


        private class EngineModule : NinjectModule
        {
            public override void Load()
            {
                Bind<IEngine>().To<StandardEngine>().InSingletonScope();
                Bind<ISettingsManager>().To<SettingsManager>().InSingletonScope();
                Bind<IBranchManager>().To<BranchManager>().InSingletonScope();
                Bind<ICommandLineManager>().To<CommandLineManager>().InSingletonScope();
                Bind<IBannerManager>().To<BannerManager>().InSingletonScope();
                Bind<IAppearanceManager>().To<AppearanceManager>().InSingletonScope();
                Bind<IErrorManager>().To<ErrorManager>().InSingletonScope();
                Bind<IDialogsManager>().To<DialogsManager>().InSingletonScope();
                Bind<IEndpointManager>().To<EndpointManager>().InSingletonScope();
                Bind<IHelpManager>().To<HelpManager>().InSingletonScope();
                Bind<IAdvancedMenuManager>().To<AdvancedMenuManager>().InSingletonScope();
                
                Bind<IClientIdentifier>().To<InstallationIdClientIdentifier>()
                    .InSingletonScope()
                    .WithConstructorArgument("productProxy", new SoftwareLicensingProductProxy())
                    .WithConstructorArgument("backupIdentifier", new StoredClientIdentifier());
            }
        }
    }

}
