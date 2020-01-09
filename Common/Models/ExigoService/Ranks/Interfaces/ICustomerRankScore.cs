namespace ExigoService
{
    public interface ICustomerRankScore
    {
        int CustomerID { get; set; }
        Rank Rank { get; set; }
        decimal RankScore { get; set; }
        decimal TotalScore { get; set; }
    }
}