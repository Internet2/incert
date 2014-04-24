using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.LinkWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Commands;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Instances.CustomControls;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Managers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Properties;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels
{
    internal class FramedButtonModel : AbstractContentModel
    {
        protected IEngine Engine { get; private set; }

        protected ISettingsManager SettingsManager
        {
            get { return Engine.SettingsManager; }
        }

        private FramedButtonImage _buttonImage;
        private FramedButtonCaptionText _caption;
        private FramedButtonCaptionText _subCaption;

        private double _height;
        private double _width;
        private Brush _background;
        private Brush _glowBrush;
        private TextContentModel _buttonContent;
        private bool _defaultButton;
        private bool _cancelButton;
        private Thickness _borderSize;
        private string _settingKey;
        private string _text;
        private ImageSource _imageSource;
        private ImageSource _mouseOverImageSource;

        public FramedButtonModel(IEngine engine, AbstractModel parentModel)
            : base(parentModel)
        {
            Engine = engine;
        }

        public Brush GlowBrush
        {
            get { return _glowBrush; }
            set
            {
                _glowBrush = value;
                OnPropertyChanged();
            }
        }

        public Thickness BorderSize
        {
            get { return _borderSize; }
            set
            {
                _borderSize = value;
                OnPropertyChanged();
            }
        }

        public double Width
        {
            get { return _width; }
            set
            {
                _width = value;
                OnPropertyChanged();
            }
        }

        public double Height
        {
            get { return _height; }
            set
            {
                _height = value;
                OnPropertyChanged();
            }
        }

        public Brush Background
        {
            get { return _background; }
            set
            {
                _background = value;
                OnPropertyChanged();
            }
        }

        public TextContentModel ButtonContentModel
        {
            get { return _buttonContent; }
            set
            {
                _buttonContent = value;
                OnPropertyChanged();
            }
        }

        public bool IsDefaultButton
        {
            get { return _defaultButton; }
            set
            {
                _defaultButton = value;
                OnPropertyChanged();
            }
        }

        public bool IsCancelButton
        {
            get { return _cancelButton; }
            set
            {
                _cancelButton = value;
                OnPropertyChanged();
            }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }

        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                OnPropertyChanged();
            }
        }

        public ImageSource MouseOverImageSource
        {
            get
            {
                return _mouseOverImageSource ?? _imageSource;
            }
            set
            {
                _mouseOverImageSource = value;
                OnPropertyChanged();
            }
        }

        public FramedButtonImage ButtonImage
        {
            get { return _buttonImage; }
            set { _buttonImage = value; OnPropertyChanged(); }
        }

        public FramedButtonCaptionText Caption
        {
            get { return _caption; }
            set { _caption = value; OnPropertyChanged(); }
        }

        public FramedButtonCaptionText SubCaption
        {
            get { return _subCaption; }
            set { _subCaption = value; OnPropertyChanged(); }
        }

        public override T LoadContent<T>(AbstractContentWrapper wrapper)
        {
            _settingKey = wrapper.SettingKey;
            SettingsManager.RemoveTemporarySettingString(_settingKey);
            var content = new FramedButtonControl
            {
                DataContext = this,
                Foreground = TextBrush,
                Command = new ButtonSettingsCommand(SettingsManager, this, GetLinkWrapper(wrapper as FramedButton))

            };
            InitializeBindings(content);
            SetDefaultValues(wrapper as FramedButton);
            InitializeValues(wrapper);
            Content = content;
            return content as T;
        }

        private SettingsLink GetLinkWrapper(FramedButton wrapper)
        {
            return wrapper == null
                ? null
                : new SettingsLink(Engine) { Target = wrapper.SettingKey, Value = wrapper.Value };
        }

        protected virtual void SetDefaultValues(FramedButton wrapper)
        {
            if (wrapper == null)
                return;

            Width = wrapper.Width;
            Height = wrapper.Height;

            Background = AppearanceManager.GetBrushForColor(wrapper.Background, AppearanceManager.BackgroundBrush);
            Style = GetNamedStyle(wrapper.GlowEffect ? "FramedButton" : "FramedButtonNoGlow");
            GlowBrush = AppearanceManager.GetBrushForColor(wrapper.GlowColor, AppearanceManager.LinkTextBrush);
            ButtonContentModel = new TextContentModel(this);
            ButtonContentModel.LoadContent<DependencyObject>(wrapper);
            ButtonContentModel.Margin = new Thickness(0);
            ButtonContentModel.Padding = new Thickness(0);
            IsDefaultButton = wrapper.IsDefaultButton;
            IsCancelButton = wrapper.IsCancelButton;
            BorderSize = wrapper.BorderSize;
            Text = wrapper.GetText();

            if (wrapper.Image != null)
            {
                ButtonImage = new FramedButtonImage(Engine.SettingsManager, wrapper.Image);
            }

            if (wrapper.Caption != null)
            {
                Caption = new FramedButtonCaptionText(Engine.AppearanceManager, wrapper.Caption);
            }

            if (wrapper.SubCaption != null)
            {
                SubCaption = new FramedButtonCaptionText(Engine.AppearanceManager, wrapper.SubCaption);
            }

        }


        public class FramedButtonCaptionText : PropertyNotifyBase
        {
            private string _value;
            private VerticalAlignment _verticalAlignment;
            private HorizontalAlignment _textAlignment;
            private Thickness _margin;
            private Thickness _padding;
            private double _fontSize;
            private FontFamily _fontFamily;

            public FramedButtonCaptionText(IAppearanceManager appearanceManager, FramedButton.ButtonTextContent wrapper)
            {
                Value = wrapper.Value;
                VerticalAlignment = wrapper.VerticalAlignment;
                HorizontalAlignment = wrapper.HorizontalAlignment;
                Margin = wrapper.Margin;
                Padding = wrapper.Padding;

                if (wrapper.FontSize.HasValue)
                {
                    FontSize = wrapper.FontSize.Value;
                }

                FontFamily = wrapper.FontFamily ?? appearanceManager.DefaultFontFamily;

            }

            public string Value
            {
                get { return _value; }
                set { _value = value; OnPropertyChanged(); }
            }

            public VerticalAlignment VerticalAlignment
            {
                get { return _verticalAlignment; }
                set { _verticalAlignment = value; OnPropertyChanged(); }
            }

            public HorizontalAlignment HorizontalAlignment
            {
                get { return _textAlignment; }
                set { _textAlignment = value; OnPropertyChanged(); }
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

            public double FontSize
            {
                get { return _fontSize; }
                set { _fontSize = value; OnPropertyChanged(); }
            }

            public FontFamily FontFamily
            {
                get { return _fontFamily; }
                set { _fontFamily = value; OnPropertyChanged(); }
            }
        }

        public class FramedButtonImage : PropertyNotifyBase
        {
            private ImageSource _image;
            private ImageSource _mouseOverImage;
            private VerticalAlignment _verticalAlignment;
            private HorizontalAlignment _horizontalAlignment;
            private Thickness _margin;

            public FramedButtonImage(ISettingsManager settingsManager, FramedButton.ButtonImageContent wrapper)
            {
                {
                    ImageSource = settingsManager.GetTemporaryObject(wrapper.Key) as BitmapFrame;
                    MouseOverImageSource = settingsManager.GetTemporaryObject(wrapper.MouseOverKey) as BitmapFrame;

                    Margin = wrapper.Margin;
                    VerticalAlignment = wrapper.VerticalAlignment;
                    HorizontalAlignment = wrapper.Alignment;
                }
            }
            public ImageSource ImageSource
            {
                get { return _image; }
                set { _image = value; OnPropertyChanged(); }
            }

            public ImageSource MouseOverImageSource
            {
                get
                {
                    return _mouseOverImage ?? _image;
                }
                set { _mouseOverImage = value; OnPropertyChanged(); }
            }

            public VerticalAlignment VerticalAlignment
            {
                get { return _verticalAlignment; }
                set { _verticalAlignment = value; OnPropertyChanged(); }
            }

            public HorizontalAlignment HorizontalAlignment
            {
                get { return _horizontalAlignment; }
                set { _horizontalAlignment = value; OnPropertyChanged(); }
            }

            public Thickness Margin
            {
                get { return _margin; }
                set { _margin = value; OnPropertyChanged(); }
            }
        }
    }
}
