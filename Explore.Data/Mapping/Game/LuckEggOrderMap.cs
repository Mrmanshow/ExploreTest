using Explore.Core.Domain.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Data.Mapping.Game
{
    public partial class LuckEggOrderMap : ExploreEntityTypeConfiguration<LuckEggOrder>
    {
        public LuckEggOrderMap()
        {
            this.ToTable("LuckEggOrder");
            this.HasKey(x => x.Id);

            this.HasMany(a => a.LuckEggOrderDetails)
                .WithRequired()
                .HasForeignKey(x => x.OrderId);
        }
    }
}
