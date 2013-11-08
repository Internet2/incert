using System.Windows;
using System.Windows.Threading;

namespace Org.InCommon.InCert.Engine.Extensions
{
    public static class DoEventsExtension
    {
        private delegate void ExitFrameHandler(DispatcherFrame frame);

        public static void DoEvents(this Application application)
        {
            var frame = new DispatcherFrame();
            application.Dispatcher.BeginInvoke(DispatcherPriority.Background, new ExitFrameHandler(frm => frm.Continue = false), frame);
            Dispatcher.PushFrame(frame);
        }
    }
}
