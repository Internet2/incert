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
    class CheckBoxContentModel : AbstractContentModel
    {
        private readonly ISettingsManager _manager;
        private ICommand _command;
        private string _text;

        public CheckBoxContentModel(AbstractModel parentModel, ISettingsManager manager)
            : base(parentModel)
        {
            _manager = manager;
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; OnPropertyChanged(); }
        }

        public ICommand Command
        {
            get { return _command;}
            set { _command = value; OnPropertyChanged(); }
        }

        public override T LoadContent<T>(AbstractContentWrapper wrapper)
        {

            //Command = new RelayCommand(param => RootDialogModel.ResetFocus());
            Text = wrapper.GetText();
            var content = new CheckboxParagraph {DataContext = this};
            
            InitializeBindings(content, wrapper.SettingKey);
            InitializeValues(wrapper);
            
            Content = content;
            

            return content as T;
        }

        private void InitializeBindings(CheckboxParagraph target, string settingKey)
        {
            InitializeVisiblityBinding(target);
            InitializeEnabledBinding(target);
            InitializeDockBinding(target);

            target.CheckBox.SetBinding(ToggleButton.IsCheckedProperty,
               new Binding
               {
                   Converter = new SettingToBoolConverter(this, _manager),
                   ConverterParameter = settingKey,
                   Mode = BindingMode.TwoWay,
                   Path = new PropertyPath("SettingProperty"),
                   Source = _manager.BindingProxy,
                   UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,

               });
        }

    
      
    }
}
