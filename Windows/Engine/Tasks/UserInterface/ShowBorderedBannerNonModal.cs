using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.UserInterface;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    class ShowBorderedBannerNonModal:AbstractTask
    {
        [PropertyAllowedFromXml]
        public string Banner
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string Dialog
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public ShowBorderedBannerNonModal(IEngine engine)
            : base(engine)
        {
         
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var dialog = DialogsManager.GetDialog<BorderedDialogModel>(Dialog);
                if (dialog == null)
                    return new DialogInstanceNotFound { Dialog = Dialog };

                DialogsManager.ActiveDialogKey = Dialog;

                return dialog.ShowBanner(Banner);
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return string.Format("Show non-modal banner ({0}) in bordered dialog ({1})", Banner, Dialog);
        }
    }
}
