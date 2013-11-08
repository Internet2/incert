using System;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.ControlActions;
using log4net;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Commands
{
    class ButtonResultCommand : AbstractButtonCommand
    {
        private static readonly ILog Log = Logger.Create();
        
        private readonly IHasResult _context;
        private readonly AbstractTaskResult _result;

        public ButtonResultCommand(Object context, AbstractTaskResult result)
            : base(context as IHasControlActions)
        {
            _context = context as IHasResult;
            _result = result;
        }

        public override void Execute(object parameter)
        {
            try
            {
                if (_result == null)
                    return;

                if (_context == null)
                    return;

                _context.Result = _result;

                base.Execute(parameter);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to execute a ButtonResultCommand: {0}", e.Message);
            }
        }

        public override bool CanExecute(object parameter)
        {
            if (_result == null)
                return false;

           

            return true;
        }

    }
}
