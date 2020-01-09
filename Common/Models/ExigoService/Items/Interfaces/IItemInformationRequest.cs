using System.Collections.Generic;

namespace ExigoService
{
    // minimum properties to get Item information
    public interface IItemInformationRequest
    {
        int WarehouseID { get; set; }
        string CurrencyCode { get; set; }
        int PriceTypeID { get; set; }
        int? LanguageID { get; set; }
    }
}
