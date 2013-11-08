using System.Windows;
using Ninject;

namespace Org.InCommon.InCert.Engine.Extensions
{
    public static class EngineExtensions
    {
        
        public static IKernel CurrentKernel(this Application application)
        {
            var ourApp = application as App;
            return ourApp == null ? null : ourApp.Kernel;
        }


        
    }
}
