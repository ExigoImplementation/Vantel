using ExigoService;

namespace Common.Api.ExigoWebService
{
    public partial class ChargeCreditCardTokenOnFileRequest
    {
        public static explicit operator ChargeCreditCardTokenOnFileRequest(ExigoService.CreditCard card)
        {
            var model = new ChargeCreditCardTokenOnFileRequest();
            if (card == null) return model;

            model.CreditCardAccountType = card.Type.ToAccountCreditCardType();
            model.CvcCode               = card.CVV;

            return model;
        }
    }

    public partial class ChargeCreditCardTokenOnFileRequest
    {
        public ChargeCreditCardTokenOnFileRequest() { }
        public ChargeCreditCardTokenOnFileRequest(CreditCard card)
        {
            CreditCardAccountType = card.Type.ToAccountCreditCardType();
            CvcCode               = card.CVV;
        }
    }
}