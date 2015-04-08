using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels.SchemeHandlers;

namespace Org.InCommon.InCert.Engine.Extensions
{
    public static class UriExtensions
    {
        public static bool IsInternalScheme(this Uri uri)
        {
            return EmbeddedResourceSchemeHandlerFactory.IsSchemeUri(uri) ||
                   ArchiveSchemeHandlerFactory.IsSchemeUri(uri);
        }

        public static bool IsSameHost(this Uri uri, Uri compare)
        {
            return uri.Host.Equals(compare.Host);
        }
    }
}
