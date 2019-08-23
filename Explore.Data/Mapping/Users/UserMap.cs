using Explore.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Data.Mapping.Users
{
    public partial class UserMap : ExploreEntityTypeConfiguration<User>
    {
        public UserMap()
        {
            this.ToTable("Users");
            this.HasKey(c => c.Id);

            this.HasOptional(c => c.UserAssets);
        }
    }
}
