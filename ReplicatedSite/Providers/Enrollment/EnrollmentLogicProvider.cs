using ReplicatedSite.Models;
using Common.Providers;
using ExigoService;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Common;

namespace ReplicatedSite.Providers
{
    public class EnrollmentLogicProvider : BaseLogicProvider
    {
        #region Constructors
        public EnrollmentLogicProvider() : base() { }
        public EnrollmentLogicProvider(Controller controller, ShoppingCartItemsPropertyBag cart, EnrollmentPropertyBag propertyBag)
        {
            Controller = controller;
            Cart = cart;
            PropertyBag = propertyBag;
        }
        #endregion

        #region Properties
        public ShoppingCartItemsPropertyBag Cart { get; set; }
        public EnrollmentPropertyBag PropertyBag { get; set; }
        #endregion

        #region Logic
        public override CheckLogicResult CheckLogic()
        {
            if (!HasValidConfiguration())
            {
                return CheckLogicResult.Failure(RedirectToAction("EnrollmentConfiguration"));
            }

            if (!HasValidPackDetails(Cart.Items))
            {
                return CheckLogicResult.Failure(RedirectToAction("Packs"));
            }

            //if (!HasValidOrderDetails(Cart.Items))
            //{
            //    return CheckLogicResult.Failure(RedirectToAction("ProductList"));
            //}

            if (!HasValidShippingAddress(PropertyBag.ShippingAddress))
            {
                return CheckLogicResult.Failure(RedirectToAction("EnrolleeInfo"));
            }

            if (!HasValidPaymentMethod(PropertyBag.PaymentMethod))
            {
                return CheckLogicResult.Failure(RedirectToAction("EnrolleeInfo"));
            }

            return CheckLogicResult.Success(RedirectToAction("Review"));
        }

        public bool HasValidConfiguration()
        {
            // If the enroller ID hasn't been set the perform additional checks
            if(PropertyBag.EnrollerID == 0)
            {
                var siteOwnerID = Identity.Owner.CustomerID;

                // If there is more than one market and/or the customer is on the orphan account make sure they go to the page
                if (GlobalSettings.Markets.AvailableMarkets.Count > 1 || siteOwnerID == GlobalSettings.ReplicatedSites.DefaultAccountID)
                {
                    return false;
                }                
                else
                {
                    // The customer is not visiting the corporate site and there is only one market, set some default values so the customer will skip this page
                    PropertyBag.EnrollerID = siteOwnerID;
                    PropertyBag.SelectedMarket = GlobalSettings.Markets.AvailableMarkets.FirstOrDefault().Name;
                    ExigoDAL.PropertyBags.Update(PropertyBag);

                    return true;
                }
            }

            return true;
        }
        public bool HasValidPackDetails(IEnumerable<IShoppingCartItem> items)
        {
            return items.Where(c => c.Type == ShoppingCartItemType.EnrollmentPack || c.Type == ShoppingCartItemType.EnrollmentAutoOrderPack).Count() > 0;
        }
        //public bool HasValidOrderDetails(IEnumerable<IShoppingCartItem> items)
        //{
        //    return items.Where(c => c.Type == ShoppingCartItemType.Order || c.Type == ShoppingCartItemType.AutoOrder).Count() > 0;
        //}
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