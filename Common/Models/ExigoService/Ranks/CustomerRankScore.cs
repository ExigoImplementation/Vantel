namespace ExigoService
{
    public class CustomerRankScore : ICustomerRankScore
    {
        public CustomerRankScore()
        {
            this.Rank = Rank.Default;
        }

        public int CustomerID { get; set; }
        public Rank Rank { get; set; }
        public decimal RankScore { get; set; }
        public decimal TotalScore { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
    }
}