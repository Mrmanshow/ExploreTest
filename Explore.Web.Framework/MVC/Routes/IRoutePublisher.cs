using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace Explore.Web.Framework.MVC.Routes
{
    /// <summary>
    /// 路由发布
    /// </summary>
    public interface IRoutePublisher
    {
        /// <summary>
        /// 注册路由
        /// </summary>
        /// <param name="routes">Routes</param>
        void RegisterRoutes(RouteCollection routes);
    }
}
