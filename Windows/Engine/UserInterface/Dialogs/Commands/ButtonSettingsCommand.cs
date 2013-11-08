using System;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.LinkWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models;
using log4net;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Commands
{
    class ButtonSettingsCommand : AbstractButtonCommand
    {
        private static readonly ILog Log = Logger.Create();

        private readonly string _key;
        private readonly string _value;
        private readonly ISettingsManager _manager;
        private readonly AbstractModel _model;

        public ButtonSettingsCommand(ISettingsManager manager, AbstractModel model, SettingsLink wrapper)
            : base(model)
        {
            if (wrapper == null)
                throw new Exception("Expected settings link wrapper");

            _manager = manager;
            _model = model;
            _key = wrapper.Target;
            _value = wrapper.Value;
        }

        public override void Execute(object parameter)
        {
            try
            {
                var wrapper = new StringSettingWrapper(_key, _value, _model);
                _manager.BindingProxy.SettingProperty = wrapper;

                base.Execute(parameter);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to execute a ButtonSettingsCommand: {0}", e.Message);
            }
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
