using Explore.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Authentication
{
    /// <summary>
    /// 身份验证服务接口
    /// </summary>
    public partial interface IAuthenticationService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="customer">用户对象</param>
        /// <param name="createPersistentCookie">指示是否创建持久cookie的值</param>
        void SignIn(Customer customer, bool createPersistentCookie);

        /// <summary>
        /// 登出
        /// </summary>
        void SignOut();

        /// <summary>
        /// 获得认证用户
        /// </summary>
        /// <returns>用户对象</returns>
        Customer GetAuthenticatedCustomer();
    }
}
