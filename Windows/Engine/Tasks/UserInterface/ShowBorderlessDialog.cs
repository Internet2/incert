using System;
using System.Windows.Media;
using System.Windows.Media.Effects;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.UserInterface;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    class ShowBorderlessDialog:AbstractTask
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
        public bool Shadowed { get; set; }

        public ShowBorderlessDialog(IEngine engine)
            : base(engine)
        {
           
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var dialog = DialogsManager.GetDialog<BorderlessDialogModel>(Dialog);
                if (dialog == null)
                    return new DialogInstanceNotFound { Dialog = Dialog };
                if (Shadowed)
                {
                    dialog.DropShadow = new DropShadowEffect
                    {
                        BlurRadius = 10,
                        Color = Colors.Black,
                        Opacity = .8,
                        ShadowDepth = 8
                    };
                }
                else
                {
                    dialog.DropShadow = null;
                }

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
            return string.Format("Show borderless dialog ({0})",Dialog);
        }
    }
}
