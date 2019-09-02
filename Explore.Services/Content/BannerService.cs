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

        #endregion
    }
}
