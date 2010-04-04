using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using OpenIDSelector.ProviderRetrievers;
using System.Drawing;
namespace OpenIDSelector
{
    /// <summary>
    /// OpenID Provider Selector
    /// </summary>
    [ToolboxData("<{0}:NETOpenIDSelector runat=server LoadjQuery=true></{0}:NETOpenIDSelector>"), 
    ToolboxBitmap(typeof(NETOpenIDSelector),"OpenIDSelector.NETOpenIDSelector.bmp") 
    ]
    public class NETOpenIDSelector : WebControl, INamingContainer
    {
        /// <summary>
        /// Occurs when the user chooses an OpenID Provider
        /// </summary>
        [Bindable(true)]
        public event EventHandler<ProviderChosenEventArgs> SelectorChosen;

        private Button submit;
        private TextBox fullUrlField;
        private ProviderList ProvList;
        internal static Page ControlPage;

        private const string FULLFIELDNAME = "fullfield";

        /// <summary>
        /// Indicates whether jQuery should be included automatically when the server control loads
        /// </summary>
        [Bindable(true), DefaultValue(false)]
        public bool LoadjQuery
        {
            get
            {
                return ViewState["LoadjQuery"] != null ? (bool) ViewState["LoadjQuery"] : false;
            }
            set
            {
                ViewState["LoadjQuery"] = value;
            }
        }

        /// <summary>
        /// Gets the Username Format Specifier
        /// </summary>
        public string UsernameFormatSpecifier
        {
            get
            {
                return ProjectSettings.UsernameSpecifier;
            }
        }


        public override ControlCollection Controls
        {
            get
            {
                EnsureChildControls();
                return base.Controls;
            }
        }

        /// <summary>
        /// Adds a new provider to the end of the list of existing Providers
        /// </summary>
        /// <param name="provider">The new Provider</param>
        /// <returns>1 if the Provider was successfully added to list of existing Providers; 0 otherwise</returns>
        public int AddProvider(Provider provider)
        {
            return ProvList.Add(provider);
        }

        /// <summary>
        /// Adds a set of new Providers to the end of the list of existing Providers
        /// </summary>
        /// <param name="providers">The set of new Providers</param>
        /// <returns>The number of Providers that where added to the list of existing Providers</returns>
        public int AddProviders(IEnumerable<Provider> providers)
        {
            return ProvList.Add(providers);
        }

        /// <summary>
        /// Adds a set of new Providers retrieved from an xml file to the end of the list of existing Providers
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <exception cref="FileNotFoundException">Throws if the xml file does not exist</exception>
        /// <returns>The number of Providers that where added to the list of existing Providers</returns>
        public int AddProviders(string xmlPath)
        {
            xmlPath = Page.MapPath(xmlPath);
            var xmlDocument = XDocument.Load(xmlPath);
            var provs = new ProvidersFromXML(xmlDocument);
            return AddProviders(provs.Providers());
        }

        protected override void OnInit(EventArgs e)
        {
            ControlPage = Page;
            submit = new Button { ID = "submittoserver", CssClass = "submitopenid" };
            
            fullUrlField = new TextBox { ID = FULLFIELDNAME, CssClass = "fullfield" };
            ProvList = new ProviderList();

            submit.Click += (s, ev) =>
                                {
                                    EnsureChildControls();
                                    var fieldUrl = Page.Request.Form[FULLFIELDNAME];
                                    if (SelectorChosen != null)
                                    {
                                        SelectorChosen(this, new ProviderChosenEventArgs(fieldUrl));
                                    }
                                };
            
            base.OnInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (LoadjQuery)
            {
                Page.ClientScript.RegisterClientScriptInclude("jquery", ProjectSettings.jQueryPath);
            }
            Page.ClientScript.RegisterClientScriptInclude("openid", Helpers.GetWebResourceUrl("js.openid.js"));
            AddCSSFile("css/openid.css");
        }

        /// <summary>
        /// Render is overridden to supress the default span tag
        /// http://blog.dreasgrech.com/2010/04/removing-default-span-from-custom-web.html
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            RenderContents(writer);
        } 

        string GetBeginTag()
        {
            var sb = new StringBuilder("<div class='openid ");
            if (!String.IsNullOrEmpty(CssClass))
            {
                sb.Append(CssClass);
            }
            sb.Append("' ");
            sb.Append("style='");

            if (!Width.IsEmpty)
            {
                sb.Append(String.Format("width: {0}px{1}", Width.Value, !Height.IsEmpty ? "," : ""));
            }

            if (!Height.IsEmpty)
            {
                sb.Append(String.Format("height: {0}px", Height.Value));
            }
            sb.Append("'>");
            return sb.ToString();
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            output.Write(GetBeginTag());
            var listing = ProvList.GetBulletedList();
            output.Write(listing);

            fullUrlField.RenderControl(output);

            RenderProviderContainer(output);

            submit.RenderControl(output);

            output.Write("</div>");

        }

        protected override void CreateChildControls()
        {
            Controls.Add(submit);
        }

        /// <summary>
        /// Adds the CSS file to the page
        /// </summary>
        /// <param name="cssfile">Path to the CSS file</param>
        void AddCSSFile(string cssfile)
        {
            var cssLink = String.Format("<link href='{0}' rel='stylesheet' type='text/css'/>", Helpers.GetWebResourceUrl(cssfile.Replace('/', '.')));
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "openidcss", cssLink, false);
        }

        /// <summary>
        /// Renders the Login Container from the html file
        /// </summary>
        /// <param name="output"></param>
        static void RenderProviderContainer(TextWriter output)
        {
            var loginHTML = Helpers.ReadEmbeddedResource("OpenIDSelector", "OpenIDSelector.html.LoginBox.htm");
            output.Write(loginHTML);
        }
    }
}
