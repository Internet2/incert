using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Converters;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Instances.CustomControls;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels
{
    class OptionParagraphContentModel : AbstractContentModel
    {
        private readonly ISettingsManager _manager;
        private ICommand _command;

        public OptionParagraphContentModel(AbstractModel parentModel, ISettingsManager manager)
            : base(parentModel)
        {
            _manager = manager;
        }

        public ICommand Command
        {
            get { return _command; }
            set { _command = value; OnPropertyChanged(); }
        }

        private string _text;
        private string _group;

        public string Text
        {
            get { return _text; }
            set { _text = value; OnPropertyChanged(); }
        }

        public string GroupName
        {
            get { return _group; }
            set
            {
                _group = value;
                OnPropertyChanged();
            }
        }

        public override T LoadContent<T>(AbstractContentWrapper wrapper)
        {
            var result = new OptionParagraph { DataContext = this };
            Text = wrapper.GetText();

            InitializeBindings(result, wrapper.SettingKey);
            InitializeValues(wrapper);
            Visibility = Visibility.Visible;
            Enabled = true;

            var optionWrapper = wrapper as OptionInputField;
            if (optionWrapper != null)
                GroupName = optionWrapper.Group;

            Content = result;
            return result as T;
        }

        private void InitializeBindings(FrameworkElement target, string settingKey)
        {
            target.SetBinding(ToggleButton.IsCheckedProperty,
               new Binding
               {
                   Converter = new SettingToBoolConverter(this, _manager),
                   ConverterParameter = settingKey,
                   Mode = BindingMode.TwoWay,
                   Path = new PropertyPath("SettingProperty"),
                   Source = _manager.BindingProxy,
                   UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
               }
                   );
        }
    }
}
