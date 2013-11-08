using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.Mockup.ContentModels
{
    public class MockContentPanelModel : MockBaseContentModel
    {
        public List<Label> ChildInstances
        {
            get
            {
                var result = new List<Label>
                    {
                        new Label
                            {
                                Visibility = Visibility.Visible,
                                Content = "Mock ContentPanelModel"
                            },
                            new Label
                            {
                                Visibility = Visibility.Visible,
                                Content = new TextBlock{Text ="Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum", TextWrapping = TextWrapping.Wrap}
                            }
                    };

                foreach (var item in result)
                {
                    DockPanel.SetDock(item, Dock.Top);
                }

                return result;
            }
        } 
    }
}
