using System.Threading.Tasks;
using System.Windows;

namespace Org.InCommon.InCert.Engine.Extensions
{
    public static class TaskExtensions
    {
        public static void WaitUntilExited(this Task instance)
        {
            if (instance == null)
                return;
            
            while (!instance.IsCompleted && !instance.IsFaulted)
            {
                instance.Wait(5);
                Application.Current.DoEvents(250);
            }
        }

        public static string GetIssueMessage(this Task instance, string defaultMessage)
        {
            if (instance == null)
                return defaultMessage;

            return instance.Exception == null 
                ? defaultMessage 
                : instance.Exception.GetBaseException().Message;
        }
    }
}
