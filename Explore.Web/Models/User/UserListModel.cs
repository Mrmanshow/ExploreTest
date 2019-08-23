using Explore.Web.Framework;
using Explore.Web.Framework.MVC;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Explore.Web.Models.User
{
    public partial class CustomerListModel : BaseExploreModel
    {
        [ExploreResourceDisplayName("Admin.Users.List.RegisterStartDate")]
        [UIHint("DateNullable")]
        public DateTime? SearchUserRegisterStartDate { get; set; }

        [ExploreResourceDisplayName("Admin.Users.List.RegisterEndDate")]
        [UIHint("DateNullable")]
        public DateTime? SearchUserRegisterEndDate { get; set; }

        [ExploreResourceDisplayName("Admin.Users.List.SearchUsername")]
        public string SearchUsername { get; set; }

        [ExploreResourceDisplayName("Admin.Users.List.SearchNickName")]
        public string SearchNickName { get; set; }
    }
}