using Explore.Core;
using Explore.Core.Caching;
using Explore.Core.Domain.Customers;
using Explore.Core.Infrastructure;
using Explore.Services.Customers.Cache;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Explore.Services.Common;

namespace Explore.Services.Customers
{
    public static class CustomerExtensions
    {
        /// <summary>
        /// Get full name
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <returns>Customer full name</returns>
        public static string GetFullName(this Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");
            var firstName = customer.GetAttribute<string>(SystemCustomerAttributeNames.FirstName);
            var lastName = customer.GetAttribute<string>(SystemCustomerAttributeNames.LastName);

            string fullName = "";
            if (!String.IsNullOrWhiteSpace(firstName) && !String.IsNullOrWhiteSpace(lastName))
                fullName = string.Format("{0} {1}", firstName, lastName);
            else
            {
                if (!String.IsNullOrWhiteSpace(firstName))
                    fullName = firstName;

                if (!String.IsNullOrWhiteSpace(lastName))
                    fullName = lastName;
            }
            return fullName;
        }

        /// <summary>
        /// Get customer role identifiers
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="showHidden">A value indicating whether to load hidden records</param>
        /// <returns>Customer role identifiers</returns>
        public static int[] GetCustomerRoleIds(this Customer customer, bool showHidden = false)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            var customerRolesIds = customer.CustomerRoles
               .Where(cr => showHidden || cr.Active)
               .Select(cr => cr.Id)
               .ToArray();

            return customerRolesIds;
        }

        /// <summary>
        /// 检查用户密码是否过期
        /// </summary>
        /// <param name="customer">用户</param>
        /// <returns>如果密码过期，则为true；否则为false</returns>
        public static bool PasswordIsExpired(this Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            //游客没有密码
            if (customer.IsGuest())
                return false;

            //已为用户禁用密码生存期
            if (!customer.CustomerRoles.Any(role => role.Active && role.EnablePasswordLifetime))
                return false;

            //setting disabled for all
            var customerSettings = EngineContext.Current.Resolve<CustomerSettings>();
            if (customerSettings.PasswordLifetime == 0)
                return false;

            //HTTP请求之间的缓存结果
            var cacheManager = EngineContext.Current.ContainerManager.Resolve<ICacheManager>("nop_cache_static");
            var cacheKey = string.Format(CustomerCacheEventConsumer.CUSTOMER_PASSWORD_LIFETIME, customer.Id);
            //获取当前密码使用时间
            var currentLifetime = cacheManager.Get(cacheKey, () =>
            {
                var customerPassword = EngineContext.Current.Resolve<ICustomerService>().GetCurrentPassword(customer.Id);
                //找不到密码，请返回最大值以强制客户更改密码
                if (customerPassword == null)
                    return int.MaxValue;

                return (DateTime.UtcNow - customerPassword.CreatedOnUtc).Days;
            });

            return currentLifetime >= customerSettings.PasswordLifetime;
        }
    }
}
