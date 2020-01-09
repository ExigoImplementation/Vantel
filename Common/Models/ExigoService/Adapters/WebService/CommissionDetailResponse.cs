namespace Common.Api.ExigoWebService
{
    public partial class CommissionDetailResponse
    {
        public static explicit operator ExigoService.CommissionBonusDetail(CommissionDetailResponse detail)
        {
            var model = new ExigoService.CommissionBonusDetail();
            if (detail == null) return model;

            model.FromCustomerID   = detail.FromCustomerID;
            model.FromCustomerName = detail.FromCustomerName;
            model.OrderID          = detail.OrderID;
            model.Level            = detail.Level;
            model.PaidLevel        = detail.PaidLevel;
            model.SourceAmount     = detail.SourceAmount;
            model.Percentage       = detail.Percentage;
            model.CommissionAmount = detail.CommissionAmount;

            return model;
        }
    }
}