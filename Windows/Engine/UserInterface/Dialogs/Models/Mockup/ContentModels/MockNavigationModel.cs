using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.Mockup.ContentModels
{
    class MockNavigationModel
    {
        public virtual Visibility Visibility { get { return Visibility.Visible; } }
        public string Text { get; set; }

        public SolidColorBrush TextBrush
        {
            get { return new SolidColorBrush(Colors.White); }
        }

        public bool Enabled { get { return true; } }

        public ICommand Command
        {
            get { return null; }
        }

        public bool IsDefaultButton {get { return false; }}
        public bool IsCancelButton { get { return false; }}
    }
}