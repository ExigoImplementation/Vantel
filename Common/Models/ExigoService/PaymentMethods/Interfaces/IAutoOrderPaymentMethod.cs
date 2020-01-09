using Common.Api.ExigoWebService;

namespace ExigoService
{
    public interface IAutoOrderPaymentMethod
    {
        int[] AutoOrderIDs { get; set; }

        bool IsUsedInAutoOrders { get; }
        AutoOrderPaymentType AutoOrderPaymentType { get; }
    }
}