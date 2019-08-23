using Explore.Core.Domain.Cms;
using Explore.Core.Domain.Customers;
using Explore.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Cms
{
    /// <summary>
    /// Widget service
    /// </summary>
    public partial class WidgetService : IWidgetService
    {
        #region Fields

        private readonly IPluginFinder _pluginFinder;
        private readonly WidgetSettings _widgetSettings;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="pluginFinder">Plugin finder</param>
        /// <param name="widgetSettings">Widget settings</param>
        public WidgetService(IPluginFinder pluginFinder,
            WidgetSettings widgetSettings)
        {
            this._pluginFinder = pluginFinder;
            this._widgetSettings = widgetSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 加载活动工具
        /// </summary>
        /// <param name="customer">只允许将记录加载到指定的用户；传递空值以忽略ACL权限</param>
        /// <param name="storeId">只允许加载指定存储中的记录；通过0加载所有记录</param>
        /// <returns>工具</returns>
        public virtual IList<IWidgetPlugin> LoadActiveWidgets(Customer customer = null, int storeId = 0)
        {
            return LoadAllWidgets(customer, storeId)
                .Where(x => _widgetSettings.ActiveWidgetSystemNames.Contains(x.PluginDescriptor.SystemName, StringComparer.InvariantCultureIgnoreCase)).ToList();
        }

        /// <summary>
        /// 加载活动工具
        /// </summary>
        /// <param name="widgetZone">工具区域</param>
        /// <param name="customer">只允许将记录加载到指定的用户；传递空值以忽略ACL权限</param>
        /// <param name="storeId">只允许加载指定存储中的记录；通过0加载所有记录</param>
        /// <returns>工具</returns>
        public virtual IList<IWidgetPlugin> LoadActiveWidgetsByWidgetZone(string widgetZone, Customer customer = null, int storeId = 0)
        {
            if (String.IsNullOrWhiteSpace(widgetZone))
                return new List<IWidgetPlugin>();

            return LoadActiveWidgets(customer, storeId)
                .Where(x => x.GetWidgetZones().Contains(widgetZone, StringComparer.InvariantCultureIgnoreCase)).ToList();
        }

        /// <summary>
        /// Load widget by system name
        /// </summary>
        /// <param name="systemName">System name</param>
        /// <returns>Found widget</returns>
        public virtual IWidgetPlugin LoadWidgetBySystemName(string systemName)
        {
            var descriptor = _pluginFinder.GetPluginDescriptorBySystemName<IWidgetPlugin>(systemName);
            if (descriptor != null)
                return descriptor.Instance<IWidgetPlugin>();

            return null;
        }

        /// <summary>
        /// 加载所有工具
        /// </summary>
        /// <param name="customer">只允许将记录加载到指定的用户；传递空值以忽略ACL权限</param>
        /// <param name="storeId">只允许加载指定存储中的记录；通过0加载所有记录</param>
        /// <returns>工具</returns>
        public virtual IList<IWidgetPlugin> LoadAllWidgets(Customer customer = null, int storeId = 0)
        {
            return _pluginFinder.GetPlugins<IWidgetPlugin>(customer: customer, storeId: storeId).ToList();
        }

        #endregion
    }
}
