using System.Windows;
using System.Windows.Controls;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels
{
    class HorizontalDividerModel:AbstractContentModel
    {
        public HorizontalDividerModel(AbstractModel parentModel) : base(parentModel)
        {
           
        }

        public override T LoadContent<T>(AbstractContentWrapper wrapper)
        {
            var line = new Border
                {
                    BorderThickness = new Thickness(0,GetHeightFromWrapper(wrapper as HorizontalDivider),0,0),
                    BorderBrush = AppearanceManager.GetBrushForColor(wrapper.Color, AppearanceManager.BorderBrush)
                };

            InitializeBindings(line);
            InitializeValues(wrapper);
            Content = line;
            return line as T;
        }

        private static int GetHeightFromWrapper(HorizontalDivider wrapper)
        {
            return wrapper == null ? 0 : wrapper.Height;
        }
    }
}
