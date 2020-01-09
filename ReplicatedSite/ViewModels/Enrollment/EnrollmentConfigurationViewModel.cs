using Common;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ReplicatedSite.ViewModels
{
    public class EnrollmentConfigurationViewModel
    {
        public int EnrollerID { get; set; }
        public MarketName MarketName { get; set; }
        public List<SelectListItem> MarketSelectList { get; set; }
        public bool ItemsInShoppingCart { get; set; }
        public bool UseShoppingItems { get; set; }
    }
}