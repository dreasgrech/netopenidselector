using System;
using System.IO;
using System.Reflection;

namespace OpenIDSelector
{
    internal static class Helpers
    {
        public static string GetWebResourceUrl(string filename)
        {
            return NETOpenIDSelector.ControlPage.ClientScript.GetWebResourceUrl(typeof(NETOpenIDSelector), String.Format("{0}.{1}", ProjectSettings.ProjectName, filename));
        }

        public static string GetWebResourceImageUrl(string image)
        {
            return "images" + GetWebResourceUrl("images." + image);
        }

        internal static string ReadEmbeddedResource(string assemblyName, string resouceName)
        {
            var assembly = Assembly.Load(assemblyName);
            using (var stream = assembly.GetManifestResourceStream(resouceName))
            {
                if (stream == null)
                {
                    return String.Empty;
                }

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
