namespace ExigoService
{
    public class CommissionBonusDetail : ICommissionBonusDetail
    {
        public int BonusID { get; set; }
        public string BonusDescription { get; set; }
        public int FromCustomerID { get; set; }
        public string FromCustomerName { get; set; }
        public int OrderID { get; set; }
        public int Level { get; set; }
        public int PaidLevel { get; set; }
        public decimal SourceAmount { get; set; }
        public decimal Percentage { get; set; }
        public decimal CommissionAmount { get; set; }
    }
}