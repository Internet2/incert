using System.Windows.Input;
using System.Windows.Media;
using Org.InCommon.InCert.BootstrapperEngine.Commands;

namespace Org.InCommon.InCert.BootstrapperEngine.Models.Mockup.Controls
{
    public class MockBottomButtonModel
    {
        public ICommand Command
        {
            get { return new RelayCommand(DoNothing); }
        }

        private void DoNothing(object obj)
        {
        }

        public string Text { get; set; }
        public Brush Foreground { get { return new SolidColorBrush(Colors.White); } }
        public Brush TextBrush { get { return new SolidColorBrush(Colors.Chartreuse); } }
        public bool Visible { get { return true; } }
    }
}
