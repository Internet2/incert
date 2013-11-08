using System.ComponentModel;
using Org.InCommon.InCert.BootstrapperEngine.Properties;

namespace Org.InCommon.InCert.BootstrapperEngine.PropertyNotifiers
{
    /// <summary>
    /// It provides support for property change notifications.
    /// </summary>
    public abstract class PropertyNotifyBase : INotifyPropertyChanged
    {
        protected delegate void OnUiThreadDelegate();

        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler == null)
                return;
            
            handler(this, new PropertyChangedEventArgs(propertyName));
        }




    }
}