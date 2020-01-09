using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Common.Filters
{
    public class LuhnAttribute : ValidationAttribute, IClientValidatable
    {
        public bool AllowEmpty { get; set; }

        public override bool IsValid(object value)
        {
            string cardNumber = (string)value;

            if (string.IsNullOrEmpty(cardNumber))
            {
                return AllowEmpty;
            }

            return GlobalUtilities.ValidateCreditCard(cardNumber);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new LuhnRule(ErrorMessage, AllowEmpty);
        }

        public class LuhnRule : ModelClientValidationRule
        {
            public LuhnRule(string errorMessage, bool allowEmpty)
            {
                ErrorMessage = errorMessage;
                ValidationType = "luhn";

                ValidationParameters["allowempty"] = allowEmpty;
            }
        }
    }
}
