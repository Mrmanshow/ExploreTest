
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
using Explore.Services.Media;
using Explore.Web.Framework.Controllers;
using Explore.Web.Extensions;

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
        private readonly IPictureService _pictureService;

        #endregion

        #region Ctor

        public BannerController(IPermissionService permissionService,
            ILocalizationService localizationService,
            IDateTimeHelper dateTimeHelper,
            IBannerService bannerService,
            IBannerModelFactory bannerModelFactory,
            IPictureService pictureService)
        {
            this._permissionService = permissionService;
            this._localizationService = localizationService;
            this._dateTimeHelper = dateTimeHelper;
            this._bannerService = bannerService;
            this._bannerModelFactory = bannerModelFactory;
            this._pictureService = pictureService;
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
            model.AvailableBannerStatuses = BannerStatus.Display.ToSelectList(false).ToList();
            model.AvailableBannerStatuses.Insert(0, new SelectListItem 
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

        public virtual ActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageBanner))
                return AccessDeniedView();

            var model = new BannerModel();

            model.AvailableBannerStatuses = BannerStatus.Display.ToSelectList(true).ToList();
            model.AvailableBannerTypes = BannerType.Index.ToSelectList(true).ToList();

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual ActionResult Create(BannerModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageBanner))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var banner = model.ToEntity();

                banner.CreateTime = DateTime.UtcNow;
                banner.BannerImg = "";
                banner.Pictures.Add(new BannerPicture { PictureId = model.PictureModel.PictureId });

                _bannerService.InsertBanner(banner);

                if (continueEditing)
                {
                    //selected tab
                    SaveSelectedTabName();

                    return RedirectToAction("Edit", new { id = banner.Id });
                }
            }

            model.AvailableBannerStatuses = BannerStatus.Display.ToSelectList(true).ToList();
            model.AvailableBannerTypes = BannerType.Index.ToSelectList(true).ToList();

            return View(model);
        }

        public virtual ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageBanner))
                return AccessDeniedView();

            var banner = _bannerService.GetBannerById(id);
            if (banner == null)
                return RedirectToAction("List");

            var model = _bannerModelFactory.PrepareBannerModel(banner);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual ActionResult Edit(BannerModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageBanner))
                return AccessDeniedView();

            var banner = _bannerService.GetBannerById(model.Id);

            if (ModelState.IsValid)
            {
                banner = model.ToEntity(banner);
                banner.Pictures.First().PictureId = model.PictureModel.PictureId;

                _bannerService.UpdateBanner(banner);


                if (continueEditing)
                {
                    //selected tab
                    SaveSelectedTabName();

                    return RedirectToAction("Edit", new { id = banner.Id });
                }
                return RedirectToAction("List");
            }

            //model = _bannerModelFactory.PrepareBannerModel(banner);

            BannerStatus status = (BannerStatus)Enum.Parse(typeof(BannerStatus), banner.Status.ToString());
            model.AvailableBannerStatuses = status.ToSelectList(true).ToList();

            BannerType type = (BannerType)Enum.Parse(typeof(BannerType), banner.Type.ToString());
            model.AvailableBannerTypes = type.ToSelectList(true).ToList();

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageBanner))
                return AccessDeniedView();

            var banner = _bannerService.GetBannerById(id);
            if (banner == null)
                return RedirectToAction("List");

            _bannerService.DeleteBanner(banner);

            return RedirectToAction("List");
        }

        public virtual ActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageBanner))
                return AccessDeniedView();

            if (selectedIds != null)
            {
                var banners = _bannerService.GetBannersByIds(selectedIds.ToArray());
                _bannerService.DeleteBanners(banners);
            }

            return Json(new { Result = true });
        }

        #endregion


    }
}