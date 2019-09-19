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
    public partial class GameDailyStatisticsListModel : BaseExploreEntityModel
    {
        public GameDailyStatisticsListModel()
        {
            AvailableGames = new List<SelectListItem>();
        }

        [ExploreResourceDisplayName("Admin.Game.GameDailyStatistics.BeginDate")]
        [UIHint("Date")]
        public DateTime BeginDate { set; get; }

        [ExploreResourceDisplayName("Admin.Game.GameDailyStatistics.EndDate")]
        [UIHint("Date")]
        public DateTime EndDate { set; get; }

        [ExploreResourceDisplayName("Admin.Game.GameDailyStatistics.GameType")]
        public int GameType { set; get; }

        public IList<SelectListItem> AvailableGames { set; get; }

    }
}