using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace Explore.Web.Framework.Menu
{
    public class SiteMapNode
    {
        /// <summary>
        /// 初始化类的新实例 <see cref="SiteMapNode"/>.
        /// </summary>
        public SiteMapNode()
        {
            RouteValues = new RouteValueDictionary();
            ChildNodes = new List<SiteMapNode>();
        }

        /// <summary>
        /// 获取或设置系统名称
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// 获取或设置标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 获取或设置操作的控制器
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// 获取或设置方法名
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// 获取或设置路由参数
        /// </summary>
        public RouteValueDictionary RouteValues { get; set; }

        /// <summary>
        /// 获取或设置URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 获取或设置子节点
        /// </summary>
        public IList<SiteMapNode> ChildNodes { get; set; }

        /// <summary>
        /// 获取或设置图标class (Font Awesome: http://fontawesome.io/)
        /// </summary>
        public string IconClass { get; set; }

        /// <summary>
        /// 获取或设置该项可见
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否在新选项卡（窗口）中打开URL
        /// </summary>
        public bool OpenUrlInNewTab { get; set; }
    }
}
