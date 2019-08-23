using Explore.Core;
using Explore.Core.Caching;
using Explore.Core.Data;
using Explore.Core.Domain.Common;
using Explore.Core.Domain.Customers;
using Explore.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Customers
{
    /// <summary>
    /// Customer service
    /// </summary>
    public partial class CustomerService : ICustomerService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        private const string CUSTOMERROLES_ALL_KEY = "Nop.customerrole.all-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : system name
        /// </remarks>
        private const string CUSTOMERROLES_BY_SYSTEMNAME_KEY = "Nop.customerrole.systemname-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string CUSTOMERROLES_PATTERN_KEY = "Nop.customerrole.";

        #endregion

        #region Fields

        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<CustomerPassword> _customerPasswordRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<CustomerRole> _customerRoleRepository;

        #endregion

        #region Ctor

        public CustomerService(ICacheManager cacheManager, 
            IRepository<Customer> customerRepository,
            IRepository<CustomerPassword> customerPasswordRepository,
            IRepository<CustomerRole> customerRoleRepository)
        {
            this._customerRepository = customerRepository;
            this._customerPasswordRepository = customerPasswordRepository;
            this._customerRoleRepository = customerRoleRepository;
            this._cacheManager = cacheManager;
        }

        #endregion

        #region Methods

        #region Customers

        /// <summary>
        /// Gets a customer
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>A customer</returns>
        public virtual Customer GetCustomerById(int customerId)
        {
            if (customerId == 0)
                return null;

            return _customerRepository.GetById(customerId);
        }

        /// <summary>
        /// 根据GUID获取客户
        /// </summary>
        /// <param name="customerGuid">用户 GUID</param>
        /// <returns>用户对象</returns>
        public virtual Customer GetCustomerByGuid(Guid customerGuid)
        {
            if (customerGuid == Guid.Empty)
                return null;

            var query = from c in _customerRepository.Table
                        where c.CustomerGuid == customerGuid
                        orderby c.Id
                        select c;
            var customer = query.FirstOrDefault();
            return customer;
        }

        /// <summary>
        /// 按系统名称获取用户
        /// </summary>
        /// <param name="systemName">系统名称</param>
        /// <returns>用户对象</returns>
        public virtual Customer GetCustomerBySystemName(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName))
                return null;

            var query = from c in _customerRepository.Table
                        orderby c.Id
                        where c.SystemName == systemName
                        select c;
            var customer = query.FirstOrDefault();
            return customer;
        }

        /// <summary>
        /// 按用户名获取用户
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>用户对象</returns>
        public virtual Customer GetCustomerByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            var query = from c in _customerRepository.Table
                        orderby c.Id
                        where c.Username == username
                        select c;
            var customer = query.FirstOrDefault();
            return customer;
        }

        /// <summary>
        /// Gets all customer roles
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Customer roles</returns>
        public virtual IList<CustomerRole> GetAllCustomerRoles(bool showHidden = false)
        {
            string key = string.Format(CUSTOMERROLES_ALL_KEY, showHidden);
            return _cacheManager.Get(key, () =>
            {
                var query = from cr in _customerRoleRepository.Table
                            orderby cr.Name
                            where showHidden || cr.Active
                            select cr;
                var customerRoles = query.ToList();
                return customerRoles;
            });
        }

        /// <summary>
        /// 插入游客
        /// </summary>
        /// <returns>用户</returns>
        public virtual Customer InsertGuestCustomer()
        {
            var customer = new Customer
            {
                CustomerGuid = Guid.NewGuid(),
                Active = true,
                CreatedOnUtc = DateTime.UtcNow,
                LastActivityDateUtc = DateTime.UtcNow,
            };

            //添加'Guests'权限
            var guestRole = GetCustomerRoleBySystemName(SystemCustomerRoleNames.Guests);
            if (guestRole == null)
                throw new ExploreException("'Guests' role could not be loaded");
            customer.CustomerRoles.Add(guestRole);

            _customerRepository.Insert(customer);

            return customer;
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="customer">用户</param>
        public virtual void UpdateCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            _customerRepository.Update(customer);

            //event notification
            //_eventPublisher.EntityUpdated(customer);
        }


        #endregion


        #region Customer roles

        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <param name="systemName">用户权限系统名</param>
        /// <returns>用户权限</returns>
        public virtual CustomerRole GetCustomerRoleBySystemName(string systemName)
        {
            if (String.IsNullOrWhiteSpace(systemName))
                return null;

            string key = string.Format(CUSTOMERROLES_BY_SYSTEMNAME_KEY, systemName);
            return _cacheManager.Get(key, () =>
            {
                var query = from cr in _customerRoleRepository.Table
                            orderby cr.Id
                            where cr.SystemName == systemName
                            select cr;
                var customerRole = query.FirstOrDefault();
                return customerRole;
            });
        }

        #endregion

        #region Customer passwords

        /// <summary>
        /// 获取用户密码
        /// </summary>
        /// <param name="customerId">客户Id；传递空值以加载所有记录</param>
        /// <param name="passwordFormat">密码格式；传递空值以加载所有记录</param>
        /// <param name="passwordsToReturn">返回密码的数目；传递空值以加载所有记录</param>
        /// <returns>用户密码列表</returns>
        public virtual IList<CustomerPassword> GetCustomerPasswords(int? customerId = null,
            PasswordFormat? passwordFormat = null, int? passwordsToReturn = null)
        {
            var query = _customerPasswordRepository.Table;

            //过滤用户
            if (customerId.HasValue)
                query = query.Where(password => password.CustomerId == customerId.Value);

            //过滤密码格式
            if (passwordFormat.HasValue)
                query = query.Where(password => password.PasswordFormatId == (int)(passwordFormat.Value));

            //获取最新的密码
            if (passwordsToReturn.HasValue)
                query = query.OrderByDescending(password => password.CreatedOnUtc).Take(passwordsToReturn.Value);

            return query.ToList();
        }

        /// <summary>
        /// 获取当前用户密码
        /// </summary>
        /// <param name="customerId">用户Id</param>
        /// <returns>用户密码对象</returns>
        public virtual CustomerPassword GetCurrentPassword(int customerId)
        {
            if (customerId == 0)
                return null;

            //返回最新密码
            return GetCustomerPasswords(customerId, passwordsToReturn: 1).FirstOrDefault();
        }

        /// <summary>
        /// Insert a customer password
        /// </summary>
        /// <param name="customerPassword">Customer password</param>
        public virtual void InsertCustomerPassword(CustomerPassword customerPassword)
        {
            if (customerPassword == null)
                throw new ArgumentNullException("customerPassword");

            _customerPasswordRepository.Insert(customerPassword);

        }

        #endregion

        #endregion
    }
}
