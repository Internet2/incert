using System.Collections.Generic;
using System.Windows.Media;
using Org.InCommon.InCert.BootstrapperEngine.Models.Mockup.Controls;
using Org.InCommon.InCert.BootstrapperEngine.Views.Controls;

namespace Org.InCommon.InCert.BootstrapperEngine.Models.Mockup.Pages
{
    public class MockOptionPageModel
    {
        public Brush Background {get {return new SolidColorBrush(Colors.Teal);}}
        public Brush Foreground { get { return new SolidColorBrush(Colors.White); } }
        public string Title { get { return "Title"; } }
        public string Description { get
        {
            return
                "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";
        } }

        public List<OptionGroup> Children
        {
            get
            {
                return new List<OptionGroup>
                {
                    new OptionGroup {DataContext = new MockOptionGroup{GroupName = "testing group 1"}},
                    new OptionGroup {DataContext = new MockOptionGroup{GroupName = "testing group 2"}}
                };
            }
        }

        public object FrameBackground
        {
           get { return new SolidColorBrush(Colors.SteelBlue);  }
        }
    }
}

