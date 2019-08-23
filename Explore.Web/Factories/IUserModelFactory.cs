using Explore.Core.Domain.Users;
using Explore.Web.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Explore.Web.Factories
{
    public partial interface IUserModelFactory
    {
        IList<UserModel> PrepareUserListModel(IList<User> customer);
    }
}