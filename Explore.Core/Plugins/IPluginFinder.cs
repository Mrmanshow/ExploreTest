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
    public interface IPluginFinder
    {
        /// <summary>
        /// Check whether the plugin is available in a certain store
        /// </summary>
        /// <param name="pluginDescriptor">Plugin descriptor to check</param>
        /// <param name="storeId">Store identifier to check</param>
        /// <returns>true - available; false - no</returns>
        bool AuthenticateStore(PluginDescriptor pluginDescriptor, int storeId);

        /// <summary>
        /// Check that plugin is authorized for the specified customer
        /// </summary>
        /// <param name="pluginDescriptor">Plugin descriptor to check</param>
        /// <param name="customer">Customer</param>
        /// <returns>True if authorized; otherwise, false</returns>
        bool AuthorizedForUser(PluginDescriptor pluginDescriptor, Customer customer);

        /// <summary>
        /// Gets plugin groups
        /// </summary>
        /// <returns>Plugins groups</returns>
        IEnumerable<string> GetPluginGroups();

        /// <summary>
        /// 获取插件
        /// </summary>
        /// <typeparam name="T">要获取的插件类型。</typeparam>
        /// <param name="loadMode">加载插件模式</param>
        /// <param name="customer">只允许将记录加载到指定的用户；传递空值以忽略ACL权限</param>
        /// <param name="storeId">只允许加载指定存储中的记录；通过0加载所有记录</param>
        /// <param name="group">按插件组筛选；传递空值以加载所有记录</param>
        /// <returns>组件</returns>
        IEnumerable<T> GetPlugins<T>(LoadPluginsMode loadMode = LoadPluginsMode.InstalledOnly,
            Customer customer = null, int storeId = 0, string group = null) where T : class, IPlugin;

        /// <summary>
        /// Get plugin descriptors
        /// </summary>
        /// <param name="loadMode">Load plugins mode</param>
        /// <param name="customer">Load records allowed only to a specified customer; pass null to ignore ACL permissions</param>
        /// <param name="storeId">Load records allowed only in a specified store; pass 0 to load all records</param>
        /// <param name="group">Filter by plugin group; pass null to load all records</param>
        /// <returns>Plugin descriptors</returns>
        IEnumerable<PluginDescriptor> GetPluginDescriptors(LoadPluginsMode loadMode = LoadPluginsMode.InstalledOnly,
            Customer customer = null, int storeId = 0, string group = null);

        /// <summary>
        /// Get plugin descriptors
        /// </summary>
        /// <typeparam name="T">The type of plugin to get.</typeparam>
        /// <param name="loadMode">Load plugins mode</param>
        /// <param name="customer">Load records allowed only to a specified customer; pass null to ignore ACL permissions</param>
        /// <param name="storeId">Load records allowed only in a specified store; pass 0 to load all records</param>
        /// <param name="group">Filter by plugin group; pass null to load all records</param>
        /// <returns>Plugin descriptors</returns>
        IEnumerable<PluginDescriptor> GetPluginDescriptors<T>(LoadPluginsMode loadMode = LoadPluginsMode.InstalledOnly,
            Customer customer = null, int storeId = 0, string group = null) where T : class, IPlugin;

        /// <summary>
        /// Get a plugin descriptor by its system name
        /// </summary>
        /// <param name="systemName">Plugin system name</param>
        /// <param name="loadMode">Load plugins mode</param>
        /// <returns>>Plugin descriptor</returns>
        PluginDescriptor GetPluginDescriptorBySystemName(string systemName, LoadPluginsMode loadMode = LoadPluginsMode.InstalledOnly);

        /// <summary>
        /// Get a plugin descriptor by its system name
        /// </summary>
        /// <typeparam name="T">The type of plugin to get.</typeparam>
        /// <param name="systemName">Plugin system name</param>
        /// <param name="loadMode">Load plugins mode</param>
        /// <returns>>Plugin descriptor</returns>
        PluginDescriptor GetPluginDescriptorBySystemName<T>(string systemName, LoadPluginsMode loadMode = LoadPluginsMode.InstalledOnly)
            where T : class, IPlugin;

        /// <summary>
        /// Reload plugins
        /// </summary>
        void ReloadPlugins();
    }
}
