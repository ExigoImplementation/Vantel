using ExigoService;
using System.Collections.Generic;

namespace Backoffice.ViewModels.Orders
{
    public class OrderListViewModel
    {
        public OrderListViewModel()
        {
            Orders = new List<Order>();
        }

        public int Count { get; set; }
        public List<Order> Orders { get; set; }
    }
}
