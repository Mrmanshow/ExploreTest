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
        public static readonly PermissionRecord ManageBanner = new PermissionRecord { Name = "Admin area. Manage Banner", SystemName = "ManageBanner", Category = "Banner" };
        public static readonly PermissionRecord ManageGameDailyStatistics = new PermissionRecord { Name = "Admin area. Manage Game Daily Statistics", SystemName = "ManageGameDailyStatistics", Category = "GameDailyStatistics" };
        public static readonly PermissionRecord ManageGameLaba = new PermissionRecord { Name = "Admin area. Manage Game Laba", SystemName = "ManageGameLaba", Category = "GameLaba" };
        public static readonly PermissionRecord ManageGameLabaList = new PermissionRecord { Name = "Admin area. Manage Game Laba List", SystemName = "ManageGameLabaList", Category = "GameLaba" };
        public static readonly PermissionRecord ManageGameLabaRoute = new PermissionRecord { Name = "Admin area. Manage Game Laba Route", SystemName = "ManageGameLabaRoute", Category = "GameLaba" };



        public virtual IEnumerable<PermissionRecord> GetPermissions()
        {
            return new[] 
            {
                AccessAdminPanel,
                ManageAdministrator,
                ManageUser,
                ManageBanner,
                ManageGameDailyStatistics,
                ManageGameLaba,
                ManageGameLabaList
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
                        ManageUser,
                        ManageBanner,
                        ManageGameDailyStatistics,
                        ManageGameLaba
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
