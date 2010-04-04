using System;
using System.IO;
using System.Reflection;

namespace OpenIDSelector
{
    static internal class Helpers
    {
        internal static string ReadEmbeddedResource(string assemblyName, string resouceName)
        {
            var assembly = Assembly.Load(assemblyName);
            using (var stream = assembly.GetManifestResourceStream(resouceName))
            {
                if (stream == null)
                {
                    return "";
                }
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static string GetWebResourceUrl(string filename)
        {
            return NETOpenIDSelector.ControlPage.ClientScript.GetWebResourceUrl(typeof (NETOpenIDSelector),
                                                                               String.Format("{0}.{1}",
                                                                                             ProjectSettings.ProjectName,
                                                                                             filename));
        }

        public static string GetWebResourceImageUrl(string image)
        {
            return "images" + GetWebResourceUrl("images." + image);
        }

    }
}
