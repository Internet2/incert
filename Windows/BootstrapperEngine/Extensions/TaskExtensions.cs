using System.Threading.Tasks;
using System.Windows.Threading;

namespace Org.InCommon.InCert.BootstrapperEngine.Extensions
{
    public static class TaskExtensions
    {
        // adapted from http://blogs.planetsoftware.com.au/paul/archive/2010/12/05/waiting-for-a-task-donrsquot-block-the-main-ui-thread.aspx
        public static void WaitWithPumping(this Task task)
        {
            if (task == null)
                return;

            var nestedFrame = new DispatcherFrame();
            task.ContinueWith(_ => nestedFrame.Continue = false);
            Dispatcher.PushFrame(nestedFrame);
            task.Wait();
        }


    }
}
