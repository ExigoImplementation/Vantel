using System;
using System.Collections.Generic;

namespace ExigoService
{
    public class ResourceFilters
    {
        public ResourceFilters()
        {
            this.Categories = string.Empty;
            this.MediaFilter = new List<string>();
            this.CategoryFilter = string.Empty;
            this.RankFilter = string.Empty;
            this.MarketFilter = string.Empty;
            this.KeyWord = new List<string>();
            this.isResourceManager = false;
            this.LanguageFilter = string.Empty;
        }

        public string Categories { get; set; }

        public List<string> MediaFilter { get; set; }

        public string CategoryFilter { get; set; }

        public string RankFilter { get; set; }

        public string MarketFilter { get; set; }

        public List<string> KeyWord { get; set; }

        public bool isResourceManager { get; set; }

        public string LanguageFilter { get; set; }
    }
}
