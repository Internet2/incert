using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels
{
    public class AbstractContentModel : AbstractModel
    {
        private TextAlignment _alignment;
        private FontWeight _fontWeight;
        private FontStyle _fontStyle;
        private double _fontSize;
        private FontFamily _fontFamily;
        private Thickness _margin;
        private Thickness _padding;
        private Brush _textBrush;
        private TextWrapping _textWrapping;
        private VerticalAlignment _verticalAlignment;
        private Style _style;
        

        public AbstractContentModel(AbstractModel parentModel)
            : base(parentModel)
        {
            FontFamily = AppearanceManager.DefaultFontFamily;
            FontSize = 11;
            
        }

        public Style Style
        {
            get { return _style; }
            set { _style = value; OnPropertyChanged(); }
        }

        public VerticalAlignment VerticalAlignment
        {
            get { return _verticalAlignment; }
            set { _verticalAlignment = value; OnPropertyChanged(); }
        }

        public TextWrapping TextWrapping
        {
            get { return _textWrapping; }
            set { _textWrapping = value; OnPropertyChanged(); }
        }

        public FontFamily FontFamily
        {
            get { return _fontFamily; }
            set { _fontFamily = value; OnPropertyChanged(); }
        }

        public virtual Brush TextBrush
        {
            get { return _textBrush; }
            set { _textBrush = value; OnPropertyChanged(); }
        }

        public TextAlignment Alignment
        {
            get { return _alignment; }
            set { _alignment = value; OnPropertyChanged(); }
        }

        public FontWeight FontWeight
        {
            get { return _fontWeight; }
            set { _fontWeight = value; OnPropertyChanged(); }
        }

        public FontStyle FontStyle
        {
            get { return _fontStyle; }
            set { _fontStyle = value; OnPropertyChanged(); }
        }

        public double FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; OnPropertyChanged(); }
        }

        public Thickness Margin
        {
            get { return _margin; }
            set { _margin = value; OnPropertyChanged(); }
        }

        public Thickness Padding
        {
            get { return _padding; }
            set { _padding = value; OnPropertyChanged(); }
        }

        protected override void InitializeBindings(DependencyObject target)
        {
            base.InitializeBindings(target);

            InitializeFontFamilyBinding(target);
            InitializeFontSizeBinding(target);
            InitializeFontStyleBinding(target);
            InitializeFontWeightBinding(target);
            InitializeMarginBinding(target);
            InitializePaddingBinding(target);
            InitializeTextBrushBinding(target);
            InitializeAlignmentBinding(target);
            InitializeVerticalAlignmentBinding(target);
            InitializeTextWrappingBinding(target);
            InitializeStyleBinding(target);
        }

        protected virtual void InitializeValues(AbstractContentWrapper wrapper)
        {
            InitializeValues();
            VerticalAlignment = wrapper.VerticalAlignment;
            FontFamily = wrapper.FontFamily ?? AppearanceManager.DefaultFontFamily;
            Alignment = wrapper.Alignment.GetValueOrDefault(TextAlignment.Left);
            TextBrush = AppearanceManager.GetBrushForColor(wrapper.Color, AppearanceManager.BodyTextBrush);
            Margin = wrapper.Margin.GetValueOrDefault(AppearanceManager.DefaultMargin);
            Padding = wrapper.Padding.GetValueOrDefault(AppearanceManager.DefaultPadding);
            FontSize = wrapper.FontSize.GetValueOrDefault(12);
            FontStyle = wrapper.FontStyle.GetValueOrDefault(FontStyles.Normal);
            FontWeight = wrapper.FontWeight.GetValueOrDefault(FontWeights.Normal);
            Dock = wrapper.Dock;
            Enabled = wrapper.Enabled;
        }

        protected virtual void InitializeStyleBinding(DependencyObject instance)
        {
            BindingOperations.SetBinding(instance, FrameworkElement.StyleProperty, GetOneWayBinding(this, "Style"));
        }

        protected virtual void InitializeTextWrappingBinding(DependencyObject instance)
        {
            TextWrapping = TextWrapping.Wrap;
            BindingOperations.SetBinding(instance, TextBlock.TextWrappingProperty, GetOneWayBinding(this, "TextWrapping"));
        }

        protected virtual void InitializeVerticalAlignmentBinding(DependencyObject instance)
        {
           
            BindingOperations.SetBinding(instance, FrameworkElement.VerticalAlignmentProperty, GetOneWayBinding(this, "VerticalAlignment"));
        }

        protected virtual void InitializeTextBrushBinding(DependencyObject instance)
        {
            
            BindingOperations.SetBinding(instance, TextBlock.ForegroundProperty, GetOneWayBinding(this, "TextBrush"));
        }

        protected virtual void InitializeFontFamilyBinding(DependencyObject instance)
        {
            BindingOperations.SetBinding(instance, TextBlock.FontFamilyProperty, GetOneWayBinding(this, "FontFamily"));
        }

        protected virtual void InitializeMarginBinding(DependencyObject instance)
        {
            BindingOperations.SetBinding(instance, FrameworkElement.MarginProperty, GetOneWayBinding(this, "Margin"));
           
        }

        protected virtual void InitializePaddingBinding(DependencyObject instance)
        {
            var binding = GetOneWayBinding(this, "Padding");
            BindingOperations.SetBinding(instance, TextBlock.PaddingProperty, binding);
            BindingOperations.SetBinding(instance, Control.PaddingProperty, binding);
            
        }

        protected virtual void InitializeFontSizeBinding(DependencyObject instance)
        {
            BindingOperations.SetBinding(instance, TextBlock.FontSizeProperty, GetOneWayBinding(this, "FontSize"));
        }

        protected virtual void InitializeFontStyleBinding(DependencyObject instance)
        {
            BindingOperations.SetBinding(instance, TextBlock.FontStyleProperty, GetOneWayBinding(this, "FontStyle"));
        }

        protected virtual void InitializeFontWeightBinding(DependencyObject instance)
        {
            BindingOperations.SetBinding(instance, TextBlock.FontWeightProperty, GetOneWayBinding(this, "FontWeight"));
        }

        protected virtual void InitializeAlignmentBinding(DependencyObject instance)
        {
            BindingOperations.SetBinding(instance, TextBlock.TextAlignmentProperty, GetOneWayBinding(this, "Alignment"));
        }
    }
}
