using Explore.Core.Domain.Game.Laba;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Data.Mapping.Game.Laba
{
    public partial class LabaOrderMap: ExploreEntityTypeConfiguration<LabaOrder>
    {
        public LabaOrderMap()
        {
            this.ToTable("LabaOrder");
            this.HasKey(x => x.Id);
        }
    }
}
