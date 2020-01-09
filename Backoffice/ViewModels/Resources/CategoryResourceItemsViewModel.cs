using ExigoService;
using System.Collections.Generic;

namespace Backoffice.ViewModels
{
    public class CategoryResourceItemsViewModel
    {        
        public CategoryResourceItemsViewModel()
        {
            CategoryItem = new ResourceCategoryItem();
        }

        public ResourceCategoryItem CategoryItem { get; set; }
        public IEnumerable<ResourceType> Types { get; set; }
        public IEnumerable<ResourceItem> ResourceList { get; set; }
        public bool IsResourceManager { get; set; }
    }
}