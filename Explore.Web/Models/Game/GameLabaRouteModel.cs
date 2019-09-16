using Explore.Web.Framework;
using Explore.Web.Framework.MVC;
using Explore.Web.Validators.Laba;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Explore.Web.Models.Game
{
    //[Validator(typeof(LabaRouteValidator))]
    public partial class GameLabaRouteModel : BaseExploreEntityModel
    {
        [ExploreResourceDisplayName("Admin.LabaRoute.Fields.X1")]
        public int X1 { set; get; }

        [ExploreResourceDisplayName("Admin.LabaRoute.Fields.X2")]
        public int X2 { set; get; }

        [ExploreResourceDisplayName("Admin.LabaRoute.Fields.X3")]
        public int X3 { set; get; }

        [ExploreResourceDisplayName("Admin.LabaRoute.Fields.X4")]
        public int X4 { set; get; }

        [ExploreResourceDisplayName("Admin.LabaRoute.Fields.X5")]
        public int X5 { set; get; }

        [ExploreResourceDisplayName("Admin.LabaRoute.Fields.Y1")]
        public int Y1 { set; get; }

        [ExploreResourceDisplayName("Admin.LabaRoute.Fields.Y2")]
        public int Y2 { set; get; }

        [ExploreResourceDisplayName("Admin.LabaRoute.Fields.Y3")]
        public int Y3 { set; get; }

        [ExploreResourceDisplayName("Admin.LabaRoute.Fields.Y4")]
        public int Y4 { set; get; }

        [ExploreResourceDisplayName("Admin.LabaRoute.Fields.Y5")]
        public int Y5 { set; get; }

        [ExploreResourceDisplayName("Admin.LabaRoute.Fields.Status")]
        public string Status { set; get; }
        public IList<SelectListItem> AvaliableLabaRouteStatues { set; get; }

        [ExploreResourceDisplayName("Admin.LabaRoute.Fields.Sequence")]
        public int Sequence { set; get; }

        public DateTime CreateTime { set; get; }



    }
}