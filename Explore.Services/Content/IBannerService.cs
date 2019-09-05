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

        /// <summary>
        /// 根据Id获取Banner详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Banner GetBannerById(int id);

        /// <summary>
        /// 获取轮播图列表根据ID
        /// </summary>
        /// <param name="bannerIds"></param>
        /// <returns></returns>
        IList<Banner> GetBannersByIds(int[] bannerIds);

        /// <summary>
        /// 添加轮播图
        /// </summary>
        /// <param name="banner"></param>
        void InsertBanner(Banner banner);

        /// <summary>
        /// 更新轮播图
        /// </summary>
        /// <param name="banner"></param>
        void UpdateBanner(Banner banner);

        /// <summary>
        /// 删除轮播图
        /// </summary>
        /// <param name="banner"></param>
        void DeleteBanner(Banner banner);

        /// <summary>
        /// 删除选中轮播图
        /// </summary>
        /// <param name="banners"></param>
        void DeleteBanners(IList<Banner> banners);
    }
}
