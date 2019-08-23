using Explore.Services.Security;
using Explore.Services.Users;
using Explore.Web.Factories;
using Explore.Web.Framework.Kendoui;
using Explore.Web.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Explore.Web.Controllers
{
    public class UsersController : BaseAdminController
    {
        #region Fields

        private readonly IPermissionService _permissionService;
        private readonly IUsersService _usersService;
        private readonly IUserModelFactory _userModelFactory;

        #endregion

        #region Ctor

        public UsersController(IPermissionService permissionService,
            IUsersService usersService,
            IUserModelFactory userModelFactory)
        {
            this._permissionService = permissionService;
            this._usersService = usersService;
            this._userModelFactory = userModelFactory;
        }

        #endregion


        #region UserList

        public virtual ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUser))
                return AccessDeniedView();

            var model = new CustomerListModel();

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult UserList(DataSourceRequest command, CustomerListModel model)
        {
            //我们使用自己的活页夹来搜索客户的错误属性
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUser))
                return AccessDeniedKendoGridJson();

            var users = _usersService.GetAllUsers(
                username: model.SearchUsername,
                nickname: model.SearchNickName,
                registerStartDate: model.SearchUserRegisterStartDate,
                registerEndDate: model.SearchUserRegisterEndDate,
                pageIndex: command.Page - 1,
                pageSize: command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = _userModelFactory.PrepareUserListModel(users),
                Total = users.TotalCount
            };

            return Json(gridModel);
        }

        #endregion
    }
}