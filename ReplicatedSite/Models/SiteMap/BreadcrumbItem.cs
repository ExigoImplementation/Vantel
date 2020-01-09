namespace ReplicatedSite.Models.SiteMap
{
    public class BreadcrumbItem 
    {
        public string Description { get; set; }
        public string UrlPath { get; set; }
        public bool Clickeable { get; set; } = true;        
    }
}