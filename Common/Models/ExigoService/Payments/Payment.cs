using System;

namespace ExigoService
{
    public class Payment : IPayment
    {
        public int PaymentID { get; set; }
        public int CustomerID { get; set; }
        public int OrderID { get; set; }
        public int PaymentTypeID { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public int WarehouseID { get; set; }
        public string BillingName { get; set; }
        public int? CreditCardTypeID { get; set; }
        public string CreditCardNumber { get; set; }
        public string AuthorizationCode { get; set; }
        public string Memo { get; set; }

        public PaymentType PaymentType { get; set; }
    }
}
