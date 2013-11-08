using System;
using System.Windows.Controls;
using System.Windows.Media;
using Org.InCommon.InCert.BootstrapperEngine.PropertyNotifiers;

namespace Org.InCommon.InCert.BootstrapperEngine.Models.Pages
{
    public abstract class AbstractPageModel : PropertyNotifyBase, IDisposable
    {
        protected PagedViewModel Model { get; private set; }
        private Brush _foreground;
        private Brush _background;
        private Page _page;

        protected AbstractPageModel(Page page, PagedViewModel model)
        {
            Model = model;
            Page = page;
            Foreground = Model.Foreground;
            Background = Model.Background;
            Page.DataContext = this;
        }

        public Page Page
        {
            get { return _page; }
            set
            {
                _page = value;
                OnPropertyChanged("Page");
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

        public virtual void Dispose()
        {
            
        }
    }
}
