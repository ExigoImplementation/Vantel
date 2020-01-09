using System;
using ExigoService;
using Common.Api.ExigoWebService;

namespace Backoffice.Models
{
    public class ShoppingCartCheckoutPropertyBag : BasePropertyBag
    {
        private string version = "2.0.0";
        private int expires    = 31;



        #region Constructors
        public ShoppingCartCheckoutPropertyBag()
        {
            CustomerID = Identity.Current.CustomerID;
            Expires = expires;
        }
        #endregion

        #region Shared Properties
        public int CustomerID { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public int ShipMethodID { get; set; }
        public IPaymentMethod PaymentMethod { get; set; }
        public bool IsSubmitting { get; set; }
        public int NewOrderID { get; set; }
        public string OrderException { get; set; }
        public int PartyID { get; set; }
        public int HostID { get; set; }
        public int GuestID { get; set; }

        #region Shopping With Auto Order Only Properties
        public int NewAutoOrderID { get; set; }
        public FrequencyType AutoOrderFrequencyType { get; set; }
        public DateTime AutoOrderStartDate { get; set; }
        public ShippingAddress AutoOrderShippingAddress { get; set; }
        public ShippingAddress AutoOrderBillingAddress { get; set; }
        public IPaymentMethod AutoOrderPaymentMethod { get; set; }
        public bool AutoOrderBillingSameAsShipping { get; set; }
        public int AutoOrderShipMethodID { get; set; }
        #endregion

        #endregion

        #region Methods
        public override T OnBeforeUpdate<T>(T propertyBag)
        {
            propertyBag.Version = version;

            return propertyBag;
        }
        public override bool IsValid()
        {
            var currentCustomerID = Identity.Current.CustomerID;
            return this.Version == version && this.CustomerID == currentCustomerID;
        }
        public bool HasValidParty()
        {
            return this.PartyID > 0;
        }
        public bool IsHostessOrder()
        {
            return this.HostID > 0;
        }
        public bool IsGuestOrder()
        {
            return this.GuestID > 0;
        }
        #endregion
    }
}