using Explore.Web.Framework;
using Explore.Web.Framework.MVC;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FluentValidation.Attributes;
using Explore.Web.Validators.Content;

namespace Explore.Web.Models.Content
{
    [Validator(typeof(BannerValidate))]
    public partial class BannerModel : BaseExploreEntityModel
    {
        public BannerModel()
        {
            AvailableBannerStatuses = new List<SelectListItem>();
            AvailableBannerTypes = new List<SelectListItem>();
            PictureModel = new ProductPictureModel();
        }


        [ExploreResourceDisplayName("Admin.Content.Banner.Fields.Theme")]
        [AllowHtml]
        public string Theme { get; set; }

        [ExploreResourceDisplayName("Admin.Content.Banner.Fields.BannerLink")]
        public string BannerLink { get; set; }

        [ExploreResourceDisplayName("Admin.Content.Banner.Fields.BannerOrder")]
        public int BannerOrder { get; set; }

        [ExploreResourceDisplayName("Admin.Content.Banner.Fields.ShowBeginDate")]
        [UIHint("Date")]
        public DateTime ShowBeginDate { get; set; }

        [ExploreResourceDisplayName("Admin.Content.Banner.Fields.ShowEndDate")]
        [UIHint("Date")]
        public DateTime ShowEndDate { get; set; }

        public DateTime CreateTime { get; set; }


        [ExploreResourceDisplayName("Admin.Content.Banner.Fields.LinkType")]
        public bool LinkType { set; get; }

        [ExploreResourceDisplayName("Admin.Content.Banner.Fields.Status")]
        public string Status { get; set; }
        public IList<SelectListItem> AvailableBannerStatuses { set; get; }

        [ExploreResourceDisplayName("Admin.Content.Banner.Fields.Type")]
        public string Type { get; set; }
        public IList<SelectListItem> AvailableBannerTypes { set; get; }

        public ProductPictureModel PictureModel { get; set; }

        public partial class ProductPictureModel : BaseExploreEntityModel
        {
            [UIHint("Picture")]
            [ExploreResourceDisplayName("Admin.Content.Products.Banner.Fields.PictureId")]
            public int PictureId { get; set; }

            [ExploreResourceDisplayName("Admin.Catalog.Products.Pictures.Fields.PictureUrl")]
            public string PictureUrl { get; set; }
        }
    }
}