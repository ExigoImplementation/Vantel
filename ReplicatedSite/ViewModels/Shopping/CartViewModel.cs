using ReplicatedSite.Models;
using ExigoService;
using System.Collections.Generic;

namespace ReplicatedSite.ViewModels
{
    public class CartViewModel : IShoppingViewModel
    {
        public IEnumerable<Item> OrderItems { get; set; }
        public IEnumerable<Item> AutoOrderItems { get; set; }

        public ShoppingCartCheckoutPropertyBag PropertyBag { get; set; }
        public string[] Errors { get; set; }
    }
}