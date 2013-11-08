using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Errors;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.UserInterface;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    class ShowErrorDialog : AbstractTask
    {
       [PropertyAllowedFromXml]
        public string Dialog
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string Banner
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string ErrorKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string ErrorInfoKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string TopicKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string AdvancedMenuGroupKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public ShowErrorDialog(IEngine engine) :
            base(engine)
        {
            
            Dialog = "Main dialog";
            Banner = "Error banner";
            ErrorInfoKey = "current issue info";
            ErrorKey = "CurrentIssue";
            TopicKey = "current issue help topic";
            AdvancedMenuGroupKey = "current issue advanced menu group";
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var issue = GetErrorResult(previousResults);
                var info = ErrorManager.GetErrorEntry(issue);
                SettingsManager.SetTemporaryObject(ErrorInfoKey, info);
                SettingsManager.SetTemporarySettingString(TopicKey, info.Topic);
                SettingsManager.SetTemporarySettingString(AdvancedMenuGroupKey, info.AdvancedMenuGroup);

                var dialog = DialogsManager.GetDialog<BorderedDialogModel>(Dialog);
                return dialog == null ? new DialogInstanceNotFound { Dialog = Dialog } :
                    dialog.ShowBannerModal(Banner);

            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        private ErrorResult GetErrorResult(IResult previousResults)
        {
            var result = SettingsManager.GetTemporaryObject(ErrorKey) as ErrorResult;
            if (result != null)
                return result;

            if (previousResults is ErrorResult)
                SettingsManager.SetTemporaryObject(ErrorKey, previousResults);
            else
                SettingsManager.SetTemporaryObject(
                    ErrorInfoKey,
                    new ExceptionOccurred(
                        new Exception(
                            "Could not determine issue details. Please consult the utility log for more details.")));

            return SettingsManager.GetTemporaryObject(ErrorKey) as ErrorResult;
        }

        public override string GetFriendlyName()
        {
            return "Show error dialog";
        }
    }
}
