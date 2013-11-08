using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels
{
    class EllipseGlyphWrapper:AbstractContentModel
    {
        public EllipseGlyphWrapper(AbstractModel parentModel) : base(parentModel)
        {
        }

        public override T LoadContent<T>(AbstractContentWrapper wrapper)
        {
            var glyphWrapper = wrapper as EllipseGlyph;
            if (glyphWrapper == null)
                return default(T);

            var text = glyphWrapper.Glyph;
            if (string.IsNullOrWhiteSpace(text))
                return(default(T));

            FontFamily = wrapper.FontFamily ?? AppearanceManager.DefaultFontFamily;
            FontSize = wrapper.FontSize.GetValueOrDefault(12);
            FontWeight = wrapper.FontWeight.GetValueOrDefault(FontWeights.Normal);
            FontStyle = wrapper.FontStyle.GetValueOrDefault(FontStyles.Normal);
            TextBrush = AppearanceManager.BodyTextBrush;
            
            var height = GetTextHeight(text);
            height = height + (int) (height*.35);

            var ellipseBorderSize = height/10;
            var label = new Label
                {
                    Content = text,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontFamily = FontFamily,
                    FontSize = FontSize,
                    FontWeight = FontWeight,
                    Foreground = TextBrush,
                    Margin = new Thickness(0),
                    Padding = new Thickness(0)
                };

            

            var ellipse = new Ellipse
                {
                    Stroke = TextBrush,
                    StrokeThickness = ellipseBorderSize

                };

            

            var grid = new Grid
                {
                    Width = height,
                    Height = height
                };

            grid.Children.Add(label);
            grid.Children.Add(ellipse);

            Content = grid;
            InitializeBindings(grid);
            InitializeValues(wrapper);

            return grid as T;
        }

       

        private double GetTextHeight(string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                    return 48;

                var typeface = new Typeface(FontFamily, FontStyle, FontWeight, FontStretches.Normal);
                var formattedText = new FormattedText(
                   value,
                    CultureInfo.InvariantCulture,
                    FlowDirection.LeftToRight,
                    typeface, FontSize, TextBrush);

                return formattedText.Height;
            }
            catch (Exception)
            {
                return 200;
            }
        }


    }
}
