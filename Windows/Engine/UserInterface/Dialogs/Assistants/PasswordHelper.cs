using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Assistants
{

    /// <summary>
    /// Helper class allowing us to bind to a password box's password property
    /// </summary>
    /// <see cref="http://stackoverflow.com/questions/888466/passwordbox-binding" />
    public class PasswordHelper
    {
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached("Password",
            typeof(string), typeof(PasswordHelper), new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        public static readonly DependencyProperty AttachProperty = DependencyProperty.RegisterAttached("Attach",
            typeof(bool), typeof(PasswordHelper), new PropertyMetadata(false, Attach));

        private static readonly DependencyProperty IsUpdatingProperty = DependencyProperty.RegisterAttached("IsUpdating", 
            typeof(bool), typeof(PasswordHelper));

        public static readonly DependencyProperty PasswordLengthProperty = DependencyProperty.RegisterAttached("PasswordLength",
            typeof(int), typeof(PasswordHelper), new UIPropertyMetadata(0));

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.RegisterAttached("Watermark", 
            typeof (string), typeof (PasswordHelper));

        public static void SetAttach(DependencyObject dp, bool value)
        {
            dp.SetValue(AttachProperty, value);
        }

        public static bool GetAttach(DependencyObject dp)
        {
            return (bool)dp.GetValue(AttachProperty);
        }

        public static SecureString GetPassword(DependencyObject dp)
        {
            return (SecureString)dp.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(PasswordProperty, value);
        }

        private static bool GetIsUpdating(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsUpdatingProperty);
        }

        public static string GetWatermark(DependencyObject instance)
        {
            return (string) instance.GetValue(WatermarkProperty);
        }

        public static void SetWatermark(DependencyObject instance, string value)
        {
            instance.SetValue(WatermarkProperty, value);
        }

        private static void SetIsUpdating(DependencyObject dp, bool value)
        {
            dp.SetValue(IsUpdatingProperty, value);
        }

        private static void OnPasswordPropertyChanged(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox == null)
                return;

            passwordBox.PasswordChanged -= PasswordChanged;

            if (!GetIsUpdating(passwordBox))
            {
                passwordBox.Password = (string)e.NewValue;
            }
            passwordBox.PasswordChanged += PasswordChanged;

            SetPasswordLength(passwordBox, passwordBox.Password.Length);
        }

        private static void Attach(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;

            if (passwordBox == null)
                return;

            if ((bool)e.OldValue)
            {
                passwordBox.PasswordChanged -= PasswordChanged;
            }

            if ((bool)e.NewValue)
            {
                passwordBox.PasswordChanged += PasswordChanged;
            }
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox == null)
                return;

            SetIsUpdating(passwordBox, true);
            SetPassword(passwordBox, passwordBox.Password);
            SetIsUpdating(passwordBox, false);
        }

        public static int GetPasswordLength(DependencyObject obj)
        {
            return (int)obj.GetValue(PasswordLengthProperty);
        }

        public static void SetPasswordLength(DependencyObject obj, int value)
        {
            obj.SetValue(PasswordLengthProperty, value);
        }
    }
}
