using Backoffice.Models;
using ExigoService;
using System.Collections.Generic;

namespace Backoffice.ViewModels
{
    public class CartViewModel : IShoppingViewModel
    {
        public IEnumerable<Item> OrderItems { get; set; }
        public IEnumerable<Item> AutoOrderItems { get; set; }

        public ShoppingCartCheckoutPropertyBag PropertyBag { get; set; }
        public string[] Errors { get; set; }
    }
}