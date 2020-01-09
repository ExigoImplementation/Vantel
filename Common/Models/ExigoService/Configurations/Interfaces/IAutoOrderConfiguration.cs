using Common.Api.ExigoWebService;

namespace ExigoService
{
    public interface IAutoOrderConfiguration : IOrderConfiguration
    {
        FrequencyType DefaultFrequencyType { get; set; }

        new int WarehouseID { get; }
        new string CurrencyCode { get; }
        new int PriceTypeID { get; }
        new int LanguageID { get; }
        new string DefaultCountryCode { get; }
        new int DefaultShipMethodID { get; }
        new int CategoryID { get; }
    }
}