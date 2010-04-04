using System.Collections.Generic;

namespace OpenIDSelector.ProviderRetrievers
{
    internal interface IProviderRetriever
    {
        IEnumerable<Provider> Providers();
    }
}