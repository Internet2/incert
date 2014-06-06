using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.LinkWrappers;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.HyperlinkModels
{
    class NextButtonHyperlinkModel:AbstractHyperlinkModel
    {
        public NextButtonHyperlinkModel(AbstractModel parentModel) : base(parentModel)
        {
            
        }

        public override void LoadContent(AbstractLink wrapper)
        {
            base.LoadContent(wrapper);
            RootDialogModel.NextModel.PropertyChanged += PropertyChangedHandler;
        }

        public override Brush TextBrush
        {
            get
            {
                 
                if (!Parent.Enabled)
                    return LinkBrush.MakeTransparent(45);

                if (!RootDialogModel.NextModel.Enabled)
                    return LinkBrush.MakeTransparent(45);

                return LinkBrush;

            }
            
        }



        public override ICommand Command
        {
            get { return RootDialogModel.NextModel.Command; }
            
        }

        protected override void SetEnabledBinding(Hyperlink instance)
        {
            instance.SetBinding(ContentElement.IsEnabledProperty, GetOneWayBinding(RootDialogModel.NextModel, "Enabled"));
        }

        
        public override bool Enabled
        {
            get { return RootDialogModel.NextModel.Enabled; }
            
        }
    }
}
