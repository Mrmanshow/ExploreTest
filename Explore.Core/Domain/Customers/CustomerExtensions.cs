using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Domain.Customers
{
    public static class CustomerExtensions
    {
        #region Customer role

        /// <summary>
        /// 获取一个值，该值指示用户是否在某个用户角色中
        /// </summary>
        /// <param name="customer">用户</param>
        /// <param name="customerRoleSystemName">用户角色系统名称</param>
        /// <param name="onlyActiveCustomerRoles">一个值，指示我们是否应只在活动的客户角色中查找</param>
        /// <returns>Result</returns>
        public static bool IsInCustomerRole(this Customer customer,
            string customerRoleSystemName, bool onlyActiveCustomerRoles = true)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            if (String.IsNullOrEmpty(customerRoleSystemName))
                throw new ArgumentNullException("customerRoleSystemName");

            var result = customer.CustomerRoles
                .FirstOrDefault(cr => (!onlyActiveCustomerRoles || cr.Active) && (cr.SystemName == customerRoleSystemName)) != null;
            return result;
        }

        /// <summary>
        /// 获取一个值，该值指示客户是否为搜索引擎
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <returns>Result</returns>
        public static bool IsSearchEngineAccount(this Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            if (String.IsNullOrEmpty(customer.SystemName))
                return false;

            var result = customer.SystemName.Equals(SystemCustomerNames.SearchEngine, StringComparison.InvariantCultureIgnoreCase);
            return result;
        }

        /// <summary>
        /// 获取一个值，该值指示客户是否为后台任务的内置记录
        /// </summary>
        /// <param name="customer">用户</param>
        /// <returns>结果</returns>
        public static bool IsBackgroundTaskAccount(this Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            if (String.IsNullOrEmpty(customer.SystemName))
                return false;

            var result = customer.SystemName.Equals(SystemCustomerNames.BackgroundTask, StringComparison.InvariantCultureIgnoreCase);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether customer is administrator
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="onlyActiveCustomerRoles">A value indicating whether we should look only in active customer roles</param>
        /// <returns>Result</returns>
        public static bool IsAdmin(this Customer customer, bool onlyActiveCustomerRoles = true)
        {
            return IsInCustomerRole(customer, SystemCustomerRoleNames.Administrators, onlyActiveCustomerRoles);
        }

        /// <summary>
        /// 获取一个值，该值指示客户是否已注册
        /// </summary>
        /// <param name="customer">用户</param>
        /// <param name="onlyActiveCustomerRoles">一个值，指示我们是否应只在活动的客户角色中查找</param>
        /// <returns>Result</returns>
        //public static bool IsRegistered(this Customer customer, bool onlyActiveCustomerRoles = true)
        //{
        //    return IsInCustomerRole(customer, SystemCustomerRoleNames.Registered, onlyActiveCustomerRoles);
        //}

        /// <summary>
        /// Gets a value indicating whether customer is guest
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="onlyActiveCustomerRoles">A value indicating whether we should look only in active customer roles</param>
        /// <returns>Result</returns>
        public static bool IsGuest(this Customer customer, bool onlyActiveCustomerRoles = true)
        {
            return IsInCustomerRole(customer, SystemCustomerRoleNames.Guests, onlyActiveCustomerRoles);
        }

        #endregion
    }
}
