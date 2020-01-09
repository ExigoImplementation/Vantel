using Backoffice.Models;
using ExigoService;
using System.Collections.Generic;

namespace Backoffice.ViewModels
{
    public class ShippingAddressesViewModel : IShoppingViewModel
    {
        public IEnumerable<ShippingAddress> Addresses { get; set; }
        public ShoppingCartCheckoutPropertyBag PropertyBag { get; set; }
        public string[] Errors { get; set; }
    }
}