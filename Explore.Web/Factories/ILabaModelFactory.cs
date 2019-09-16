using Explore.Core.Domain.Game.Laba;
using Explore.Web.Models.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Explore.Web.Factories
{
    public partial interface ILabaModelFactory
    {
        IList<GameLabaRouteModel> PrepareGameLabaRouteListModel(IList<LabaWinRoute> labaWinRoutes);

        IList<GameLabaRouteModel> PrepareGameLabaRouteListModel(IList<LabaWinRouteNew> labaWinRoutesNew);

        GameLabaRouteModel PrepareGameLabaRouteModel(LabaWinRoute labaWinRoute);

        GameLabaRouteModel PrepareGameLabaRouteModel(LabaWinRouteNew labaWinRoute);

    }
}