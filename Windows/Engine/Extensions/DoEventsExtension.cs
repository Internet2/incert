using System;
using System.Management.Instrumentation;
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
                Thread.Sleep(TimeSpan.FromMilliseconds(milliseconds));
                frame.Continue = false;
            });
            thread.Start();
            Dispatcher.PushFrame(frame);
        }

        public static bool InvokeIfRequired(this object target, Action action)
        {
            var instance = target as ContentControl;
            if (instance == null || !instance.InvokeRequired())
            {
                return false;
            }

            instance.Dispatcher.Invoke(action);
            return true;
        }

        public static bool InvokeRequired(this object target)
        {
            var instance = target as ContentControl;
            return instance.InvokeRequired();
        }
        
        public static bool InvokeRequired(this ContentControl target)
        {
            if (target == null)
            {
                return false;
            }

            return !target.Dispatcher.CheckAccess();
        }

        public static T Invoke<T>(this object target, Func<T> action)
        {
            var instance = target as ContentControl;
            if (instance == null)
            {
                throw new Exception("could not convert object to content control");
            }

            return instance.Dispatcher.Invoke(action);
        }
    }
}
