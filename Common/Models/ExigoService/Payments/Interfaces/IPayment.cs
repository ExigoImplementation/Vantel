using System;

namespace ExigoService
{
    public interface IPayment
    {
        int PaymentID { get; set; }
        int CustomerID { get; set; }
        int OrderID { get; set; }
        int PaymentTypeID { get; set; }
        DateTime PaymentDate { get; set; }
        decimal Amount { get; set; }
        string CurrencyCode { get; set; }
        int WarehouseID { get; set; }
        string BillingName { get; set; }
        int? CreditCardTypeID { get; set; }
        string CreditCardNumber { get; set; }
        string AuthorizationCode { get; set; }
        string Memo { get; set; }

        PaymentType PaymentType { get; set; }
    }
}
