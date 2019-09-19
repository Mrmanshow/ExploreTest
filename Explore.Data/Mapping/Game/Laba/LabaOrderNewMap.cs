using Explore.Core.Domain.Game.Laba;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Data.Mapping.Game.Laba
{
    public partial class LabaOrderNewMap: ExploreEntityTypeConfiguration<LabaOrderNew>
    {
        public LabaOrderNewMap()
        {
            this.ToTable("LabaOrderNew");
            this.HasKey(x => x.Id);

            this.HasRequired(c => c.User).
                 WithMany().
                 HasForeignKey(c => c.UserId);
        }
    }
}
