using ExigoService;
using System.Collections.Generic;

namespace ReplicatedSite.ViewModels
{
    public class ShoppingItemListViewModel : BootstrapColumnConfigViewModel
    {
        public List<Item> Items { get; set; }
    }
}