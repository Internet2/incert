using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
        private readonly Dictionary<string, string> _buttonStyleDictionary = new Dictionary<string, string>
        {
            {"glow", "FramedButton"},
            {"default", "FramedButtonNoGlow"},
            {"invert", "InvertingFramedButton"},
            {"","FramedButtonNoGlow"}
        };

        private readonly Dictionary<string, string> _captionStyleDictionary = new Dictionary<string, string>
        {
            {"glow", ""},
            {"default", ""},
            {"invert", "InvertingTextBlock"},
            {"",""}
        };


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
        private CornerRadius _cornerRadius;
        private Brush _borderBrush;

        private ImageSource _imageSource;
        private ImageSource _mouseOverImageSource;
        private HorizontalAlignment _horizontalAlignment;

        public FramedButtonModel(IEngine engine, AbstractModel parentModel)
            : base(parentModel)
        {
            Engine = engine;
        }

        public HorizontalAlignment HorizontalAlignment
        {
            get { return _horizontalAlignment; }
            set { _horizontalAlignment = value; OnPropertyChanged(); }
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

        public CornerRadius CornerRadius
        {
            get { return _cornerRadius; }
            set { _cornerRadius = value; OnPropertyChanged(); }
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

        public Brush BorderBrush
        {
            get { return _borderBrush; }
            set { _borderBrush = value; OnPropertyChanged(); }
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

            HorizontalAlignment = wrapper.HorizontalAlignment;
            Background =  AppearanceManager.GetBrushForColor(wrapper.Background, AppearanceManager.BackgroundBrush);
            Style = GetNamedStyle(_buttonStyleDictionary[wrapper.EffectName]);
            GlowBrush = AppearanceManager.GetBrushForColor(wrapper.EffectArgument, AppearanceManager.LinkTextBrush);
            BorderBrush = AppearanceManager.GetBrushForColor(wrapper.BorderColor, AppearanceManager.BodyTextBrush);
            ButtonContentModel = new TextContentModel(this);
            ButtonContentModel.LoadContent<DependencyObject>(wrapper);
            ButtonContentModel.Margin = new Thickness(0);
            ButtonContentModel.Padding = new Thickness(0);
            IsDefaultButton = wrapper.IsDefaultButton;
            IsCancelButton = wrapper.IsCancelButton;
            BorderSize = wrapper.BorderSize;
            CornerRadius = wrapper.CornerRadius;
            Text = wrapper.GetText();

            if (wrapper.Image != null)
            {
                ButtonImage = new FramedButtonImage(Engine.SettingsManager, wrapper.Image);
            }

            if (wrapper.Caption != null)
            {
                Caption = new FramedButtonCaptionText(Engine.AppearanceManager, wrapper.Caption)
                {
                    Style = GetNamedStyle(_captionStyleDictionary[wrapper.EffectName])
                };
            }
            else
            {
                Caption = new FramedButtonCaptionText
                {
                    Visibility = Visibility.Collapsed,
                    FontSize = 10,
                    FontFamily = AppearanceManager.DefaultFontFamily
                }; 
            }

            if (wrapper.SubCaption != null)
            {
                SubCaption = new FramedButtonCaptionText(Engine.AppearanceManager, wrapper.SubCaption)
                {
                    Style = GetNamedStyle(_captionStyleDictionary[wrapper.EffectName])
                };
            }
            else
            {
                SubCaption = new FramedButtonCaptionText
                {
                    Visibility = Visibility.Collapsed, 
                    FontSize = 10, 
                    FontFamily = AppearanceManager.DefaultFontFamily
                }; 
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
            private FontWeight _fontWeight;
            private Style _style;
            private Visibility _visibility;

            public FramedButtonCaptionText(){}

            public FramedButtonCaptionText(IAppearanceManager appearanceManager, FramedButton.ButtonTextContent wrapper)
            {
                Value = wrapper.Value;
                VerticalAlignment = wrapper.VerticalAlignment;
                HorizontalAlignment = wrapper.HorizontalAlignment;
                Margin = wrapper.Margin;
                Padding = wrapper.Padding;
                FontWeight = wrapper.FontWeight;
                FontSize = wrapper.FontSize;

                FontFamily = wrapper.FontFamily ?? appearanceManager.DefaultFontFamily;
                Visibility = Visibility.Visible;
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

            public FontWeight FontWeight
            {
                get { return _fontWeight; }
                set { _fontWeight = value; OnPropertyChanged(); }
            }

            public Style Style
            {
                get { return _style; }
                set { _style = value; OnPropertyChanged(); }
            }

            public Visibility Visibility
            {
                get { return _visibility; }
                set { _visibility = value; OnPropertyChanged(); }
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
