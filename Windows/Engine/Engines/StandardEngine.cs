using System;
using System.Windows;
using Ninject;
using Org.InCommon.InCert.Engine.AdvancedMenu;
using Org.InCommon.InCert.Engine.CommandLineProcessors;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Help;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Errors;
using Org.InCommon.InCert.Engine.Results.Errors.Control;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.Mapping;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.TaskBranches;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.BannerWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Managers;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using log4net;

namespace Org.InCommon.InCert.Engine.Engines
{
    public class StandardEngine : IEngine, IInitializable
    {
        private static readonly ILog Log = Logger.Create();
        public EngineModes Mode { get; set; }
        public Guid Identifier { get; private set; }

        public ISettingsManager SettingsManager { get; private set; }
        public IBranchManager BranchManager { get; private set; }
        public ICommandLineManager CommandLineManager { get; private set; }
        public IBannerManager BannerManager { get; private set; }
        public IAppearanceManager AppearanceManager { get; private set; }
        public IErrorManager ErrorManager { get; private set; }
        public IDialogsManager DialogsManager { get; private set; }
        public IHelpManager HelpManager { get; private set; }
        public IAdvancedMenuManager AdvancedMenuManager { get; private set; }
        public IEndpointManager EndpointManager { get; private set; }

        public StandardEngine(
            ISettingsManager settingsManager,
            IBranchManager branchManager,
            ICommandLineManager commandLineManager,
            IBannerManager bannerManager,
            IAppearanceManager appearanceManager,
            IErrorManager errorManager,
            IDialogsManager dialogsManager,
            IHelpManager helpManager,
            IAdvancedMenuManager advancedMenuManager,
            IEndpointManager endpointManager
            )
        {
            SettingsManager = settingsManager;
            BranchManager = branchManager;
            BannerManager = bannerManager;
            AppearanceManager = appearanceManager;
            CommandLineManager = commandLineManager;
            ErrorManager = errorManager;
            DialogsManager = dialogsManager;
            HelpManager = helpManager;
            AdvancedMenuManager = advancedMenuManager;
            EndpointManager = endpointManager;

            Identifier = Guid.NewGuid();
        }
        
        public IResult Execute()
        {
            try
            {
                var result = ExecuteBranch(BranchRoles.Default, null);
                
                result.LogFatalIfError();

                if (result is ErrorResult)
                    ExecuteBranch(BranchRoles.Error, result);
                
                result = ExecuteBranch(BranchRoles.Restart, result);
                ExecuteBranch(BranchRoles.Final, result);

                return result;
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while executing the standard engine: {0}", e);
                return new ExceptionOccurred(e);
            }
        }
        
        private IResult ExecuteBranch(BranchRoles role, IResult result)
        {
            var manager = Application.Current.CurrentKernel().Get<IBranchManager>();
            var branch = manager.GetBranchForRole(role, Mode, true);
            return branch == null ? new BranchNotFound(role, Mode) : branch.Execute(result);
        }


        public void Initialize()
        {
            Mode = EngineModes.Normal;
            SettingsManager.Initialize();
            BranchManager.Initialize();
            AppearanceManager.Initialize();
            BannerManager.Initialize();
            CommandLineManager.Initialize();
            ErrorManager.Initialize();
            DialogsManager.Initialize();
            HelpManager.Initialize();
            AdvancedMenuManager.Initialize();

        }
    }
}
