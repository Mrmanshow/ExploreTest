using Explore.Core.Domain.Game;
using Explore.Core.Domain.Game.Laba;
using Explore.Services;
using Explore.Services.Game;
using Explore.Services.Localization;
using Explore.Services.Security;
using Explore.Web.Factories;
using Explore.Web.Framework.Controllers;
using Explore.Web.Framework.Kendoui;
using Explore.Web.Models.Game;
using Explore.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Explore.Web.Controllers
{

    public partial class LabaController : BaseAdminController
    {
        #region Fields

        private readonly IPermissionService _permissionService;
        private readonly ILocalizationService _localizationService;
        private readonly IGameLabaService _gameLabaServivce;
        private readonly ILabaModelFactory _labaModelFactor;

        #endregion

        #region Ctor

        public LabaController(IPermissionService permissionService,
            ILocalizationService localizationService,
            IGameLabaService gameLabaServivce,
            ILabaModelFactory labaModelFactor)
        {
            this._permissionService = permissionService;
            this._localizationService = localizationService;
            this._gameLabaServivce = gameLabaServivce;
            this._labaModelFactor = labaModelFactor;
        }

        #endregion

        public virtual ActionResult RouteList(int labaType = 0)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageGameLabaRoute))
                return AccessDeniedView();

            var model = new GameLabaRouteListModel();
            model.AvailableRouteStatues = LabaRouteStatus.Display.ToSelectList(false).ToList();
            model.AvailableRouteStatues.Insert(0, new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = "-1", Selected = true });

            model.AvasliableLabaTypes = LabaType.Laba.ToSelectList(true).ToList();
            model.LabaTypeId = labaType;
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult RouteList(DataSourceRequest command, GameLabaRouteListModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageGameLabaRoute))
                return AccessDeniedView();

            var routeStatusIds = !model.LabaRouteStatusIds.Contains(-1) ? model.LabaRouteStatusIds : null;

            switch (model.LabaTypeId)
            {
                case 0:
                    var list = _gameLabaServivce.SearchLabaRoutes(routeStatusIds, command.Page - 1, command.PageSize);
                    var gridModel = new DataSourceResult
                    {
                        Data = _labaModelFactor.PrepareGameLabaRouteListModel(list),
                        Total = list.TotalCount
                    };
                    return Json(gridModel);
                default:
                    var listNew = _gameLabaServivce.SearchLabaRoutesNew(routeStatusIds, command.Page - 1, command.PageSize);
                    var gridModelNew = new DataSourceResult
                    {
                        Data = _labaModelFactor.PrepareGameLabaRouteListModel(listNew),
                        Total = listNew.TotalCount
                    };

                    return Json(gridModelNew);
            }

        }


        public virtual ActionResult EditRoute(int id, int labaType)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageGameLabaRoute))
                return AccessDeniedView();

            switch (labaType)
            {
                case 0:
                    
                    var route = _gameLabaServivce.GetLabaWinRouteById(id);

                    if (route == null)
                    {
                        return RedirectToAction("RouteList");
                    }

                    var model = _labaModelFactor.PrepareGameLabaRouteModel(route);

                    return View(model);
                default:
                    
                    var routeNew = _gameLabaServivce.GetLabaWinRouteNewById(id);

                    if (routeNew == null)
                    {
                        return RedirectToAction("RouteList");
                    }

                    var modelNew = _labaModelFactor.PrepareGameLabaRouteModel(routeNew);

                    return View(modelNew);
            }

        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual ActionResult EditRoute(GameLabaRouteModel model, int id, bool continueEditing, int labaType = 0)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageGameLabaRoute))
                return AccessDeniedView();


            if (ModelState.IsValid)
            {
   
                switch (labaType)
                {
                    case 0:
                        var route = _gameLabaServivce.GetLabaWinRouteById(id);
                        route = model.ToEntity(route);
                        _gameLabaServivce.UpdateLabaRoute(route);
                        break;
                    default:
                         var routeNew = _gameLabaServivce.GetLabaWinRouteNewById(id);
                         routeNew = model.ToEntity(routeNew);
                         _gameLabaServivce.UpdateLabaRouteNew(routeNew);
                        break;
                }

                if (continueEditing)
                {
                    return RedirectToAction("EditRoute", new { id = id, labaType = labaType });
                }
                return RedirectToAction("RouteList", new { labaType = labaType });
            }

            return View(model);
        }
    }
}