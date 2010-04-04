using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace OpenIDSelector.ProviderRetrievers
{
    internal class ProvidersFromXML : IProviderRetriever
    {
        XDocument XMLDocument { get; set; }
        Func<string, string> ImageProcesser { get; set; }

        public ProvidersFromXML(XDocument xmlDocument) : this(xmlDocument, null) { }
        public ProvidersFromXML(XDocument xmlDocument, Func<string, string> imageProcesser)
        {
            XMLDocument = xmlDocument;
            ImageProcesser = imageProcesser;
        }

        public IEnumerable<Provider> Providers()
        {
            var providers = XMLDocument.Descendants("provider");
            foreach (var prov in providers)
            {
                XAttribute nameAttribute = prov.Attribute("name");
                XElement imageElement = prov.Element("image"), urlElement = prov.Element("url");

                string name = nameAttribute != null ? nameAttribute.Value : "",
                    image = imageElement != null ? imageElement.Value : "",
                    url = urlElement != null ? urlElement.Value : "";

                if (ImageProcesser != null)
                {
                    image = ImageProcesser(image);
                }

                yield return new Provider(name, url, image);
            }
        }
    }
}
