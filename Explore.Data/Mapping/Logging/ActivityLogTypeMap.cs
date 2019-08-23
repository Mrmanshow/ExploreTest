using Explore.Core.Domain.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Data.Mapping.Logging
{
    public partial class ActivityLogTypeMap : ExploreEntityTypeConfiguration<ActivityLogType>
    {
        public ActivityLogTypeMap()
        {
            this.ToTable("ActivityLogType");
            this.HasKey(alt => alt.Id);

            this.Property(alt => alt.SystemKeyword).IsRequired().HasMaxLength(100);
            this.Property(alt => alt.Name).IsRequired().HasMaxLength(200);
        }
    }
}
