using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Instances.CustomControls;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.Mockup.ContentModels;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.Mockup.DialogModels
{
    internal class MockAdvancedMenuDialog
    {
        public MockAdvancedMenuDialog()
        {
            CloseModel = new MockNavigationModel { Text = "Next" };
            RunModel = new MockNavigationModel { Text = "Back" };
            HelpModel = new MockHiddenNavigtationModel { Text = "Help" };
        }

    public string Title { get { return "Instructions"; } }
        public string Description
        {
            get
            {
                return
                    "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.";
            }
        }
        public string WindowTitle { get { return "Mock Advanced Menu"; } }
        public Brush ContainerBackGround { get { return new SolidColorBrush(Colors.LightGoldenrodYellow); } }
        public Brush Background { get { return new SolidColorBrush(Colors.LightGreen); } }
        public FontFamily FontFamily { get { return new FontFamily("Verdana"); } }
        public Brush TextBrush { get { return new SolidColorBrush(Colors.Black); } }
        public double Width { get { return 500; } }
        public double Height { get { return 550; } }
        public bool IsEnabled { get { return true; } }
        public MockNavigationModel CloseModel { get; private set; }
        public MockNavigationModel RunModel { get; private set; }
        public MockNavigationModel HelpModel { get; private set; }
        public double Left { get { return 0; } }
        public double Top { get { return 0; } }
        public ICommand RunCommand { get { return null; } }
        public Cursor Cursor { get { return Cursors.Hand; } }

        public ICommand ClearFocusCommand { get { return null; } }



        public List<AdvancedMenuItemContainer> Groups
        {
            get
            {
                return new List<AdvancedMenuItemContainer>
                    {
                        new AdvancedMenuItemContainer {DataContext = new MockAdvancedMenuGroupContainer()},
                        new AdvancedMenuItemContainer {DataContext = new MockAdvancedMenuGroupContainer()},
                    };
            }
        }
    }
}
