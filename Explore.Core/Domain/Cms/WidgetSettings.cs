using Explore.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Domain.Cms
{
    public class WidgetSettings : ISettings
    {
        public WidgetSettings()
        {
            ActiveWidgetSystemNames = new List<string>();
        }

        /// <summary>
        /// 获取或设置活动小部件的系统名称。
        /// </summary>
        public List<string> ActiveWidgetSystemNames { get; set; }
    }
}
