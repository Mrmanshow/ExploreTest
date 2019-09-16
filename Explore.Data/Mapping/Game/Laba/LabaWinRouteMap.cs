using Explore.Core.Domain.Game.Laba;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Data.Mapping.Game.Laba
{
    public partial class LabaWinRouteMap: ExploreEntityTypeConfiguration<LabaWinRoute>
    {
        public LabaWinRouteMap()
        {
            this.ToTable("LabaWinRoute");
            this.HasKey(x => x.Id);


            this.Ignore(x => x.LabaRouteStatus);
        }
    }
}
