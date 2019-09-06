using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Domain.Game
{
    public partial class LuckEggOrderDetail: BaseEntity
    {
        public int OrderId { set; get; }

        public DateTime CreateTime { set; get; }
    }
}
