using Explore.Services.Localization;
using Explore.Web.Framework.Validators;
using Explore.Web.Models.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;

namespace Explore.Web.Validators.Content
{
    public partial class BannerValidate : BaseExploreValidator<BannerModel>
    {
        public BannerValidate(ILocalizationService localizationService)
        {
            RuleFor(x => x.PictureModel.PictureId).NotEqual(0)
                .WithMessage(localizationService.GetResource("Admin.Content.Banner.Fields.PictureId.Required"));
            RuleFor(x => x.ShowBeginDate).NotNull()
                .WithMessage(localizationService.GetResource("Admin.Content.Banner.Fields.ShowBeginDate.Required"));
            RuleFor(x => x.ShowEndDate).NotNull()
                .WithMessage(localizationService.GetResource("Admin.Content.Banner.Fields.ShowEndDate.Required"));
        }
    }
}