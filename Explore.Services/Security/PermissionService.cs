using Explore.Core;
using Explore.Core.Caching;
using Explore.Core.Data;
using Explore.Core.Domain.Customers;
using Explore.Core.Domain.Security;
using Explore.Services.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Security
{
    /// <summary>
    /// Permission service
    /// </summary>
    public partial class PermissionService : IPermissionService
    {
        #region Constants
        /// <summary>
        /// 缓存密钥
        /// </summary>
        /// <remarks>
        /// {0} : 用户权限ID
        /// {1} : 权限系统名称
        /// </remarks>
        private const string PERMISSIONS_ALLOWED_KEY = "Nop.permission.allowed-{0}-{1}";
        /// <summary>
        /// 清除缓存的密钥模式
        /// </summary>
        private const string PERMISSIONS_PATTERN_KEY = "Nop.permission.";
        #endregion

        #region Fields

        private readonly IRepository<PermissionRecord> _permissionRecordRepository;
        private readonly ICustomerService _customerService;
        private readonly IWorkContext _workContext;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="permissionRecordRepository">权限仓储</param>
        /// <param name="customerService">用户服务</param>
        /// <param name="workContext">工作上下文</param>
        /// <param name="cacheManager">缓存服务</param>
        public PermissionService(IRepository<PermissionRecord> permissionRecordRepository,
            ICustomerService customerService,
            IWorkContext workContext,
            ICacheManager cacheManager)
        {
            this._permissionRecordRepository = permissionRecordRepository;
            this._customerService = customerService;
            this._workContext = workContext;
            this._cacheManager = cacheManager;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// 授权许可
        /// </summary>
        /// <param name="permissionRecordSystemName">权限记录系统名称</param>
        /// <param name="customerRole">用户权限</param>
        /// <returns>true - 授权; 否则, false</returns>
        protected virtual bool Authorize(string permissionRecordSystemName, CustomerRole customerRole)
        {
            if (String.IsNullOrEmpty(permissionRecordSystemName))
                return false;

            string key = string.Format(PERMISSIONS_ALLOWED_KEY, customerRole.Id, permissionRecordSystemName);
            return _cacheManager.Get(key, () =>
            {
                foreach (var permission1 in customerRole.PermissionRecords)
                    if (permission1.SystemName.Equals(permissionRecordSystemName, StringComparison.InvariantCultureIgnoreCase))
                        return true;

                return false;
            });
        }

        #endregion

        #region Methods

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="permission">权限</param>
        public virtual void DeletePermissionRecord(PermissionRecord permission)
        {
            if (permission == null)
                throw new ArgumentNullException("permission");

            _permissionRecordRepository.Delete(permission);

            _cacheManager.RemoveByPattern(PERMISSIONS_PATTERN_KEY);
        }

        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="permissionId">权限id</param>
        /// <returns>权限</returns>
        public virtual PermissionRecord GetPermissionRecordById(int permissionId)
        {
            if (permissionId == 0)
                return null;

            return _permissionRecordRepository.GetById(permissionId);
        }

        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="systemName">权限系统名</param>
        /// <returns>权限</returns>
        public virtual PermissionRecord GetPermissionRecordBySystemName(string systemName)
        {
            if (String.IsNullOrWhiteSpace(systemName))
                return null;

            var query = from pr in _permissionRecordRepository.Table
                        where pr.SystemName == systemName
                        orderby pr.Id
                        select pr;

            var permissionRecord = query.FirstOrDefault();
            return permissionRecord;
        }

        /// <summary>
        /// 获取权限列表
        /// </summary>
        /// <returns>权限列表</returns>
        public virtual IList<PermissionRecord> GetAllPermissionRecords()
        {
            var query = from pr in _permissionRecordRepository.Table
                        orderby pr.Name
                        select pr;
            var permissions = query.ToList();
            return permissions;
        }

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="permission">权限</param>
        public virtual void InsertPermissionRecord(PermissionRecord permission)
        {
            if (permission == null)
                throw new ArgumentNullException("permission");

            _permissionRecordRepository.Insert(permission);

            _cacheManager.RemoveByPattern(PERMISSIONS_PATTERN_KEY);
        }

        /// <summary>
        /// 更新权限
        /// </summary>
        /// <param name="permission">权限</param>
        public virtual void UpdatePermissionRecord(PermissionRecord permission)
        {
            if (permission == null)
                throw new ArgumentNullException("permission");

            _permissionRecordRepository.Update(permission);

            _cacheManager.RemoveByPattern(PERMISSIONS_PATTERN_KEY);
        }

        /// <summary>
        /// Install permissions
        /// </summary>
        /// <param name="permissionProvider">Permission provider</param>
        public virtual void InstallPermissions(IPermissionProvider permissionProvider)
        {
            //install new permissions
            var permissions = permissionProvider.GetPermissions();
            foreach (var permission in permissions)
            {
                var permission1 = GetPermissionRecordBySystemName(permission.SystemName);
                if (permission1 == null)
                {
                    //new permission (install it)
                    permission1 = new PermissionRecord
                    {
                        Name = permission.Name,
                        SystemName = permission.SystemName,
                        Category = permission.Category,
                    };


                    //default customer role mappings
                    var defaultPermissions = permissionProvider.GetDefaultPermissions();
                    //foreach (var defaultPermission in defaultPermissions)
                    //{
                    //    var customerRole = _customerService.GetCustomerRoleBySystemName(defaultPermission.CustomerRoleSystemName);
                    //    if (customerRole == null)
                    //    {
                    //        //new role (save it)
                    //        customerRole = new CustomerRole
                    //        {
                    //            Name = defaultPermission.CustomerRoleSystemName,
                    //            Active = true,
                    //            SystemName = defaultPermission.CustomerRoleSystemName
                    //        };
                    //        _customerService.InsertCustomerRole(customerRole);
                    //    }


                    //    var defaultMappingProvided = (from p in defaultPermission.PermissionRecords
                    //                                  where p.SystemName == permission1.SystemName
                    //                                  select p).Any();
                    //    var mappingExists = (from p in customerRole.PermissionRecords
                    //                         where p.SystemName == permission1.SystemName
                    //                         select p).Any();
                    //    if (defaultMappingProvided && !mappingExists)
                    //    {
                    //        permission1.CustomerRoles.Add(customerRole);
                    //    }
                    //}

                    //save new permission
                    InsertPermissionRecord(permission1);

                    //save localization
                    //permission1.SaveLocalizedPermissionName(_localizationService, _languageService);
                }
            }
        }

        /// <summary>
        /// Uninstall permissions
        /// </summary>
        /// <param name="permissionProvider">Permission provider</param>
        public virtual void UninstallPermissions(IPermissionProvider permissionProvider)
        {
            var permissions = permissionProvider.GetPermissions();
            foreach (var permission in permissions)
            {
                var permission1 = GetPermissionRecordBySystemName(permission.SystemName);
                if (permission1 != null)
                {
                    DeletePermissionRecord(permission1);

                    //delete permission locales
                    //permission1.DeleteLocalizedPermissionName(_localizationService, _languageService);
                }
            }

        }

        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="permission">权限记录</param>
        /// <returns>true - 授权; 否则, false</returns>
        public virtual bool Authorize(PermissionRecord permission)
        {
            return Authorize(permission, _workContext.CurrentCustomer);
        }

        /// <summary>
        /// 权限授权
        /// </summary>
        /// <param name="permission">权限记录</param>
        /// <param name="customer">用户</param>
        /// <returns>true - 授权; 否则, false</returns>
        public virtual bool Authorize(PermissionRecord permission, Customer customer)
        {
            if (permission == null)
                return false;

            if (customer == null)
                return false;

            //old implementation of Authorize method
            //var customerRoles = customer.CustomerRoles.Where(cr => cr.Active);
            //foreach (var role in customerRoles)
            //    foreach (var permission1 in role.PermissionRecords)
            //        if (permission1.SystemName.Equals(permission.SystemName, StringComparison.InvariantCultureIgnoreCase))
            //            return true;

            //return false;

            return Authorize(permission.SystemName, customer);
        }

        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="permissionRecordSystemName">授权记录名称</param>
        /// <returns>true - 授权; 否则, false</returns>
        public virtual bool Authorize(string permissionRecordSystemName)
        {
            return Authorize(permissionRecordSystemName, _workContext.CurrentCustomer);
        }

        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="permissionRecordSystemName">授权记录名称</param>
        /// <param name="customer">Customer</param>
        /// <returns>true - 授权; 否则, false</returns>
        public virtual bool Authorize(string permissionRecordSystemName, Customer customer)
        {
            if (String.IsNullOrEmpty(permissionRecordSystemName))
                return false;

            var customerRoles = customer.CustomerRoles.Where(cr => cr.Active);
            foreach (var role in customerRoles)
                if (Authorize(permissionRecordSystemName, role))
                    //是的，我们有这样的许可
                    return true;

            //未找到权限
            return false;
        }

        #endregion
    }
}
