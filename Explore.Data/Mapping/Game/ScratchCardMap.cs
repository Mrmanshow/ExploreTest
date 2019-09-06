using Explore.Core.Domain.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Data.Mapping.Game
{
    public partial class ScratchCardMap: ExploreEntityTypeConfiguration<ScratchCard>
    {
        public ScratchCardMap()
        {
            this.ToTable("ScratchCard");
            this.HasKey(s => s.Id);
        }
    }
}
