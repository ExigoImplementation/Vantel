using Backoffice.Filters;
using Backoffice.ViewModels;
using Common;
using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Dapper;
using Common.Services;

namespace Backoffice.Controllers
{
    [RoutePrefix("resources")]
    public class ResourcesController : Controller
    {
        private const int InitialOrderValue = 1;


        public ActionResult ResourceList()
        {
            var model = new ResourceListViewModel();

            model.ResourceCategories = ExigoDAL.GetResourceCategories(new GetResourceCategoriesRequest()).OrderBy(c => c.CategoryOrder);
            model.CountryAvailability = GlobalSettings.Markets.AvailableMarkets;
            model.Languages = ExigoDAL.GetUniqueLanguages().ToList();
            model.ResourceTypes = ExigoDAL.GetResourceTypes(new GetResourceTypeRequest()).OrderBy(rt => rt.SortOrder).ToList();
            model.CategoryTranslation = ExigoDAL.GetCategoryTranslations(new GetTranslatedCategoryRequest());
            model.Tags = ExigoDAL.GetTagsForResources(new GetTagsForResourcesRequest());

            //Create ListItems for the Market/Language DropDown
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var market in model.CountryAvailability)
            {
                SelectListItem item = new SelectListItem()
                {
                    Value = market.Countries.FirstOrDefault(),
                    Text = CommonResources.Countries(market.Countries.FirstOrDefault()),
                    Selected = Identity.Current.Country == market.Countries.FirstOrDefault()
                };
                items.Add(item);
            }
            model.CountryList = items;

            return View(model);
        }

        [GlobalAdminsFilter]
        public ActionResult ManageResources()
        {
            //set up the model/service and fetch data
            var model = new ResourceListViewModel();

            model.ResourceCategories = ExigoDAL.GetResourceCategories(new GetResourceCategoriesRequest()).OrderBy(c => c.CategoryOrder);
            model.CountryAvailability = GlobalSettings.Markets.AvailableMarkets;
            model.Languages = ExigoDAL.GetUniqueLanguages().ToList();
            model.ResourceTypes = ExigoDAL.GetResourceTypes(new GetResourceTypeRequest()).OrderBy(rt => rt.SortOrder).ToList();
            model.CategoryTranslation = ExigoDAL.GetCategoryTranslations(new GetTranslatedCategoryRequest());
            model.Tags = ExigoDAL.GetTagsForResources(new GetTagsForResourcesRequest());

            //Create ListItems for the Market/Language DropDown
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var market in model.CountryAvailability)
            {
                SelectListItem item = new SelectListItem()
                {
                    Value = market.Countries.FirstOrDefault(),
                    Text = CommonResources.Countries(market.Countries.FirstOrDefault()),
                    Selected = Identity.Current.Country == market.Countries.FirstOrDefault()
                };
                items.Add(item);
            }
            model.CountryList = items;

            return View(model);
        }

        [GlobalAdminsFilter]
        public ActionResult ManageResourceCategories()
        {
            //set up the model/service and fetch data
            var model = new ResourceCategoryListViewModel();

            string ResourceCategoryText;


            model.ResourceCategories = ExigoDAL.GetResourceCategories(new GetResourceCategoriesRequest() { ParentID = Guid.Empty }).OrderBy(c => c.CategoryOrder);

            List<SelectListItem> categorySelectItems = new List<SelectListItem>();
            foreach (var category in model.ResourceCategories.ToList())
            {
                category.HasChildren = ExigoDAL.GetResourceCategories(new GetResourceCategoriesRequest() { ParentID = category.CategoryID }).Any();
                //var childCount = context.ResourceCategories.Where(c => c.ParentID == category.CategoryID).Count();
                //category.HasChildren = (childCount > 0);
                ResourceCategoryText = ExigoDAL.GetCategoryTranslations(new GetTranslatedCategoryRequest() { Language = Identity.Current.Language.LanguageDescription, CategoryID = category.CategoryID }).FirstOrDefault().TranslatedCategoryDescription;

                if (ResourceCategoryText.IsNullOrEmpty())//#71613 Ivan S. 2015-11-06 Validated when the translation is not available in the resource files
                {
                    ResourceCategoryText = category.CategoryDescription;
                }
                categorySelectItems.Add(new SelectListItem
                {
                    Text = ResourceCategoryText,
                    Value = category.CategoryID.ToString()
                });
            }
            model.ResourceCategorySelectListItems = categorySelectItems;

            return View(model);
        }

        public ActionResult ResourceItems(Guid? editItemID)
        {
            Guid ID = Guid.NewGuid();

            var model = new ResourceListViewModel();

            model.CountryAvailability = GlobalSettings.Markets.AvailableMarkets;

            model.Languages = ExigoDAL.GetUniqueLanguages().ToList();

            model.ResourceTypes = ExigoDAL.GetResourceTypes(new GetResourceTypeRequest());

            model.Statuses = ExigoDAL.GetResourceStatuses();

            model.ResourceCategories = ExigoDAL.GetResourceCategories(new GetResourceCategoriesRequest());

            model.Tags = ExigoDAL.GetTagsForResources(new GetTagsForResourcesRequest());

            model.IsEdit = false;

            model.CurrentTags = new List<string>();

            if (editItemID != null && editItemID != Guid.Empty)
            {
                ID = editItemID ?? ID;
                model.Resource = ExigoDAL.GetResourceItems(new GetResourcesRequest() { ItemID = ID }).FirstOrDefault();

                var TagIDs = ExigoDAL.GetResourceItemTags(new GetResourceItemTagsRequest() { ItemID = ID }).Select(t => t.TagID);
                model.CurrentTags = model.Tags.Where(t => TagIDs.Contains(t.TagID)).Select(t => t.Name).Distinct().ToList();
                model.Resource.ResourceCategoryItems = ExigoDAL.GetResourceCategoryItems(new GetResourceCategoryItemsRequest() { ItemID = ID });
                model.Resource.ResourceAvailabilities = ExigoDAL.GetResourceAvailabilities(new GetResourceAvailabilitiesRequest() { ItemID = ID });
                //SET Default Values for DropDownList on View
                model.TypeID = model.Resource.TypeID;
                model.StatusID = model.Resource.StatusID;
                model.Language = model.Resource.Language;

                model.IsEdit = true;
            }

            model.EditItemID = editItemID ?? Guid.Empty;

            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteResource(ResourceListViewModel res)
        {
            var itemID = res.EditItemID;
            var categoryID = res.DeleteCategoryID;


            //A resource will be allowed in mulitple categories Thus delete this entries from this table
            var categoryItems = ExigoDAL.GetResourceCategoryItems(new GetResourceCategoryItemsRequest() { ItemID = itemID });
            foreach (var item in categoryItems)
            {

                //2015-09-08
                //Ivan S.
                //66
                //Reorders the following resources (setting their order to a minus 1 value)
                var itemOrder = item.ItemOrder;
                var categoryItemOrder = ExigoDAL.GetResourceCategoryItems(new GetResourceCategoryItemsRequest() { CategoryID = categoryID, ItemOrder = itemOrder }).ToList();
                foreach (var catitem in categoryItemOrder)
                {
                    catitem.ItemOrder = catitem.ItemOrder - 1;
                    ExigoDAL.ModifyResourceCategoryItemOrder(catitem);
                }

            }
            //Delete the availabilitiy of the resource
            var availability = ExigoDAL.GetResourceAvailabilities(new GetResourceAvailabilitiesRequest() { ItemID = itemID }).Select(v => v.AvailabilityID).ToList();
            ExigoDAL.DeleteResourceAvailabilities(availability);

            //Delete The Tags Associated with the item
            var tags = ExigoDAL.GetResourceItemTags(new GetResourceItemTagsRequest() { ItemID = itemID }).Select(t => t.TagID).ToList();
            ExigoDAL.DeleteResourceItemTags(tags);

            //Delete the resource itself
            var resource = ExigoDAL.GetResourceItems(new GetResourcesRequest() { ItemID = itemID }).FirstOrDefault();
            ExigoDAL.DeleteResourceItem(resource);

            return RedirectToAction("ManageResources");
        }

        public JsonNetResult DeleteCategory(Guid categoryID)
        {
            try
            {
                //Create new List Guids
                List<Guid> categoryList = new List<Guid>();

                //Get Children Categories
                categoryList = ExigoDAL.GetResourceCategories(new GetResourceCategoriesRequest() { ParentID = categoryID }).Select(cat => cat.CategoryID).ToList();

                //Add Parent Category to List
                categoryList.Add(categoryID);

                //reset the resource items
                var resources = ExigoDAL.GetResourceCategoryItems(new GetResourceCategoryItemsRequest() { CategoryIDs = categoryList }).Select(v => v.ItemID);

                if (resources.Any())
                {
                    using (var context = ExigoDAL.Sql())
                    {
                        context.Execute(@"
                            UPDATE 
                                ExigoWebContext.ResourceCategoryItems
                            SET
                                CategoryID = cast(cast(0 as binary) as uniqueidentifier)
                            WHERE
                                CategoryID IN @categorylist
                            AND
                                ItemID IN @resources
                         
                        ", new { categorylist = categoryList, resources = resources });
                    }
                }



                var allCategoryItems = ExigoDAL.GetResourceCategoryItems(new GetResourceCategoryItemsRequest());

                var listWithNoCategory = allCategoryItems.Where(c => c.CategoryID == Guid.Empty);
                var deleteItemList = new List<ResourceCategoryItem>();

                foreach (var item in listWithNoCategory)
                {                    
                    if(allCategoryItems.Where(c => c.ItemID == item.ItemID && c.CategoryID != Guid.Empty).Any())
                    {
                        deleteItemList.Add(item);
                    }
                }

                var deleteIDs = deleteItemList.Select(c => c.ItemID);

                if (deleteItemList.Any())
                {
                    using (var context = ExigoDAL.Sql())
                    {
                        context.Execute(@"
                            DELETE FROM 
                            ExigoWebContext.ResourceCategoryItems
                        WHERE
                            CategoryID = cast(cast(0 as binary) as uniqueidentifier)
                        AND
                            ItemID IN @itemids
                        
                         
                        ", new { itemids = deleteIDs });
                    }
                }
                
                //delete the Category and Children
                var category = ExigoDAL.GetResourceCategories(new GetResourceCategoriesRequest() { CategoryID = categoryID }).FirstOrDefault();
                var categoryOrder = category.CategoryOrder ?? 0;
                using (var context = ExigoDAL.Sql())
                {
                    context.Execute(@"
                        DELETE FROM 
                            ExigoWebContext.ResourceCategories
                        WHERE
                            CategoryID = @categoryid
                        OR  
                            ParentID = @categoryid
                         
                    ", new { categoryid = category.CategoryID });

                }

                //delete the translations
                var translatedCategoryIDs = ExigoDAL.GetCategoryTranslations(new GetTranslatedCategoryRequest() { CategoryID = categoryID }).Select(t => t.TranslatedCategoryID);
                using (var context = ExigoDAL.Sql())
                {
                    context.Execute(@"
                        DELETE FROM 
                            ExigoWebContext.ResourceTranslatedCategoryItems
                        WHERE
                            TranslatedCategoryID IN @translatedids
                         
                    ", new { translatedids = translatedCategoryIDs });
                }

                //2015-09-08
                //Ivan S.
                //66
                //Decreases the order for the following categories
                var categoryOrders = ExigoDAL.GetResourceCategories(new GetResourceCategoriesRequest() { CategoryOrder = categoryOrder }).Select(rc => rc.CategoryID);
                using (var context = ExigoDAL.Sql())
                {
                    context.Execute(@"
                        UPDATE 
                            ExigoWebContext.ResourceCategories
                        SET
                            CategoryOrder = CategoryOrder - 1
                        WHERE
                            CategoryID IN @categoryorders
                         
                    ", new { categoryorders = categoryOrders });
                }

                return new JsonNetResult(new
                {
                    success = true,
                });
            }
            catch (Exception ex)
            {
                return new JsonNetResult(new
                {
                    success = true,
                    message = ex.Message
                });
            }
        }




        [HttpPost]
        public JsonNetResult CreateCategory(List<TranslatedCategory> transdesc, Guid? parentID)
        {
            try
            {


                //2015-09-08
                //Ivan S.
                //66
                //Sets the initial order for the new category to the maximum number for all the categories
                var lastCategory = ExigoDAL.GetResourceCategories(new GetResourceCategoriesRequest()).OrderByDescending(rc => rc.CategoryOrder).FirstOrDefault();
                int? lastCategoryOrder = InitialOrderValue - 1;
                if (lastCategory != null)
                    lastCategoryOrder = lastCategory.CategoryOrder;
                var NewOrder = ++lastCategoryOrder;
                var CategoryID = Guid.NewGuid();
                if (parentID == null) parentID = Guid.Empty;
                foreach (var description in transdesc)
                {
                    //English is the Default Language and will be used as the Description on the Categories Table
                    if (description.Language == "English")
                    {
                        ResourceCategory Category = new ResourceCategory()
                        {
                            CategoryID = CategoryID,
                            CategoryDescription = description.TranslatedCategoryDescription,
                            CategoryOrder = NewOrder,
                            ParentID = (Guid)parentID

                        };
                        ExigoDAL.AddResourceCategory(Category);
                    }
                    //Adds an entry for each language translation provided in the TranslatedCategoryItems table
                    ResourceTranslatedCategoryItem TCategory = new ResourceTranslatedCategoryItem()
                    {
                        TranslatedCategoryID = Guid.NewGuid(),
                        CategoryID = CategoryID,
                        Language = description.Language,
                        TranslatedCategoryDescription = description.TranslatedCategoryDescription
                    };
                    ExigoDAL.AddCategoryTranslation(TCategory);

                }

                return new JsonNetResult(new
                {
                    success = true
                });
            }
            catch (Exception ex)
            {
                return new JsonNetResult(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        public JsonNetResult CreateResource(CreateResource res)
        {
            try
            {

                // Create Resource Item
                ResourceItem resource = new ResourceItem()
                {
                    ItemID = Guid.NewGuid(),
                    Title = res.Title,
                    ItemDescription = res.ItemDescription,
                    TypeID = res.TypeID,
                    Url = res.Url,
                    UrlThumbnail = res.UrlThumbnail,
                    CreatedDate = DateTime.Now.ToCST(),
                    PostDate = res.PostDate >= DateTime.Now.AddHours(1) ? res.PostDate : null,
                    StatusID = res.StatusID,
                    LanguageID = Convert.ToInt32(res.Language)
                };
                ExigoDAL.CreateResourceItem(resource);

                //Create Resource Category Item
                foreach (var categoryID in res.CategoryID)
                {
                    //2015-09-08
                    //Ivan S.
                    //66
                    //Sets the initial order for the new resource to the maximum number for that category
                    var lastResource = ExigoDAL.GetResourceCategoryItems(new GetResourceCategoryItemsRequest() { CategoryID = categoryID })
                        .OrderByDescending(r => r.ItemOrder)
                        .FirstOrDefault();
                    int lastCategoryOrder = InitialOrderValue - 1;
                    if (lastResource != null)
                        lastCategoryOrder = lastResource.ItemOrder;
                    var NewOrder = ++lastCategoryOrder;

                    ResourceCategoryItem categoryItem = new ResourceCategoryItem()
                    {
                        ItemID = resource.ItemID,
                        CategoryID = categoryID,
                        ItemOrder = NewOrder
                    };
                    ExigoDAL.CreateResourceCategoryItem(categoryItem);


                }

                // Create the ResourceAvailability Entries
                if (res.Markets != null && res.Markets.Count() > 0)
                {
                    foreach (var CountryCode in res.Markets)
                    {
                        ResourceAvailability availableresource = new ResourceAvailability()
                        {
                            AvailabilityID = Guid.NewGuid(),
                            ItemID = resource.ItemID,
                            CountryCode = CountryCode,
                            LanguageID = Convert.ToInt32(res.Language)
                        };
                        ExigoDAL.CreateResourceAvailabilities(availableresource);
                    }
                }

                //Create Tags
                List<Tag> Tags = new List<Tag>();
                if (res.Keywords.Count() > 0)
                {
                    var existingtags = ExigoDAL.GetTagsForResources(new GetTagsForResourcesRequest() { Names = res.Keywords });
                    var existingtagnames = existingtags.Select(t => t.Name).ToList();
                    var needtocreatetags = res.Keywords.Except(existingtagnames);
                    foreach (var name in needtocreatetags)
                    {
                        //See if Tag exist in DB if not create one

                        Tag newTag = new Tag()
                        {
                            TagID = Guid.NewGuid(),
                            Name = name
                        };
                        ExigoDAL.CreateTag(newTag);
                        Tags.Add(newTag);
                    }
                    Tags.AddRange(existingtags);
                }
                // Create the ResourceItemTag Entries
                if (Tags.Any())
                {
                    foreach (var tag in Tags)
                    {
                        ResourceItemTag rTag = new ResourceItemTag()
                        {
                            TagID = tag.TagID,
                            ItemID = resource.ItemID
                        };
                        ExigoDAL.CreateResourceItemTag(rTag);
                    }
                }

                return new JsonNetResult(new
                {
                    success = true,
                });
            }
            catch (Exception ex)
            {
                return new JsonNetResult(new
                {
                    success = false,
                    message = ex.Message
                });
                
            }
        }

        [HttpPost]
        public JsonNetResult EditResource(CreateResource res, string editID)
        {
            try
            {
                var itemID = Guid.Parse(editID);

                //Update ResoureItem

                ResourceItem originalResource = new ResourceItem()
                {
                    ItemID = itemID,
                    Title = res.Title,
                    ItemDescription = res.ItemDescription,
                    Url = res.Url,
                    UrlThumbnail = res.UrlThumbnail,
                    TypeID = res.TypeID,
                    StatusID = res.StatusID,
                    PostDate = res.PostDate ?? DateTime.Now.ToCST(),
                    Language = res.Language,
                    CreatedDate = DateTime.Now.ToCST(),
                };

                ExigoDAL.ModifyResourceItem(originalResource);

                //Current Availabilities
                List<ResourceAvailability> currentAvailability = ExigoDAL.GetResourceAvailabilities(new GetResourceAvailabilitiesRequest() { ItemID = originalResource.ItemID }).ToList();

                //Wanted Availabilities
                List<ResourceAvailability> keepAvailabilities = new List<ResourceAvailability>();
                List<Available> neededAvailabilities = new List<Available>();
                foreach (var market in res.Markets)
                {
                    var availability = new Available();
                    availability.Language = res.Language;
                    availability.Market = market;
                    var exist = currentAvailability.Where(ca => ca.Language == availability.Language && ca.Market == availability.Market).FirstOrDefault();
                    if (exist == null) { neededAvailabilities.Add(availability); }
                    else { keepAvailabilities.Add(exist); }


                }



                //Add NEW Availabilities
                foreach (var avail in neededAvailabilities)
                {
                    ResourceAvailability availableresource = new ResourceAvailability()
                    {
                        AvailabilityID = Guid.NewGuid(),
                        ItemID = originalResource.ItemID,
                        Market = avail.Market,
                        Language = avail.Language
                    };
                    ExigoDAL.CreateResourceAvailabilities(availableresource);
                }

                //Delete Availabilities no longer wanted
                List<Guid> deleteAvailabilities = currentAvailability.Except(keepAvailabilities).Select(t => t.AvailabilityID).ToList();
                ExigoDAL.DeleteResourceAvailabilities(deleteAvailabilities);

                //Modify ResourceCategoryItems

                // Current ResourceCategoryItems CategoryIDs
                var RCI = ExigoDAL.GetResourceCategoryItems(new GetResourceCategoryItemsRequest() { ItemID = originalResource.ItemID });
                var currentRCI = RCI.Select(rci => rci.CategoryID);

                // Wanted CategoryIDs
                var wantedRCI = res.CategoryID;

                // Needed CategoryIDs
                var neededRCI = wantedRCI.Except(currentRCI);

                //Add New CategoryItems


                foreach (var category in neededRCI)
                {
                    var lastResource = ExigoDAL.GetResourceCategoryItems(new GetResourceCategoryItemsRequest() { CategoryID = category })
                        .OrderByDescending(r => r.ItemOrder)
                        .FirstOrDefault();
                    int lastItemOrder = InitialOrderValue - 1;
                    if (lastResource != null) lastItemOrder = lastResource.ItemOrder;

                    ResourceCategoryItem categoryItem = new ResourceCategoryItem()
                    {
                        ItemID = originalResource.ItemID,
                        CategoryID = category,
                        ItemOrder = lastItemOrder
                    };
                    ExigoDAL.CreateResourceCategoryItem(categoryItem);

                }
                //Delete Unwanted CategoryItems
                var unwantedRCICategoryIDs = currentRCI.Except(wantedRCI).ToList();
                if (unwantedRCICategoryIDs.Any())
                {
                    ExigoDAL.DeleteResourceCategoryItems(itemID, unwantedRCICategoryIDs);
                }


                List<Tag> tags = new List<Tag>();



                if (res.Keywords.Any())
                {
                    // check to see if tag existis with these keywords. if not add it

                    //Get A List Of Tags Using Provided Keywords
                    var existingTags = ExigoDAL.GetTagsForResources(new GetTagsForResourcesRequest() { Names = res.Keywords });
                    var existingTagNames = existingTags.Select(et => et.Name);

                    //Get a List of Keywords we need to make tags for
                    var newTagsneeded = res.Keywords.Except(existingTagNames);

                    //Create The New Tags
                    List<Tag> NewlyCreatedTags = new List<Tag>();
                    foreach (var keyword in newTagsneeded)
                    {
                        Tag newTag = new Tag()
                        {
                            TagID = Guid.NewGuid(),
                            Name = keyword
                        };
                        NewlyCreatedTags.Add(newTag);
                        ExigoDAL.CreateTag(newTag);
                    }

                    //Combine existing and New Tags
                    var completeTagList = existingTags.Union(NewlyCreatedTags).ToList();
                    var completeTagIDs = completeTagList.Select(v => v.TagID);

                    //Get a list of ItemTags currently associated with the Item
                    var currentItemTagIDs = ExigoDAL.GetResourceItemTags(new GetResourceItemTagsRequest() { ItemID = originalResource.ItemID }).Select(v => v.TagID);

                    //Get a list of Tags we need to make ItemTags for
                    var newItemTagsneeded = completeTagList.Where(t => !currentItemTagIDs.Contains(t.TagID));

                    // Update ResourceItemTags Table with new Tags
                    foreach (var tagneeded in newItemTagsneeded)
                    {
                        ResourceItemTag rit = new ResourceItemTag()
                        {
                            TagID = tagneeded.TagID,
                            ItemID = itemID,
                        };
                        ExigoDAL.CreateResourceItemTag(rit);
                    }

                    // remove any pivot table tags that are no longer being used for this resource
                    var nolongerwanted = currentItemTagIDs.Except(completeTagIDs).ToList();
                    if (nolongerwanted.Count > 0)
                    {
                        ExigoDAL.DeleteResourceItemTags(nolongerwanted);
                    }
                }

                return new JsonNetResult(new
                {
                    success = true,
                });
            }
            catch (Exception ex)
            {
                return new JsonNetResult(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        public JsonNetResult EditCategory(List<TranslatedCategory> items, Guid categoryID, Guid? parentID)
        {

            try
            {
                foreach (var item in items.Where(i => !i.Language.IsNullOrEmpty()))
                {
                    if (item.Language == "English")
                    {
                        //Update the Resource
                        var resourceCategory = ExigoDAL.GetResourceCategories(new GetResourceCategoriesRequest() { CategoryID = categoryID }).FirstOrDefault();
                        resourceCategory.CategoryDescription = item.TranslatedCategoryDescription;
                        resourceCategory.ParentID = parentID ?? Guid.Empty;
                        ExigoDAL.ModifyResourceCategory(resourceCategory);
                    }
                    //Update the Translations
                    var translatedCategory = ExigoDAL.GetCategoryTranslations(new GetTranslatedCategoryRequest() { Language = item.Language, CategoryID = categoryID }).FirstOrDefault();
                    if (translatedCategory != null)
                    {
                        translatedCategory.TranslatedCategoryDescription = item.TranslatedCategoryDescription;
                        ExigoDAL.ModifyResourceTranslationDescription(translatedCategory);
                    }
                    else
                    {
                        ResourceTranslatedCategoryItem TCategory = new ResourceTranslatedCategoryItem()
                        {
                            TranslatedCategoryID = Guid.NewGuid(),
                            CategoryID = categoryID,
                            Language = item.Language,
                            TranslatedCategoryDescription = item.TranslatedCategoryDescription
                        };
                        ExigoDAL.AddCategoryTranslation(TCategory);
                    }


                }

                return new JsonNetResult(new
                {
                    success = true
                });
            }
            catch (Exception ex)
            {
                return new JsonNetResult(new
                {
                    success = false,
                    message = ex.Message
                });
            }

        }

        [HttpPost]
        public JsonNetResult GetModal(Guid categoryID, Guid parentCategoryID)
        {
            var model = new ResourceCategoryViewModel();

            try
            {
                //context.ResourceCategories.Where(c => c.ParentID == null && c.CategoryID != categoryID)
                List<SelectListItem> ListItems = new List<SelectListItem>();
                List<ResourceCategory> categories;
                using (var context = ExigoDAL.Sql())
                {
                    categories = context.Query<ResourceCategory>(@"
                        SELECT 
                            CategoryID,
                            CategoryDescription
                        FROM
                            ExigoWebContext.ResourceCategories
                        WHERE
                            ParentID = @parentid
                        AND
                            CategoryID <> @categoryid

                        ", new { parentid = Guid.Empty, categoryid = categoryID }).ToList();
                    foreach (var cat in categories)
                    {
                        SelectListItem item = new SelectListItem()
                        {
                            Value = cat.CategoryID.ToString(),
                            Text = cat.CategoryDescription
                        };
                        ListItems.Add(item);
                    }
                }
                model.ParentCategories = ListItems;


                model.allLanguages = ExigoDAL.GetUniqueLanguages().ToList();

                if (categoryID != Guid.Empty)
                {
                    model.Categories = ExigoDAL.GetCategoryTranslations(new GetTranslatedCategoryRequest() { CategoryID = categoryID }).ToList();
                }
                var parentCat = ExigoDAL.GetResourceCategories(new GetResourceCategoriesRequest() { CategoryID = categoryID }).FirstOrDefault();
                model.SelectedParentCategoryID = parentCat == null ? Guid.Empty : parentCat.ParentID;
                model.hasSubCategories = categoryID == Guid.Empty ? false : ExigoDAL.GetResourceCategories(new GetResourceCategoriesRequest { ParentID = categoryID }).Count() > 0;
                var html = this.RenderPartialViewToString("partials/_categorypartial", model);

                return new JsonNetResult(new
                {
                    success = true,
                    html = html
                });
            }
            catch (Exception ex)
            {
                return new JsonNetResult(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        public JsonNetResult LoadCategories(Guid parentCategoryID)
        {
            try
            {
                var model = new SubcategoryListViewModel();
                model.ParentCategoryID = parentCategoryID;
                model.ParentCategoryDescription = ExigoDAL.GetResourceCategories(new GetResourceCategoriesRequest() { CategoryID = parentCategoryID }).FirstOrDefault().CategoryDescription;
                model.ResourceCategories = ExigoDAL.GetResourceCategories(new GetResourceCategoriesRequest() { ParentID = parentCategoryID }).OrderBy(v => v.CategoryOrder);
                var html = this.RenderPartialViewToString("partials/_categorylist", model);

                return new JsonNetResult(new
                {
                    success = true,
                    html = html
                });
            }
            catch (Exception ex)
            {
                return new JsonNetResult(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        public JsonNetResult GetResourceList(ResourceFilters filter)
        {
            try
            {
                var model = new ResourceListViewModel();
                model.Filter = filter;
                model.IsResourceManager = filter.isResourceManager;
                model.ResourceCategories = new JavaScriptSerializer().Deserialize<IEnumerable<ExigoService.ResourceCategory>>(filter.Categories);
                var filteredcatfilter = filter.CategoryFilter != null ? new JavaScriptSerializer().Deserialize<string>(filter.CategoryFilter) : "";
                var categoryFilter = (filteredcatfilter != null && filteredcatfilter != "") ? Guid.Parse(filteredcatfilter) : Guid.Empty;
                model.ResourceList = ExigoDAL.GetResourceItems(new GetResourcesRequest());
                model.CategoryItemList = ExigoDAL.GetResourceCategoryItems(new GetResourceCategoryItemsRequest()).OrderBy(t => t.ItemOrder);
                model.Types = ExigoDAL.GetResourceTypes(new GetResourceTypeRequest() { TypeDescriptions = filter.MediaFilter });
                //Get a List of CategoryIDs from the Rescource Category List
                var catIds = model.ResourceCategories.Select(c => c.CategoryID).ToList();
                //Get all resources assigned to a CURRENT Category ID
                var assigned = model.CategoryItemList.Where(c => catIds.Contains(c.CategoryID)).ToList();
                //Assign all remaining items so they may be displayed in the Unassigned Category
                model.UnassignedResources = model.CategoryItemList.Except(assigned).ToList();
                //Get a list of Unassigned Resources before filtering by market
                var unassignedresourceIDs = model.UnassignedResources.Select(ur => ur.ItemID).ToList();
                model.UnassignedRsourceList = model.ResourceList.Where(rl => unassignedresourceIDs.Contains(rl.ItemID)).ToList();

                if (filter.MarketFilter != null && filter.MarketFilter != string.Empty)
                {
                    if (filter.LanguageFilter != null && filter.LanguageFilter != string.Empty)
                    {
                        var language = Convert.ToInt32(filter.LanguageFilter);
                        var cc = filter.MarketFilter;
                        model.GetAvailableResources = ExigoDAL.GetResourceAvailabilities(new GetResourceAvailabilitiesRequest() { CountryCode = cc, LanguageID = language });
                        model.AvailableResources = model.GetAvailableResources.Select(ar => ar.ItemID).ToList();

                    }
                    else
                    {
                        var cc = filter.MarketFilter;
                        model.GetAvailableResources = ExigoDAL.GetResourceAvailabilities(new GetResourceAvailabilitiesRequest() { CountryCode = cc });
                        model.AvailableResources = model.GetAvailableResources.Select(ar => ar.ItemID).ToList();
                    }
                }

                model.ResourceList = model.ResourceList.Where(x => model.AvailableResources.Contains(x.ItemID));

                bool filtered = false;
                if (filter.MediaFilter != null && filter.MediaFilter.Count() > 0)
                {
                    var typeIDs = model.Types.Select(v => v.TypeID);
                    filtered = true;
                    model.ResourceList = model.ResourceList.Where(x => typeIDs.Contains(x.TypeID)).ToList();
                }


                if (categoryFilter != null && categoryFilter != Guid.Empty)
                {
                    model.SelectedCategoryID = model.ResourceCategories.Where(c => c.CategoryID == categoryFilter).FirstOrDefault().CategoryID;
                }

                if (filter.KeyWord != null && filter.KeyWord.Any())
                {
                    var keyword = filter.KeyWord;
                    var tag = ExigoDAL.GetTagsForResources(new GetTagsForResourcesRequest() { Names = keyword }).FirstOrDefault();
                    if (tag != null)
                    {
                        var resourceitemtagIDs = ExigoDAL.GetResourceItemTags(new GetResourceItemTagsRequest() { TagID = tag.TagID }).AsEnumerable().Select(rt => rt.ItemID);

                        //var tagitems = model.ResourceList.Where(rl => resourceitemtagIDs.Contains(rl.ItemID));
                        //var matchingitems = model.ResourceList.Where(rl => keyword.Contains(rl.Title) || rl.ItemDescription.ToLower().Contains(keyword));
                        //var keywordSearchResult = tagitems.Union(matchingitems).ToList().Select(r => r.ItemID);
                        //model.ResourceList = model.ResourceList.Where(rl => keywordSearchResult.Contains(rl.ItemID));

                        model.ResourceList = model.ResourceList.Where(rl => resourceitemtagIDs.Contains(rl.ItemID));
                    }
                }

                var html = this.RenderPartialViewToString("partials/_resourcelist", model);

                return new JsonNetResult(new
                {
                    success = true,
                    html = html,
                    isFiltered = filtered
                });
            }
            catch (Exception ex)
            {
                return new JsonNetResult(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        public JsonNetResult SortCategoryByDate(Guid categoryID, string sortType, ResourceFilters filter)
        {
            try
            {
                var model = new CategoryResourceItemsViewModel();
                var availableResources = new List<Guid>();
                model.CategoryItem = ExigoDAL.GetResourceCategoryItems(new GetResourceCategoryItemsRequest() { CategoryID = categoryID }).FirstOrDefault();
                model.IsResourceManager = filter.isResourceManager;
                model.ResourceList = ExigoDAL.GetResourceItemsByCategory(categoryID);
                model.Types = ExigoDAL.GetResourceTypes(new GetResourceTypeRequest() { TypeDescriptions = filter.MediaFilter });

                if (filter.MarketFilter != null && filter.MarketFilter != string.Empty)
                {
                    if (filter.LanguageFilter != null && filter.LanguageFilter != string.Empty)
                    {
                        var language = Convert.ToInt32(filter.LanguageFilter);
                        var cc = filter.MarketFilter;
                        var getAvailableResources = ExigoDAL.GetResourceAvailabilities(new GetResourceAvailabilitiesRequest() { CountryCode = cc, LanguageID = language });
                        availableResources = getAvailableResources.Select(ar => ar.ItemID).ToList();
                    }
                    else
                    {
                        var cc = filter.MarketFilter;
                        var getAvailableResources = ExigoDAL.GetResourceAvailabilities(new GetResourceAvailabilitiesRequest() { CountryCode = cc });
                        availableResources = getAvailableResources.Select(ar => ar.ItemID).ToList();
                    }
                }

                model.ResourceList = model.ResourceList.Where(x => availableResources.Contains(x.ItemID));

                if (!model.IsResourceManager)
                {
                    model.ResourceList = model.ResourceList.Where(c => c.StatusID == ExigoDAL.ResourceStatuses.Active && (c.PostDate <= DateTime.Now.ToCST() || c.PostDate == null)).ToList();
                }

                switch (sortType)
                {
                    case "asc":
                        model.ResourceList = model.ResourceList.OrderBy(c => c.CreatedDate);
                        break;
                    case "desc":
                        model.ResourceList = model.ResourceList.OrderByDescending(c => c.CreatedDate);
                        break;
                    default:
                        model.ResourceList = model.ResourceList.OrderBy(c => c.ItemOrder);
                        break;
                }

                if (filter.MediaFilter != null && filter.MediaFilter.Count() > 0)
                {
                    var typeIDs = model.Types.Select(v => v.TypeID);
                    model.ResourceList = model.ResourceList.Where(x => typeIDs.Contains(x.TypeID)).ToList();
                }

                if (filter.KeyWord != null && filter.KeyWord.Any())
                {
                    var keyword = filter.KeyWord;
                    var tag = ExigoDAL.GetTagsForResources(new GetTagsForResourcesRequest() { Names = keyword }).FirstOrDefault();
                    if (tag != null)
                    {
                        var resourceitemtagIDs = ExigoDAL.GetResourceItemTags(new GetResourceItemTagsRequest() { TagID = tag.TagID }).AsEnumerable().Select(rt => rt.ItemID);

                        model.ResourceList = model.ResourceList.Where(rl => resourceitemtagIDs.Contains(rl.ItemID));
                    }
                }

                var html = this.RenderPartialViewToString("Partials/_CategoryResourceItems", model);

                return new JsonNetResult(new
                {
                    success = true,
                    html = html
                });
            }
            catch (Exception ex)
            {
                return new JsonNetResult(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }




        public class TranslatedCategory
        {
            public string TranslatedCategoryDescription { get; set; }
            public string Language { get; set; }
        }

        public class Available
        {
            public string Market { get; set; }
            public string Language { get; set; }
        }
    }
}
