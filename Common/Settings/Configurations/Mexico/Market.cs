using ExigoService;
using System.Collections.Generic;

namespace Common
{
    public class MexicoMarket : Market
    {
        public MexicoMarket()
            : base()
        {
            Name        = MarketName.Mexico;
            Description = "Mexico";
            CookieValue = "MX";
            CultureCode = "es-US";
            IsDefault   = true;
            Countries   = new List<string> { "MX" };
            PhoneMask = "000000000000";
            CellMask = "0000000000000";
            TaxMask = "00-00000000-0";
            AvailablePaymentTypes = new List<IPaymentMethod>
            {
                new CreditCard()
            };

            AvailableLanguages = new List<Language> 
            { 
                new Language(Languages.English, "en-US"),
                new Language(Languages.Spanish, "es-US")
            };

            AvailableAutoOrderFrequencyTypes = new List<Common.Api.ExigoWebService.FrequencyType>
            {
                Api.ExigoWebService.FrequencyType.Monthly,
                Api.ExigoWebService.FrequencyType.EveryFourWeeks
            };

            AvailableShipMethods = new List<int> { 6, 7 };
        }

        public override IMarketConfiguration GetConfiguration()
        {
            return new MexicoConfiguration();
        }
    }
}