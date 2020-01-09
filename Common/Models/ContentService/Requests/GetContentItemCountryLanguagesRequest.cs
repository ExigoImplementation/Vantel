using System;
using System.Collections.Generic;

namespace Common.Services
{
    public class GetContentItemCountryLanguagesRequest
    {
        public GetContentItemCountryLanguagesRequest()
        {
            CountryCode = "";
            ContentItems = new List<ContentItem>();
        }
        public string[] ContentItemIDs        { get; set; }
        public string CountryCode             { get; set; }
        public int? LanguageID                { get; set; }
        public bool GetAllAlerts              { get; set; }
        public List<ContentItem> ContentItems { get; set; }
    }
}