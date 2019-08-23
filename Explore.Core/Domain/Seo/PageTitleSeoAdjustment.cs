using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Domain.Seo
{
    /// <summary>
    /// Represents a page title SEO adjustment
    /// </summary>
    public enum PageTitleSeoAdjustment
    {
        /// <summary>
        /// Pagename comes after storename
        /// </summary>
        PagenameAfterStorename = 0,
        /// <summary>
        /// Storename comes after pagename
        /// </summary>
        StorenameAfterPagename = 10
    }
}
