using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Domain.Users
{
    /// <summary>
    /// 用户账户实体
    /// </summary>
    public partial class UserAssets : BaseEntity
    {

        public int UserId { set; get; }

        public decimal GoldCoin { set; get; }

    }
}
