using ExigoService;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Backoffice.ViewModels
{
    public class ResourceListViewModel
    {
        public ResourceListViewModel()
        {
            AvailableResources = new List<Guid>();
            Resource = new ResourceItem();
        }

        public IEnumerable<ResourceItem> Resources { get; set; }
        public IEnumerable<ResourceItem> ResourceList { get; set; }
        public IEnumerable<ResourceCategoryItem> CategoryItemList { get; set; }
        public IEnumerable<ResourceCategory> ResourceCategories { get; set; }
        public IEnumerable<ResourceType> ResourceTypes { get; set; }
        public IEnumerable<ResourceTranslatedCategoryItem> CategoryTranslation { get; set; }
        public List<string> Ranks { get; set; }
        public IEnumerable<ResourceType> Types { get; set; }
        public ResourceItem Resource { get; set; }

        public string CategoryDescription { get; set; }
        public Guid SelectedCategoryID { get; set; }

        //Edit properties
        public IEnumerable<ResourceCategory> EditItemCategories { get; set; }
        public Guid EditItemID { get; set; }
        public Guid DeleteCategoryID { get; set; }
        public List<Market> EditItemAvailability { get; set; }
        public IEnumerable<Tag> Tags { get; set; }



        public Guid ItemID { get; set; }
        public string Title { get; set; }
        public string ItemDescription { get; set; }
        public string Url { get; set; }
        public string UrlThumbnail { get; set; }
        public DateTime? PostDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string Rank { get; set; }
        public int RankID { get; set; }

        public Guid CategoryID { get; set; }
        public Guid StatusID { get; set; }
        public Guid TypeID { get; set; }
        public DateTime CreatedDate { get; set; }
        public IEnumerable<IMarket> CountryAvailability { get; set; }
        public string SelectedAvailability { get; set; }
        public IEnumerable<ResourceAvailability> GetAvailableResources { get; set; }
        public List<Guid> AvailableResources { get; set; }
        public List<ResourceCategoryItem> UnassignedResources { get; set; }
        public List<ResourceItem> UnassignedRsourceList { get; set; }
        public IEnumerable<ExigoService.Language> Languages { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }

        public List<SelectListItem> CountryList { get; set; }
        public ResourceStatus Status { get; set; }
        public ResourceFilters Filter { get; set; }
        public List<ResourceStatus> Statuses { get; set; }
        public List<string> CurrentTags { get; set; }
        public bool IsResourceManager { get; set; }
        public bool IsEdit { get; set; }
        public List<SelectListItem> RankList { get; set; }
    }
}