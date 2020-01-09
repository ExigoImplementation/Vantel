using Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Dapper;


namespace ExigoService
{
    public static partial class ExigoDAL
    {
        public static IEnumerable<Language> GetLanguages()
        {
            // TODO: remove. 
            // logic obsolete: use GLobalUtilities.GetLanguage, or market.AvailableLanguage
            var distinctLanguages = GlobalSettings.Languages.AvailableLanguages
                // filter out duplicate culture codes (same language from multiple markets)
                .GroupBy(l => l.CultureCode) 
                .Where(group => group.Any())
                .Select(group => group.FirstOrDefault())
                .ToList();
            
            return distinctLanguages;
        }

        public static IEnumerable<Language> GetUniqueLanguages()
        {
            // Get a list of the available markets
            var availableLanguages = new List<Language>();
            var markets = GlobalSettings.Markets.AvailableMarkets;
            foreach (var market in markets)
            {
                foreach (var language in market.AvailableLanguages)
                {
                    var lang = language.CultureCode.Substring(0, 2);
                    if (!availableLanguages.Any(c => c.CultureCode.StartsWith(lang)))
                    {
                        availableLanguages.Add(language);
                    }
                }
            }

            // Get a list of the available languages from the avialble markets from above
            return availableLanguages.ToList();
        }
    }
}