using Explore.Core.Domain.Game.Laba;
using Explore.Web.Extensions;
using Explore.Web.Models.Game;
using Explore.Services.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Explore.Core;
using Explore.Services;

namespace Explore.Web.Factories
{
    public partial class LabaModelFactory : ILabaModelFactory
    {
        #region Field

        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public LabaModelFactory(ILocalizationService localizationService,
            IWorkContext workContext)
        {
            this._localizationService = localizationService;
            this._workContext = workContext;
        }

        #endregion



        #region Route

        public virtual IList<GameLabaRouteModel> PrepareGameLabaRouteListModel(IList<LabaWinRoute> labaWinRoutes)
        {
            var list = labaWinRoutes.Select(x =>
            {
                var model = x.ToModel();

                model.Status = x.LabaRouteStatus.GetLocalizedEnum(_localizationService, _workContext);

                return model;
            });

            return list.ToList();
        }

        public virtual IList<GameLabaRouteModel> PrepareGameLabaRouteListModel(IList<LabaWinRouteNew> labaWinRoutes)
        {
            var list = labaWinRoutes.Select(x =>
            {
                var model = x.ToModel();
                model.Status = x.LabaRouteStatus.GetLocalizedEnum(_localizationService, _workContext);

                return model;
            });

            return list.ToList();
        }

        public virtual GameLabaRouteModel PrepareGameLabaRouteModel(LabaWinRoute labaWinRoute)
        {
            var model = labaWinRoute.ToModel();

            LabaRouteStatus type = (LabaRouteStatus)Enum.Parse(typeof(LabaRouteStatus), model.Status.ToString());
            model.AvaliableLabaRouteStatues = type.ToSelectList(true).ToList();

            return model;
        }

        public virtual GameLabaRouteModel PrepareGameLabaRouteModel(LabaWinRouteNew labaWinRouteNew)
        {
            var model = labaWinRouteNew.ToModel();

            LabaRouteStatus type = (LabaRouteStatus)Enum.Parse(typeof(LabaRouteStatus), model.Status.ToString());
            model.AvaliableLabaRouteStatues = type.ToSelectList(true).ToList();

            return model;
        }

        #endregion

        #region Order
        
        public virtual IList<GameLabaOrderModel> PrepareGameLabaOrderListModel(IList<LabaOrder> labaOrders)
        {
            var list = labaOrders.Select(x =>
            {
                var model = x.ToModel();

                return model;
            });

            return list.ToList();
        }

        public virtual IList<GameLabaOrderModel> PrepareGameLabaOrderNewListModel(IList<LabaOrderNew> labaOrdersNew)
        {
            var list = labaOrdersNew.Select(x =>
            {
                var model = x.ToModel();

                return model;
            });

            return list.ToList();
        }
        #endregion
    }
}