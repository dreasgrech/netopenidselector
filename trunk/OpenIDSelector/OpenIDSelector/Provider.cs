using System;
using System.Text;
using OpenIDSelector.Exceptions;

namespace OpenIDSelector
{
    public enum ProviderTypes
    {
        Direct,
        Username
    }

    /// <summary>
    /// An OpenID Provider
    /// </summary>
    public class Provider
    {
        /// <summary>
        /// The required name of the Provider
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type of the provider (Direct / Username)
        /// </summary>
        internal ProviderTypes Type { get; set; }

        /// <summary>
        /// An optional image that is displayed on the control
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// <para>The required URL of the Provider</para>
        /// <para>If the URL requires a username, make sure that it includes the {u} specifier</para>
        /// <para>Example: http://yourprovider.com/{u}</para>
        /// <para>If the URL is a direct URL, do not include the {u} specifier</para>
        /// <para>Example: http://yourprovider.com/</para>
        /// </summary>
        /// <exception cref="OpenIDSelector.Exceptions.InvalidURLException">Throws an InvalidURLException if the Provider URL is not a valid URL</exception>
        public string Url { get; set; }

        /// <summary>
        /// Creates a new Provider, given the name, URL and an optional image
        /// </summary>
        /// <param name="name">The required name of the Provider</param>
        /// <param name="image">An optional image of the Provider</param>
        /// <param name="url">The required URL of the provider</param>
        /// <exception cref="OpenIDSelector.Exceptions.InvalidURLException">Throws if the Provider URL is not a valid URL</exception>
        public Provider(string name, string url, string image)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new InvalidURLException("Provider must have a url");
            }

            if (!(url.Contains("http://") || url.Contains("https://")))
            {
                url = String.Format("http://{0}", url);
            }

            Url = url;

            Name = name;

            if (string.IsNullOrEmpty(image))
            {
                image = ProjectSettings.DefaultProviderImage;
            }

            Image = image;
            Type = GetProviderType();
        }

        /// <summary>
        /// Creates a new Provider, given the name and the URL
        /// </summary>
        /// <param name="name">The required name of the Provider</param>
        /// <param name="url">The required URL of the Provider</param>
        public Provider(string name, string url) : this(name, url, "")
        {
        }

        /// <summary>
        /// Builds a li html listing of the Provider
        /// </summary>
        /// <returns>Returns the Provider in li html format</returns>
        internal string GetHTMLListing()
        {
            var sb = new StringBuilder(String.Format("<li class='{0}' title='{1}'>", Type, Name));

            //Provider Image
            sb.Append(String.Format("<img src='{0}' alt='icon'/>", Image));

            //Provider URL
            sb.Append(String.Format("<span style='display:none'>{0}</span>",Url));
                              
            sb.Append("</li>");
            return sb.ToString();
        }

        ProviderTypes GetProviderType()
        {
            return Url.Contains(ProjectSettings.UsernameSpecifier) ? ProviderTypes.Username : ProviderTypes.Direct;
        }

    }
}
