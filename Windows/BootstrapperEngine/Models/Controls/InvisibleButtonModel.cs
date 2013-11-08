using System.Windows;

namespace Org.InCommon.InCert.BootstrapperEngine.Models.Controls
{
    class InvisibleButtonModel:AbstractBottomButtonModel
    {
        public InvisibleButtonModel(PagedViewModel model) : base(model)
        {
        }

        public override Visibility Visibility
        {
            get { return Visibility.Hidden; }
        }
    }
}
