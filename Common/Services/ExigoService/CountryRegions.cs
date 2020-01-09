using Common.Api.ExigoWebService;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System;

namespace ExigoService
{
    public static partial class ExigoDAL
    {
        public static IEnumerable<Country> GetCountries()
        {
            dynamic countries = new List<Country>();
            using (var context = ExigoDAL.Sql())
            {
                countries = context.Query<Country>(@"
                                SELECT CountryCode
                                      ,CountryName = CountryDescription
                                      ,Priority
                                FROM Countries
                                ORDER BY Priority
                    ").AsEnumerable();
            }

            return countries;
        }
        public static IEnumerable<Region> GetRegions(string CountryCode)
        {
            dynamic regions = new List<Region>();
            using (var context = ExigoDAL.Sql())
            {
                regions = context.Query<Region>(@"
                                SELECT CountryCode
                                      ,RegionCode
                                      ,RegionName = RegionDescription
                                FROM CountryRegions
                                WHERE CountryCode = @countryCode
                                ORDER BY RegionCode
                    ", new
                     {
                         countryCode = CountryCode
                     }).AsEnumerable();
            }

            return regions;
        }
    }
}