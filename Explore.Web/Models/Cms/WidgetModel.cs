using Explore.Web.Framework;
using Explore.Web.Framework.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Explore.Web.Models.Cms
{
    public partial class WidgetModel : BaseExploreModel
    {
        [ExploreResourceDisplayName("Admin.ContentManagement.Widgets.Fields.FriendlyName")]
        [AllowHtml]
        public string FriendlyName { get; set; }

        [ExploreResourceDisplayName("Admin.ContentManagement.Widgets.Fields.SystemName")]
        [AllowHtml]
        public string SystemName { get; set; }

        [ExploreResourceDisplayName("Admin.ContentManagement.Widgets.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [ExploreResourceDisplayName("Admin.ContentManagement.Widgets.Fields.IsActive")]
        public bool IsActive { get; set; }


        public string ConfigurationActionName { get; set; }
        public string ConfigurationControllerName { get; set; }
        public RouteValueDictionary ConfigurationRouteValues { get; set; }
    }
}