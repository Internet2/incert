using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Instances.CustomControls;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.Mockup.ContentModels
{
    class MockAdvancedMenuGroupContainer
    {
        public string Title { get { return "Mock Menu Item Title"; } }
        
        public Brush BackGround {get {return new SolidColorBrush(Colors.LightGoldenrodYellow);}}
        public Brush TextBrush { get { return new SolidColorBrush(Colors.Black); } }
        public FontFamily FontFamily { get { return new FontFamily("Verdana"); } }
        public bool IsEnabled { get { return true; } }
        public Visibility ContainerVisibility {get {return Visibility.Visible;}}
        public ICommand Command { get { return null; } }

        public List<AdvancedMenuEntry> Children
        {
            get {return new List<AdvancedMenuEntry>
                {
                    new AdvancedMenuEntry {DataContext = new MockAdvancedMenuItem()},
                    new AdvancedMenuEntry {DataContext = new MockAdvancedMenuItem()}
                };}
        } 
    }
}
