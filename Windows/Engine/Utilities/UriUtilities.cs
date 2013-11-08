using System;
using System.Linq;

namespace Org.InCommon.InCert.Engine.Utilities
{
    public static class UriUtilities
    {
        public static Uri ResolveUri(string value, Uri defaultUri)
        {
            if (string.IsNullOrWhiteSpace(value))
                return defaultUri;
            
            var escapedUrl = Uri.EscapeUriString(value);
            if (string.IsNullOrWhiteSpace(escapedUrl))
                return defaultUri;

            if (!Uri.IsWellFormedUriString(escapedUrl, UriKind.RelativeOrAbsolute))
                return defaultUri;

            var uri = new Uri(escapedUrl, UriKind.RelativeOrAbsolute);
            return uri.IsAbsoluteUri ? uri : new Uri(defaultUri, uri);
        }

        public static Uri ResolveUri(string value, string defaultUrl)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            var escapedValue = Uri.EscapeUriString(defaultUrl);
            if (string.IsNullOrWhiteSpace(escapedValue))
                return null;

            if (!Uri.IsWellFormedUriString(escapedValue, UriKind.RelativeOrAbsolute))
                return null;

            var defaultUri = new Uri(escapedValue, UriKind.RelativeOrAbsolute);
            return ResolveUri(value, defaultUri);
        }

        public static Uri ResolveAbsoluteUri(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            var escapedValue = Uri.EscapeUriString(value);
            return !Uri.IsWellFormedUriString(escapedValue, UriKind.Absolute) ? 
                null : new Uri(escapedValue, UriKind.Absolute);
        }

        public static string GetTargetFromUri(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "";
            
            var escapedValue = Uri.EscapeUriString(value);
            if (string.IsNullOrWhiteSpace(escapedValue))
                return "";

            return !Uri.IsWellFormedUriString(escapedValue, UriKind.RelativeOrAbsolute) 
                ? "" : GetTargetFromUri(new Uri(value, UriKind.RelativeOrAbsolute));
        }

        public static string GetTargetFromUri(Uri uri)
        {
            var parts = uri.OriginalString.Split('/');
            var target = parts[parts.Length - 1];
            if (string.IsNullOrWhiteSpace(target))
                return "";
            
            var queryMarker = target.IndexOf('?');
            if (queryMarker >= 0)
                target = target.Substring(0, queryMarker);
            
            return Uri.UnescapeDataString(target);
        }

        public static string GetUriWithoutTarget(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return "";
            
            var escapedValue = Uri.EscapeUriString(url);
            if (string.IsNullOrWhiteSpace(escapedValue))
                return "";

            return !Uri.IsWellFormedUriString(escapedValue, UriKind.Absolute)
                ? "" : GetUriWithoutTarget(new Uri(url, UriKind.Absolute));
        }

        public static string GetUriWithoutTarget(Uri uri)
        {
            if (!uri.IsAbsoluteUri)
                return "";
            
            var lastSegment = uri.Segments.Last();
            return uri.AbsoluteUri.Replace(lastSegment,"");
        }

        public static bool IsValidAbsoluteUri(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;
            
            var escapedValue = Uri.EscapeUriString(value);
            return Uri.IsWellFormedUriString(escapedValue, UriKind.Absolute);
        }
    }
}
