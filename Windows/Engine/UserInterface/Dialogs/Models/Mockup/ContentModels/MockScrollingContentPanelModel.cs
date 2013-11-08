using System.Windows.Controls;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.Mockup.ContentModels
{
    class MockScrollingContentPanelModel:MockBorderedContentPanelModel
    {
        public ScrollBarVisibility VerticalScrollBarVisibility { get { return ScrollBarVisibility.Visible; } }
        public ScrollBarVisibility HorizontalScrollBarVisibility {get {return ScrollBarVisibility.Disabled;}}
        public bool CanScroll { get { return true; } }
    }
}
