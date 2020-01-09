namespace Common.Api.ExigoWebService
{
    public partial class CommissionResponse
    {
        public static explicit operator ExigoService.RealTimeCommission(CommissionResponse commission)
        {
            var model = new ExigoService.RealTimeCommission();
            if (commission == null) return model;

            model.CustomerID               = commission.CustomerID;
            model.CurrencyCode             = commission.CurrencyCode;
            model.Total                    = commission.CommissionTotal;

            model.Period                   = new ExigoService.Period();
            model.Period.PeriodTypeID      = commission.PeriodType;
            model.Period.PeriodID          = commission.PeriodID;
            model.Period.PeriodDescription = commission.PeriodDescription;

            return model;
        }
    }
}