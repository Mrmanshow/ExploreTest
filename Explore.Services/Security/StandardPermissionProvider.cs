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
    /// Standard permission provider
    /// </summary>
    public partial class StandardPermissionProvider : IPermissionProvider
    {
        //admin area permissions
        public static readonly PermissionRecord AccessAdminPanel = new PermissionRecord { Name = "Access admin area", SystemName = "Admin", Category = "Standard" };
        public static readonly PermissionRecord ManageAdministrator = new PermissionRecord { Name = "Admin area. Manage Administrator", SystemName = "ManageAdministrator", Category = "Administrator" };
        public static readonly PermissionRecord ManageUser = new PermissionRecord { Name = "Admin area. Manage User", SystemName = "ManageUser", Category = "User" };


        public virtual IEnumerable<PermissionRecord> GetPermissions()
        {
            return new[] 
            {
                AccessAdminPanel,
                ManageAdministrator,
                ManageUser
            };
        }

        public virtual IEnumerable<DefaultPermissionRecord> GetDefaultPermissions()
        {
            return new[] 
            {
                new DefaultPermissionRecord 
                {
                    CustomerRoleSystemName = SystemCustomerRoleNames.Administrators,
                    PermissionRecords = new[] 
                    {
                        AccessAdminPanel,
                        ManageAdministrator,
                        ManageUser
                    }
                },
                new DefaultPermissionRecord 
                {
                    CustomerRoleSystemName = SystemCustomerRoleNames.Guests,
                    //PermissionRecords = new[] 
                    //{
                    //    AccessAdminPanel,
                    //    EnableShoppingCart,
                    //    EnableWishlist,
                    //    PublicStoreAllowNavigation
                    //}
                }
            };
        }
    }
}
