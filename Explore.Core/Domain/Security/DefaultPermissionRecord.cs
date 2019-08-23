using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Domain.Security
{
    /// <summary>
    /// Represents a default permission record
    /// </summary>
    public class DefaultPermissionRecord
    {
        public DefaultPermissionRecord()
        {
            this.PermissionRecords = new List<PermissionRecord>();
        }

        /// <summary>
        /// 获取或设置用户角色系统名称
        /// </summary>
        public string CustomerRoleSystemName { get; set; }

        /// <summary>
        /// 获取或设置权限
        /// </summary>
        public IEnumerable<PermissionRecord> PermissionRecords { get; set; }
    }
}
