using Explore.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Data.Mapping.Users
{
    public partial class UserAssetsMap : ExploreEntityTypeConfiguration<UserAssets>
    {
        public UserAssetsMap()
        {
            this.ToTable("UserAssets");
            this.HasKey(c => c.Id);
        }
    }
}
