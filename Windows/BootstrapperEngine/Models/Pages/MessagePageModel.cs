using Org.InCommon.InCert.BootstrapperEngine.Views.Pages;

namespace Org.InCommon.InCert.BootstrapperEngine.Models.Pages
{
    public class MessagePageModel : AbstractPageModel
    {
        public MessagePageModel(PagedViewModel model)
            : base(new MessagePage(), model)
        {
        }

        private string _text;
        public string Text
        {
            get { return _text; } 
            set { _text = value; OnPropertyChanged("Text"); }
        }


    }
}
