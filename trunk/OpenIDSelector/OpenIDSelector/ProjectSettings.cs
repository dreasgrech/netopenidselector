
namespace OpenIDSelector
{
    internal static class ProjectSettings
    {
        private const string assemblyName = "OpenIDSelector";
        private const string cacheKey = "providers";
        private const string usernameSpecifier = "{u}";
        private const string jqueryPath = "http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js";

        public static string ProjectName
        {
            get
            {
                return assemblyName;
            }
        }

        public static string CacheKey
        {
            get
            {
                return cacheKey;
            }
        }

        public static string UsernameSpecifier
        {
            get
            {
                return usernameSpecifier;
            }
        }

        public static string jQueryPath
        {
            get
            {
                return jqueryPath;
            }
        }

        public static string DefaultProviderImage
        {
            get
            {
                return Helpers.GetWebResourceImageUrl("default.png");
            }
        }
    }
}
