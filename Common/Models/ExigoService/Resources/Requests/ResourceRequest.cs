using System;
using System.Collections.Generic;

namespace ExigoService
{
    public class CreateResourceRequest
    {
        
        public string Title { get; set; }
        public List<Guid> CategoryID { get; set; }
        public string Url { get; set; }
        public string UrlThumbnail { get; set; }
        public Guid TypeID { get; set; }
        //public Status Status { get; set; }
        public DateTime? PostDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public List<string> Keywords { get; set; }
        public List<string> MarketLanguages { get; set; }
        public string ItemDescription { get; set; }
        public string Rank { get; set; }
        public int RankID { get; set; }

    }
    #region ResourceItems
    public class GetResourcesRequest
    {
        public Guid ItemID { get; set; }
        public Guid TypeID { get; set; }
        public Guid StatusID { get; set; }
        public Guid CategoryID { get; set; }
        public string Rank { get; set; }
        public int RankID { get; set; }
        public string SearchFilter { get; set; }
        public IEnumerable<Guid> ItemIDs { get; set; }
    }

    #endregion

    #region Translated Category
    public class GetTranslatedCategoryRequest
    {
        public Guid TranslatedCategoryID { get; set; }
        public Guid CategoryID { get; set; }
        public string Language { get; set; }
        public string TranslatedCategoryDescription { get; set; }
        public IEnumerable<Guid> CategoryIDs { get; set; }
    }

    public class ModifyTranslatedCategoryRequest
    {
        public Guid TranslatedCategoryID { get; set; }
        public List<Guid> TranslatedCategoryIDs { get; set; }
    }

    #endregion

    #region ResourceType
    public class GetResourceTypeRequest
    {

        public Guid TypeID { get; set; }
        public string TypeDescription { get; set; }
        public List<string> TypeDescriptions { get; set; }
    }

    #endregion

    #region Resource Category Items
    public class GetResourceCategoryItemsRequest
    {
        public List<Guid> CategoryIDs { get; set; }
        public Guid ItemID { get; set; }
        public Guid CategoryID { get; set; }
        public IEnumerable<Guid> ItemIDs { get; set; }
        public int ItemOrder { get; set; }
    }

    public class ModifyResourceCategoryRequest
    {
        public ResourceCategoryItem CategoryItem { get; set; }
        public List<ResourceCategoryItem> CategoryItems { get; set; }

    }

   

    #endregion

    #region Resource Categories
    public class GetResourceCategoriesRequest
    {
        public Guid CategoryID { get; set; }
        public IEnumerable<Guid> CategoryIDs { get; set; }
        public int CategoryOrder { get; set; }
        public Guid ParentID { get; set; }
    }

    #endregion

    #region Resource Availabilities
    public class GetResourceAvailabilitiesRequest
    {
        public Guid AvailabilityID { get; set; }
        public Guid ItemID { get; set; }
        public int? LanguageID { get; set; }
        public string CountryCode { get; set; }
    }

    #endregion

    #region Resource Tags
    public class GetResourceItemTagsRequest
    {
        public Guid TagID { get; set; }
        public Guid ItemID { get; set; }
        public List<Guid> TagIDs { get; set; }
    }

    #endregion

    #region Tags For Resources
    public class GetTagsForResourcesRequest
    {
        public Guid TagID { get; set; }
        public string Name { get; set; }
        public List<string> Names { get; set; }
    }
    #endregion
}
