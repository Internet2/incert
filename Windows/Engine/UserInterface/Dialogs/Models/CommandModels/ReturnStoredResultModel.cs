using System;
using System.Windows.Input;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Commands;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.CommandModels
{
    class ReturnStoredResultModel:AbstractCommandModel
    {
        private readonly ISettingsManager _manager;
        private readonly AbstractDialogModel _model;
        private readonly string _resultKey;

        private ICommand _command;
        
        public ReturnStoredResultModel(ISettingsManager manager, AbstractDialogModel model, string key) : base(model)
        {
            _manager = manager;
            _model = model;
            _resultKey = key;
        }

        public override ICommand Command
        {
            get
            {
                return _command ?? (_command = new ClearFocusCommand(
                                                   RootDialogModel.DialogInstance,
                                                   param => SetResultForKey(),
                                                   param => ResultExistsForKey()));
            }
        }

        private bool ResultExistsForKey()
        {
            return (_manager.GetTemporaryObject(_resultKey) as AbstractTaskResult) != null;
        }

        private void SetResultForKey()
        {
            _model.Result = _manager.GetTemporaryObject(_resultKey) as AbstractTaskResult ??
               new ExceptionOccurred(
                   new Exception(string.Format("No valid result object exists for the key {0}", _resultKey)));
        }


    }
}
