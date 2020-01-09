using System;

namespace ExigoService
{
    public interface ICreditCard : IPaymentMethod, IAutoOrderPaymentMethod
    {
        CreditCardType Type { get; set; }
        string NameOnCard { get; set; }
        string CardNumber { get; set; }
        int ExpirationMonth { get; set; }
        int ExpirationYear { get; set; }
        string CVV { get; set; }

        new int[] AutoOrderIDs { get; set; }

        Address BillingAddress { get; set; }

        string GetToken();
        DateTime ExpirationDate { get; }
        bool IsExpired { get; }


        string Token { get; set; }
        string Display { get; set; }
    }
}