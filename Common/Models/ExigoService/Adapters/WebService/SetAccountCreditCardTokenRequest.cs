using ExigoService;

namespace Common.Api.ExigoWebService
{
    public partial class SetAccountCreditCardTokenRequest
    {
        public SetAccountCreditCardTokenRequest() { }        

        /// <summary>
        /// Sets up SetAccountCreditCardTokenRequest api request. NOTE: This automatically sets the credit card type as Primary by default.
        /// </summary>
        /// <param name="card">ExigoService.CreditCard</param>
        /// <returns>SetAccountCreditCardTokenRequest</returns>
        public SetAccountCreditCardTokenRequest(CreditCard card)
        {
            CreditCardToken       = card.Token;    //.GetToken();
            ExpirationMonth       = card.ExpirationMonth;
            ExpirationYear        = card.ExpirationYear;
            CreditCardAccountType = AccountCreditCardType.Primary;

            BillingName           = card.NameOnCard;
            BillingAddress        = card.BillingAddress.AddressDisplay;
            BillingCity           = card.BillingAddress.City;
            BillingState          = card.BillingAddress.State;
            BillingZip            = card.BillingAddress.Zip;
            BillingCountry        = card.BillingAddress.Country;
        }


        public static explicit operator SetAccountCreditCardTokenRequest(ExigoService.CreditCard card)
        {
            var model = new SetAccountCreditCardTokenRequest();
            if (card == null) return model;

            model.CreditCardToken       = card.Token;  //.GetToken();
            model.ExpirationMonth       = card.ExpirationMonth;
            model.ExpirationYear        = card.ExpirationYear;
            model.CreditCardAccountType = AccountCreditCardType.Primary;

            model.BillingName           = card.NameOnCard;
            model.BillingAddress        = card.BillingAddress.Address1;
            model.BillingAddress2       = card.BillingAddress.Address2;
            model.BillingCity           = card.BillingAddress.City;
            model.BillingState          = card.BillingAddress.State;
            model.BillingZip            = card.BillingAddress.Zip;
            model.BillingCountry        = card.BillingAddress.Country;

            return model;
        }
    }
}