using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Domain.Customers
{
    /// <summary>
    /// 表示用户密码
    /// </summary>
    public partial class CustomerPassword : BaseEntity
    {
        public CustomerPassword()
        {
            this.PasswordFormat = PasswordFormat.Clear;
        }

        /// <summary>
        /// 获取或设置用户标识符
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// 获取或设置密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 获取或设置密码格式标识符
        /// </summary>
        public int PasswordFormatId { get; set; }

        /// <summary>
        /// 获取或设置密码salt
        /// </summary>
        public string PasswordSalt { get; set; }

        /// <summary>
        /// 获取或设置实体创建的日期和时间
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// 获取或设置密码格式
        /// </summary>
        public PasswordFormat PasswordFormat
        {
            get { return (PasswordFormat)PasswordFormatId; }
            set { this.PasswordFormatId = (int)value; }
        }

        /// <summary>
        /// 获取或设置客户
        /// </summary>
        public virtual Customer Customer { get; set; }
    }
}
