using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.LinkWrappers;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.HyperlinkModels
{
    class AdvancedButtonHyperlinkModel:AbstractHyperlinkModel
    {
        public AdvancedButtonHyperlinkModel(AbstractModel parentModel) : base(parentModel)
        {
        }

        public override void LoadContent(AbstractLink wrapper)
        {
            base.LoadContent(wrapper);
            RootDialogModel.AdvancedModel.PropertyChanged += PropertyChangedHandler;
        }

        public override Brush TextBrush
        {
            get
            {

                if (!Parent.Enabled)
                    return LinkBrush.MakeTransparent(45);

                if (!RootDialogModel.AdvancedModel.Enabled)
                    return LinkBrush.MakeTransparent(45);

                return LinkBrush;

            }

        }
       

        protected override void SetEnabledBinding(Hyperlink instance)
        {
            instance.SetBinding(ContentElement.IsEnabledProperty, GetOneWayBinding(RootDialogModel.AdvancedModel, "Enabled"));
        }

        protected override void SetCommandBinding(Hyperlink instance)
        {
            instance.SetBinding(Hyperlink.CommandProperty, GetOneWayBinding(RootDialogModel.AdvancedModel, "Command"));
        }
    }
}
