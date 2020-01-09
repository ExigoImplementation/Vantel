using ExigoService;
using System;

namespace Common.Api.ExigoWebService
{

    public partial class OrderDetailRequest
    {
        public OrderDetailRequest() { }
        public OrderDetailRequest(IShoppingCartItem item)
        {
            this.ItemCode = item.ItemCode;
            this.Quantity = item.Quantity;
        }
    }
}