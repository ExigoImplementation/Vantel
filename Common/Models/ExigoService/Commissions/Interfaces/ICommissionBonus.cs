namespace ExigoService
{
    public interface ICommissionBonus
    {
        int BonusID { get; set; }
        string BonusDescription { get; set; }
        decimal Total { get; set; }
    }
}