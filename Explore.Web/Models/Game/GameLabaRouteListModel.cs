using Explore.Web.Framework;
using Explore.Web.Framework.MVC;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Explore.Web.Models.Game
{
    public partial class GameLabaRouteListModel: BaseExploreModel
    {
        public GameLabaRouteListModel()
        {
            LabaRouteStatusIds = new List<int>();
            AvailableRouteStatues = new List<SelectListItem>();
            AvasliableLabaTypes = new List<SelectListItem>();
        }


        [ExploreResourceDisplayName("Admin.Game.Laba.RouteStatusIds")]
        [UIHint("MultiSelect")]
        public List<int> LabaRouteStatusIds { set; get; }

        [ExploreResourceDisplayName("Admin.Game.Laba.LabaTypeId")]
        public int LabaTypeId { set; get; }


        public IList<SelectListItem> AvailableRouteStatues { set; get; }

        public IList<SelectListItem> AvasliableLabaTypes { set; get; }
    }
}