using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Org.InCommon.InCert.BootstrapperEngine.PropertyNotifiers;

namespace Org.InCommon.InCert.BootstrapperEngine.Models.Controls
{
    public abstract class AbstractBottomButtonModel : PropertyNotifyBase
    {
        private ICommand _command;
        private Brush _foreground;
        private Brush _background;
        private string _text;
       

        protected AbstractBottomButtonModel(PagedViewModel model)
        {
            Model = model;
            Foreground = model.Foreground;
            Background = model.Background;
        }

        protected  PagedViewModel Model { get; private set; }

        public ICommand Command
        {
            get { return _command; }
            set
            {
                _command = value;
                OnPropertyChanged("Command");
            }
        }

        public Brush Foreground
        {
            get { return _foreground; }
            set { _foreground = value; OnPropertyChanged("Foreground"); }
        }

        public Brush Background
        {
            get { return _background; }
            set { _background = value; OnPropertyChanged("Background"); }
        }

        public abstract Visibility Visibility { get;  }
        
        public string Text
        {
            get { return _text; }
            set { _text = value; OnPropertyChanged("Text"); }
        }
    }
}
