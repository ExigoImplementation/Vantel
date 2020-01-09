namespace ExigoService
{
    public class GetHistoricalCommissionBonusDetailsRequest : DataRequest
    {
        public GetHistoricalCommissionBonusDetailsRequest()
            : base()
        {
        }

        public int CommissionRunID { get; set; }
    }
}