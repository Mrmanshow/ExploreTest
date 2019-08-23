using Explore.Core.Domain.Customers;
using Explore.Web.Framework.Validators;
using Explore.Web.Models.Customer;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Explore.Web.Validators.Customer
{
    public partial class LoginValidator : BaseExploreValidator<LoginModel>
    {
        public LoginValidator(CustomerSettings customerSettings)
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("请输入用户名");
            RuleFor(x => x.Password).NotEmpty().WithMessage("请输入密码");
        }
    }
}