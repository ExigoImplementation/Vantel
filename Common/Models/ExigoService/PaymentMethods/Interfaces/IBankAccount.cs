namespace ExigoService
{
    public interface IBankAccount : IPaymentMethod, IAutoOrderPaymentMethod
    {
        BankAccountType Type { get; set; }

        string NameOnAccount { get; set; }
        string BankName { get; set; }
        string AccountNumber { get; set; }
        string RoutingNumber { get; set; }
        Address BillingAddress { get; set; }

        new int[] AutoOrderIDs { get; set; }
    }

    public enum BankAccountType
    {
        New = 0,
        Primary = 1
    }
}