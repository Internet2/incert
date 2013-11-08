using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.Mockup.ContentModels
{
    public class MockBaseContentModel
    {
        public VerticalAlignment VerticalAlignment
        {
            get { return VerticalAlignment.Stretch; }
        }

        public HorizontalAlignment HorizontalAlignment
        {
            get { return HorizontalAlignment.Stretch;
                
            }
        }

        public TextWrapping TextWrapping
        {
            get { return TextWrapping.Wrap; }
         }

        public FontFamily FontFamily
        {
            get { return new FontFamily("Verdana"); }
        }

        public virtual Brush TextBrush
        {
            get { return new SolidColorBrush(Colors.Black); }
        }

        public TextAlignment Alignment
        {
            get { return TextAlignment.Left; }
        }

        public FontWeight FontWeight
        {
            get { return FontWeights.Normal; }
        }

        public FontStyle FontStyle
        {
            get { return FontStyles.Normal; }
        }

        public double FontSize
        {
            get { return 12; }
        }

        public Thickness Margin
        {
            get { return new Thickness(0); }
        }

        public Thickness Padding
        {
            get { return new Thickness(0); }
         }

        public Visibility Visibility {get {return Visibility.Visible;}}

        public bool Enabled { get { return true; } }

        public Dock Dock {get {return Dock.Top;}}

        public Brush Background { get {return new SolidColorBrush(Colors.CadetBlue);}}
    }
}
