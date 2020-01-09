using ExigoService;

namespace Common.Api.ExigoWebService
{
    public partial class ChargeCreditCardTokenRequest
    {
        public ChargeCreditCardTokenRequest() { }
        public ChargeCreditCardTokenRequest(CreditCard card)
        {
            CreditCardToken = card.Token;
            CvcCode         = card.CVV;

            BillingName     = card.NameOnCard;
            BillingAddress  = card.BillingAddress.AddressDisplay;
            BillingCity     = card.BillingAddress.City;
            BillingState    = card.BillingAddress.State;
            BillingZip      = card.BillingAddress.Zip;
            BillingCountry  = card.BillingAddress.Country;
        }

        public static explicit operator ChargeCreditCardTokenRequest(ExigoService.CreditCard card)
        {
            var model = new ChargeCreditCardTokenRequest();
            if (card == null) return model;

            model.CreditCardToken = card.Token;
            model.CvcCode         = card.CVV;

            model.BillingName     = card.NameOnCard;
            model.BillingAddress  = card.BillingAddress.Address1;
            model.BillingAddress2 = card.BillingAddress.Address2;
            model.BillingCity     = card.BillingAddress.City;
            model.BillingState    = card.BillingAddress.State;
            model.BillingZip      = card.BillingAddress.Zip;
            model.BillingCountry  = card.BillingAddress.Country;

            return model;
        }
    }
}