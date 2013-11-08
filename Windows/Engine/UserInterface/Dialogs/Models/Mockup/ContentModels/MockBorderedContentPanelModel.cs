using System.Windows;
using System.Windows.Media;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.Mockup.ContentModels
{
    public class MockBorderedContentPanelModel:MockContentPanelModel
    {
        public Thickness BorderSize { get { return new Thickness(2); } }
        public Brush BorderBrush { get { return new SolidColorBrush(Colors.Black); } }
        public CornerRadius CornerRadius { get { return new CornerRadius(5); } }
        
    }
}
