using Explore.Web.Framework;
using Explore.Web.Framework.MVC;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Explore.Web.Models.Content
{
    public partial class BannerListModel : BaseExploreModel
    {
        public BannerListModel()
        {
            BannerStatusIds = new List<int>();
            AvailableBannerStatuses = new List<SelectListItem>();
        }

        [ExploreResourceDisplayName("Admin.Content.Banner.List.SearchStartDate")]
        [UIHint("DateNullable")]
        public DateTime? SearchStartDate { get; set; }

        [ExploreResourceDisplayName("Admin.Content.Banner.List.SearchEndDate")]
        [UIHint("DateNullable")]
        public DateTime? SearchEndDate { get; set; }

        [ExploreResourceDisplayName("Admin.Content.List.Banner.BannerStatus")]
        [UIHint("MultiSelect")]
        public List<int> BannerStatusIds { set; get; }

        [ExploreResourceDisplayName("Admin.Content.List.Banner.Theme")]
        [AllowHtml]
        public string Theme { set; get; }

        public IList<SelectListItem> AvailableBannerStatuses { set; get; }
    }
}