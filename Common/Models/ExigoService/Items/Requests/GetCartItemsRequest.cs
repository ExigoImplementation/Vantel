using System.Collections.Generic;

namespace ExigoService
{
    public class GetCartItemsRequest
    {
        public IOrderConfiguration Configuration { get; set; }
        public IEnumerable<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}