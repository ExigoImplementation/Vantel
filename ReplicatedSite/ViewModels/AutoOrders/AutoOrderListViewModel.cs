using ExigoService;
using System.Collections.Generic;

namespace ReplicatedSite.ViewModels
{
    public class AutoOrderListViewModel
    {
        public AutoOrderListViewModel()
        {
            AutoOrders = new List<IAutoOrder>();
        }

        public IEnumerable<IAutoOrder> AutoOrders { get; set; }
    }
}