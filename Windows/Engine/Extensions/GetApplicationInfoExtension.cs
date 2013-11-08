using System;
using System.Reflection;
using System.Windows;

namespace Org.InCommon.InCert.Engine.Extensions
{
    public static class GetApplicationInfoExtension
    {
        private const string DefaultCompany = "InCommon";
        private const string DefaultProduct = "InCert";

        public static Version GetVersion(this Application application)
        {
            return Assembly.GetExecutingAssembly().GetName().Version;
        }

        public static String GetCompanyName(this Application application)
        {
            var attribute = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyCompanyAttribute>();
            return attribute == null ? DefaultCompany : attribute.Company;
        }

        public static string GetProductName(this Application application)
        {
            var attribute = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyProductAttribute>();
            return attribute == null ? DefaultProduct : attribute.Product;
        }
    }
}
