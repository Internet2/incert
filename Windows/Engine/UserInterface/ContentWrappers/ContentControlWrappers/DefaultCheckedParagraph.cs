using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers
{
    class DefaultCheckedParagraph : AbstractContentWrapper
    {
        public DefaultCheckedParagraph(IEngine engine)
            : base(engine)
        {
        }

        public override System.Type GetSupportingModelType()
        {
            return typeof(CheckedParagraphModel);
        }

       /* public override FrameworkElement GetWrappingElement(AbstractModel model)
        {
            try
            {
                var controlWrapper = new DockPanel();
                StyleElement(controlWrapper, "CheckedParagraphOuterPanelStyle", model);

                var checkMark = GetCheckMarkGeometry(model);
                controlWrapper.Children.Add(checkMark);

                var container = new StackPanel { Name = "ContentContainer" };
                StyleElement(container, "CheckedParagraphInnerPanelStyle", model);

                var content = base.GetWrappingElement(model) as TextBlock;
                if (content == null)
                    return null;

                var fill = AppearanceManager.DisabledTextBrush;
                content.Foreground = fill;
                content.VerticalAlignment = VerticalAlignment.Center;
                container.Children.Add(content);

                DockPanel.SetDock(container, Dock.Right);
                controlWrapper.Children.Add(container);
                return controlWrapper;
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return null;
            }
        }*/

       /* private UIElement GetCheckMarkGeometry(AbstractModel model)
        {
            try
            {
                var checkPath = AppearanceManager.GetElementEntry<Geometry>("CheckMarkGeometry");
                if (checkPath == null)
                {
                    Log.Warn("Could not find resource 'CheckMarkGeometry' in context");
                    return null;
                }

                var path = new Path
                    {
                        Name = "Check",
                        Data = checkPath,
                        Fill = AppearanceManager.TitleTextBrush,
                        Stroke = AppearanceManager.DisabledTextBrush,
                        Visibility = Visibility.Hidden
                    };

                StyleElement(path,"CheckMarkPathStyle", model);

                var rectangle = new Rectangle
                    {
                        Name = "Rectangle",
                        Style = AppearanceManager.GetStyle("CheckMarkRectangleStyle"),
                        Stroke = AppearanceManager.DisabledTextBrush,
                    };
                StyleElement(rectangle, "CheckMarkRectangleStyle", model);

                var grid = new Grid();
                StyleElement(grid, "CheckMarkGridStyle", model);
                grid.Children.Add(rectangle);
                grid.Children.Add(path);

                return grid;

            }
            catch (Exception e)
            {
                Log.Warn(e);
                return null;
            }
        }*/

        public static void ConfigureControls(FrameworkElement instance, Visibility checkMarkVisibility, Brush rectangleBrush, Brush checkBrush, Brush textBrush)
        {
            var check = UserInterfaceUtilities.GetControlByName<Path>(instance, "Check");
            if (check != null)
            {
                check.Visibility = checkMarkVisibility;
                check.Stroke = checkBrush;
                check.Fill = checkBrush;
            }
               

            var rectangle = UserInterfaceUtilities.GetControlByName<Rectangle>(instance, "Rectangle");
            if (rectangle != null)
                rectangle.Stroke = rectangleBrush;

            var text = UserInterfaceUtilities.GetControlByName<TextBlock>(instance, "Content");
            if (text != null)
                text.Foreground = textBrush;

            var subtitle = UserInterfaceUtilities.GetControlByName<TextBlock>(instance, "Subtitle");
            if (subtitle != null)
                subtitle.Foreground = textBrush;
        }

    }
}
