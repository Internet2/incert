namespace Org.InCommon.InCert.BootstrapperEngine.Extensions
{
    public static class StringExtensions
    {

        // adapted from http://www.dotnetperls.com/uppercase-first-letter
        public static string UppercaseFirst(this string value)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            
            // Return char and concat substring.
            return char.ToUpper(value[0]) + value.Substring(1);
        }

        public static string ToPipeName(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return string.Format("{0}_pipe", value.Replace(" ", ""));
        }
    }
}
