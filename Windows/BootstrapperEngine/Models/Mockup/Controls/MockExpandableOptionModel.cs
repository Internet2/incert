using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Org.InCommon.InCert.BootstrapperEngine.Models.Mockup.Controls
{
    public class MockExpandableOptionModel
    {
        public MockExpandableOptionModel ()
        {
            Title = "Testing";
            GroupName = "Testing Group";
        }
        
        private bool _isChecked = true;

        public Brush Background {get {return new SolidColorBrush(Colors.Teal);}}
        public Brush Foreground {get {return new SolidColorBrush(Colors.White);}}
        public Brush LinkBrush {get {return new SolidColorBrush(Colors.SpringGreen);}}
        public Brush TitleBrush { get {return new SolidColorBrush(Colors.Yellow);} }
        public Brush BorderBrush {get
        {
            return !IsChecked ? Background : new SolidColorBrush(Colors.Thistle);
        }
        }
        public bool IsChecked { get { return _isChecked; } set { _isChecked = value; } }
        public string Title { get; set; }
        public string GroupName { get; set; }
        public string Description { get
        {
            return
                "This is an extended description for this option.  If the user selects this option, this description will appear.  Otherwise, it will be hidden.";
        } }
        public string DetailsLinkText { get { return "more details"; } }
        
        
        public ICommand DetailsCommand { get { return null; } }
        public Thickness Margin {get {return new Thickness(8);}}
    }
}
