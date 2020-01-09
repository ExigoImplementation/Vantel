using Common.Models.ShoppingService.Interfaces;
using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Common.Services
{
    public class ShoppingService: IItemConverterService, IDisposable
    {
        public ShoppingService(IOrderConfiguration orderConfiguration)
        {
            this._OrderConfiguration = orderConfiguration;
        }
        public IOrderConfiguration _OrderConfiguration { get; private set; }

        private IItemConverterService _ItemConverterService { get; set; }
        private IItemConverterService ItemConverterService { get
            {
                if (this._ItemConverterService == null)
                {
                    this._ItemConverterService = new ItemConverterService(this._OrderConfiguration);
                }
                return this._ItemConverterService;
            }
        }


        /// <summary>
        /// Convert items and children into cart items
        /// </summary>
        /// <param name="parentItem"></param>
        /// <param name="parentQuantity"></param>
        /// <param name="childItems"></param>
        /// <returns></returns>
        public ShoppingCartItem GetShoppingCartItem(string parentItemCode, int parentQuantity, Dictionary<string, int> childItems = null)
        {
            return ItemConverterService.GetShoppingCartItem(parentItemCode, parentQuantity, childItems);
        }

        /// <summary>
        /// Convert ShoppingCartItems into OrderDetail Requests
        /// </summary>
        /// <param name="cartItems"></param>
        /// <returns></returns>
        public List<Api.ExigoWebService.OrderDetailRequest> GetOrderDetails(List<IShoppingCartItem> cartItems)
        {
            return ItemConverterService.GetOrderDetails(cartItems);
        }



        public void Dispose()
        {
            if (this._OrderConfiguration != null) { this._OrderConfiguration = null; }
            if (this._ItemConverterService != null) { this._ItemConverterService.Dispose(); }
        }
    }
}