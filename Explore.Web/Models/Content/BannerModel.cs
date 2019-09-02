using Explore.Web.Framework.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Explore.Web.Models.Content
{
    public partial class BannerModel : BaseExploreEntityModel
    {
        public string BannerImg { get; set; }

        public string Theme { get; set; }

        public string Type { get; set; }

        public string BannerLink { get; set; }

        public int BannerOrder { get; set; }

        public string ShowBeginDate { get; set; }

        public string ShowEndDate { get; set; }

        public string CreateTime { get; set; }

        public string Status { get; set; }
    }
}