using Explore.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Data.Mapping.Customers
{
    public partial class CustomerPasswordMap : ExploreEntityTypeConfiguration<CustomerPassword>
    {
        public CustomerPasswordMap()
        {
            this.ToTable("CustomerPassword");
            this.HasKey(password => password.Id);

            this.HasRequired(password => password.Customer)
                .WithMany()
                .HasForeignKey(password => password.CustomerId);

            this.Ignore(password => password.PasswordFormat);
        }
    }
}
