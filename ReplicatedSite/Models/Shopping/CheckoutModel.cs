using ExigoService;

namespace ReplicatedSite.Models
{
    public class CheckoutModel
    {
        public ShippingAddress ShippingAddress { get; set; }
        public int ShipMethodID { get; set; }
        public bool BillingAddressSameAsShipping { get; set; }
        public CreditCard CreditCardPayment { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }

    }
}