using Explore.Core.Domain.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Data.Mapping.Game
{
    public partial class LuckEggOrderDetailMap: ExploreEntityTypeConfiguration<LuckEggOrderDetail>
    {
        public LuckEggOrderDetailMap()
        {
            this.ToTable("LuckEggOrderDetail");
            this.HasKey(x => x.Id);

        }
    }
}
