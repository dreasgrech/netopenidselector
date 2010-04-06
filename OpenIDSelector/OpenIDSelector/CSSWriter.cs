using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace OpenIDSelector
{
    internal class CSSWriter
    {
        public CSSWriter(Page page)
        {
            Page = page;
        }

        public Page Page { get; set; }

        /// <summary>
        /// Adds the CSS file to the page
        /// </summary>
        /// <param name="key">The key to be used for the file registration</param>
        /// <param name="cssfile">Path to the CSS file</param>
        /// <param name="isWebResource">Specifies whether this CSS file is an embedded resource in the Assembly</param>
        /// <returns>Returns the key that is associated with the CSS file include</returns>
        public string IncludeFile(string key, string cssfile, bool isWebResource)
        {
            string url = isWebResource ? Helpers.GetWebResourceUrl(cssfile) : cssfile,
                   cssLink = String.Format("<link href='{0}' rel='stylesheet' type='text/css'/>", url);
            Page.ClientScript.RegisterClientScriptBlock(GetType(), key, cssLink, false);
            return key;
        }

        /// <summary>
        /// Adds the CSS file to the page
        /// </summary>
        /// <param name="cssfile">Path to the CSS file</param>
        /// <param name="isWebResource">Specifies whether this CSS file is an embedded resource in the Assembly</param>
        /// <returns>Returns the key that is associated with the CSS file include</returns>
        public string IncludeFile(string cssfile, bool isWebResource)
        {
            var key = Guid.NewGuid().ToString();
            return IncludeFile(key, cssfile, isWebResource);
        }
    }
}
