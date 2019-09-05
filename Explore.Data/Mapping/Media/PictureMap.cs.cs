using Explore.Core.Domain.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Data.Mapping.Media
{
    public partial class PictureMap : ExploreEntityTypeConfiguration<Picture>
    {
        public PictureMap()
        {
            this.ToTable("Picture");
            this.HasKey(p => p.Id);
            this.Property(p => p.PictureBinary).IsMaxLength();
            this.Property(p => p.MimeType).IsRequired().HasMaxLength(40);
            this.Property(p => p.SeoFilename).HasMaxLength(300);
        }
    }
}
