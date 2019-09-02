using Explore.Core.Domain.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Data.Mapping.Content
{
    public partial class BannerMap : ExploreEntityTypeConfiguration<Banner>
    {
        public BannerMap()
        {
            this.ToTable("BannerImg");
            this.HasKey(c => c.Id);

            this.Ignore(c => c.BannerStatus);
            this.Ignore(c => c.BannerType);

        }
    }
}
