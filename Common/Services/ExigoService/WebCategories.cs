using Common;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace ExigoService
{
    public static partial class ExigoDAL
    {
        #region Web Categories
        /// <summary>
        /// Gets and caches all Web categories for a specific WebID
        /// </summary>
        /// <param name="webID">Override for Default WebID. Defaults to GlobalSettings.Items.WebID if none is provided.</param>
        /// <returns></returns>
        private static List<WebCategory> GetAllWebCategories(int? webID)
        {
            var _webID = webID ?? GlobalSettings.Items.WebID;
            var cacheKey = $"GetAllWebCategories_{_webID}";
            if (!MemoryCache.Default.Contains(cacheKey))
            {
                using (var context = ExigoDAL.Sql())
                {
                    var webCategories = context.Query<WebCategory>(@"
                        SELECT
                            WebID
                            , WebCategoryID
                            , ParentID
                            , WebCategoryDescription
                            , NestedLevel
                            , SortOrder
                        FROM
                            WebCategories
                        WHERE
                            WebID = @webID
                    ", new
                    {
                        webID = _webID
                    }).ToList();

                    MemoryCache.Default.Add(cacheKey, webCategories, DateTime.Now.AddMinutes(GlobalSettings.Caching.CacheTimeouts.Long));
                }
            }
            var data = MemoryCache.Default.Get(cacheKey) as List<WebCategory>;
            return data;
        }

        /// <summary>
        /// Gets web categories for the specified parentCategoryID
        /// </summary>
        /// <param name="request"></param>
        /// <param name="categoryList">(DO NOT USE)Internal variable used to pass master list of categories for recursive calls.  </param>
        /// <returns></returns>
        public static List<WebCategory> GetChildWebCategories(GetChildWebCategoriesRequest request, List<WebCategory> categoryList = null)
        {
            // get all categories
            var allCategories = categoryList ?? GetAllWebCategories(request.WebID);
            // get the children
            var webCategories = allCategories
                .Where(c => c.ParentID == request.TopCategoryID)
                // adds sort order upon getting the initial list of child categories. 
                // If sorting at any point after this, sort order is not guaranteed across parent/child
                .OrderBy(c => c.SortOrder) 
                .ToList();

            // if requesting all children: populate children recursively
            if (request.GetAllChildCategories)
            {
                // create a temp list for holding grandchildren
                var childCategories = new List<WebCategory>();

                // get the children of each child (grandchildren)
                foreach (var child in webCategories)
                {
                    // set up request to get grandchildren
                    var grandchildRequest = new GetChildWebCategoriesRequest(request);
                    grandchildRequest.TopCategoryID = child.WebCategoryID;
                    var grandchildren = ExigoDAL.GetChildWebCategories(grandchildRequest, allCategories);

                    // if flat list is requested: Add to a temporary list 
                    if (request.NestedType == NestedType.UniLevel)
                    {
                        childCategories.AddRange(grandchildren);
                    }

                    // if nesting is requested: add grandchildren to the child's subcategories 
                    if (request.NestedType == NestedType.Nested)
                    {
                        child.Subcategories = grandchildren;
                    }
                }

                // if flat list is requested: add temp list of grandchildren to our original list.
                if (request.NestedType == NestedType.UniLevel)
                {
                    webCategories.AddRange(childCategories);
                }
            }

            // return children
            return webCategories;
        }
        public static List<int> GetChildCategoryIDs(int parentCategoryID)
        {
            var categories = GetChildWebCategories(new GetChildWebCategoriesRequest
            {
                TopCategoryID = parentCategoryID,
                GetAllChildCategories = true,
                NestedType = NestedType.UniLevel,
            });
            var ids = categories.Select(c => c.WebCategoryID)
                .Distinct()
                .ToList();
            return ids;
        }
        #endregion

        #region Web Category Items
        public static List<WebCategoryItem> GetWebCategoryItems(int WebCategoryID, int? webID = null)
        {
            var _webID = webID ?? GlobalSettings.Items.WebID;
            var cacheKey = $"GetWebCategoryItems_{WebCategoryID}_{_webID}";
            if (!MemoryCache.Default.Contains(cacheKey))
            {
                using (var context = ExigoDAL.Sql())
                {
                    var webCategories = context.Query<WebCategoryItem>(@"
                        SELECT
                            wci.WebID
                            , wci.WebCategoryID
                            , i.ItemCode
                            , wci.SortOrder
                        FROM
                            WebCategoryItems AS wci
                                LEFT JOIN Items AS i
                                    ON i.ItemID = wci.ItemID
                        WHERE
                            wci.WebID = @webID
                            AND wci.WebCategoryID = @webCategoryID
                    ", new
                    {
                        webID = _webID,
                        webCategoryID = WebCategoryID
                    }).ToList();

                    MemoryCache.Default.Add(cacheKey, webCategories, DateTime.Now.AddMinutes(GlobalSettings.Caching.CacheTimeouts.Long));
                }
            }
            var data = MemoryCache.Default.Get(cacheKey) as List<WebCategoryItem>;
            return data;
        }

        public static List<string> GetWebCategoryItemCodes(List<int> WebCategoryIDs, int? webID = null)
        {
            var webCategoryItems = new List<WebCategoryItem>();
            foreach (var catID in WebCategoryIDs)
            {
                var wcItems = GetWebCategoryItems(catID, webID);
                wcItems = wcItems.OrderBy(wci => wci.SortOrder).ToList();
                webCategoryItems.AddRange(wcItems);
            }
            var itemCodes = webCategoryItems
                .Select(wci => wci.ItemCode)
                .Distinct()
                .ToList();
            return itemCodes;
        }
        #endregion
    }
}