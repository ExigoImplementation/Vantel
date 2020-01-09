using ExigoService;
using System.Collections.Generic;

namespace ReplicatedSite.ViewModels
{
    public class PacksViewModel
    {
        public IEnumerable<Item> OrderItems { get; set; }
        public IEnumerable<Item> AutoOrderItems { get; set; }

        public ShoppingCartItem SelectedOrderItem { get; set; }
        public ShoppingCartItem SelectedAutoOrderItem { get; set; }

        public int CustomerTypeID { get; set; }
    }
}