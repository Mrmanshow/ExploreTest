using Explore.Core.Domain.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Data.Mapping.Localization
{
    public partial class LocaleStringResourceMap : ExploreEntityTypeConfiguration<LocaleStringResource>
    {
        public LocaleStringResourceMap()
        {
            this.ToTable("LocaleStringResource");
            this.HasKey(lsr => lsr.Id);
            this.Property(lsr => lsr.ResourceName).IsRequired().HasMaxLength(200);
            this.Property(lsr => lsr.ResourceValue).IsRequired();
        }
    }
}
