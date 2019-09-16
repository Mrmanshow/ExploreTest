using Explore.Core;
using Explore.Core.Caching;
using Explore.Core.Data;
using Explore.Core.Domain.Game;
using Explore.Core.Domain.Game.Laba;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Game
{
    public partial class GameLabaService : IGameLabaService
    {
        #region Constans

        //public const string GAME_LABA_WIN_ROUTES_KEY = "Explore.labawinroutes.date-{0}-{1}-{2}-{3}-{4}";

        #endregion

        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IRepository<LabaWinRoute> _labaWinRouteRepository;
        private readonly IRepository<LabaWinRouteNew> _labaWinRouteNewRepository;

        #endregion

        #region Ctor

        public GameLabaService(IRepository<LabaWinRoute> labaWinRouteRepository,
            IRepository<LabaWinRouteNew> labaWinRouteNewRepository,
            ICacheManager cacheManager)
        {
            this._cacheManager = cacheManager;
            this._labaWinRouteRepository = labaWinRouteRepository;
            this._labaWinRouteNewRepository = labaWinRouteNewRepository;
        }

        #endregion


        public virtual IPagedList<LabaWinRoute> SearchLabaRoutes(List<int> rsIds = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = from t in _labaWinRouteRepository.Table
                        select t;

            if (rsIds != null)
                query = query.Where(x => rsIds.Contains(x.Status));

            query = query.OrderBy(x => x.Sequence);

            return new PagedList<LabaWinRoute>(query, pageIndex, pageSize);
        }

        public virtual IPagedList<LabaWinRouteNew> SearchLabaRoutesNew(List<int> rsIds = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = from t in _labaWinRouteNewRepository.Table
                        select t;

            if (rsIds != null)
                query = query.Where(x => rsIds.Contains(x.Status));

            query = query.OrderBy(x => x.Sequence);

            return new PagedList<LabaWinRouteNew>(query, pageIndex, pageSize);
        }

        public virtual LabaWinRoute GetLabaWinRouteById(int id)
        {
            if (id == 0)
                return null;

            var query = from t in _labaWinRouteRepository.Table
                        select t;

            var model = query.Where(c => c.Id == id).ToList().First();

            return model;
        }

        public virtual LabaWinRouteNew GetLabaWinRouteNewById(int id)
        {
            if (id == 0)
                return null;

            var query = from t in _labaWinRouteNewRepository.Table
                        select t;

            var model = query.Where(c => c.Id == id).ToList().First();

            return model;
        }


        public virtual void UpdateLabaRoute(LabaWinRoute model)
        {
            if (model == null)
                throw new ArgumentNullException("route");

            _labaWinRouteRepository.Update(model);
        }

        public virtual void UpdateLabaRouteNew(LabaWinRouteNew model)
        {
            if (model == null)
                throw new ArgumentNullException("route");

                _labaWinRouteNewRepository.Update(model);
        }
    }
}
