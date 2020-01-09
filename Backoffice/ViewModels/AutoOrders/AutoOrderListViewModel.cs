using ExigoService;
using System.Collections.Generic;

namespace Backoffice.ViewModels
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