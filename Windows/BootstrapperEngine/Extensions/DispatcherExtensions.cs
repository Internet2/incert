using System.Windows.Threading;

namespace Org.InCommon.InCert.BootstrapperEngine.Extensions
{
    public static class DispatcherExtensions
    {
        private delegate void ExitFrameHandler(DispatcherFrame frame);
        
        public static void DoEvents(this Dispatcher dispatcher)
        {
            var frame = new DispatcherFrame();
            dispatcher.BeginInvoke(DispatcherPriority.Background, new ExitFrameHandler(frm => frm.Continue = false), frame);
            Dispatcher.PushFrame(frame);
        }
    }
}
