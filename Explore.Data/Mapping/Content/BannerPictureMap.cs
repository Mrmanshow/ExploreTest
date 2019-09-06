using Explore.Core.Domain.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Data.Mapping.Content
{
    public partial class BannerPictureMap : ExploreEntityTypeConfiguration<BannerPicture>
    {
        public BannerPictureMap()
        {
            this.ToTable("Banner_Picture_Mapping");
            this.HasKey(pp => pp.Id);


            //this.HasRequired(b => b.Picture)
            //    .WithMany()
            //    .HasForeignKey(b => b.PictureId);

            this.HasRequired(b => b.Banner)
                .WithMany(c => c.Pictures)
                .HasForeignKey(c => c.BannerId);

        }
    }
}
