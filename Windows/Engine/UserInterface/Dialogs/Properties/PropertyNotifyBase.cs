using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using Org.InCommon.InCert.Engine.Properties;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Properties
{
    /// <summary>
    /// It provides support for property change notifications.
    /// </summary>
    public abstract class PropertyNotifyBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void FlagPropertyAsChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected Binding GetOneWayBinding(object source, string path)
        {
            return new Binding
                {
                    Source = source,
                    Path = new PropertyPath(path),
                    Mode = BindingMode.OneWay
                };
        }

        protected Binding GetTwoWayBinding(object source, string path)
        {
            return new Binding
            {
                Source = source,
                Path = new PropertyPath(path),
                Mode = BindingMode.TwoWay
            };
        }

       
    }
}