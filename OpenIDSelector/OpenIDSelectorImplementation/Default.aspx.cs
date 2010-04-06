using System;
using System.Collections.Generic;
using OpenIDSelector;

namespace OpenIDSelectorImplementation
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            NETOpenIDSelector1.AddProviders("provs.xml");
        }

        protected void Selected(object sender, ProviderChosenEventArgs selector)
        {
            var url = selector.SelectorURL;
        }
    }
}
