using System;
using System.Text;

namespace OpenIDSelector
{
    public enum ProviderType
    {
        Direct,
        UserName
    }

    /// <summary>
    /// An OpenID Provider
    /// </summary>
    public class Provider
    {
        private string url;

        private string image;

        /// <summary>
        /// Creates a new Provider, given the name, URL and an optional image
        /// </summary>
        /// <param name="name">The required name of the Provider</param>
        /// <param name="url">The required URL of the provider</param>
        /// <param name="image">An optional image of the Provider</param>
        /// <exception cref="OpenIDSelector.InvalidProviderURLException">Throws if the Provider URL is not a valid URL</exception>
        public Provider(string name, string url, string image)
        {
            Url = url;
            Name = name;
            Image = image;
        }

        /// <summary>
        /// Creates a new Provider, given the name and the URL
        /// </summary>
        /// <param name="name">The required name of the Provider</param>
        /// <param name="url">The required URL of the Provider</param>
        /// <exception cref="OpenIDSelector.InvalidProviderURLException">Throws if the Provider URL is not a valid URL</exception>
        public Provider(string name, string url)
            : this(name, url, String.Empty)
        {
        }

        /// <summary>
        /// Gets or sets the required name of the Provider
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets an optional image that is displayed on the control
        /// </summary>
        public string Image
        {
            get
            {
                return image;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    value = ProjectSettings.DefaultProviderImage;
                }

                image = value;
            }
        }

        /// <summary>
        /// Gets or sets the required URL of the Provider
        /// <para>If the URL requires a username, make sure that it includes the {u} specifier</para>
        /// <para>Example: http://yourprovider.com/{u}</para>
        /// <para>If the URL is a direct URL, do not include the {u} specifier</para>
        /// <para>Example: http://yourprovider.com/</para>
        /// </summary>
        /// <exception cref="OpenIDSelector.Exceptions.InvalidProviderURLException">Throws an InvalidURLException if the Provider URL is not a valid URL</exception>
        public string Url
        {
            get
            {
                return url;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new InvalidProviderURLException("Provider must have a url");
                }

                if (!(value.Contains("http://") || value.Contains("https://")))
                {
                    value = String.Format("http://{0}", value);
                }

                url = value;

                // Because the Type of the Provider depends on the url,
                // the Type is updated every time the url is changed. 
                Type = GetProviderType();
            }
        }

        /// <summary>
        /// Gets or sets the type of the provider (Direct / Username)
        /// </summary>
        internal ProviderType Type { get; set; }

        public override string ToString()
        {
            return String.Format("{0}, {1}, {2}", Name, Url, Image);
        }

        /// <summary>
        /// Builds a li html listing of the Provider
        /// </summary>
        /// <returns>Returns the Provider in li html format</returns>
        internal string GetHTMLListing()
        {
            var sb = new StringBuilder(String.Format("<li class='{0}' title='{1}'>", Type, Name));

            // Provider Image
            sb.Append(String.Format("<img src='{0}' alt='icon'/>", Image));

            // Provider URL
            sb.Append(String.Format("<span style='display:none'>{0}</span>", Url));

            sb.Append("</li>");
            return sb.ToString();
        }

        private ProviderType GetProviderType()
        {
            return Url.Contains(ProjectSettings.UsernameSpecifier) ? ProviderType.UserName : ProviderType.Direct;
        }
    }
}
