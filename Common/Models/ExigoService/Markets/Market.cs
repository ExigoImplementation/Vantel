using Common;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ExigoService
{
    public class Market : IMarket
    {
        public Market()
        {
            this.Configuration = GetConfiguration();
            
        }

        public MarketName Name { get; set; }
        public string Description { get; set; }
        public string CookieValue { get; set; }
        public string CultureCode { get; set; }
        public bool IsDefault { get; set; }
        public IEnumerable<string> Countries { get; set; }
        public string PhoneMask { get; set; }
        public string CellMask { get; set; }
        public string TaxMask { get; set; }
        public List<IPaymentMethod> AvailablePaymentTypes { get; set; }
        public List<Language> AvailableLanguages { get; set; }
        public List<Common.Api.ExigoWebService.FrequencyType> AvailableAutoOrderFrequencyTypes { get; set; }
        public List<int> AvailableShipMethods { get; set; }
        public IMarketConfiguration Configuration { get; set; }
        public List<SelectListItem> AvailableCardTypes { get {
                return new List<SelectListItem>() {
                    new SelectListItem(){ Text="Visa", Value="1", Selected = true },
                    new SelectListItem(){ Text="MasterCard", Value="2" },
                    new SelectListItem(){ Text="American Express", Value="3" },
                    new SelectListItem(){ Text="Discover", Value="4" }
                };
            }
        }
        public bool CVVRequired { get; set; } = false;
        public virtual IMarketConfiguration GetConfiguration()
        {
            return new UnitedStatesConfiguration();
        }
        /// <summary>
        /// Gets the first country of the market
        /// </summary>
        public string MainCountry {get { return this.Countries.FirstOrDefault(); } }
    }
}