using Explore.Core.Domain.Content;
using Explore.Web.Models.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Explore.Web.Extensions;
using Explore.Services.Localization;
using Explore.Core;
using Explore.Services;
using Explore.Services.Media;

namespace Explore.Web.Factories
{
    public  class BannerModeIFactory: IBannerModelFactory
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly IPictureService _pictureService;

        #endregion

        #region Ctor

        public BannerModeIFactory(ILocalizationService localizationService,
            IWorkContext workContext,
            IPictureService pictureService)
        {
            this._workContext = workContext;
            this._localizationService = localizationService;
            this._pictureService = pictureService;
        }

        #endregion

        #region Methods

        public virtual IList<BannerModel> PrepareBannerListModel(IList<Banner> banners)
        {
            var list = banners.Select(x =>
            {
                var model = x.ToModel();
                model.Status = x.BannerStatus.GetLocalizedEnum(_localizationService, _workContext);
                model.Type = x.BannerType.GetLocalizedEnum(_localizationService, _workContext);

                model.PictureModel.PictureUrl = _pictureService.GetPictureUrl(model.PictureModel.PictureId);
                return model;
            });

            return list.ToList();
        }

        public virtual BannerModel PrepareBannerModel(Banner banner)
        {
            var model = banner.ToModel();

            //string status = Enum.GetName(typeof(BannerStatus), banner.Status);
            BannerStatus status = (BannerStatus)Enum.Parse(typeof(BannerStatus), banner.Status.ToString());
            model.AvailableBannerStatuses = status.ToSelectList(true).ToList();

            BannerType type = (BannerType)Enum.Parse(typeof(BannerType), banner.Type.ToString());
            model.AvailableBannerTypes = type.ToSelectList(true).ToList();

            return model;
        }

        #endregion
    }
}