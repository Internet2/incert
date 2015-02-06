using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Effects;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Errors.UserInterface;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    class ShowBorderlessHtmlDialog : AbstractHtmlBannerTask
    {
        public ShowBorderlessHtmlDialog(IEngine engine)
            : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public bool Shadowed { get; set; }

        public override IResult Execute(IResult previousResults)
        {
            var dialog = DialogsManager.GetDialog<BorderlessDialogModel>(Dialog);
            if (dialog == null)
                return new DialogInstanceNotFound { Dialog = Dialog };

            var banner = GetOrCreateBanner();

            SetShadow(dialog);
            DialogsManager.ActiveDialogKey = Dialog;

            dialog.PreloadContent(banner);
            SetAddress(dialog, Url);

            var result = dialog.ShowBanner(banner);
            if (!result.IsOk())
            {
                return result;
            }

            WaitForLoad(dialog);

            return result;
        }

        private void SetShadow(BorderlessDialogModel dialog)
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
