using System.Windows.Controls;
using Org.InCommon.InCert.BootstrapperEngine.Views.Pages;

namespace Org.InCommon.InCert.BootstrapperEngine.Models.Mockup.Pages
{
    public class MockPageModel
    {
        public Page Page {get {return new ButtonPage {DataContext = new MockButtonPageModel()};}}
    }
}
