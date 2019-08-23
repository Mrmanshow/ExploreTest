using Explore.Core;
using Explore.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Customers
{
    /// <summary>
    /// Customer service interface
    /// </summary>
    public partial interface ICustomerService
    {
        #region Customers

        /// <summary>
        /// Gets a customer
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>A customer</returns>
        Customer GetCustomerById(int customerId);

        /// <summary>
        /// 根据GUID获取客户
        /// </summary>
        /// <param name="customerGuid">用户 GUID</param>
        /// <returns>用户对象</returns>
        Customer GetCustomerByGuid(Guid customerGuid);

        /// <summary>
        /// 按系统名称获取用户
        /// </summary>
        /// <param name="systemName">系统名称</param>
        /// <returns>用户对象</returns>
        Customer GetCustomerBySystemName(string systemName);

        /// <summary>
        /// 按用户名获取用户
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>用户对象</returns>
        Customer GetCustomerByUsername(string username);

        /// <summary>
        /// Gets all customer roles
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Customer roles</returns>
        IList<CustomerRole> GetAllCustomerRoles(bool showHidden = false);

        /// <summary>
        /// 插入游客
        /// </summary>
        /// <returns>用户</returns>
        Customer InsertGuestCustomer();

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="customer">用户</param>
        void UpdateCustomer(Customer customer);

        #endregion

        #region Customer roles

        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <param name="systemName">用户权限系统名</param>
        /// <returns>用户权限</returns>
        CustomerRole GetCustomerRoleBySystemName(string systemName);

        #endregion

        #region Customer passwords

        /// <summary>
        /// 获取当前用户密码
        /// </summary>
        /// <param name="customerId">用户Id</param>
        /// <returns>用户密码对象</returns>
        CustomerPassword GetCurrentPassword(int customerId);

        /// <summary>
        /// Insert a customer password
        /// </summary>
        /// <param name="customerPassword">Customer password</param>
        void InsertCustomerPassword(CustomerPassword customerPassword);

        #endregion
    }
}
