using Explore.Web.Framework.MVC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FluentValidation.Attributes;
using Explore.Web.Validators.Customer;
using Explore.Web.Framework;

namespace Explore.Web.Models.Customer
{
    [Validator(typeof(LoginValidator))]
    public partial class LoginModel : BaseExploreModel
    {
        public bool CheckoutAsGuest { get; set; }

        public bool UsernamesEnabled { get; set; }
        [ExploreResourceDisplayName("Account.Login.Fields.UserName")]
        [AllowHtml]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [NoTrim]
        [ExploreResourceDisplayName("Account.Login.Fields.Password")]
        [AllowHtml]
        public string Password { get; set; }

        [ExploreResourceDisplayName("Account.Login.Fields.RememberMe")]
        public bool RememberMe { get; set; }

        public bool DisplayCaptcha { get; set; }
    }
}