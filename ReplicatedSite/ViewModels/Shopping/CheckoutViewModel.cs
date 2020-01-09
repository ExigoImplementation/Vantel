using ExigoService;
using ReplicatedSite.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReplicatedSite.ViewModels
{
    public class CheckoutViewModel
    {
        public CheckoutViewModel()
        {
            ShippingAddress = ShippingAddress == null ? new ShippingAddress() : ShippingAddress;
            CreditCardPayment = CreditCardPayment == null ? new CreditCard() : CreditCardPayment;
        }
        public CheckoutViewModel(ShoppingCartCheckoutPropertyBag propertyBag)
        {
            ShippingAddress = propertyBag.ShippingAddress;
            CreditCardPayment = propertyBag.PaymentMethod as CreditCard;
            ShipMethodID = propertyBag.ShipMethodID;
            BillingAddressSameAsShipping = propertyBag.BillingAddressSameAsShipping;
        }
        
        public OrderCalculationResponse OrderCalculationResponse { get; set; }
        
        public OrderCalculationResponse AutoOrderCalculationResponse { get; set; }

        public decimal SubTotal { get; set; }
        public string DeliveryType { get; set; }
        public decimal Total { get; set; }

        public ShippingAddress ShippingAddress { get; set; }
        public List<ShipMethod> ShipMethods { get; set; }
        public int ShipMethodID { get; set; }

        public bool BillingAddressSameAsShipping { get; set; }

        public CreditCard CreditCardPayment { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }

        public IEnumerable<Item> ShoppingCartItems { get; set; }
        public IEnumerable<Item> AutoOrderShoppingCartItems { get; set; }
        
        public DateTime AutoOrderStartDate { get; set; }
    }
}