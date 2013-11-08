using System;
using System.Windows.Input;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.ControlActions;
using log4net;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Commands
{
    abstract class AbstractButtonCommand:ICommand
    {
        private static readonly ILog Log = Logger.Create();
        private readonly IHasControlActions _actionHost;
        
        protected AbstractButtonCommand(IHasControlActions actionHost)
        {
            _actionHost = actionHost;
        }
        
        public virtual void Execute(object parameter)
        {
            try
            {
                if (_actionHost == null)
                    return;

                _actionHost.DoActions(false);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to execute a button command: {0}", e.Message);
            }
            
        }

        public abstract bool CanExecute(object parameter);

        public event EventHandler CanExecuteChanged;
    }
}
