using ExigoService;
using System;

namespace ReplicatedSite.ViewModels
{
    public class OrderCompleteViewModel
    {
        public OrderCompleteViewModel() { }
        public OrderCompleteViewModel(Order order)
        {
            Order = order;
        }

        public Order Order { get; set; }
        public AutoOrder AutoOrder { get; set; }

        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }
        public ShippingAddress Recipient { get; set; }
        public string Token { get; set; }
    }
}