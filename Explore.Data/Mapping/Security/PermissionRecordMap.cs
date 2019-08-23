using Explore.Core.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Data.Mapping.Security
{
    public partial class PermissionRecordMap : ExploreEntityTypeConfiguration<PermissionRecord>
    {
        public PermissionRecordMap()
        {
            this.ToTable("PermissionRecord");
            this.HasKey(pr => pr.Id);
            this.Property(pr => pr.Name).IsRequired();
            this.Property(pr => pr.SystemName).IsRequired().HasMaxLength(255);
            this.Property(pr => pr.Category).IsRequired().HasMaxLength(255);

            this.HasMany(pr => pr.CustomerRoles)
                .WithMany(cr => cr.PermissionRecords)
                .Map(m => m.ToTable("PermissionRecord_Role_Mapping"));
        }
    }
}
