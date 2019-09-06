using Explore.Core;
using Explore.Core.Domain.Game;
using Explore.Services;
using Explore.Services.ExportImport;
using Explore.Services.Game;
using Explore.Services.Security;
using Explore.Web.Factories;
using Explore.Web.Framework.Controllers;
using Explore.Web.Framework.Kendoui;
using Explore.Web.Framework.MVC;
using Explore.Web.Models.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Explore.Web.Controllers
{
    public partial class GameStatisticsController : BaseAdminController
    {
        #region Fields

        private readonly IPermissionService _permissionService;
        private readonly IGameStatisticsService _gameStatisticsService;
        private readonly IGameStatisticsModelFactory _gameStatisticsModelFactory;
        private readonly IExportManager _exportManager;

        #endregion

        #region Ctor

        public GameStatisticsController(IGameStatisticsService gameStatisticsService,
                                        IPermissionService permissionService,
                                        IGameStatisticsModelFactory gameStatisticsModelFactory,
                                        IExportManager exportManager)
        {
            this._gameStatisticsService = gameStatisticsService;
            this._permissionService = permissionService;
            this._gameStatisticsModelFactory = gameStatisticsModelFactory;
            this._exportManager = exportManager;
        }

        #endregion


        #region DailyData

        public virtual ActionResult DailyList()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageGameDailyStatistics))
                return AccessDeniedView();

            var model = new GameDailyStatisticsListModel();
            model.BeginDate = DateTime.Now.AddDays(-7);
            model.AvailableGames = GameType.ScratchCard.ToSelectList(false).ToList();

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult GameDailyList(DataSourceRequest command, GameDailyStatisticsListModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageGameDailyStatistics))
                return AccessDeniedView();

            model.EndDate = model.EndDate.AddDays(1);

            var list = _gameStatisticsService.GetGameStatisticsByDate(model.GameType, model.BeginDate, model.EndDate, command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = _gameStatisticsModelFactory.PrepareGameDailyStatisticsListModel(list),
                Total = list.TotalCount
            };

            return Json(gridModel);
        }


        #region Export

        [HttpPost, ActionName("DailyList")]
        [FormValueRequired("exportxml-all")]
        public virtual ActionResult ExportXmlAll(GameDailyStatisticsListModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageGameDailyStatistics))
                return AccessDeniedView();

            model.EndDate = model.EndDate.AddDays(1);

            var list = _gameStatisticsService.GetGameStatisticsByDate(model.GameType, model.BeginDate, model.EndDate);

            try
            {
                var xml = _exportManager.ExportGameDailyStatisticToXml(list);
                return new XmlDownloadResult(xml, "游戏每日流水.xml");
            }
            catch (Exception exc)
            {
                //ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }

        [HttpPost, ActionName("DailyList")]
        [FormValueRequired("exportexcel-all")]
        public virtual ActionResult ExportExcelAll(GameDailyStatisticsListModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageGameDailyStatistics))
                return AccessDeniedView();

            model.EndDate = model.EndDate.AddDays(1);

            var list = _gameStatisticsService.GetGameStatisticsByDate(model.GameType, model.BeginDate, model.EndDate);

            try
            {
                var bytes = _exportManager.ExportGameDailyStatisticToXlsx(list);

                return File(bytes, MimeTypes.TextXlsx, "游戏每日流水.xlsx");
            }
            catch (Exception exc)
            {
                //ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }


        #endregion

        #endregion
    }
}