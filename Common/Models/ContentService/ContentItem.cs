using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Common.Services
{
    public class ContentItem
    {
        public IEnumerable<string> CountryIDList  { get; set; }
        public List<string> CountryLanguageList   { get; set; }
        public int LanguageID                     { get; set; }
        public string ContentDescription          { get; set; }
        public string Content                     { get; set; }
        public string CountryID                   { get; set; }
        public string SectionID                   { get; set; }
        public string SiteDescription             { get; set; }
        public string ViewID                      { get; set; }
        public Guid? ContentItemID                { get; set; }
        public Guid SiteID                        { get; set; }
        public Guid EnvironmentID                 { get; set; }
        public Guid ItemID                        { get; set; }
        public Guid ContentItemCountryLanguageID  { get; set; }
        public DateTime ValidFrom                 { get; set; }
        public DateTime? ExpirationDate           { get; set; }
    }
}