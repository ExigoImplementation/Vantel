using Backoffice.Models;
using ExigoService;
using System.Collections.Generic;

namespace Backoffice.ViewModels
{
    public class OrderReviewViewModel : IShoppingViewModel
    {
        public IEnumerable<Item> Items { get; set; }
        public OrderCalculationResponse OrderTotals { get; set; }
        public IEnumerable<IShipMethod> ShipMethods { get; set; }
        public int ShipMethodID { get; set; }
        public ShoppingCartCheckoutPropertyBag PropertyBag { get; set; }
        public string[] Errors { get; set; }

        // Auto Order
        public OrderCalculationResponse AutoOrderTotals { get; set; }
        public IEnumerable<IShipMethod> AutoOrderShipMethods { get; set; }
    }
}