using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using OpenIDSelector.ProviderRetrievers;

namespace OpenIDSelector
{
    /// <summary>
    /// OpenID Provider Selector
    /// </summary>
    [ToolboxData("<{0}:NETOpenIDSelector runat=server LoadjQuery=true></{0}:NETOpenIDSelector>"),
    ToolboxBitmap(typeof(NETOpenIDSelector), "OpenIDSelector.NETOpenIDSelector.bmp")]
    public class NETOpenIDSelector : WebControl, INamingContainer
    {
        private const string FULLFIELDNAME = "fullfield";

        private Button submit;
        private TextBox fullUrlField;
        private ProviderList provList;
        private CSSWriter css;

        /// <summary>
        /// Occurs when the user chooses an OpenID Provider
        /// </summary>
        [Bindable(true)]
        public event EventHandler<ProviderChosenEventArgs> SelectorChosen;

        /// <summary>
        /// Gets or sets a value indicating whether jQuery should be included automatically when the server control loads
        /// </summary>
        [Bindable(true), DefaultValue(false)]
        public bool LoadjQuery
        {
            get
            {
                return ViewState["LoadjQuery"] != null ? (bool)ViewState["LoadjQuery"] : false;
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

        internal static Page ControlPage { get; set; }

        /// <summary>
        /// Adds a new provider to the end of the list of existing Providers
        /// </summary>
        /// <param name="provider">The new Provider</param>
        /// <returns>1 if the Provider was successfully added to list of existing Providers; 0 otherwise</returns>
        public int AddProvider(Provider provider)
        {
            return provList.Add(provider);
        }

        /// <summary>
        /// Adds a set of new Providers to the end of the list of existing Providers
        /// </summary>
        /// <param name="providers">The set of new Providers</param>
        /// <returns>The number of Providers that where added to the list of existing Providers</returns>
        public int AddProviders(IEnumerable<Provider> providers)
        {
            return provList.Add(providers);
        }

        /// <summary>
        /// Adds a set of new Providers retrieved from an xml file to the end of the list of existing Providers
        /// </summary>
        /// <param name="xmlPath">The path for the XML file</param>
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
            css = new CSSWriter(Page);
            submit = new Button { ID = "submittoserver", CssClass = "submitopenid" };

            fullUrlField = new TextBox { ID = FULLFIELDNAME, CssClass = "fullfield" };
            provList = new ProviderList();

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
            css.IncludeFile("css.openid.css", true);
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

        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.Write(GetBeginTag());
            var listing = provList.GetBulletedList();
            writer.Write(listing);

            fullUrlField.RenderControl(writer);

            RenderProviderContainer(writer);

            submit.RenderControl(writer);

            writer.Write("</div>");
        }

        protected override void CreateChildControls()
        {
            Controls.Add(submit);
        }

        /// <summary>
        /// Renders the Login Container from the html file
        /// </summary>
        /// <param name="output">Output to write to</param>
        private static void RenderProviderContainer(TextWriter output)
        {
            var loginHTML = Helpers.ReadEmbeddedResource("OpenIDSelector", "OpenIDSelector.html.LoginBox.htm");
            output.Write(loginHTML);
        }

        private string GetBeginTag()
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
                sb.Append(String.Format("width: {0}px{1}", Width.Value, !Height.IsEmpty ? "," : String.Empty));
            }

            if (!Height.IsEmpty)
            {
                sb.Append(String.Format("height: {0}px", Height.Value));
            }

            sb.Append("'>");
            return sb.ToString();
        }
    }
}
