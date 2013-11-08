using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Org.InCommon.InCert.Engine.Logging;
using log4net;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Commands
{
    /// <summary>
    /// Executes multiple commands
    /// </summary>
    /// <remarks>adapted from http://www.codeproject.com/Articles/25808/Aggregating-WPF-Commands-with-CommandGroup</remarks>" />
    class CommandGroup : ICommand
    {
        private static readonly ILog Log = Logger.Create();
        private readonly ObservableCollection<ICommand> _commands;


        public CommandGroup()
        {
            _commands = new ObservableCollection<ICommand>();
            _commands.CollectionChanged += OnCommandsCollectionChanged;

        }
        /// <summary>
        /// Returns the collection of child commands. They are executed
        /// in the order that they exist in this collection.
        /// </summary>
        public ObservableCollection<ICommand> Commands
        {
            get
            {
                return _commands;
            }
        }

        void OnCommandsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // We have a new child command so our ability to execute may have changed.
            OnCanExecuteChanged();

            if (e.NewItems != null && 0 < e.NewItems.Count)
            {
                foreach (ICommand cmd in e.NewItems)
                    cmd.CanExecuteChanged += OnChildCommandCanExecuteChanged;
            }

            if (e.OldItems == null || 0 >= e.OldItems.Count)
                return;

            foreach (ICommand cmd in e.OldItems)
                cmd.CanExecuteChanged -= OnChildCommandCanExecuteChanged;
        }

        void OnChildCommandCanExecuteChanged(object sender, EventArgs e)
        {
            // Bubble up the child commands CanExecuteChanged event so that
            // it will be observed by WPF.
            OnCanExecuteChanged();
        }

        protected virtual void OnCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return Commands.All(cmd => cmd.CanExecute(parameter));
        }

        public void Execute(object parameter)
        {
            try
            {
                foreach (var command in Commands)
                    ExecuteMemberCommand(command, parameter);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while trying to execute member commands: {0}", e.Message);
            }
            
        }

        private static void ExecuteMemberCommand(ICommand command, object parameter)
        {
            try
            {
                command.Execute(parameter);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to execute a member command: {0}", e.Message);
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
