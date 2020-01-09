using ExigoService;
using System.Collections.Generic;

namespace Common
{
    public class CanadaMarket : Market
    {
        public CanadaMarket()
            : base()
        {
            Name        = MarketName.Canada;
            Description = "Canada";
            CookieValue = CountryCodes.Canada;
            CultureCode = CultureCodes.English_UnitedStates;
            IsDefault   = true;
            Countries   = new List<string> { CountryCodes.Canada };
            PhoneMask = "000000000000";
            CellMask = "0000000000000";
            TaxMask = "00-00000000-0";
            AvailablePaymentTypes = new List<IPaymentMethod>
            {
                new CreditCard()                
            };

            AvailableLanguages = new List<Language> 
            { 
                new Language(Languages.English, CultureCodes.English_UnitedStates)
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
            return new CanadaConfiguration();
        }
    }
}