namespace ExigoService
{
    public interface ICommissionBonusDetail
    {
        int BonusID { get; set; }
        string BonusDescription { get; set; }
        int FromCustomerID { get; set; }
        string FromCustomerName { get; set; }
        int OrderID { get; set; }
        int Level { get; set; }
        int PaidLevel { get; set; }
        decimal SourceAmount { get; set; }
        decimal Percentage { get; set; }
        decimal CommissionAmount { get; set; }
    }
}