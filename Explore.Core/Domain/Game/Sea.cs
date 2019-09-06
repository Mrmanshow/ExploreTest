using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Domain.Game
{
    public partial class Sea: BaseEntity
    {
        public int UserId { set; get; }

        public int BetSum { set; get; }

        public int WinSum { set; get; }

        public DateTime date { set; get; }
    }
}
