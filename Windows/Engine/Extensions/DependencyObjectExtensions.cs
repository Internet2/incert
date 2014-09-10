using System.Windows;
using System.Windows.Input;

namespace Org.InCommon.InCert.Engine.Extensions
{
    public static class DependencyObjectExtensions
    {
        public static void ClearFocus(this DependencyObject element)
        {
            if (element == null) return;

            var scope = FocusManager.GetFocusScope(element);
            FocusManager.SetFocusedElement(scope, null);
            Keyboard.ClearFocus();
        }
    }
}
