using Common.Services;
using ExigoService;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Common.Models
{
    public class ContentListModel
    {
        public ContentListModel()
        {
            AvailableContent = new List<Guid>();
            ContentItem = new ContentItem();
        }
        public string ViewID                                { get; set; }
        public string SectionID                             { get; set; }
        public int LanguageID                               { get; set; }
        public ContentItem ContentItem                      { get; set; }
        public List<ContentItem> ContentList                { get; set; }
        public IEnumerable<IMarket> CountryAvailability     { get; set; }
        public List<Guid> AvailableContent                  { get; set; }
        public IEnumerable<ExigoService.Language> Languages { get; set; }
        public List<SelectListItem> CountryList             { get; set; }
        public bool IsEdit                                  { get; set; }
        public string EditItemID                              { get; set; }
    }
}