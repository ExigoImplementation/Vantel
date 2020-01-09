using Backoffice.Models;
using Common;
using Common.Providers;
using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Backoffice.Providers
{
    public class ShoppingCartLogicProvider : BaseLogicProvider
    {
        #region Constructors
        public ShoppingCartLogicProvider() : base() {  }
        public ShoppingCartLogicProvider(Controller controller, ShoppingCartItemsPropertyBag cart, ShoppingCartCheckoutPropertyBag propertyBag)
        {
            Controller  = controller;
            Cart        = cart;
            PropertyBag = propertyBag;
        }
        #endregion

        #region Properties
        public ShoppingCartItemsPropertyBag Cart { get; set; }
        public ShoppingCartCheckoutPropertyBag PropertyBag { get; set; }
        #endregion 

        #region Logic
        public override CheckLogicResult CheckLogic()
        {
            if (!HasValidOrderDetails(Cart.Items))
            {
                return CheckLogicResult.Failure(RedirectToAction("Cart"));
            }

            if (!HasValidShippingAddress(PropertyBag.ShippingAddress))
            {
                return CheckLogicResult.Failure(RedirectToAction("Shipping"));
            }

            if (!HasValidAutoOrderDetails(Cart.Items))
            {
                return CheckLogicResult.Failure(RedirectToAction("AutoOrder"));
            }

            if (!HasValidPaymentMethod(PropertyBag.PaymentMethod))
            {
                return CheckLogicResult.Failure(RedirectToAction("Payment"));
            }

            return CheckLogicResult.Success(RedirectToAction("Review"));
        }   
        
        public bool HasValidOrderDetails(IEnumerable<IShoppingCartItem> items)
        {
            return items.Count() > 0;
        }        
        public bool HasAutoOrderItems(IEnumerable<IShoppingCartItem> items)
        {
            return items.Any(c => c.Type == ShoppingCartItemType.AutoOrder);
        }
        public bool HasValidAutoOrderDetails(IEnumerable<IShoppingCartItem> items)
        {
            if (!HasAutoOrderItems(items)) return true;

            return PropertyBag.AutoOrderStartDate >= DateTime.Now.BeginningOfDay();
        }
        public bool HasValidShippingAddress(ShippingAddress address)
        {
            return address != null && address.IsComplete;
        }        
        public bool HasValidPaymentMethod(IPaymentMethod paymentMethod)
        {
            return paymentMethod != null &&
                (paymentMethod is CreditCard || paymentMethod is BankAccount);
        }
        #endregion
    }
}