using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Domain.Users
{
    /// <summary>
    /// 用户实体
    /// </summary>
    public partial class User : BaseEntity
    {

        public virtual string UserName { set; get; }

        public string NickName { set; get; }

        public int LoginType { set; get; }

        public DateTime CreateTime { set; get; }

        public int? UserAssetsId { set; get; }

        #region Navigation properties

        /// <summary>
        /// 用户账户
        /// </summary>
        public virtual UserAssets UserAssets { set; get; }

        #endregion
    }
}
