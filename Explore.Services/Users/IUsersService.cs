using Explore.Core;
using Explore.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Users
{
    /// <summary>
    /// 用户服务接口
    /// </summary>
    public partial interface IUsersService
    {
        /// <summary>
        /// 分页获取用户列表
        /// </summary>
        /// <param name="username"></param>
        /// <param name="nickname"></param>
        /// <param name="registerStartDate"></param>
        /// <param name="registerEndDate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<User> GetAllUsers(string username = "", string nickname = "",
            DateTime? registerStartDate = null, DateTime? registerEndDate = null,
            int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
