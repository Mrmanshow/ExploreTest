using Explore.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Data.Mapping.Common
{
    public partial class GenericAttributeMap : ExploreEntityTypeConfiguration<GenericAttribute>
    {
        public GenericAttributeMap()
        {
            this.ToTable("GenericAttribute");
            this.HasKey(ga => ga.Id);

            this.Property(ga => ga.KeyGroup).IsRequired().HasMaxLength(400);
            this.Property(ga => ga.Key).IsRequired().HasMaxLength(400);
            this.Property(ga => ga.Value).IsRequired();
        }
    }
}
