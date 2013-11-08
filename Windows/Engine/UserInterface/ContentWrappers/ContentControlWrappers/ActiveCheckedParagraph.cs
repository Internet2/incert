using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers
{
    class ActiveCheckedParagraph : DefaultCheckedParagraph
    {
       
        public ActiveCheckedParagraph(IEngine engine) : 
            base(engine)
        {
           
        }

       /* public override FrameworkElement GetWrappingElement(AbstractModel model)
        {
            try
            {
                var decorator = base.GetWrappingElement(model);
                if (decorator == null)
                    return null;

                var rectangle = UserInterfaceUtilities.GetControlByName<Rectangle>(decorator, "Rectangle");
                if (rectangle != null)
                    rectangle.Stroke = new SolidColorBrush(AppearanceManager.GetColorForStyle("Body"));

                var content = UserInterfaceUtilities.GetControlByName<TextBlock>(decorator, "Content");
                if (content != null)
                    content.Foreground = new SolidColorBrush(AppearanceManager.GetColorForStyle("Body"));

                return decorator;
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return null;
            }
        }*/

       
       
        }

    
}
