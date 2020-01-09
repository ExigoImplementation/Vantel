using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.Services;

namespace Backoffice.ViewModels
{
    public class SubcategoryListViewModel
    {
        public SubcategoryListViewModel() { }

        public IEnumerable<ResourceCategory> ResourceCategories { get; set; }

        //public IEnumerable<CategorySortNode> Nodes { get; set; }

        public Guid ParentCategoryID { get; set; }
        public string ParentCategoryDescription { get; set; }
    }
}