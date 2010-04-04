using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OpenIDSelector;
  using System.Reflection;

namespace OpenIDSelectorImplementation
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var newprov = new Provider("Dreas", "www.facebook.com/{u}");

            NETOpenIDSelector1.AddProvider(newprov);

            //ProviderSelector1.Width = 10;
            NETOpenIDSelector1.CssClass = "dreasclass";
            //NETOpenIDSelector1.AddProviders("provs.xml");
        }

        protected void Selected(object sender, ProviderChosenEventArgs selector)
        {
            var x = selector.SelectorURL;
        }
    }
}
