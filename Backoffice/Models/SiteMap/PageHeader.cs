using System.Collections.Generic;

namespace Backoffice.Models.SiteMap
{
    public class PageHeader
    {
        public PageHeader()
        {
            BreadcrumbItems = new List<BreadcrumbItem>();
        }

        public string PageName { get; set; }
        public string PageDescription { get; set; }
        public List<BreadcrumbItem> BreadcrumbItems { get; set; }
        public string DefaultBreadcrumbLinkSitemapID { get; set; }
        public string DefaultParentLinkSitemapID { get; set; }

        public bool DisplayBreadcrumbLinksAsDropdown
        {
            get
            {
                return DefaultBreadcrumbLinkSitemapID.IsNotNullOrEmpty();
            }
        }
    }
}