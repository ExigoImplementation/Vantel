using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Backoffice.Controllers
{
    [RoutePrefix("customsort")]
    [Route("{action=index}")]
    public class CustomSortController : Controller
    {
        #region Resource Management
        [HttpPost]
        public ActionResult SaveResourceCategorySort(string parentID, SortValues[] sortValues)
        {
            try
            {
                var categories = new List<ResourceCategory>();
                var guidParentID = (parentID == "0") ? Guid.Empty : Guid.Parse(parentID);
                
                categories = ExigoDAL.GetResourceCategories(new GetResourceCategoriesRequest() { ParentID = guidParentID }).ToList();

                if (categories.Count() == 0)
                {
                    return new JsonNetResult(new { success = false });
                }

                foreach (var cat in categories)
                {
                    var valueItem = sortValues.Where(sv => sv.NodeID == cat.CategoryID).FirstOrDefault();
                    if (valueItem != null)
                    {
                        ResourceCategory modifyCategory = new ResourceCategory()
                        {
                            CategoryOrder = valueItem.SortIndex,
                            CategoryID = cat.CategoryID,
                            ParentID = guidParentID
                        };
                        ExigoDAL.ModifyResourceCategoryOrder(modifyCategory);
                    }
                }

                return new JsonNetResult(new
                {
                    success = true
                });
            }
            catch
            {
                return new JsonNetResult(new
                {
                    success = false
                });
            }


        }

        [HttpPost]
        public ActionResult SaveResourceItemSort(string parentID, SortValues[] sortValues)
        {
            try
            {
                List<ResourceCategoryItem> items;

                if (parentID != "0")
                {
                    items = ExigoDAL.GetResourceCategoryItems(new GetResourceCategoryItemsRequest() { CategoryID = Guid.Parse(parentID) }).ToList();
                    //context.ResourceCategoryItems.Where(c => c.CategoryID == Guid.Parse(parentID)).ToList();
                }
                else
                {
                    items = ExigoDAL.GetResourceCategoryItems(new GetResourceCategoryItemsRequest() { CategoryID = Guid.Empty }).ToList();
                    //context.ResourceCategoryItems.Where(c => c.CategoryID == null).ToList();
                }

                if (items.Count() == 0)
                {
                    return new JsonNetResult(new { success = false });
                }

                foreach (var item in items)
                {
                    var valueItem = sortValues.Where(v => v.NodeID == item.ItemID).FirstOrDefault();
                    if (valueItem != null)
                    {
                        ResourceCategoryItem modifyRCItem = new ResourceCategoryItem()
                        {
                            ItemOrder = valueItem.SortIndex,
                            ItemID = item.ItemID,
                            CategoryID = Guid.Parse(parentID)
                        };
                        ExigoDAL.ModifyResourceCategoryItemOrder(modifyRCItem);
                    }
                }

                return new JsonNetResult(new
                {
                    success = true
                });
            }
            catch
            {
                return new JsonNetResult(new
                {
                    success = false
                });
            }


        }
        #endregion
    }
}