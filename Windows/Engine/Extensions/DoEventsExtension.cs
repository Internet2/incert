using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
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

        public static void DoEvents(this Dispatcher dispatcher)
        {
            var frame = new DispatcherFrame();
            dispatcher.BeginInvoke(DispatcherPriority.Background, new ExitFrameHandler(frm => frm.Continue = false), frame);
            Dispatcher.PushFrame(frame);
        }

        public static void DoEvents(this Application application, double milliseconds)
        {
            var frame = new DispatcherFrame();
            var thread = new Thread(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(milliseconds));
                frame.Continue = false;
            });
            thread.Start();
            Dispatcher.PushFrame(frame);
        }



        public static bool InvokeIfRequired(this object target, Action action)
        {
            var instance = target as ContentControl;
            if (instance == null)
            {
                return false;
            }

            if (instance.Dispatcher.CheckAccess())
            {
                return false;
            }

            instance.Dispatcher.Invoke(action);
            return true;
        }
    }
}
