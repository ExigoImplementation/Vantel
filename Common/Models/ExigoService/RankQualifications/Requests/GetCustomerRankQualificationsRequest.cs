namespace ExigoService
{
    public class GetCustomerRankQualificationsRequest
    {
        public int CustomerID { get; set; }
        public int PeriodTypeID { get; set; }
        public int? RankID { get; set; }
    }
}