using Explore.Core.Domain.Users;
using Explore.Web.Models.User;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Explore.Web.Extensions;

namespace Explore.Web.Factories
{
    public partial class UserModelFactory : IUserModelFactory
    {
        #region Fields


        #endregion

        #region Ctor

        public UserModelFactory()
        {

        }

        #endregion

        #region Methods

        public virtual IList<UserModel> PrepareUserListModel(IList<User> customer)
        {
            var list = new List<UserModel>();
            foreach (var model in customer)
            {
                var userModel = model.ToModel();
                switch (model.LoginType)
                {
                    case 0:
                        userModel.LoginType = "手机";
                        break;
                    case 1:
                        userModel.LoginType = "FaceBook";
                        break;
                    case 2:
                        userModel.LoginType = "谷歌";
                        break;
                }
                list.Add(userModel);
            }

            return list;
        }

        #endregion
    }
}