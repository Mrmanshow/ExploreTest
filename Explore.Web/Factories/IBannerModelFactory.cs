using Explore.Core.Domain.Content;
using Explore.Web.Models.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Explore.Web.Factories
{
    public partial interface IBannerModelFactory
    {
        IList<BannerModel> PrepareBannerListModel(IList<Banner> customer);
    }
}