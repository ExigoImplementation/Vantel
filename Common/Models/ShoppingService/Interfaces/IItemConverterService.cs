using System;
using System.Collections.Generic;

namespace Common.Models.ShoppingService.Interfaces
{
    interface IItemConverterService : IDisposable
    {
        ExigoService.IOrderConfiguration _OrderConfiguration { get; }
        /// <summary>
        /// Convert items and children into cart items
        /// </summary>
        /// <param name="parentItemCode"></param>
        /// <param name="parentQuantity"></param>
        /// <param name="childItems"></param>
        /// <returns></returns>
        ExigoService.ShoppingCartItem GetShoppingCartItem(string parentItemCode, int parentQuantity, Dictionary<string,int> childItems = null);

        /// <summary>
        /// Convert ShoppingCartItems into OrderDetail Requests
        /// </summary>
        /// <param name="cartItems"></param>
        /// <returns></returns>
        List<Api.ExigoWebService.OrderDetailRequest> GetOrderDetails(List<ExigoService.IShoppingCartItem> cartItems);
    }
}
