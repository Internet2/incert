using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.LinkWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Commands;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels
{
    class FramedButtonModel:AbstractContentModel
    {

        protected IEngine Engine { get; private set; }
        protected ISettingsManager SettingsManager { get { return Engine.SettingsManager; } }
        private double _height;
        private double _width;
        private Brush _background;
        private Brush _glowBrush;
        private TextContentModel _buttonContent;
        private bool _defaultButton;
        private bool _cancelButton;
        private Thickness _borderSize;
        private string _settingKey;

        public FramedButtonModel(IEngine engine, AbstractModel parentModel) : base(parentModel)
        {
            Engine = engine;
        }

        public Brush GlowBrush
        {
            get { return _glowBrush; }
            set { _glowBrush = value; OnPropertyChanged(); }
        }

        public Thickness BorderSize
        {
            get { return _borderSize; }
            set { _borderSize = value; OnPropertyChanged();}
        }
        
        public double Width
        {
            get { return _width; }
            set { _width = value; OnPropertyChanged(); }
        }

        public double Height
        {
            get { return _height; }
            set { _height = value; OnPropertyChanged(); }
        }

        public Brush Background
        {
            get { return _background; }
            set { _background = value; OnPropertyChanged(); }
        }

        public TextContentModel ButtonContentModel
        {
            get { return _buttonContent; }
            set { _buttonContent = value; OnPropertyChanged(); }
        }

        public bool IsDefaultButton
        {
            get { return _defaultButton; }
            set { _defaultButton = value; OnPropertyChanged(); }
        }

        public bool IsCancelButton
        {
            get { return _cancelButton; }
            set { _cancelButton = value; OnPropertyChanged(); }
        }

        public override T LoadContent<T>(AbstractContentWrapper wrapper)
        {
            _settingKey = wrapper.SettingKey;
            SettingsManager.RemoveTemporarySettingString(_settingKey);
            var content = new Button
                {
                    Foreground = TextBrush,
                    Command = new ButtonSettingsCommand(SettingsManager, this, GetLinkWrapper(wrapper as FramedButton))
                };

            SetDefaultValues(wrapper as FramedButton);
            InitializeBindings(content);
            InitializeValues(wrapper);
            Content = content;
            return content as T;
        }

        private SettingsLink GetLinkWrapper(FramedButton wrapper)
        {
            return wrapper == null ? null : new SettingsLink (Engine) {Target = wrapper.SettingKey, Value = wrapper.Value};
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
        }


        protected override void InitializeBindings(DependencyObject target)
        {
            base.InitializeBindings(target);
            SetWidthBinding(target);
            SetHeightBinding(target);
            SetBackgroundBinding(target);
            SetContentBinding(target);
            SetGlowColorBinding(target);
            SetIsCancelButtonBinding(target);
            SetIsDefaultButtonBinding(target);
            SetBorderSizeBinding(target);
        }
        
        private void SetWidthBinding(DependencyObject target)
        {
            BindingOperations.SetBinding(target, FrameworkElement.WidthProperty, GetOneWayBinding(this, "Width"));
        }

        private void SetHeightBinding(DependencyObject target)
        {
            BindingOperations.SetBinding(target, FrameworkElement.HeightProperty, GetOneWayBinding(this, "Height"));
        }

        private void SetBackgroundBinding(DependencyObject target)
        {
            BindingOperations.SetBinding(target, Control.BackgroundProperty, GetOneWayBinding(this, "Background"));
        }

        private void SetContentBinding(DependencyObject target)
        {
            BindingOperations.SetBinding(target, ContentControl.ContentProperty, GetOneWayBinding(ButtonContentModel, "Content"));
        }

        private void SetGlowColorBinding(DependencyObject target)
        {
            BindingOperations.SetBinding(target, Control.BorderBrushProperty, GetOneWayBinding(this, "GlowBrush"));
        }

        private void SetIsDefaultButtonBinding(DependencyObject target)
        {
            BindingOperations.SetBinding(target, Button.IsDefaultProperty, GetOneWayBinding(this, "IsDefaultButton"));
        }

        private void SetIsCancelButtonBinding(DependencyObject target)
        {
            BindingOperations.SetBinding(target, Button.IsCancelProperty, GetOneWayBinding(this, "IsCancelButton"));
        }

        private void SetBorderSizeBinding(DependencyObject target)
        {
            BindingOperations.SetBinding(target, Control.BorderThicknessProperty, GetOneWayBinding(this, "BorderSize"));
        }
    }
}
