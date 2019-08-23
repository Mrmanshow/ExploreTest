using Explore.Core;
using Explore.Core.Data;
using Explore.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Users
{
    public partial class UsersService: IUsersService
    {
        #region Constants

        #endregion

        #region Fields

        private readonly IRepository<User> _userRepository;

        #endregion

        #region Ctor

        public UsersService(IRepository<User> userRepository)
        {
            this._userRepository = userRepository;
        }

        #endregion


        #region Users

        public virtual IPagedList<User> GetAllUsers(string username = "", string nickname = "",
            DateTime? registerStartDate = null, DateTime? registerEndDate = null,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _userRepository.Table;
            if (registerStartDate.HasValue)
                query = query.Where(c => registerStartDate < c.CreateTime);
            if (registerEndDate.HasValue)
            {
                registerEndDate = registerEndDate.Value.AddDays(1);
                query = query.Where(c => registerEndDate.Value <= c.CreateTime); 
            }
            if (!String.IsNullOrWhiteSpace(username))
                query = query
                    .Where(c => c.UserName.Contains(username));
            if (!String.IsNullOrWhiteSpace(nickname))
                query = query
                    .Where(c => c.NickName.Contains(nickname));

            query = query.OrderByDescending(c => c.CreateTime);

            var users = new PagedList<User>(query, pageIndex, pageSize);

            return users;
        }
        #endregion
    }
}
