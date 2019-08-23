using Explore.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Plugins
{
    /// <summary>
    /// 插件查找
    /// </summary>
    public class PluginFinder : IPluginFinder
    {
        #region Fields

        private IList<PluginDescriptor> _plugins;
        private bool _arePluginsLoaded;

        #endregion

        #region Utilities

        /// <summary>
        /// 确保插件已加载
        /// </summary>
        protected virtual void EnsurePluginsAreLoaded()
        {
            if (!_arePluginsLoaded)
            {
                var foundPlugins = PluginManager.ReferencedPlugins.ToList();
                foundPlugins.Sort();
                _plugins = foundPlugins.ToList();

                _arePluginsLoaded = true;
            }
        }

        /// <summary>
        /// 检查插件是否在某个存储中可用
        /// </summary>
        /// <param name="pluginDescriptor">插件描述</param>
        /// <param name="loadMode">加载插件模式</param>
        /// <returns>true-可用；false-否</returns>
        protected virtual bool CheckLoadMode(PluginDescriptor pluginDescriptor, LoadPluginsMode loadMode)
        {
            if (pluginDescriptor == null)
                throw new ArgumentNullException("pluginDescriptor");

            switch (loadMode)
            {
                case LoadPluginsMode.All:
                    //no filering
                    return true;
                case LoadPluginsMode.InstalledOnly:
                    return pluginDescriptor.Installed;
                case LoadPluginsMode.NotInstalledOnly:
                    return !pluginDescriptor.Installed;
                default:
                    throw new Exception("Not supported LoadPluginsMode");
            }
        }

        /// <summary>
        /// Check whether the plugin is in a certain group
        /// </summary>
        /// <param name="pluginDescriptor">Plugin descriptor to check</param>
        /// <param name="group">Group</param>
        /// <returns>true - available; false - no</returns>
        protected virtual bool CheckGroup(PluginDescriptor pluginDescriptor, string group)
        {
            if (pluginDescriptor == null)
                throw new ArgumentNullException("pluginDescriptor");

            if (String.IsNullOrEmpty(group))
                return true;

            return group.Equals(pluginDescriptor.Group, StringComparison.InvariantCultureIgnoreCase);
        }

        #endregion

        #region Methods

        /// <summary>
        /// 检查插件是否在某个存储中可用
        /// </summary>
        /// <param name="pluginDescriptor">要检查的插件描述</param>
        /// <param name="storeId">要检查的存储标识</param>
        /// <returns>true-可用；false-否</returns>
        public virtual bool AuthenticateStore(PluginDescriptor pluginDescriptor, int storeId)
        {
            if (pluginDescriptor == null)
                throw new ArgumentNullException("pluginDescriptor");

            //不需要验证
            if (storeId == 0)
                return true;

            if (!pluginDescriptor.LimitedToStores.Any())
                return true;

            return pluginDescriptor.LimitedToStores.Contains(storeId);
        }

        /// <summary>
        /// 检查插件是否可用于指定的用户
        /// </summary>
        /// <param name="pluginDescriptor">要检查的插件描述</param>
        /// <param name="customer">用户</param>
        /// <returns>授权为true；否则为false</returns>
        public virtual bool AuthorizedForUser(PluginDescriptor pluginDescriptor, Customer customer)
        {
            if (pluginDescriptor == null)
                throw new ArgumentNullException("pluginDescriptor");

            if (customer == null || !pluginDescriptor.LimitedToCustomerRoles.Any())
                return true;

            var customerRoleIds = customer.CustomerRoles.Where(role => role.Active).Select(role => role.Id);

            return pluginDescriptor.LimitedToCustomerRoles.Intersect(customerRoleIds).Any();
        }

        /// <summary>
        /// Gets plugin groups
        /// </summary>
        /// <returns>Plugins groups</returns>
        public virtual IEnumerable<string> GetPluginGroups()
        {
            return GetPluginDescriptors(LoadPluginsMode.All).Select(x => x.Group).Distinct().OrderBy(x => x);
        }

        /// <summary>
        /// 获取插件
        /// </summary>
        /// <typeparam name="T">要获取的插件类型。</typeparam>
        /// <param name="loadMode">加载插件模式</param>
        /// <param name="customer">只允许将记录加载到指定的用户；传递空值以忽略ACL权限</param>
        /// <param name="storeId">只允许加载指定存储中的记录；通过0加载所有记录</param>
        /// <param name="group">按插件组筛选；传递空值以加载所有记录</param>
        /// <returns>组件</returns>
        public virtual IEnumerable<T> GetPlugins<T>(LoadPluginsMode loadMode = LoadPluginsMode.InstalledOnly,
            Customer customer = null, int storeId = 0, string group = null) where T : class, IPlugin
        {
            return GetPluginDescriptors<T>(loadMode, customer, storeId, group).Select(p => p.Instance<T>());
        }

        /// <summary>
        /// 获取插件描述
        /// </summary>
        /// <param name="loadMode">加载插件模式</param>
        /// <param name="customer">只允许将记录加载到指定的用户；传递空值以忽略ACL权限</param>
        /// <param name="storeId">只允许加载指定存储中的记录；通过0加载所有记录</param>
        /// <param name="group">按插件组筛选；传递空值以加载所有记录</param>
        /// <returns>插件描述</returns>
        public virtual IEnumerable<PluginDescriptor> GetPluginDescriptors(LoadPluginsMode loadMode = LoadPluginsMode.InstalledOnly,
            Customer customer = null, int storeId = 0, string group = null)
        {
            //确保插件已加载
            EnsurePluginsAreLoaded();

            return _plugins.Where(p => CheckLoadMode(p, loadMode) && AuthorizedForUser(p, customer) && AuthenticateStore(p, storeId) && CheckGroup(p, group));
        }

        /// <summary>
        /// 获取插件描述符
        /// </summary>
        /// <typeparam name="T">要获取的插件类型。</typeparam>
        /// <param name="loadMode">加载插件模式</param>
        /// <param name="customer">只允许将记录加载到指定的用户；传递空值以忽略ACL权限</param>
        /// <param name="storeId">只允许加载指定存储中的记录；通过0加载所有记录</param>
        /// <param name="group">按插件组筛选；传递空值以加载所有记录</param>
        /// <returns>插件描述</returns>
        public virtual IEnumerable<PluginDescriptor> GetPluginDescriptors<T>(LoadPluginsMode loadMode = LoadPluginsMode.InstalledOnly,
            Customer customer = null, int storeId = 0, string group = null)
            where T : class, IPlugin
        {
            return GetPluginDescriptors(loadMode, customer, storeId, group)
                .Where(p => typeof(T).IsAssignableFrom(p.PluginType));
        }

        /// <summary>
        /// Get a plugin descriptor by its system name
        /// </summary>
        /// <param name="systemName">Plugin system name</param>
        /// <param name="loadMode">Load plugins mode</param>
        /// <returns>>Plugin descriptor</returns>
        public virtual PluginDescriptor GetPluginDescriptorBySystemName(string systemName, LoadPluginsMode loadMode = LoadPluginsMode.InstalledOnly)
        {
            return GetPluginDescriptors(loadMode)
                .SingleOrDefault(p => p.SystemName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Get a plugin descriptor by its system name
        /// </summary>
        /// <typeparam name="T">The type of plugin to get.</typeparam>
        /// <param name="systemName">Plugin system name</param>
        /// <param name="loadMode">Load plugins mode</param>
        /// <returns>>Plugin descriptor</returns>
        public virtual PluginDescriptor GetPluginDescriptorBySystemName<T>(string systemName, LoadPluginsMode loadMode = LoadPluginsMode.InstalledOnly)
            where T : class, IPlugin
        {
            return GetPluginDescriptors<T>(loadMode)
                .SingleOrDefault(p => p.SystemName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Reload plugins
        /// </summary>
        public virtual void ReloadPlugins()
        {
            _arePluginsLoaded = false;
            EnsurePluginsAreLoaded();
        }

        #endregion
    }
}
