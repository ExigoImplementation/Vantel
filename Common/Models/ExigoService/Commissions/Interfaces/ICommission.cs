namespace ExigoService
{
    public interface ICommission
    {
        int CustomerID { get; set; }
        string CurrencyCode { get; set; }
        decimal Total { get; set; }
        Period Period { get; set; }
    }
}