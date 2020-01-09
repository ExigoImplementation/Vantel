using Common;
using Common.Exceptions;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace ExigoService
{
    public static partial class ExigoDAL
    {
        public static IEnumerable<Item> GetItems(GetItemsRequest request)
        {
            // If we don't have what we need to make this call, stop here.
            if (request.Configuration == null)
                throw new InvalidRequestException("ExigoService.GetItems() requires an OrderConfiguration.");

            if (request.Configuration.CategoryID == 0 && !request.CategoryID.HasValue && !request.ItemCodes.Any())
                throw new InvalidRequestException("ExigoService.GetItems() requires either a CategoryID or a collection of item codes."); ;

            #region Categories
            // Set some defaults
            var topCategoryID = request.CategoryID ?? request.Configuration.CategoryID;

            var categoryIDs = new List<int>() { topCategoryID };
            
            // Get all category ids underneath the request's category id
            if (request.IncludeAllChildCategories)
            {
                var childCategoryIDs = GetChildCategoryIDs(topCategoryID);
                categoryIDs.AddRange(childCategoryIDs);
            }
            // if no categories, stop here (this should never happen. included for consistency)
            if (!categoryIDs.Any()) { yield break; }
            #endregion

            #region Item Codes
            // If we requested specific categories, get the item codes in the categories
            var itemCodes = new List<string>();
            var hasCartItems = request.ShoppingCartItems.Any();

            // if we have cart items, use those first
            if (hasCartItems)
            {
                var parentItems = request.ShoppingCartItems
                    .ToList();
                var parentItemCodes = parentItems
                    .Select(i => i.ItemCode)
                    .Distinct()
                    .ToList();
                itemCodes.AddRange(parentItemCodes);
            }
            // get and/or filter itemcodes
            else
            {
                var availableItemCodes = ExigoDAL.GetWebCategoryItemCodes(categoryIDs);
                if (request.ItemCodes.Any())
                {

                    // if we have no cart items, decide whether to restrict or not.
                    if (request.IgnoreCategoryRestrictions)
                    {
                        // If we're ignoring category restrictions, get the item codes requested only. 
                        // (This should only be done for [dynamic/static] kit children. Items must still be available in the requested warehouse)
                        itemCodes.AddRange(request.ItemCodes);
                    }
                    else
                    {
                        // Default behavior: filter request item codes by the item codes available in the selected categories
                        var filteredItemCodes = availableItemCodes.Where(c => request.ItemCodes.Any(rc => rc == c));
                        itemCodes.AddRange(filteredItemCodes);
                    }
                }
                else
                {
                    // if no item codes are requested, add all available item codes
                    itemCodes.AddRange(availableItemCodes);
                }
            }

            // If we don't have any item codes, stop here. (This will happen if request.ItemCodes are not available in the categories requested)
            if (!itemCodes.Any()) yield break;
            #endregion

            #region Items
            // get the item information             
            var priceTypeID = (request.PriceTypeID.HasValue && request.PriceTypeID.Value > 0) ? request.PriceTypeID.Value : request.Configuration.PriceTypeID;
            var items = GetItemInformation(new GetItemInformationRequest (request) {
                PriceTypeID = priceTypeID,
                ItemCodes = itemCodes
            });
            // If we don't have any items, stop here.
            if (!items.Any()) yield break;
            #endregion

            #region Additional Details
            // Populate the group members and dynamic kits
            PopulateAdditionalItemData(new PopulateAdditionalItemDataRequest(request) {
                Items = items,
                WebCategoryIDs = categoryIDs
            });
            #endregion

            // if we provided shopping cart items, return our items relative to our shoppingCartItems. 
            // (Note: This process will duplicate items if there were both order and autoorder items of the same code. This is necessary and expected.)
            if (hasCartItems)
            {
                items = MergeShoppingCartItemDetailsWith<Item>(request.ShoppingCartItems, items).ToList();
            }

            // Return the data
            foreach (var item in items)
            {
                yield return item;
            }
        }
        public static List<Item> GetItems(IEnumerable<ShoppingCartItem> shoppingCartItems, IOrderConfiguration configuration, int languageID, int _priceTypeID = 0)
        {
            var results = new List<Item>();

            // If we don't have what we need to make this call, stop here.
            if (configuration == null)
                throw new InvalidRequestException("ExigoService.GetItems() requires an OrderConfiguration.");

            if (shoppingCartItems.Count() == 0)
                return results;


            // Create the contexts we will use
            var priceTypeID = (_priceTypeID > 0) ? _priceTypeID : configuration.PriceTypeID;

            var itemcodes = new List<string>();

            shoppingCartItems.ToList().ForEach(c => itemcodes.Add(c.ItemCode));

            var apiItems = GetItemInformation(new GetItemInformationRequest(configuration, languageID) {
                ItemCodes = itemcodes,
                PriceTypeID = priceTypeID
            });

            // Populate the group members and dynamic kits
            if (apiItems.Any())
            {
                var request = new GetItemsRequest
                {
                    LanguageID = languageID,
                    Configuration = configuration
                };
                PopulateAdditionalItemData(new PopulateAdditionalItemDataRequest(configuration, languageID)
                {
                    Items = apiItems
                });
            }

            foreach (var cartItem in shoppingCartItems)
            {
                var apiItem = apiItems.FirstOrDefault(i => i.ItemCode == cartItem.ItemCode);

                if (apiItem != null)
                {
                    var newItem = (Item)apiItem.DeepClone();
                    cartItem.CopyPropertiesTo<IShoppingCartItem>(newItem);
                    results.Add(newItem);
                }
            }

            // Return the data
            return results;
        }
        private static List<Item> GetItemInformation(GetItemInformationRequest request)
        {
            using (var context = ExigoDAL.Sql())
            {
                var sql = $@"
                	SELECT
	                    i.ItemID
	                    , i.ItemCode
	                    , ItemDescription = 
		                    CASE 
			                    WHEN i.ItemTypeID = {ItemTypes.Standard} AND i.IsGroupMaster = 1 
                                THEN COALESCE( NULLIF(i.GroupDescription,''), NULLIF(il.ItemDescription,''), i.ItemDescription)
			                    ELSE COALESCE( NULLIF(il.ItemDescription, ''), i.ItemDescription)
		                    END
	                    , i.Weight
	                    , i.ItemTypeID
	                    , TinyImageUrl  = i.TinyImageName
	                    , SmallImageUrl = i.SmallImageName
	                    , LargeImageUrl = i.LargeImageName";
                if (request.IncludeShortDescriptions)
                {
                    sql += @"
	                    , ShortDetail1  = COALESCE( NULLIF(il.ShortDetail ,''), i.ShortDetail)
	                    , ShortDetail2  = COALESCE( NULLIF(il.ShortDetail2,''), i.ShortDetail2)
	                    , ShortDetail3  = COALESCE( NULLIF(il.ShortDetail3,''), i.ShortDetail3)
	                    , ShortDetail4  = COALESCE( NULLIF(il.ShortDetail4,''), i.ShortDetail4)";
                }
                else
                {
                    // populate web errorcode 403 "forbidden" if not requested
                    sql += @"
	                    , ShortDetail1  = 'Web:403'
	                    , ShortDetail2  = 'Web:403'
	                    , ShortDetail3  = 'Web:403'
	                    , ShortDetail4  = 'Web:403'";
                }
                if (request.IncludeLongDescriptions)
                {
                    sql += @"
	                    , LongDetail1   = COALESCE( NULLIF(il.LongDetail  ,''), i.LongDetail)
	                    , LongDetail2   = COALESCE( NULLIF(il.LongDetail2 ,''), i.LongDetail2)
	                    , LongDetail3   = COALESCE( NULLIF(il.LongDetail3 ,''), i.LongDetail3)
	                    , LongDetail4   = COALESCE( NULLIF(il.LongDetail4 ,''), i.LongDetail4)";
                }
                else
                {
                    // populate web errorcode 403 "forbidden" if not requested
                    sql += @"
	                    , LongDetail1   = 'Web:403'
	                    , LongDetail2   = 'Web:403'
	                    , LongDetail3   = 'Web:403'
	                    , LongDetail4   = 'Web:403'";
                }
                sql += $@"
	                    , i.IsVirtual
	                    , i.AllowOnAutoOrder
	                    , IsGroupMaster = CAST(CASE WHEN i.ItemTypeID = {ItemTypes.Standard} AND i.IsGroupMaster = 1 THEN 1 ELSE 0 END AS BIT)
                        , i.GroupMembersDescription
	                    , IsDynamicKitMaster = CAST(CASE WHEN i.ItemTypeID = {ItemTypes.DynamicKit} THEN 1 ELSE 0 END AS BIT)";
                if (request.IncludeExtendedFields)
                {
                    sql += @"
	                    , i.Field1
                        , i.Field2
	                    , i.Field3
	                    , i.Field4
	                    , i.Field5
	                    , i.Field6
	                    , i.Field7
	                    , i.Field8
	                    , i.Field9
	                    , i.Field10";
                }
                else
                {
                    // populate web errorcode 403 "forbidden" if not requested
                    sql += @"
	                    , Field1       = 'Web:403'
                        , Field2       = 'Web:403'
	                    , Field3       = 'Web:403'
	                    , Field4       = 'Web:403'
	                    , Field5       = 'Web:403'
	                    , Field6       = 'Web:403'
	                    , Field7       = 'Web:403'
	                    , Field8       = 'Web:403'
	                    , Field9       = 'Web:403'
	                    , Field10      = 'Web:403'";
                }
                sql += @"
                        , OtherCheck1  = i.OtherCheck1
	                    , OtherCheck2  = i.OtherCheck2
	                    , OtherCheck3  = i.OtherCheck3
	                    , OtherCheck4  = i.OtherCheck4
	                    , OtherCheck5  = i.OtherCheck5
	                    , ip.Price
	                    , ip.CurrencyCode
	                    , BV           = ip.BusinessVolume
	                    , CV           = ip.CommissionableVolume";
                if (request.IncludeExtendedlItemPrices)
                {
                    sql += @"
                        , OtherPrice1  = ip.Other1Price
	                    , OtherPrice2  = ip.Other2Price
	                    , OtherPrice3  = ip.Other3Price
	                    , OtherPrice4  = ip.Other4Price
	                    , OtherPrice5  = ip.Other5Price
	                    , OtherPrice6  = ip.Other6Price
	                    , OtherPrice7  = ip.Other7Price
	                    , OtherPrice8  = ip.Other8Price
	                    , OtherPrice9  = ip.Other9Price
	                    , OtherPrice10 = ip.Other10Price";
                }
                else
                {
                    // populate -1 if not requested
                    sql += @"
                        , OtherPrice1  = -1
	                    , OtherPrice2  = -1
	                    , OtherPrice3  = -1
	                    , OtherPrice4  = -1
	                    , OtherPrice5  = -1
	                    , OtherPrice6  = -1
	                    , OtherPrice7  = -1
	                    , OtherPrice8  = -1
	                    , OtherPrice9  = -1
	                    , OtherPrice10 = -1";
                }
                sql += @"
                    FROM Items AS i
	                    LEFT JOIN ItemPrices AS ip
		                    ON ip.ItemID = i.ItemID
	                    LEFT JOIN ItemWarehouses AS iw
		                    ON iw.ItemID = i.ItemID
						LEFT JOIN ItemLanguages AS il
		                    ON il.ItemID = i.ItemID
						    AND il.LanguageID = @languageID
					WHERE 
                        i.ItemCode in @itemCodes
		                AND iw.WarehouseID = @warehouse
                        AND ip.PriceTypeID = @priceTypeID
						AND ip.CurrencyCode = @currencyCode";
                var apiItems = context.Query<Item>(sql
                    , new {
                        warehouse     = request.WarehouseID,
                        priceTypeID   = request.PriceTypeID,
                        currencyCode  = request.CurrencyCode,
                        languageID    = request.LanguageID,
                        itemCodes     = request.ItemCodes
                    }).ToList();
                return apiItems;
            }
        }

        // Calls to populate additional data
        private static void PopulateAdditionalItemData(PopulateAdditionalItemDataRequest request)
        {
            GlobalUtilities.RunAsyncTasks(
                () => { PopulateItemImages(request.Items); },
                () => {
                    if (request.IncludeGroupMembers)
                    {
                        try
                        {
                            PopulateGroupMembers(request.Items, request);
                        }
                        catch (Exception ex)
                        {
                            // populating group members should not stop parent process as this executes asynchronously
                            // TODO: logging should be added here
                        }
                    }
                },
                () => {
                    if (request.IncludeStaticKitChildren)
                    {
                        try
                        {
                            PopulateStaticKitMembers(request.Items, request);
                        }
                        catch (Exception ex)
                        {
                            // populating static kit members should not stop parent process as this executes asynchronously
                            // TODO: logging should be added here
                        }
                    }
                },
                () =>
                {
                    if (request.IncludeDynamicKitChildren)
                    {
                        try
                        {
                            PopulateDynamicKitMembers(request.Items, request);
                        }
                        catch (Exception ex)
                        {
                            // populating dynamic kit members should not stop parent process as this executes asynchronously
                            // TODO: logging should be added here
                        }
                    }
                },
                () => { PopulateMasterSortOrder(request.Items, request.WebCategoryIDs); }
            );
        }
        private static void PopulateItemImages(IEnumerable<Item> items)
        {
            foreach (var item in items)
            {
                item.TinyImageUrl = GlobalUtilities.GetProductImagePath(item.TinyImageUrl);
                item.SmallImageUrl = GlobalUtilities.GetProductImagePath(item.SmallImageUrl);
                item.LargeImageUrl = GlobalUtilities.GetProductImagePath(item.LargeImageUrl);
            }
        }
        private static void PopulateGroupMembers(IEnumerable<Item> items, IItemInformationRequest request, int maxDepth = GlobalSettings.Items.MaxGroupItemRecursion)
        {
            // ensure max recursion is not exceeded
            var _maxDepth = maxDepth <= GlobalSettings.Items.MaxGroupItemRecursion ? maxDepth : GlobalSettings.Items.MaxGroupItemRecursion;
            // if recursion is exceeded, stop
            if (_maxDepth <= 0) { return; }

            // Determine if we have any group master items
            // Group masters should ONLY be standard items.
            // Static kits cannot have a dynamic component and do not add group children in admin or through the API.
            // Dynamic kits can accomplish group member functionality through configuring categories with qty of 1 so should not use groups directly.
            var groupMasterItems = items
                .Where(i => i.ItemTypeID == ItemTypes.Standard)
                .Where(c => c.IsGroupMaster).ToList();
            // if no group masters, stop here
            if (!groupMasterItems.Any()) { return; }

            // get the master itemcodes
            var groupMasterItemCodes = groupMasterItems
                .Select(c => c.ItemCode)
                .Distinct()
                .ToList();
            // Get a list of group member items for all the group master items
            var itemGroupMembers = new List<ItemGroupMember>();

            using (var context = ExigoDAL.Sql())
            {
                itemGroupMembers = context.Query<ItemGroupMember>(@"
                    SELECT 
	                    mi.ItemCode                  AS MasterItemCode
	                    , ci.ItemCode
	                    , igm.GroupMemberDescription AS MemberDescription
	                    , igm.[Priority]             AS SortOrder
                    FROM 
                        ItemGroupMembers AS igm
	                        LEFT JOIN Items AS mi
		                        ON mi.ItemID = igm.MasterItemID
	                        LEFT JOIN Items AS ci
		                        ON ci.ItemID = igm.ItemID 
                    WHERE
                        mi.ItemCode IN @masterItemCodes
                ", new {
                    masterItemCodes = groupMasterItemCodes
                }).ToList();
            }
            // if no group members, stop here
            if (!itemGroupMembers.Any()) { return; }


            // get the member itemcodes
            var memberItemCodes = itemGroupMembers
                .Select(m => m.ItemCode)
                .Distinct()
                .ToList();

            // get the item details
            // Items are returned based on their availability 
            var apiItems = GetItemInformation(new GetItemInformationRequest(request)
            {
                ItemCodes = memberItemCodes
            });

            // populate image urls.
            PopulateItemImages(apiItems);

            // attempt to get additional group layers
            PopulateGroupMembers(apiItems, request, _maxDepth - 1);

            //bind the item group members to the group master items               
            foreach (var groupMasterItem in groupMasterItems)
            {
                // get the group members for this master item
                var members = itemGroupMembers
                    .Where(m => m.MasterItemCode == groupMasterItem.ItemCode)
                    .Where(m => m.ItemCode != groupMasterItem.ItemCode)// suppress group masters from their own members
                    .OrderBy(m => m.SortOrder)
                    .ToList();

                foreach (var member in members)
                {
                    // get the apiItem for this group member
                    var apiItem = apiItems
                        .FirstOrDefault(mi => mi.ItemCode == member.ItemCode);
                    // if no item found, skip this member as it was not available
                    if(apiItem == null) { continue; }
                    // create a copy for the member
                    member.Item = apiItem.DeepClone();
                    // Add ShoppingCartItem flag for master group item
                    member.Item.GroupMasterItemCode = groupMasterItem.ItemCode;
                    // add to the master item
                    groupMasterItem.GroupMembers.Add(member);
                }
            }
        }
        private static void PopulateStaticKitMembers(IEnumerable<Item> items, IItemInformationRequest request, int maxDepth = GlobalSettings.Items.MaxStaticKitRecursion)
        {
            // ensure max recursion is not exceeded
            var _maxDepth = maxDepth <= GlobalSettings.Items.MaxStaticKitRecursion ? maxDepth : GlobalSettings.Items.MaxStaticKitRecursion;
            // if recursion is exceeded, stop
            if(_maxDepth <= 0) { return; }

            // Determine if we have any Static Kits
            var staticKitMasterItems = items
                .Where(i => i.ItemTypeID == ItemTypes.StaticKit)
                .ToList();
            // if no static kits, stop here
            if (!staticKitMasterItems.Any()) { return; }


            // get the master itemcodes
            var staticKitMasterItemCodes = staticKitMasterItems
                .Select(c => c.ItemCode)
                .Distinct()
                .ToList();
            // Get a list of group member items for all the group master items
            var itemStaticKitMembers = new List<ItemStaticKitMember>();

            using (var context = ExigoDAL.Sql())
            {
                itemStaticKitMembers = context.Query<ItemStaticKitMember>(@"
                    SELECT 
	                    mi.ItemCode AS MasterItemCode
	                    ,ci.ItemCode
	                    ,iskm.Quantity
                    FROM 
	                    ItemStaticKitMembers AS iskm
		                    LEFT JOIN Items AS mi
			                    ON mi.ItemID = iskm.MasterItemID
		                    LEFT JOIN Items AS ci
			                    ON ci.ItemID = iskm.ItemID
                    WHERE
                        mi.ItemCode IN @masterItemCodes
                ", new
                {
                    masterItemCodes = staticKitMasterItemCodes
                }).ToList();
            }
            // if no kit members, stop here
            if (!itemStaticKitMembers.Any()) { return; }

            // get the member itemcodes
            var memberItemCodes = itemStaticKitMembers
                .Select(m => m.ItemCode)
                .Distinct()
                .ToList();

            // get the item details
            // Items are returned based on their availability 
            var apiItems = GetItemInformation(new GetItemInformationRequest(request)
            {
                ItemCodes = memberItemCodes
            });

            // Static kit members can ONLY be standard items. (Order-calc cannot handle [tax|shipping] calculation on kit children beyond 1 level of children as of 2019/03/14)
            apiItems = apiItems.Where(i => i.ItemTypeID == ItemTypes.Standard).ToList();

            // populate image urls.
            PopulateItemImages(apiItems);

            // attempt to get additional kit layers
            PopulateStaticKitMembers(apiItems, request, _maxDepth - 1);

            // bind the item Kit members to the kit master items               
            foreach (var staticKitMasterItem in staticKitMasterItems)
            {
                // get the kit members for this master item
                var members = itemStaticKitMembers
                    .Where(m => m.MasterItemCode == staticKitMasterItem.ItemCode)
                    // .OrderBy(m => m.SortOrder) no default sort order for kit items at this time
                    .ToList();

                foreach (var member in members)
                {
                    // get the apiItem for this kit member
                    var apiItem = apiItems
                        .FirstOrDefault(mi => mi.ItemCode == member.ItemCode);
                    // if no item found, skip this member as it was not available
                    if (apiItem == null) { continue; }
                    // create a copy of the kit member
                    var kitMember = apiItem.DeepClone();
                    // add ShoppingCartItem flag for the kit member's quantity (NOT USED FOR ADDING TO CART)
                    kitMember.Quantity = member.Quantity;
                    // add to the master item
                    staticKitMasterItem.StaticKitChildren.Add(kitMember);
                }
            }
        }
        private static void PopulateDynamicKitMembers(IEnumerable<Item> items, IItemInformationRequest request)
        {
            // Dynamic Kits should never have recursion

            // Determine if we have any Dynamic Kits
            var dynamicKitMasterItems = items
                .Where(i => i.ItemTypeID == ItemTypes.DynamicKit)
                .ToList();
            // if no static kits, stop here
            if (!dynamicKitMasterItems.Any()) { return; }


            // get the master item codes
            var dynamicKitMasterItemCodes = dynamicKitMasterItems
                .Select(c => c.ItemCode)
                .Distinct()
                .ToList();

            // Get a list of group member items for all the group master items
            var dynamicKitCategories = GetDynamicKitCategories(dynamicKitMasterItemCodes);
            // if no kit categories, stop here
            if (!dynamicKitCategories.Any()) { return; }

            var dynamicKitCategoryIDs = dynamicKitCategories
                .Select(c => c.DynamicKitCategoryID)
                .Distinct()
                .ToList();
            var dynamicKitCategoryItems = GetDynamicKitCategoryItems(dynamicKitCategoryIDs);
            // if no kit category items, stop here
            if (!dynamicKitCategoryItems.Any()) { return; }

            // get the member itemcodes
            var memberItemCodes = dynamicKitCategoryItems
                .Select(m => m.ItemCode)
                .Distinct()
                .ToList();

            // get the item details
            // Items are returned based on their availability 
            var apiItems = GetItemInformation(new GetItemInformationRequest(request)
            {
                ItemCodes = memberItemCodes
            });

            // Dynamic kit members can ONLY be standard items. (Order-calc cannot handle [tax|shipping] calculation on kit children beyond 1 level of children as of 2019/03/14)
            apiItems = apiItems.Where(i => i.ItemTypeID == ItemTypes.Standard).ToList();

            // populate image urls.
            PopulateItemImages(apiItems);

            // bind the kit categories to their master items
            foreach (var dynamicKitMaster in dynamicKitMasterItems)
            {
                // get the matching kit categories
                var categories = dynamicKitCategories
                    .Where(dkc => dkc.MasterItemCode == dynamicKitMaster.ItemCode)
                    // .OrderBy(m => m.SortOrder) no default sort order for dynamic kit categories at this time
                    .ToList();

                // bind the item Kit members to their categories 
                foreach (var dynamicKitCategory in categories)
                {
                    var cat = dynamicKitCategory.DeepClone();
                    // get the kit members for this master item
                    var members = dynamicKitCategoryItems
                        .Where(dki => dki.DynamicKitCategoryID == dynamicKitCategory.DynamicKitCategoryID)
                        // .OrderBy(m => m.SortOrder) no default sort order for dynamic kit items at this time
                        .ToList();

                    foreach (var member in members)
                    {
                        // get the apiItem for this kit member
                        var apiItem = apiItems
                            .FirstOrDefault(mi => mi.ItemCode == member.ItemCode);
                        // if no item found, skip this member as it was not available
                        if (apiItem == null) { continue; }
                        // create a copy of the kit member
                        var kitMember = apiItem.DeepClone();
                        // add to the item to the category
                        cat.Items.Add(kitMember);
                    }
                    // add the category to the master
                    dynamicKitMaster.DynamicKitCategories.Add(cat);
                }
            }
        }
        private static void PopulateMasterSortOrder(IEnumerable<IItemMasterSort> items, IEnumerable<int> categoryIDs)
        {
            // If no category IDs, stop here
            if (categoryIDs == null || !categoryIDs.Any()) { return; }
            // Get Master sort order records that match the category id's
            var filteredMasterItemSort = GetMasterItemSortOrder()
                .Where(c => categoryIDs.Contains(c.WebCategoryID))
                .ToList();
            //  If no Master sort records found, return
            if (!filteredMasterItemSort.Any()) { return; }

            // Attempt to update each item
            foreach (var item in items)
            {
                // Get the master item sorts that match this itemcode
                var masteritemSort = filteredMasterItemSort.Where(c => string.Equals(c.ItemCode, item.ItemCode, StringComparison.InvariantCultureIgnoreCase)).ToList();
                // If no relative item sort exists, continue to next item
                if (!masteritemSort.Any()) { continue; }

                // If everything aligned, assign the highest available master item sort ID to the item
                item.MasterSortID = masteritemSort
                    .OrderBy(c => c.MasterSortID)
                    .Select(c => c.MasterSortID)
                    .FirstOrDefault();
            }

        }
        private static List<MasterSortItem> GetMasterItemSortOrder(int? webID = null)
        {
            var _webID = webID ?? GlobalSettings.Items.WebID;
            var cacheKey = $"GetMasterItemSortOrder{_webID}";
            if (!MemoryCache.Default.Contains(cacheKey))
            {

                using (var context = ExigoDAL.Sql())
                {
                    var sortItems = context.Query<MasterSortItem>(@"
			            ;WITH WebCat AS (
				            SELECT 
					            WebCategoryID
					            , ParentID
					            , CatSortHirearchy = CAST(1000 + SortOrder AS VARCHAR(255)) 
                                , WebID
				            FROM
					            WebCategories 
				            WHERE 
					            ParentID IS NULL
                                AND WebID = @webID

				            UNION ALL

				            SELECT 
					            child.WebCategoryID
					            , child.ParentID
					            , CatSortHirearchy =  CAST(CatSortHirearchy + '.' + CAST(1000 + child.SortOrder AS VARCHAR(255)) AS VARCHAR(255))
                                , child.WebID
				            FROM 
					            WebCat AS parent
					            INNER JOIN WebCategories AS child
						            ON child.ParentID = parent.WebCategoryID 
                                    AND child.WebID = parent.WebID 
			            )

			            SELECT 
				            MasterSortID = ROW_NUMBER() over (order by wc.CatSortHirearchy, wci.SortOrder)
				            , wc.WebCategoryID
				            , i.ItemCode
			            FROM WebCat AS wc 
				            LEFT JOIN WebCategoryItems AS wci
					            ON wc.WebCategoryID = wci.WebCategoryID
								AND wc.WebID = wci.WebID
				            LEFT JOIN Items AS i
					            on wci.ItemID = i.ItemID
		            ", new
                    {
                        webID = webID
                    }).ToList();
                    MemoryCache.Default.Add(cacheKey, sortItems, DateTime.Now.AddMinutes(GlobalSettings.Caching.CacheTimeouts.Short));
                }
            }
            var data = (List<MasterSortItem>)MemoryCache.Default.Get(cacheKey);
            return data;
        }

        /// <summary>
        /// Creates a new list of <T> for each item in cartitems using the details in DetailItems.
        /// <para>Does NOT change either list</para>
        /// <para>Output must be saved to capture results</para>
        /// </summary>
        /// <typeparam name="T">The resulting item type</typeparam>
        /// <param name="cartItems">List of shoppingcartItems. This is the master which will generate the final list</param>
        /// <param name="detailItems">List of detail items that implement IShoppingCartItem</param>
        /// <returns></returns>
        private static IEnumerable<T> MergeShoppingCartItemDetailsWith<T>(IEnumerable<IShoppingCartItem> cartItems, IEnumerable<T> detailItems)
            where T : IShoppingCartItem
        {
            var items = new List<T>();

            // if not enough data, stop
            if (cartItems == null || !cartItems.Any()) yield break;
            if (detailItems == null || !detailItems.Any()) yield break;

            // Since cart items may have more than one instance of and item, use cart item as the master for creating the final list.
            // (Ex. item  as a single is also  a member of a kit so would show twice. Only one detail is needed to populate these 2 items)
            foreach (var item in cartItems)
            {
                // find the matching detail item
                var matchingItem = detailItems.FirstOrDefault(i => i.ItemCode == item.ItemCode);
                // if none found, skip
                if (matchingItem == null) continue;
                // create a unique copy
                var newItem = matchingItem.DeepClone();
                // copy our cart item details over
                item.CopyPropertiesTo<IShoppingCartItem>(newItem);
                // add to list
                items.Add(newItem);
            }
            if (!items.Any()) yield break;
            foreach (var item in items)
            {
                yield return item;
            }
        }
    }
}