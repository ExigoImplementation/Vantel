using Common.Models.ShoppingService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExigoService;
using Common.Exceptions;

namespace Common.Services
{
    public class ItemConverterService: IItemConverterService
    {
        public ItemConverterService(IOrderConfiguration orderConfig)
        {
            this._OrderConfiguration = orderConfig;
        }

        public IOrderConfiguration _OrderConfiguration{ get; private set; }
        /// <summary>
        /// Convert items and children into cart items
        /// </summary>
        /// <param name="parentItem"></param>
        /// <param name="parentQuantity"></param>
        /// <param name="childItems"></param>
        /// <returns></returns>
        public ShoppingCartItem GetShoppingCartItem(string parentItemCode, int parentQuantity, Dictionary<string, int> childItems = null)
        {
            if (parentItemCode.IsNullOrEmpty()){ throw new ArgumentNullException("parentItemCode cannot be null"); }
            // get the db version of the item for our market
            var items = ExigoDAL.GetItems(new GetItemsRequest
            {
                Configuration = this._OrderConfiguration,
                ItemCodes = new List<string> { parentItemCode },
                IgnoreCategoryRestrictions = true,
                IncludeStaticKitChildren = true,
                IncludeDynamicKitChildren = true
            }).ToList();
            // If too many or too few items found, stop.
            if (!items.Any()) { throw new ArgumentOutOfRangeException($"Item {parentItemCode} not available in {this._OrderConfiguration.DefaultCountryCode}"); }
            if (items.Count() != 1){ throw new ArgumentOutOfRangeException($"Too many items found matching {parentItemCode}"); }
            var _parentItem = items.First();

            // create our parent item
            ShoppingCartItem responseCartItem = new ShoppingCartItem(_parentItem) {
                Quantity = parentQuantity
            };

            #region populate dynamic kit children
            var ignoreStaticCategoryChildren = true; // remove children that would always be added as they're the only option. (reduce property bag data)
            if (_parentItem.ItemTypeID == ItemTypes.DynamicKit && _parentItem.DynamicKitCategories.Any())
            {
                var kitChildItems = new List<ShoppingCartKitItem>();
                // Run through each category and populate it's children
                foreach (var category in _parentItem.DynamicKitCategories)
                {
                    var categoryChildItems = new List<ShoppingCartKitItem>();

                    // if no items are available, skip the category
                    if (category.Items == null || !category.Items.Any()){ continue; }

                    // if only one item, add it at the category's quantity
                    if (category.Items.Count() == 1)
                    {
                        if (ignoreStaticCategoryChildren) {
                            // skip adding items for this category since no user choice is involved.
                            continue;
                        }
                        categoryChildItems.Add(new ShoppingCartKitItem(category.Items.FirstOrDefault())
                        {
                            // CategoryID = category.DynamicKitCategoryID, // handled in constructor
                            Quantity = category.Quantity
                        });
                    }
                    // categories with more than one item require user choice. (verify against child items and quantities passed from user)
                    else
                    {
                        // if category quantity is 1, grab any matching item and set it (ignore item quantity passed in)
                        if (category.Quantity == 1)
                        {
                            // get cat items that match user selected children
                            var matchingItems = category.Items.Where(catItem => childItems == null || childItems.Any(childRequest => childRequest.Key.Equals_IgnoreCase(catItem.ItemCode))).ToList();
                            // too many children selected, throw error
                            if (matchingItems.Count() > 1){ throw new InvalidRequestException($"Too many children selected for dynamic kit category. Expected {category.Quantity}, got {matchingItems.Count()}."); }
                            // no children selected, select default as first child
                            if (!matchingItems.Any())
                            {
                                matchingItems = new List<Item> { category.Items.First() };
                            }
                            categoryChildItems.Add(new ShoppingCartKitItem(matchingItems.FirstOrDefault()) {
                                CategoryID = category.DynamicKitCategoryID,
                                Quantity = category.Quantity
                            });
                        }
                        // if category quantity AND category Items are more than 1, we need populate our list for as many requests that match, then verify 
                        if (category.Quantity > 1)
                        {
                            var maxQuantity = category.Quantity;
                            foreach (var catItem in category.Items)
                            {
                                // if no child items selected, stop
                                if (childItems == null) { throw new NullReferenceException($"No children selected for multi-choice category."); }

                                var requestedQuantity = childItems
                                    .Where(childRequest => childRequest.Key.Equals_IgnoreCase(catItem.ItemCode)) // find matching requests
                                    .Sum(childRequest => childRequest.Value); // get total requested quantity

                                // if this category item was not requested, skip. (ignores negative quantities as well)
                                if (requestedQuantity <= 0){ continue; }

                                // if requesting more than max, set requested value to max.
                                if (requestedQuantity > maxQuantity) {
                                    requestedQuantity = (int)maxQuantity;
                                }
                                categoryChildItems.Add(new ShoppingCartKitItem(catItem)
                                {
                                    CategoryID = category.DynamicKitCategoryID,
                                    Quantity = requestedQuantity
                                });

                                // subtract the request from the available for the next item.
                                maxQuantity -= requestedQuantity;

                                // if there is no more quantity available, stop.
                                if (maxQuantity <= 0){ break; }
                            }
                        }
                    }
                    // verify active category quantity matches added items
                    var categoryChildItemCount = categoryChildItems.Sum(i => i.Quantity);
                    if (categoryChildItemCount != category.Quantity) { throw new ArgumentOutOfRangeException($"Dynamic Kit children mismatch for item {_parentItem.ItemCode} in category {category.DynamicKitCategoryID}. Expected {category.Quantity}, got {categoryChildItemCount}."); }
                    // add the children to the parent list
                    kitChildItems.AddRange(categoryChildItems);
                }
                var activeCategories = _parentItem.DynamicKitCategories
                    .Where(c => c.Items.Any()); // only count categories with items populated by db request.
                if (ignoreStaticCategoryChildren)
                {
                    // filter out static categories for validating quantity
                    activeCategories = activeCategories.Where(c => c.Items.Count() != 1);
                }

                // validate final child item count
                var activeCategoryCount = activeCategories.Sum(c => c.Quantity);
                var kitChildrenQuantity = kitChildItems.Sum(i => i.Quantity);
                if (activeCategoryCount != kitChildrenQuantity) { throw new ArgumentOutOfRangeException($"Dynamic Kit children mismatch for item {_parentItem.ItemCode}. Expected {activeCategoryCount}, got {kitChildrenQuantity}."); }
                // add all kit children to the response
                responseCartItem.Children = kitChildItems;
            }
            #endregion

            #region populate static kit children
            if (_parentItem.ItemTypeID == ItemTypes.StaticKit && _parentItem.StaticKitChildren != null && _parentItem.StaticKitChildren.Any())
            {
                // do nothing (included for completeness)
                // static kit children are not user selected so do not need to be populated in the cart
            }
            #endregion

            return responseCartItem;
        }

        /// <summary>
        /// Convert ShoppingCartItems into OrderDetail Requests
        /// </summary>
        /// <param name="cartItems"></param>
        /// <returns></returns>
        public List<Api.ExigoWebService.OrderDetailRequest> GetOrderDetails(List<IShoppingCartItem> cartItems)
        {
            var parentCartItems = cartItems.ToList();
            var parentItemCodes = parentCartItems
                .Select(c => c.ItemCode)
                .ToList();
            // get the db version of the item for our market
            var dbItems = ExigoDAL.GetItems(new GetItemsRequest
            {
                Configuration = this._OrderConfiguration,
                ItemCodes = parentItemCodes,
                IncludeDynamicKitChildren = true,
                //~~ I had to add these properties to make this work in Enrollment SubmitCheckout - Mike M. ~~
                IgnoreCategoryRestrictions = true
            }).ToList();

            if (!dbItems.Any())
            {
                throw new InvalidRequestException($"No requested item was available in {this._OrderConfiguration.DefaultCountryCode}");
            }

            var responseDetails = new List<Api.ExigoWebService.OrderDetailRequest>();

            #region populate Normal item requests
            var standardItems = dbItems.Where(i => i.ItemTypeID == ItemTypes.Standard).ToList();
            foreach (var parentItem in standardItems)
            {
                // find the cart requests that match
                var matchingCartItems = cartItems
                    .Where(ci => ci.ItemCode == parentItem.ItemCode);
                foreach (var cartItem in matchingCartItems)
                {
                    responseDetails.Add(new Api.ExigoWebService.OrderDetailRequest(parentItem) {
                        Quantity = cartItem.Quantity
                    });
                }
            }
            #endregion


            #region populate Static Kit item requests
            var staticKitItems = dbItems.Where(i => i.ItemTypeID == ItemTypes.StaticKit).ToList();
            foreach (var parentItem in staticKitItems)
            {
                // find the cart requests that match
                var matchingCartItems = cartItems
                    .Where(ci => ci.ItemCode == parentItem.ItemCode);
                foreach (var cartItem in matchingCartItems)
                {
                    responseDetails.Add(new Api.ExigoWebService.OrderDetailRequest(parentItem) {
                        Quantity = cartItem.Quantity
                    });
                    foreach (var childItem in parentItem.StaticKitChildren)
                    {
                        responseDetails.Add(new Api.ExigoWebService.OrderDetailRequest(childItem)
                        {
                            ParentItemCode = parentItem.ItemCode,
                            Quantity = childItem.Quantity * cartItem.Quantity
                        });
                    }
                }
            }
            #endregion


            #region populate Dynamic Kit item requests
            var dynamicKitItems = dbItems.Where(i => i.ItemTypeID == ItemTypes.DynamicKit).ToList();
            foreach (var parentItem in dynamicKitItems)
            {
                // find the cart requests that match
                var kitCartItems = cartItems
                    .Where(ci => ci.ItemCode == parentItem.ItemCode);
                // add api requests for each cart kit (allows for multiple configurations for the same kit)
                foreach (var kitCartItem in kitCartItems)
                {
                    // add the parent first
                    responseDetails.Add(new Api.ExigoWebService.OrderDetailRequest(parentItem) {
                        Quantity = kitCartItem.Quantity
                    });
                    // add the children
                    foreach (var category in parentItem.DynamicKitCategories)
                    {
                        // get the requested cart children for the category
                        var cartCategoryChildren = kitCartItem.Children
                            ?.Where(c => c.CategoryID == category.DynamicKitCategoryID)
                            ?? new List<ShoppingCartKitItem>();
                        // add static categories (only one available item) at the category's quantity
                        // static categories are not added to the cart. so pull this info directly from db item and parent qty
                        if (category.Items.Count() == 1)
                        {
                            var childItem = category.Items.First();
                            responseDetails.Add(new Api.ExigoWebService.OrderDetailRequest(childItem)
                            {
                                ParentItemCode = parentItem.ItemCode,
                                Quantity = category.Quantity * kitCartItem.Quantity
                            });
                        }
                        else
                        {
                            // add single choice category chidren
                            if (category.Quantity == 1)
                            {
                                // get the first cart child that matches the category
                                var matchingcartItem = cartCategoryChildren.FirstOrDefault();

                                // get the db category child item that matches the cart request
                                var matchingCategoryItem = category.Items.FirstOrDefault(catItem => catItem.ItemCode.Equals_IgnoreCase(matchingcartItem?.ItemCode))
                                    ?? category.Items.First(); // default to the first category item if requested item is not found
                                responseDetails.Add(new Api.ExigoWebService.OrderDetailRequest(matchingCategoryItem)
                                {
                                    ParentItemCode = parentItem.ItemCode,
                                    Quantity = kitCartItem.Quantity // ignore requested qty when category qty is 1. child quantity will be that of the parent.
                                });
                            }
                            // add multi-choice category children
                            if (category.Quantity > 1)
                            {
                                // validate requested qty against a single kit
                                var maxQuantity = category.Quantity;

                                // get the cart children that matches the category
                                foreach (var catItem in category.Items)
                                {
                                    // Get the requested quantity (if any). child Qty should be based on a single kit regardless of parent quantity (kit configuration style)
                                    var requestedQuantity = cartCategoryChildren
                                        .Where(childRequest => childRequest.ItemCode.Equals_IgnoreCase(catItem.ItemCode))
                                        .Sum(childRequest => childRequest.Quantity);
                                    // if none selected, skip
                                    if (requestedQuantity <= 0){ continue; }
                                    // if requested qty is too large, set it to the max allowed
                                    if (requestedQuantity > maxQuantity){
                                        requestedQuantity = maxQuantity;
                                    }
                                    // add the child
                                    responseDetails.Add(new Api.ExigoWebService.OrderDetailRequest(catItem)
                                    {
                                        ParentItemCode = parentItem.ItemCode,
                                        Quantity = requestedQuantity * kitCartItem.Quantity // multiply the single kit child qty by the parent qty so the API gets the proper ammount
                                    });


                                    // subtract the request from the available for the next item.
                                    maxQuantity -= requestedQuantity;

                                    // if there is no more quantity available, stop.
                                    if (maxQuantity <= 0) { break; }
                                }
                                if (maxQuantity != 0){ throw new ArgumentOutOfRangeException($"Dynamic kit children mismatch for item {parentItem.ItemCode} in category {category.DynamicKitCategoryID}, Expected {category.Quantity}, got {category.Quantity - maxQuantity}"); }
                            }
                        }
                    }
                }
            }
            #endregion

            return responseDetails;
        }

        public void Dispose() {
            if(this._OrderConfiguration != null) { this._OrderConfiguration = null; }
        }
    }
}