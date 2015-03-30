using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    class ShowBorderlessHtmlBanner : AbstractHtmlBannerTask
    {
        public ShowBorderlessHtmlBanner(IEngine engine)
            : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public bool Shadowed { get; set; }

        protected override IResult ShowBanner(IResult previousResults)
        {
            try
            {
                var dialog = GetBannerDialog<BorderlessHtmlDialogModel>();
                var parent = GetParentDialog();

                var banner = GetOrCreateBanner();

                SetShadow(dialog);
                DialogsManager.ActiveDialogKey = Dialog;
                
                var result = (parent==null) 
                    ? dialog.ShowBanner(banner)
                    : parent.ShowChildBanner(dialog,banner);

                if (!result.IsOk())
                {
                    return result;
                }

                return WaitForLoad(dialog);
            }
            catch (DialogInstanceNotFound e)
            {
                return new Results.Errors.UserInterface.DialogInstanceNotFound {Dialog = e.Dialog};
            }
            catch (Exception e)
            {
                
                return new ExceptionOccurred(e);
            }
        }

        private void SetShadow(BorderlessHtmlDialogModel dialog)
        {
            if (!Shadowed)
            {
                dialog.DropShadow = null;
                return;
            }

            dialog.DropShadow = new DropShadowEffect
            {
                BlurRadius = 10,
                Color = Colors.Black,
                Opacity = .8,
                ShadowDepth = 8
            };
        }

        public override string GetFriendlyName()
        {
            return string.Format("Show html banner ({0}) in borderless dialog {1}", Url, Dialog);
        }
    }
}
