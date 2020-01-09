using System.Collections.Generic;

namespace ExigoService
{
    public class CountryRegionCollection
    {
        public IEnumerable<Country> Countries { get; set; }
        public IEnumerable<Region> Regions { get; set; }
    }
}