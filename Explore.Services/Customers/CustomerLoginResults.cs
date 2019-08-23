using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Customers
{
    /// <summary>
    /// 表示用户登录结果枚举
    /// </summary>
    public enum CustomerLoginResults
    {
        /// <summary>
        /// 登录成功
        /// </summary>
        Successful = 1,
        /// <summary>
        /// 用户不存在（电子邮件或用户名）
        /// </summary>
        CustomerNotExist = 2,
        /// <summary>
        /// 密码错误
        /// </summary>
        WrongPassword = 3,
        /// <summary>
        /// 帐户尚未激活
        /// </summary>
        NotActive = 4,
        /// <summary>
        /// 用户已被删除
        /// </summary>
        Deleted = 5,
        /// <summary>
        /// 用户未注册
        /// </summary>
        NotRegistered = 6,
        /// <summary>
        /// 已锁定
        /// </summary>
        LockedOut = 7,
    }
}
