using System.Windows.Input;
using Org.InCommon.InCert.BootstrapperEngine.Views.Pages;

namespace Org.InCommon.InCert.BootstrapperEngine.Models.Pages
{
    public class ButtonPageModel:AbstractPageModel
    {
        private ICommand _command;
        private string _buttonTitle;
        private string _buttonSubTitle;
        private string _instructions;

        private Cursor _cursor;
        
        public ButtonPageModel(PagedViewModel model) : base(new ButtonPage(), model)
        {
        }

        public ICommand Command
        {
            get { return _command; }
            set { _command = value; OnPropertyChanged("Command"); }
        }

        public string ButtonTitle
        {
            get { return _buttonTitle; }
            set { _buttonTitle = value; OnPropertyChanged("ButtonTitle"); }
        }

        public string ButtonSubTitle
        {
            get { return _buttonSubTitle; }
            set { _buttonSubTitle = value; OnPropertyChanged("ButtonSubTitle"); }
        }

        public string Instructions
        {
            get { return _instructions; }
            set { _instructions = value; OnPropertyChanged("Instructions"); }
        }

        public Cursor Cursor
        {
            get { return _cursor; }
            set { _cursor = value; OnPropertyChanged("Cursor"); }
        }

        
    }
}
