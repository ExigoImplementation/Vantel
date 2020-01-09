namespace ExigoService
{
    public class GetCustomerRanksRequest
    {
        public int CustomerID { get; set; }
        public int PeriodTypeID { get; set; }
        public int? PeriodID { get; set; }
    }
}
