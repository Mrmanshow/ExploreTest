using Explore.Core;
using Explore.Core.Domain.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Content
{
    public partial interface IBannerService
    {
        /// <summary>
        /// 获取Banner列表
        /// </summary>
        /// <param name="theme"></param>
        /// <param name="bsIds"></param>
        /// <param name="searchStartDate"></param>
        /// <param name="searchEndDate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<Banner> SearchBanners(string theme = "", List<int> bsIds = null,
            DateTime? searchStartDate = null, DateTime? searchEndDate = null,
            int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
