using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Domain.Game
{
    public partial class LabaOrder: BaseEntity
    {
        public int WinAmount { set; get; }

        public int Amount { set; get; }

        public int UserId { set; get; }

        public int Status { set; get; }

        public DateTime CreateTime { set; get; } 
    }
}
