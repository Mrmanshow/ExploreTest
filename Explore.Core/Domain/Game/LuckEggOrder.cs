using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Domain.Game
{
    public partial class LuckEggOrder: BaseEntity
    {
        private ICollection<LuckEggOrderDetail> _luckEggOrderDetails;

        public int Amount { set; get; }

        public int Status { set; get; }

        public int UserId { set; get; }

        public int WinAmount { set; get; }

        public virtual ICollection<LuckEggOrderDetail> LuckEggOrderDetails 
        {
            get {  return _luckEggOrderDetails ?? (_luckEggOrderDetails = new List<LuckEggOrderDetail>()); }
            protected set { _luckEggOrderDetails = value; } 
        }
    }
}
