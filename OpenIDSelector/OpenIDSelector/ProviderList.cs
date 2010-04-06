using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Xml.Linq;
using OpenIDSelector.ProviderRetrievers;

namespace OpenIDSelector
{
    internal class ProviderList
    {
        public ProviderList()
        {
            Context = HttpContext.Current;

            Retrievers = new List<IProviderRetriever>();
            var providersXMLText = Helpers.ReadEmbeddedResource(ProjectSettings.ProjectName, String.Format("{0}.{1}", ProjectSettings.ProjectName, "xml.providers.xml"));

            if (!String.IsNullOrEmpty(providersXMLText))
            {
                var providersXMLDocument = XDocument.Parse(providersXMLText);

                Retrievers.Add(new ProvidersFromXML(providersXMLDocument, Helpers.GetWebResourceImageUrl));
            }

            if (Providers != null)
            {
                return;
            }

            Providers = new List<Provider>();
            PopulateInitialProviders();
        }

        /// <summary>
        /// Gets the current set of Providers
        /// </summary>
        public IEnumerable<Provider> Providers
        {
            get
            {
                /*
                 * Begin Hack:  The following [if (Context == null)] is used because 
                 *              of the Visual Studio Designer, to supress the "Cache is not available" error
                 *              that is displayed in the Design View
                 *              - Dreas Grech.
                 * */
                if (Context == null)
                {
                    return new List<Provider>();
                }

                /*
                 * End Hack.
                 * */

                if (Context.Cache[ProjectSettings.CacheKey] == null)
                {
                    return null;
                }

                return (IEnumerable<Provider>)Context.Cache[ProjectSettings.CacheKey];
            }

            private set
            {
                Context.Cache[ProjectSettings.CacheKey] = value;
            }
        }

        private HttpContext Context { get; set; }

        private List<Provider> _Providers
        {
            get
            {
                return (List<Provider>) Providers;
            }
        }

        private List<IProviderRetriever> Retrievers { get; set; }

        /// <summary>
        /// Adds the new provider to the list of existing providers, if it doesn't already exist in the Page's Cache
        /// </summary>
        /// <param name="provider">The new Provider</param>
        public int Add(Provider provider)
        {
            if (!_Providers.Exists(p => p.Name == provider.Name))
            {
                _Providers.Add(provider);
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// Adds a set of new providers to the list of existing providers
        /// </summary>
        /// <param name="providers">The set of new Providers</param>
        public int Add(IEnumerable<Provider> providers)
        {
            var total = 0;
            foreach (var provider in providers)
            {
                total += Add(provider);
            }

            return total;
        }

        /// <summary>
        /// <para>Builds an html ul list from the list of Providers</para>
        /// </summary>
        /// <returns>Returns the Providers in ul html format</returns>
        public string GetBulletedList()
        {
            var list = new StringBuilder();
            list.Append("<ul class='providers'>");

            foreach (var provider in Providers)
            {
                list.Append(provider.GetHTMLListing());
            }

            list.Append("</ul>");
            return list.ToString();
        }

        private void PopulateInitialProviders()
        {
            foreach (var retriever in Retrievers)
            {
                _Providers.AddRange(retriever.Providers());
            }
        }
    }
}
