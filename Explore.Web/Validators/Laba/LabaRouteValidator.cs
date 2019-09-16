using Explore.Web.Framework.Validators;
using Explore.Web.Models.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using Explore.Services.Localization;

namespace Explore.Web.Validators.Laba
{
    public partial class LabaRouteValidator : BaseExploreValidator<GameLabaRouteModel>
    {
        public LabaRouteValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.X1).GreaterThanOrEqualTo(0).WithMessage(localizationService.GetResource("Admin.LabaRoute.Fields.X1.Required"));
            RuleFor(x => x.X2).GreaterThanOrEqualTo(0).WithMessage(localizationService.GetResource("Admin.LabaRoute.Fields.X2.Required"));
            RuleFor(x => x.X3).GreaterThanOrEqualTo(0).WithMessage(localizationService.GetResource("Admin.LabaRoute.Fields.X3.Required"));
            RuleFor(x => x.X4).GreaterThanOrEqualTo(0).WithMessage(localizationService.GetResource("Admin.LabaRoute.Fields.X4.Required"));
            RuleFor(x => x.X5).GreaterThanOrEqualTo(0).WithMessage(localizationService.GetResource("Admin.LabaRoute.Fields.X5.Required"));
            RuleFor(x => x.Y1).GreaterThanOrEqualTo(0).WithMessage(localizationService.GetResource("Admin.LabaRoute.Fields.Y1.Required"));
            RuleFor(x => x.Y2).GreaterThanOrEqualTo(0).WithMessage(localizationService.GetResource("Admin.LabaRoute.Fields.Y2.Required"));
            RuleFor(x => x.Y3).GreaterThanOrEqualTo(0).WithMessage(localizationService.GetResource("Admin.LabaRoute.Fields.Y3.Required"));
            RuleFor(x => x.Y4).GreaterThanOrEqualTo(0).WithMessage(localizationService.GetResource("Admin.LabaRoute.Fields.Y4.Required"));
            RuleFor(x => x.Y5).GreaterThanOrEqualTo(0).WithMessage(localizationService.GetResource("Admin.LabaRoute.Fields.Y5.Required"));
            RuleFor(x => x.Sequence).GreaterThanOrEqualTo(0).WithMessage(localizationService.GetResource("Admin.LabaRoute.Fields.Sequence.Required"));

        }
    }
}