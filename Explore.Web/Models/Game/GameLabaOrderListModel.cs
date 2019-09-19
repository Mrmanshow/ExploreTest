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
    public partial class GameLabaOrderListModel : BaseExploreEntityModel
    {
        public GameLabaOrderListModel()
        {
            AvasliableLabaTypes = new List<SelectListItem>();
        }

        [ExploreResourceDisplayName("Admin.Game.Laba.BeginDate")]
        [UIHint("DateNullable")]
        public DateTime? BeginDate { set; get; }

        [ExploreResourceDisplayName("Admin.Game.Laba.EndDate")]
        [UIHint("DateNullable")]
        public DateTime? EndDate { set; get; }

        [ExploreResourceDisplayName("Admin.Game.Laba.LabaTypeId")]
        public int LabaTypeId { set; get; }

        public IList<SelectListItem> AvasliableLabaTypes { set; get; }
    }
}