using System;
using System.Reflection;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using Org.InCommon.InCert.BootstrapperEngine.Enumerations;

namespace Org.InCommon.InCert.BootstrapperEngine.Extensions
{
    public static class LaunchActionExtensions
    {

        public static InstallActions ToInstallAction(this LaunchAction action)
        {
            var type = typeof(InstallActions);
            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var value = field.GetValue(null) as InstallActions;
                if (value == null)
                    continue;

                if (value.Value != action)
                    continue;

                return value;
            }

            throw new InvalidCastException(string.Format("Cannot convert {0} to valid wrapper class", action));
        }
    }
}
