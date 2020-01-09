using System.Web.Mvc;

namespace ExigoService
{
    [ModelBinder(typeof(IPaymentMethodModelBinder))]
    public interface IPaymentMethod
    {
        bool IsComplete { get; }
        bool IsValid { get; }
    }
}