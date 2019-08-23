using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Domain.Customers
{
    /// <summary>
    /// 表示用户注册类型格式枚举
    /// </summary>
    public enum UserRegistrationType
    {
        /// <summary>
        /// 标准帐户创建
        /// </summary>
        Standard = 1,
        /// <summary>
        /// 注册后需要电子邮件验证
        /// </summary>
        EmailValidation = 2,
        /// <summary>
        /// 用户应得到管理员的批准。
        /// </summary>
        AdminApproval = 3,
        /// <summary>
        /// 注册被禁用
        /// </summary>
        Disabled = 4,
    }
}
