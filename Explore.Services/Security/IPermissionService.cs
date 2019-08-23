using Explore.Core.Domain.Customers;
using Explore.Core.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Security
{
    /// <summary>
    /// 权限服务接口
    /// </summary>
    public partial interface IPermissionService
    {
        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="permission">权限</param>
        void DeletePermissionRecord(PermissionRecord permission);

        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="permissionId">权限id</param>
        /// <returns>权限</returns>
        PermissionRecord GetPermissionRecordById(int permissionId);

        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="systemName">权限系统名</param>
        /// <returns>权限</returns>
        PermissionRecord GetPermissionRecordBySystemName(string systemName);

        /// <summary>
        /// 获取权限列表
        /// </summary>
        /// <returns>权限列表</returns>
        IList<PermissionRecord> GetAllPermissionRecords();

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="permission">权限</param>
        void InsertPermissionRecord(PermissionRecord permission);

        /// <summary>
        /// 更新权限
        /// </summary>
        /// <param name="permission">权限</param>
        void UpdatePermissionRecord(PermissionRecord permission);

        /// <summary>
        /// Install permissions
        /// </summary>
        /// <param name="permissionProvider">Permission provider</param>
        void InstallPermissions(IPermissionProvider permissionProvider);

        /// <summary>
        /// Uninstall permissions
        /// </summary>
        /// <param name="permissionProvider">Permission provider</param>
        void UninstallPermissions(IPermissionProvider permissionProvider);

        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="permission">权限记录</param>
        /// <returns>true - 授权; 否则, false</returns>
        bool Authorize(PermissionRecord permission);

        /// <summary>
        /// 权限授权
        /// </summary>
        /// <param name="permission">权限记录</param>
        /// <param name="customer">用户</param>
        /// <returns>true - 授权; 否则, false</returns>
        bool Authorize(PermissionRecord permission, Customer customer);

        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="permissionRecordSystemName">授权记录名称</param>
        /// <returns>true - 授权; 否则, false</returns>
        bool Authorize(string permissionRecordSystemName);

        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="permissionRecordSystemName">授权记录名称</param>
        /// <param name="customer">Customer</param>
        /// <returns>true - 授权; 否则, false</returns>
        bool Authorize(string permissionRecordSystemName, Customer customer);
    }
}
