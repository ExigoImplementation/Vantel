using Backoffice.Models;
using ExigoService;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Common;

namespace Backoffice.ViewModels
{
    public class PaymentMethodsViewModel : IShoppingViewModel
    {
        public IEnumerable<IPaymentMethod> PaymentMethods { get; set; }
        public IEnumerable<Address> Addresses { get; set; }
        public ShoppingCartCheckoutPropertyBag PropertyBag { get; set; }

        [Required, Display(Name = "CVV", ResourceType = typeof(Common.Resources.Models)), RegularExpression(GlobalSettings.RegularExpressions.CVV, ErrorMessageResourceName = "IncorrectCVV", ErrorMessageResourceType = typeof(Common.Resources.Models))]
        public string ExistingCardCVV { get; set; }

        public string[] Errors { get; set; }
    }
}