using Explore.Core.Domain.Game;
using Explore.Web.Models.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Explore.Web.Factories
{
    public partial interface IGameStatisticsModelFactory
    {
        IList<GameDailyStatisticsModel> PrepareGameDailyStatisticsListModel(IList<GameTurnover> gameTurnovers);
    }
}