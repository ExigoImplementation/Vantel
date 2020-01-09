using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.Services;
using System.Web.Mvc;

namespace Backoffice.ViewModels
{
    public class ResourceCategoryListViewModel
    {
        public ResourceCategoryListViewModel(){}

        public IEnumerable<ExigoService.ResourceCategory> ResourceCategories { get; set; }

        //public IEnumerable<CategorySortNode> Nodes { get; set; }

        public List<SelectListItem> ResourceCategorySelectListItems { get; set; }

    }
}