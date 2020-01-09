using Common;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace ExigoService
{
    public static partial class ExigoDAL
    {
        #region Dynamic Kit Categories
        private static IEnumerable<DynamicKitCategory> GetAllDynamicKitCategories()
        {
            var cacheKey = $"GetAllDynamicKitCategories";
            if (!MemoryCache.Default.Contains(cacheKey))
            {
                using (var context = ExigoDAL.Sql())
                {
                    var categories = context.Query<DynamicKitCategory>(@"
                        SELECT 
                            mi.ItemCode AS MasterItemCode
                            , idkcm.DynamicKitCategoryID
                            , idkc.DynamicKitCategoryDescription
                            , idkcm.Quantity
                        FROM 
	                        ItemDynamicKitCategoryMembers AS idkcm
		                        LEFT JOIN Items AS mi
			                        ON mi.ItemID = idkcm.MasterItemID
		                        LEFT JOIN ItemDynamicKitCategories AS idkc
			                        ON idkc.DynamicKitCategoryID = idkcm.DynamicKitCategoryID
                    ", new { 
                    });
                    MemoryCache.Default.Add(cacheKey, categories, DateTime.Now.AddMinutes(GlobalSettings.Caching.CacheTimeouts.Short));
                }
            }

            // get the cache
            var data = MemoryCache.Default.Get(cacheKey) as List<DynamicKitCategory>;
            if (data == null) { yield return null; }

            // return the items
            foreach (var category in data)
            {
                yield return category;
            }
        }
        public static List<DynamicKitCategory> GetDynamicKitCategories(List<string> masterItemCodes)
        {
            var categories = GetAllDynamicKitCategories()
                .Where(i => masterItemCodes.Contains(i.MasterItemCode))
                .ToList();

            var clonedCategories = new List<DynamicKitCategory>();
            foreach (var category in categories)
            {
                clonedCategories.Add(category.DeepClone());
            }
            return clonedCategories;
        }
        #endregion

        #region Dynamic Kit Category Items
        public static IEnumerable<DynamicKitCategoryItem> GetAllDynamicKitCategoryItems()
        {
            var cacheKey = $"GetAllDynamicKitCategoryItems";
            if (!MemoryCache.Default.Contains(cacheKey))
            {
                using (var context = ExigoDAL.Sql())
                {
                    var categoryItems = context.Query<DynamicKitCategoryItem>(@"
                        SELECT 
	                        idkcim.DynamicKitCategoryID
	                        , i.ItemCode
                        FROM 
	                        ItemDynamicKitCategoryItemMembers AS idkcim
		                        LEFT JOIN Items AS i
			                        ON i.ItemID = idkcim.ItemID
                    ", new { 
                    }).ToList();
                    MemoryCache.Default.Add(cacheKey, categoryItems, DateTime.Now.AddMinutes(GlobalSettings.Caching.CacheTimeouts.Short));
                }
            }

            // get the cache
            var data = MemoryCache.Default.Get(cacheKey) as List<DynamicKitCategoryItem>;
            if (data == null) { yield break; }

            foreach (var catItem in data)
            {
                yield return catItem;
            }
        }
        public static List<DynamicKitCategoryItem> GetDynamicKitCategoryItems(List<int> dynamicKitCategoryIDs)
        {
            var categoryItems = GetAllDynamicKitCategoryItems()
                .Where(ci => dynamicKitCategoryIDs.Contains(ci.DynamicKitCategoryID))
                .ToList();

            var clonedCategories = new List<DynamicKitCategoryItem>();
            foreach (var item in categoryItems)
            {
                clonedCategories.Add(item.DeepClone());
            }
            return clonedCategories;
        }
        #endregion
    }
}