using Org.InCommon.InCert.BootstrapperEngine.Views.Pages;

namespace Org.InCommon.InCert.BootstrapperEngine.Models.Pages
{
    public class AskingPageModel : AbstractPageModel
    {
        private string _title;
        private string _details;
        private string _question;
        private string _glyph;

        public AskingPageModel(PagedViewModel model)
            : base(new AskingPage(), model)
        {
        }

        public string Title { get { return _title; } set { _title = value; OnPropertyChanged("Title"); } }
        public string Details { get { return _details; } set { _details = value; OnPropertyChanged("Details"); } }
        public string Question { get { return _question; } set { _question = value; OnPropertyChanged("Question"); } }
        public string Glyph {get { return _glyph; } set { _glyph = value;OnPropertyChanged("Glyph");} } 
    }
}
