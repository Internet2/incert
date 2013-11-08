using Org.InCommon.InCert.Engine.AdvancedMenu;
using Org.InCommon.InCert.Engine.CommandLineProcessors;
using Org.InCommon.InCert.Engine.Help;
using Org.InCommon.InCert.Engine.Results.Errors.Mapping;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.TaskBranches;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.BannerWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Managers;
using Org.InCommon.InCert.Engine.WebServices.Managers;

namespace Org.InCommon.InCert.Engine.Engines
{
    public interface IHasEngineFields
    {
        ISettingsManager SettingsManager { get; }
        IBranchManager BranchManager { get; }
        ICommandLineManager CommandLineManager { get; }
        IBannerManager BannerManager { get; }
        IAppearanceManager AppearanceManager { get; }
        IErrorManager ErrorManager { get; }
        IDialogsManager DialogsManager { get; }
        IHelpManager HelpManager { get; }
        IAdvancedMenuManager AdvancedMenuManager { get; }
        IEndpointManager EndpointManager { get; }
    }
}
