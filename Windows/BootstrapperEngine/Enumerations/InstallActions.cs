using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using Org.InCommon.InCert.BootstrapperEngine.Extensions;

namespace Org.InCommon.InCert.BootstrapperEngine.Enumerations
{
    public class InstallActions
    {
        public static readonly InstallActions Install = new InstallActions(LaunchAction.Install, Verbs.Install, InstallationState.InstallApplied);
        public static readonly InstallActions Upgrade = new InstallActions(LaunchAction.Install, Verbs.Update, InstallationState.InstallApplied);
        public static readonly InstallActions Repair = new InstallActions(LaunchAction.Repair, Verbs.Repair, InstallationState.RepairApplied);
        public static readonly InstallActions Remove = new InstallActions(LaunchAction.Uninstall, Verbs.Remove, InstallationState.RemoveApplied);
        public static readonly InstallActions Layout = new InstallActions(LaunchAction.Layout, null, InstallationState.LayoutApplied);
        public static readonly InstallActions Help = new InstallActions(LaunchAction.Help, null, InstallationState.HelpApplied);
        public static readonly InstallActions Modify = new InstallActions(LaunchAction.Modify, Verbs.Update, InstallationState.InstallApplied);
        public static readonly InstallActions UpdateReplace = new InstallActions(LaunchAction.UpdateReplace, null, InstallationState.InstallApplied);
        public static readonly InstallActions UpdateReplaceEmbedded = new InstallActions(LaunchAction.UpdateReplaceEmbedded, null, InstallationState.InstallApplied);
        public static readonly InstallActions Unknown = new InstallActions(LaunchAction.Unknown, null, InstallationState.Unknown);
        
        private InstallActions(LaunchAction value, Verbs verb, InstallationState successState)
        {
            Value = value;
            SuccessState = successState;
            Verb = verb;
        }

        public string GetProgressTitle(string productName)
        {
            return Verb == null 
                ? "" 
                : string.Format("{0} {1}", Verb.PresentParticiple.UppercaseFirst(), productName);
        }

        public string GetProgressNote(string productName)
        {
            return Verb == null
                ? ""
                : string.Format("It may take a few minutes to finish {0}\n{1}", Verb.PresentParticiple, productName);
        }

        public string GetIssueText(string productName, int issueCode, string issueText)
        {
            if (Verb==null)
                return string.Format("An unexpected issue has occurred:\n\n{0} ({1})", issueText, issueCode);

            return string.Format("An unexpected issue occurred while {0} {1}\n\n{2} ({3})",
                Verb.PresentParticiple, productName, issueText, issueCode);
        }

        public bool IsInstallAction()
        {
            if (Value == LaunchAction.Help)
                return false;

            if (Value == LaunchAction.Layout)
                return false;

            if (Value == LaunchAction.Uninstall)
                return false;

            if (Value == LaunchAction.Repair)
                return false;

            return true;
        }

        public InstallationState SuccessState { get; private set; }
        public LaunchAction Value { get; private set; }
        public Verbs Verb { get; private set; }
    }


}
