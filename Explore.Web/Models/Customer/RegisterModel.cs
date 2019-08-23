using Explore.Web.Framework;
using Explore.Web.Framework.MVC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Explore.Web.Models.Customer
{
    //[Validator(typeof(RegisterValidator))]
    public partial class RegisterModel : BaseExploreModel
    {
        public RegisterModel()
        {

        }

        public bool UsernamesEnabled { get; set; }

        [DisplayName("用户名")]
        [AllowHtml]
        public string Username { get; set; }

        public bool CheckUsernameAvailabilityEnabled { get; set; }

        [NoTrim]
        [ExploreResourceDisplayName("Account.Fields.ConfirmPassword")]
        [AllowHtml]
        public string Password { get; set; }

        [NoTrim]
        [DisplayName("再次输入密码")]
        [AllowHtml]
        public string ConfirmPassword { get; set; }

        //form fields & properties
        public bool GenderEnabled { get; set; }
        public string Gender { get; set; }
    }
}