using Explore.Web.Framework.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Explore.Web.Models.Cms
{
    public partial class RenderWidgetModel : BaseExploreModel
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public RouteValueDictionary RouteValues { get; set; }
    }
}