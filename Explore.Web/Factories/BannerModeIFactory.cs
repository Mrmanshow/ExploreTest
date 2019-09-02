using Explore.Core.Domain.Content;
using Explore.Web.Models.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Explore.Web.Extensions;
using Explore.Services.Localization;
using Explore.Core;

namespace Explore.Web.Factories
{
    public  class BannerModeIFactory: IBannerModelFactory
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public BannerModeIFactory(ILocalizationService localizationService,
            IWorkContext workContext)
        {
            this._workContext = workContext;
            this._localizationService = localizationService;
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

                return model;
            });

            return list.ToList();
        }

        #endregion
    }
}