using System;
using System.Windows;
using System.Windows.Input;
using Org.InCommon.InCert.Engine.Logging;
using log4net;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Commands
{
    class ClearFocusCommand:CommandGroup
    {
        private static readonly ILog Log = Logger.Create();

        public ClearFocusCommand(DependencyObject element, Action<object> execute, Predicate<object> canExecute) 
        {
            Commands.Add(new RelayCommand(param => ResetFocus(element)));
            Commands.Add(new RelayCommand(execute, canExecute));
        }

        public ClearFocusCommand(DependencyObject element, Action<object> execute)
            : this(element, execute, null)
        {
        }

        private static void ResetFocus(DependencyObject element)
        {
            try
            {
                if (element == null)
                    return;

                var scope = FocusManager.GetFocusScope(element);
                if (scope == null)
                    return;

                FocusManager.SetFocusedElement(scope, null);
                Keyboard.ClearFocus();
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to reset the dialog's focus: {0}", e.Message);
            }
        }
    }
}
