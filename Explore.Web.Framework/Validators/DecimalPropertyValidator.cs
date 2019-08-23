using Explore.Core.Domain.Directory;
using Explore.Services.Catalog;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Web.Framework.Validators
{
    public class DecimalPropertyValidator : PropertyValidator
    {
        private readonly decimal _maxValue;

        protected override bool IsValid(PropertyValidatorContext context)
        {
            decimal value;
            if (decimal.TryParse(context.PropertyValue.ToString(), out value))
            {
                return RoundingHelper.Round(value, RoundingType.Rounding001) < _maxValue;
            }
            return false;
        }

        public DecimalPropertyValidator(decimal maxValue) :
            base("Decimal value is out of range")
        {
            this._maxValue = maxValue;
        }
    }
}
