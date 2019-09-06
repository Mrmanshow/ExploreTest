using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Domain.Game
{
     public partial class ScratchCard : BaseEntity
    {
         public decimal BuyAmount { set; get; }

         public decimal Amount { set; get; }

         public int UserId { set; get; }

         public int Status { set; get; }

         public DateTime ScratchTime { set; get; }
    }
}
