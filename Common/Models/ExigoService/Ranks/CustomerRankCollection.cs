namespace ExigoService
{
    public class CustomerRankCollection
    {
        public CustomerRankCollection()
        {
            this.CurrentPeriodRank          = Rank.Default;
            this.HighestPaidRankInAnyPeriod = Rank.Default;
            this.HighestPaidRankUpToPeriod  = Rank.Default;
        }

        public Rank CurrentPeriodRank { get; set; }
        public Rank HighestPaidRankInAnyPeriod { get; set; }
        public Rank HighestPaidRankUpToPeriod { get; set; }
    }
}