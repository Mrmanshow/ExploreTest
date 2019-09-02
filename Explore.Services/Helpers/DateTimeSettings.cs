using Explore.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Helpers
{
    public class DateTimeSettings : ISettings
    {
        /// <summary>
        /// 获取或设置默认存储时区标识符
        /// </summary>
        public string DefaultStoreTimeZoneId { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否允许客户选择其时区
        /// </summary>
        public bool AllowCustomersToSetTimeZone { get; set; }
    }
}
