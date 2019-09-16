using Explore.Core;
using Explore.Core.Domain.Game;
using Explore.Core.Domain.Game.Laba;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Game
{
    public partial interface IGameLabaService
    {
        IPagedList<LabaWinRoute> SearchLabaRoutes(List<int> rsIds = null, int pageIndex = 0, int pageSize = int.MaxValue);

        LabaWinRoute GetLabaWinRouteById(int id);

        void UpdateLabaRoute(LabaWinRoute mode);

        IPagedList<LabaWinRouteNew> SearchLabaRoutesNew(List<int> rsIds = null, int pageIndex = 0, int pageSize = int.MaxValue);

        LabaWinRouteNew GetLabaWinRouteNewById(int id);

        void UpdateLabaRouteNew(LabaWinRouteNew mode);

    }
}
