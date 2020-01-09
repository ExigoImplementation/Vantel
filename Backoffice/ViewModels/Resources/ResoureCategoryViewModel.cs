using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Backoffice.ViewModels
{
    public class ResourceCategoryViewModel
    {
        public ResourceCategoryViewModel()
        {
            this.allLanguages = new List<Language>();
            this.Categories = new List<ResourceTranslatedCategoryItem>();
        }

        public string CategoryDescription { get; set; }

        public List<ResourceTranslatedCategoryItem> Categories { get; set; }

        public List<TranslatedCategory> TranslatedCategoryDescriptions { get; set; }

        public List<Language> allLanguages { get; set; }

        [Display(Name = "Parent Category")]
        public Guid SelectedParentCategoryID { get; set; }

        public IEnumerable<SelectListItem> ParentCategories { get; set; }

        public bool hasSubCategories { get; set; }

    }

    public class TranslatedCategory
    {
        public string TranslatedCategoryDescription { get; set; }
        public string Language { get; set; }       
    }
}