using Explore.Web.Framework.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Explore.Web.Models.Game
{
    public partial class GameDailyStatisticsModel : BaseExploreEntityModel
    {
        public string CreateTime { get; set; }

        public int GameUser { set; get; }

        public int GameCount { set; get; }

        public int GameWin { set; get; }

        public int GameFail { set; get; }

        public string GameType { set; get; }
    }
}