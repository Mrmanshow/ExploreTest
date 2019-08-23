using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Domain.Localization
{
    /// <summary>
    /// 表示本地字符串资源。
    /// </summary>
    public partial class LocaleStringResource : BaseEntity
    {
        /// <summary>
        /// Gets or sets the language identifier
        /// </summary>
        public int LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the resource name
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// Gets or sets the resource value
        /// </summary>
        public string ResourceValue { get; set; }
    }
}
