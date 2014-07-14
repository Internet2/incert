using System.Windows.Forms;
using System.Windows.Media;

namespace Org.InCommon.InCert.Engine.Extensions
{
    public static class BrushExtensions
    {

        public static Brush MakeTransparent(this Brush value, byte alpha)
        {
            var solidBrush = value as SolidColorBrush;
            
            return solidBrush == null ? new SolidColorBrush(Colors.White) : new SolidColorBrush(
                Color.FromArgb(
                    alpha,
                    solidBrush.Color.R,
                    solidBrush.Color.G,
                    solidBrush.Color.B));
        }
        
        public static Brush FadeBrush(this Brush value)
        {
            var solidBrush = value as SolidColorBrush;

            return solidBrush == null ? new SolidColorBrush(Colors.White) : new SolidColorBrush(solidBrush.Color.FadeColor());
        }

        public static Color FadeColor(this Color value)
        {
            var color = System.Drawing.Color.FromArgb(value.A, value.R, value.G, value.B);
            color = ControlPaint.LightLight(color);
            return new Color
            {
                A = color.A,
                R = color.R,
                G = color.G,
                B = color.B
            };
        }
    }

    
}
