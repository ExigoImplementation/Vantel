using System.Collections.Generic;

namespace ExigoService
{
    /// <summary>
    /// Object based on Database structure from [dbo].[WebCategories]
    /// </summary>
    public class WebCategory
    {
        public int WebID { get; set; }
        public int WebCategoryID { get; set; }
        public int? ParentID { get; set; }
        public string WebCategoryDescription { get; set; }
        public int NestedLevel { get; set; }
        public int SortOrder { get; set; }

        // Custom properties
        public List<WebCategory> Subcategories { get; set; }
    }
}