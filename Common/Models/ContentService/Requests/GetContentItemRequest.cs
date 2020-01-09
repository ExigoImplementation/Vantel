using System;
using System.Collections.Generic;

namespace Common.Services
{
    public class GetContentItemRequest
    {
        public GetContentItemRequest()
        {
            CountryCode = "";
        }
        public string[] ContentItemIDs     { get; set; }
        public string CountryCode          { get; set; }
        public int LanguageID              { get; set; }
        public string SiteDescription      { get; set; }
        public bool GetAlert               { get; set; }
        public bool GetAllAlerts           { get; set; }
    }
}