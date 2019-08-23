using Explore.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace Explore.Services.Cms
{
    /// <summary>
    /// 提供用于创建小工具的接口
    /// </summary>
    public partial interface IWidgetPlugin : IPlugin
    {
        /// <summary>
        /// 获取应该呈现此小工具的小工具区域。
        /// </summary>
        /// <returns>工具区域</returns>
        IList<string> GetWidgetZones();

        /// <summary>
        /// Gets a route for plugin configuration
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues);


        /// <summary>
        /// 获取用于显示小部件的路由
        /// </summary>
        /// <param name="widgetZone">窗口小部件显示区域</param>
        /// <param name="actionName">方法名</param>
        /// <param name="controllerName">控制器名</param>
        /// <param name="routeValues">路由值</param>
        void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName, out RouteValueDictionary routeValues);
    }
}
