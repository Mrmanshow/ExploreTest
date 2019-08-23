using Explore.Web.Framework.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Explore.Web.Models.User
{
    public partial class UserModel : BaseExploreEntityModel
    {
        public string UserName { set; get; }

        public string NickName { set; get; }

        public string LoginType { set; get; }

        public int GoldCoin { set; get; }

        public string CreateTime { set; get; }

    }
}