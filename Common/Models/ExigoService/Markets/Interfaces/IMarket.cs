using Common;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ExigoService
{
    public interface IMarket
    {
        MarketName Name { get; set; }
        string Description { get; set; }
        string CookieValue { get; set; }
        string CultureCode { get; set; }
        bool IsDefault { get; set; }
        IEnumerable<string> Countries { get; set; }
        List<SelectListItem> AvailableCardTypes { get; }

        IMarketConfiguration Configuration { get; set; }
    }
}