using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExigoService
{
    public class ResourceItem
    {
        public Guid ItemID { get; set; }
        public Guid TypeID { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string UrlThumbnail { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? PostDate { get; set; }
        public Guid StatusID { get; set; }
        public string Language { get; set; }
        public int LanguageID { get; set; }
        
        // Use TypeID to pull description from ResourceTypes table
        public string TypeDescription { get; set; }

        // Use StatusID to pull description from the ResourceStatuses table
        public string StatusDescription { get; set; }
        public string ItemDescription { get; set; }
        
        // Category ID and ItemOrder within that category
        public Guid CategoryID { get; set; }
        public int ItemOrder { get; set; }

        public List<ResourceCategoryItem> ResourceCategoryItems { get; set; }
        public List<ResourceItemTag> ResourceItemTags { get; set; }
        public List<ResourceAvailability> ResourceAvailabilities { get; set; }
    }

    public class ResourceCategory
    {
        public Guid CategoryID { get; set; }
        public String CategoryDescription { get; set; }
        public int? CategoryOrder { get; set; }
        public Guid ParentID { get; set; }
        public bool HasChildren { get; set; }
        public Guid SortGroupID { get; set; }
        public Guid SortNodeID { get; set; }
        public int SortIndex { get; set; }
    }

    public class ResourceCategoryItem
    {
        public Guid ItemID { get; set; }
        public Guid CategoryID { get; set; }
        public int ItemOrder { get; set; }
    }

    public class ResourceAvailability
    {
        public Guid AvailabilityID { get; set; }
        public string Language { get; set; }
        public Guid ItemID { get; set; }
        public string Market { get; set; }
        public int LanguageID { get; set; }
        public string CountryCode { get; set; }
    }

    public class ResourceItemTag
    {
        public Guid TagID { get; set; }
        public Guid ItemID { get; set; }
    }

    public class ResourceStatus
    {
        public Guid StatusID { get; set; }
        public string StatusDescription { get; set; }
    }

    public class ResourceTranslatedCategoryItem
    {
        public Guid TranslatedCategoryID { get; set; }
        public Guid CategoryID { get; set; }
        public string Language { get; set; }
        public string TranslatedCategoryDescription { get; set; }
        public Guid ParentID { get; set; }
    }

    public class ResourceType
    {
        public Guid TypeID { get; set; }
        public string TypeDescription { get; set; }
        public int SortOrder { get; set; }
    }

    public static class CustomSortGroups{


     public static int ResourceCategories
        {
            get
            {
                return 0;
            }
        }

        public static int ResourceItems
        {
            get
            {
                return 1;
            }
        }
    }

    public class SortValues
    {
        public Guid NodeID { get; set; }
        public int SortIndex { get; set; }
    }
}
