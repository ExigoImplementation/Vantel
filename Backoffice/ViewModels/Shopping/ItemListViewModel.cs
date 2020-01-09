using Backoffice.Models;
using ExigoService;
using System.Collections.Generic;

namespace Backoffice.ViewModels
{
    public class ItemListViewModel : IShoppingViewModel
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<IItem> Items { get; set; }
        public IEnumerable<WebCategory> Categories { get; set; }
        public int Page { get; set; }
        public int RecordCount { get; set; }
        public ShoppingCartCheckoutPropertyBag PropertyBag { get; set; }
        public string[] Errors { get; set; }
    }
}