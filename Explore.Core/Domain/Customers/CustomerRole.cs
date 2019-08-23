using Explore.Core.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Domain.Customers
{
    /// <summary>
    /// 表示用户权限
    /// </summary>
    public partial class CustomerRole : BaseEntity
    {
        private ICollection<PermissionRecord> _permissionRecords;

        /// <summary>
        /// 获取或设置用户权限名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示用户权限是否处于活动状态
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// 获取或设置用户权限系统名称
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示用户是否必须在指定时间后更改密码
        /// </summary>
        public bool EnablePasswordLifetime { get; set; }

        /// <summary>
        /// 获取或设置权限记录
        /// </summary>
        public virtual ICollection<PermissionRecord> PermissionRecords
        {
            get { return _permissionRecords ?? (_permissionRecords = new List<PermissionRecord>()); }
            protected set { _permissionRecords = value; }
        }
    }
}
