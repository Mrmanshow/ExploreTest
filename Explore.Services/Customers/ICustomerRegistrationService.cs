using Explore.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Customers
{
    /// <summary>
    /// 用户注册接口
    /// </summary>
    public partial interface ICustomerRegistrationService
    {
        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="usernameOrEmail">用户名或邮箱</param>
        /// <param name="password">密码</param>
        /// <returns>结果</returns>
        CustomerLoginResults ValidateCustomer(string usernameOrEmail, string password);

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="request">注册请求</param>
        /// <returns>结果</returns>
        CustomerRegistrationResult RegisterCustomer(CustomerRegistrationRequest request);

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Result</returns>
        //ChangePasswordResult ChangePassword(ChangePasswordRequest request);

        /// <summary>
        /// Sets a user email
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="newEmail">New email</param>
        /// <param name="requireValidation">Require validation of new email address</param>
        //void SetEmail(Customer customer, string newEmail, bool requireValidation);

        /// <summary>
        /// Sets a customer username
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="newUsername">New Username</param>
        //void SetUsername(Customer customer, string newUsername);
    }
}
