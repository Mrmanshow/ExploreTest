using Explore.Core.Domain.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Data.Mapping.Game
{
    public partial class SeaMap : ExploreEntityTypeConfiguration<Sea>
    {
        public SeaMap()
        {
            this.ToTable("TG_ZGame_Bet");
        }
    }
}
