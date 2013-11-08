using System.Collections.Generic;
using System.Windows.Media;
using Org.InCommon.InCert.BootstrapperEngine.Views.Controls;

namespace Org.InCommon.InCert.BootstrapperEngine.Models.Mockup.Controls
{
    public class MockOptionGroup
    {
        public MockOptionGroup()
        {
            Title = "Testing";
            GroupName = "Testing Group";
        }
        
        public Brush Background { get { return new SolidColorBrush(Colors.Teal); } }
        public Brush Foreground { get { return new SolidColorBrush(Colors.White); } }
        public Brush TitleBackground {get {return new SolidColorBrush(Colors.LightBlue);}}
       
        public string Title { get; set; }
        public string GroupName { get; set; }

        public List<ExpandableOption> Children
        {
            get
            {
                return new List<ExpandableOption>
                {
                    new ExpandableOption {DataContext = new MockExpandableOptionModel{GroupName = GroupName, IsChecked = true}},
                    new ExpandableOption {DataContext = new MockExpandableOptionModel{GroupName = GroupName, IsChecked = false}}
                };
            }
        }

        public object TitleBrush
        {
            get { return new SolidColorBrush(Colors.WhiteSmoke); }
        }
    }
}
