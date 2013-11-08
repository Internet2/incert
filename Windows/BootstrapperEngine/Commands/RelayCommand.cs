using System;
using System.Windows.Input;

namespace Org.InCommon.InCert.BootstrapperEngine.Commands
{
    public class RelayCommand : ICommand
    {
        readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute) : this(execute, null) { }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute; _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter) { _execute(parameter); }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public override string ToString()
        {
            if (_execute != null)
            {
                return _execute.Method.ToString();
            }

            return this.ToString();
        }
    }
}

