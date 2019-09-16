using Explore.Core.Domain.Users;
using Explore.Web.Models.User;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Explore.Web.Extensions;
using Explore.Services.Localization;
using Explore.Core;

namespace Explore.Web.Factories
{
    public partial class UserModelFactory : IUserModelFactory
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public UserModelFactory(ILocalizationService localizationService,
            IWorkContext workContext)
        {
            this._localizationService = localizationService;
            this._workContext = workContext;
        }

        #endregion

        #region Methods

        public virtual IList<UserModel> PrepareUserListModel(IList<User> customer)
        {
            var list = new List<UserModel>();
            list = customer.Select(x =>
            {
                var userModel = x.ToModel();
                userModel.LoginType = x.UserRegisterType.GetLocalizedEnum(_localizationService, _workContext);
                return userModel;
            }).ToList();
     
            return list;
        }

        #endregion
    }
}