
using Explore.Core.Domain.Content;
using Explore.Services.Security;
using Explore.Web.Models.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Explore.Services;
using Explore.Services.Localization;
using Explore.Web.Framework.Kendoui;
using Explore.Services.Helpers;
using Explore.Web.Factories;
using Explore.Services.Content;

namespace Explore.Web.Controllers
{
    public partial class BannerController : BaseAdminController
    {
        #region Fields

        private readonly IPermissionService _permissionService;
        private readonly ILocalizationService _localizationService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IBannerService _bannerService;
        private readonly IBannerModelFactory _bannerModelFactory;

        #endregion

        #region Ctor

        public BannerController(IPermissionService permissionService,
            ILocalizationService localizationService,
            IDateTimeHelper dateTimeHelper,
            IBannerService bannerService,
            IBannerModelFactory bannerModelFactory)
        {
            this._permissionService = permissionService;
            this._localizationService = localizationService;
            this._dateTimeHelper = dateTimeHelper;
            this._bannerService = bannerService;
            this._bannerModelFactory = bannerModelFactory;
        }

        #endregion

        #region Banner

        public virtual ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual ActionResult List(List<string> bannerStatusIds = null)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageBanner))
                return AccessDeniedView();

            var model = new BannerListModel();
            model.AvailableOrderStatuses = BannerStatus.Display.ToSelectList(false).ToList();
            model.AvailableOrderStatuses.Insert(0, new SelectListItem 
            { Text = _localizationService.GetResource("Admin.Common.All"), Value = "-1", Selected = true });

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult BannerList(DataSourceRequest command, BannerListModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageBanner))
                return AccessDeniedView();

            DateTime? startDateValue = (model.SearchStartDate == null) ? null
                : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.SearchStartDate.Value, _dateTimeHelper.CurrentTimeZone);

            DateTime? endDateValue = (model.SearchEndDate == null) ? null
                            : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.SearchEndDate.Value, _dateTimeHelper.CurrentTimeZone).AddDays(1);

            var bannerStatusIds = !model.BannerStatusIds.Contains(-1) ? model.BannerStatusIds : null;

            var banners = _bannerService.SearchBanners(model.Theme, bannerStatusIds, startDateValue, endDateValue, command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = _bannerModelFactory.PrepareBannerListModel(banners),
                Total = banners.TotalCount
            };

            return Json(gridModel);
        }

        #endregion


    }
}