using Explore.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Cms
{
    /// <summary>
    /// Widget service interface
    /// </summary>
    public partial interface IWidgetService
    {
        /// <summary>
        /// Load active widgets
        /// </summary>
        /// <param name="customer">Load records allowed only to a specified customer; pass null to ignore ACL permissions</param>
        /// <param name="storeId">Load records allowed only in a specified store; pass 0 to load all records</param>
        /// <returns>Widgets</returns>
        IList<IWidgetPlugin> LoadActiveWidgets(Customer customer = null, int storeId = 0);

        /// <summary>
        /// 加载活动工具
        /// </summary>
        /// <param name="widgetZone">工具区域</param>
        /// <param name="customer">只允许将记录加载到指定的用户；传递空值以忽略ACL权限</param>
        /// <param name="storeId">只允许加载指定存储中的记录；通过0加载所有记录</param>
        /// <returns>工具</returns>
        IList<IWidgetPlugin> LoadActiveWidgetsByWidgetZone(string widgetZone, Customer customer = null, int storeId = 0);

        /// <summary>
        /// Load widget by system name
        /// </summary>
        /// <param name="systemName">System name</param>
        /// <returns>Found widget</returns>
        IWidgetPlugin LoadWidgetBySystemName(string systemName);

        /// <summary>
        /// Load all widgets
        /// </summary>
        /// <param name="customer">Load records allowed only to a specified customer; pass null to ignore ACL permissions</param>
        /// <param name="storeId">Load records allowed only in a specified store; pass 0 to load all records</param>
        /// <returns>Widgets</returns>
        IList<IWidgetPlugin> LoadAllWidgets(Customer customer = null, int storeId = 0);
    }
}
