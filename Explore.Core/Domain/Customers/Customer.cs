using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Domain.Customers
{
    /// <summary>
    /// Represents a customer
    /// </summary>
    public partial class Customer : BaseEntity
    {
        private ICollection<ExternalAuthenticationRecord> _externalAuthenticationRecords;
        private ICollection<CustomerRole> _customerRoles;

        /// <summary>
        /// Ctor
        /// </summary>
        public Customer()
        {
            this.CustomerGuid = Guid.NewGuid();
        }

        /// <summary>
        /// 获取或设置用户GUID
        /// </summary>
        public Guid CustomerGuid { get; set; }

        /// <summary>
        /// 获取或设置用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 获取或设置管理注释
        /// </summary>
        public string AdminComment { get; set; }

        /// <summary>
        /// 隶属关系ID
        /// </summary>
        public int AffiliateId { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否需要客户重新登录
        /// </summary>
        public bool RequireReLogin { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示登录尝试失败的次数（错误的密码）
        /// </summary>
        public int FailedLoginAttempts { get; set; }
        /// <summary>
        /// 获取或设置客户无法登录（锁定）的日期和时间
        /// </summary>
        public DateTime? CannotLoginUntilDateUtc { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示用户是否处于活动状态
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示用户是否已被删除
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        ///获取或设置客户系统名称
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// 获取或设置最后一个IP地址
        /// </summary>
        public string LastIpAddress { get; set; }

        /// <summary>
        /// 获取或设置实体创建的日期和时间
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// 获取或设置上次登录的日期和时间
        /// </summary>
        public DateTime? LastLoginDateUtc { get; set; }

        /// <summary>
        /// 获取或设置上一个活动的日期和时间
        /// </summary>
        public DateTime LastActivityDateUtc { get; set; }

        /// <summary>
        ///  获取或设置客户在其中注册的存储标识符
        /// </summary>
        public int RegisteredInStoreId { get; set; }

        #region Navigation properties

        /// <summary>
        /// 获取或设置客户生成的内容
        /// </summary>
        public virtual ICollection<ExternalAuthenticationRecord> ExternalAuthenticationRecords
        {
            get { return _externalAuthenticationRecords ?? (_externalAuthenticationRecords = new List<ExternalAuthenticationRecord>()); }
            protected set { _externalAuthenticationRecords = value; }
        }

        /// <summary>
        /// 获取或设置客户权限
        /// </summary>
        public virtual ICollection<CustomerRole> CustomerRoles
        {
            get { return _customerRoles ?? (_customerRoles = new List<CustomerRole>()); }
            protected set { _customerRoles = value; }
        }

        #endregion
    }
}
