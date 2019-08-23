using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Web.Framework.Validators
{
    public static class MyValidatorExtensions
    {
        //public static IRuleBuilderOptions<T, string> IsCreditCard<T>(this IRuleBuilder<T, string> ruleBuilder)
        //{
        //    return ruleBuilder.SetValidator(new CreditCardPropertyValidator());
        //}

        public static IRuleBuilderOptions<T, decimal> IsDecimal<T>(this IRuleBuilder<T, decimal> ruleBuilder, decimal maxValue)
        {
            return ruleBuilder.SetValidator(new DecimalPropertyValidator(maxValue));
        }
    }
}
