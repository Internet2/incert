using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Converters;
using log4net;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels
{
   
    
    class ProgressTextContentModel:TextContentModel
    {
        private static readonly ILog Log = Logger.Create();

        private readonly ISettingsManager _manager;

        public ProgressTextContentModel(AbstractModel parentModel, ISettingsManager manager) : base(parentModel)
        {
            _manager = manager;
        }

        public override T LoadContent<T>(AbstractContentWrapper wrapper)
        {
            var instance = base.LoadContent<TextBlock>(wrapper);
            InitializeTextBinding(instance,wrapper);
            return instance as T;
        }

        private void InitializeTextBinding(FrameworkElement instance,AbstractContentWrapper wrapper)
        {
            if (instance == null)
            {
                return;
            }
            
            if (string.IsNullOrWhiteSpace(wrapper.SettingKey))
            {
                Log.Warn("cannot bind ProgressTextContentModel to settings if no key is specified.");
                return;
            }

            var text = wrapper.GetText();
            AppearanceManager.AddTimedMessage(this, wrapper.SettingKey,text);

            instance.SetBinding(TextBlock.TextProperty,
                new Binding
                {
                    Converter = new SettingsConverter(this, _manager),
                    ConverterParameter = wrapper.SettingKey,
                    Path = new PropertyPath("SettingProperty"),
                    Mode = BindingMode.OneWay,
                    Source = _manager.BindingProxy
                });

            _manager.BindingProxy.SettingProperty = new StringSettingWrapper(wrapper.SettingKey, text, this);
        }
    }
}
