using Explore.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Data.Mapping.Customers
{
    public partial class CustomerMap : ExploreEntityTypeConfiguration<Customer>
    {
        public CustomerMap()
        {
            this.ToTable("Customer");
            this.HasKey(c => c.Id);
            this.Property(u => u.Username).HasMaxLength(1000);
            this.Property(u => u.SystemName).HasMaxLength(400);

            this.HasMany(c => c.CustomerRoles)
                .WithMany()
                .Map(m => m.ToTable("Customer_CustomerRole_Mapping"));
        }
    }
}
