using Explore.Core;
using Explore.Core.Data;
using Explore.Core.Domain.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Content
{
    public partial class BannerService: IBannerService
    {
        #region Fields

        private readonly IRepository<Banner> _bannerRepository;

        #endregion

        #region Ctor

        public BannerService(IRepository<Banner> bannerRepository)
        {
            this._bannerRepository = bannerRepository;
        }

        #endregion

        #region Methods

        public virtual IPagedList<Banner> SearchBanners(string theme = "", List<int> bsIds = null,
            DateTime? searchStartDate = null, DateTime? searchEndDate = null,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _bannerRepository.Table;

            if (!string.IsNullOrWhiteSpace(theme))
                query = query.Where(b => b.Theme.Contains(theme));

            if (bsIds != null && bsIds.Any())
                query = query.Where(b => bsIds.Contains(b.Status));

            if (searchStartDate.HasValue)
                query = query.Where(b => b.ShowBeginDate > searchStartDate);

            if (searchEndDate.HasValue)
                query = query.Where(b => b.ShowEndDate < searchEndDate);

            query = query.OrderByDescending(b=>b.CreateTime);

            return new PagedList<Banner>(query, pageIndex, pageSize);
        }

        public virtual Banner GetBannerById(int id)
        {
            if (id == 0)
                return null;

            return _bannerRepository.GetById(id);
        }

        /// <summary>
        /// 获取轮播图列表根据ID
        /// </summary>
        /// <param name="bannerIds"></param>
        /// <returns></returns>
        public virtual IList<Banner> GetBannersByIds(int[] bannerIds)
        {
            if (bannerIds == null || bannerIds.Length == 0)
                return new List<Banner>();

            var query = from p in _bannerRepository.Table
                        where bannerIds.Contains(p.Id)
                        select p;

            var banners = query.ToList();

            return banners;
        }

        public virtual void InsertBanner(Banner banner)
        {
            if (banner == null)
                throw new ArgumentNullException("banner");

            _bannerRepository.Insert(banner);

        }

        /// <summary>
        /// 更新轮播图
        /// </summary>
        /// <param name="banner"></param>
        public virtual void UpdateBanner(Banner banner)
        {
            if (banner == null)
                throw new ArgumentNullException("banner");

            _bannerRepository.Update(banner);

        }

        /// <summary>
        /// 删除轮播图
        /// </summary>
        /// <param name="banner"></param>
        public virtual void DeleteBanner(Banner banner)
        {
            if (banner == null)
                throw new ArgumentNullException("banner");

            _bannerRepository.Delete(banner);
        }

        /// <summary>
        /// 删除选中轮播图
        /// </summary>
        /// <param name="banners"></param>
        public virtual void DeleteBanners(IList<Banner> banners)
        {
            if (banners == null)
                throw new ArgumentNullException("banner");

            _bannerRepository.Delete(banners);
        }

        #endregion
    }
}
