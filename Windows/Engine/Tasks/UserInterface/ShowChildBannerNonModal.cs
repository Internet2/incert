using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.UserInterface;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    internal class ShowChildBannerNonModal : AbstractTask
    {
        [PropertyAllowedFromXml]
        public string Banner
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string ChildDialog
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string ParentDialog
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public ShowChildBannerNonModal(IEngine engine)
            : base(engine)
        {
            
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ChildDialog))
                    throw new Exception("Dialog key cannot be null or empty");

                if (string.IsNullOrWhiteSpace(Banner))
                    throw new Exception("Banner key cannot be null or empty");

                var dialog = DialogsManager.GetExistingDialog(ParentDialog);
                if (dialog == null)
                    return new DialogInstanceNotFound {Dialog = ParentDialog};

                var childDialog = DialogsManager.GetDialog < BorderedChildDialogModel > (ChildDialog);
                return childDialog == null ? 
                    new DialogInstanceNotFound { Dialog = ChildDialog } :
                    dialog.ShowChildBanner(childDialog, Banner);
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Show child banner modal";
        }

    }
}
