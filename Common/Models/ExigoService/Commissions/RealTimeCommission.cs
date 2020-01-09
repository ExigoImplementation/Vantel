namespace ExigoService
{
    public class RealTimeCommission : ICommission
    {
        public int CustomerID { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Total { get; set; }
        public Rank PaidRank { get; set; }
        public Period Period { get; set; }

        public VolumeCollection Volumes { get; set; }
    }
}