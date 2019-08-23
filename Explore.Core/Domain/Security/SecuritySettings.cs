using Explore.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Domain.Security
{
    public class SecuritySettings : ISettings
    {
        /// <summary>
        /// 获取或设置一个值，该值指示是否强制所有页使用SSL（无论指定的[ExploreHttpsRequirementAttribute]属性如何）
        /// </summary>
        public bool ForceSslForAllPages { get; set; }

        /// <summary>
        /// Gets or sets an encryption key
        /// </summary>
        public string EncryptionKey { get; set; }

        /// <summary>
        /// Gets or sets a list of admin area allowed IP addresses
        /// </summary>
        public List<string> AdminAreaAllowedIpAddresses { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether XSRF protection for admin area should be enabled
        /// </summary>
        public bool EnableXsrfProtectionForAdminArea { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether XSRF protection for public store should be enabled
        /// </summary>
        public bool EnableXsrfProtectionForPublicStore { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether honeypot is enabled on the registration page
        /// </summary>
        public bool HoneypotEnabled { get; set; }
        /// <summary>
        /// Gets or sets a honeypot input name
        /// </summary>
        public string HoneypotInputName { get; set; }
    }
}
