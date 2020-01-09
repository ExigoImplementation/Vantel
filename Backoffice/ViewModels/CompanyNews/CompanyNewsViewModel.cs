using ExigoService;
using System.Collections.Generic;

namespace Backoffice.ViewModels
{
    public class CompanyNewsViewModel
    {
        public CompanyNewsViewModel()
        {           
            this.CompanyNewsItems = new List<CompanyNewsItem>();
        }
        
        public List<CompanyNewsItem> CompanyNewsItems { get; set; }
    }
}