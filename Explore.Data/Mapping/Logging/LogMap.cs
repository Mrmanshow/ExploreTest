using Explore.Core.Domain.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Data.Mapping.Logging
{
    public partial class LogMap : ExploreEntityTypeConfiguration<Log>
    {
        public LogMap()
        {
            this.ToTable("Log");
            this.HasKey(l => l.Id);
            this.Property(l => l.ShortMessage).IsRequired();
            this.Property(l => l.IpAddress).HasMaxLength(200);

            this.Ignore(l => l.LogLevel);

            this.HasOptional(l => l.Customer)
                .WithMany()
                .HasForeignKey(l => l.CustomerId)
            .WillCascadeOnDelete(true);

        }
    }
}
