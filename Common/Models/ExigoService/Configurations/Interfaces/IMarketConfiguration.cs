using Common;

namespace ExigoService
{
    public interface IMarketConfiguration
    {
        MarketName MarketName { get; }        

        IOrderConfiguration Orders { get; }
        IOrderConfiguration AutoOrders { get; }
        IOrderConfiguration BackOfficeOrders { get; }
        IOrderConfiguration BackOfficeAutoOrders { get; }
        IOrderConfiguration EnrollmentKits { get; }
        IOrderConfiguration EnrollmentOrders { get; }
        IOrderConfiguration EnrollmentAutoOrders { get; }
    }
}