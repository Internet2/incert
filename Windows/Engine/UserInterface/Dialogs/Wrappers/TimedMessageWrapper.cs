using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Wrappers
{
    class TimedMessageWrapper
    {
        private bool _active;

        public string BaseText { get; set; }
        public int DotCount { get; set; }
        public AbstractModel Model { get; set; }
        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }
    }
}
