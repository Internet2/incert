using System.Windows;
using System.Windows.Media;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.LinkWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Commands;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Instances.CustomControls;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels
{
    internal class FramedButtonModel : AbstractContentModel
    {

        protected IEngine Engine { get; private set; }

        protected ISettingsManager SettingsManager
        {
            get { return Engine.SettingsManager; }
        }

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
        }
    }
}
