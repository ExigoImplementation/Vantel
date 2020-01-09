using ExigoService;
using System.Collections.Generic;

namespace Common
{
    public class UnitedStatesMarket : Market
    {
        public UnitedStatesMarket()
            : base()
        {
            Name        = MarketName.UnitedStates;
            Description = "United States";
            CookieValue = CountryCodes.UnitedStates;
            CultureCode = CultureCodes.English_UnitedStates;
            IsDefault   = true;
            Countries   = new List<string> { CountryCodes.UnitedStates }; 
            PhoneMask = "000-000-0000";
            CellMask = "000-000-0000";
            TaxMask = "000-00-0000";
            AvailablePaymentTypes = new List<IPaymentMethod>
            {
                new CreditCard()
            };

            AvailableLanguages = new List<Language> 
            { 
                new Language(Languages.English, CultureCodes.English_UnitedStates),
                new Language(Languages.Spanish, CultureCodes.Spanish_UnitedStates)
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
            return new UnitedStatesConfiguration();
        }
    }
}