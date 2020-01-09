using System.Collections.Generic;

namespace ExigoService
{
    public class GetItemsRequest : IChildItemRequest, IItemDetailFilterRequest
    {
        public GetItemsRequest()
        {
            this.ItemCodes = new List<string>();
            this.ShoppingCartItems = new List<ShoppingCartItem>();
        }

        public IOrderConfiguration Configuration { get; set; }

        /////////////////////////////
        // Configuration Overrides //
        /////////////////////////////
        /// <summary>
        /// (Optional) Specific Category to return Items from. 
        /// <para> Overrides Configuration CategoryID if provided </para>
        /// </summary>
        public int? CategoryID { get; set; }
        /// <summary>
        /// (Optional) Specific price type to return. 
        /// <para> Overrides Configuration PriceTypeID if provided. </para>
        /// </summary>
        public int? PriceTypeID { get; set; }
        /// <summary>
        /// (Optional) List of item codes to get. If provided, it will filter items from the requested category(s).
        /// <para> This will NOT automatically give you these items. </para>
        /// </summary>
        public List<string> ItemCodes { get; set; }
        /// <summary>
        /// (Optional) Ignores category restricted item code filters. Will return provided ItemCodes as long as they are available in the given Warehouse, PriceType, and Currency
        /// <para>ItemCodes must be provided if you enable this flag.</para>
        /// </summary>
        public bool IgnoreCategoryRestrictions { get; set; }
        /// <summary>
        /// (Optional) Gets items from ALL child categories of the requested category if enabled.
        /// </summary>
        public bool IncludeAllChildCategories { get; set; }

        //////////////////////////
        // Cart Item Population //
        //////////////////////////
        /// <summary>
        /// (Optional) Overrides ItemCodes list if provided. Will return items with IShoppingCartItem properties preserved
        /// <para>ItemCodes will be ignored if this is provided</para>
        /// <para>IgnoreCategoryRestrictions will be enabled if this is provided, regardless of request settings.</para>
        /// </summary>
        public IEnumerable<ShoppingCartItem> ShoppingCartItems { get; set; }

        //////////////////
        // Translations //
        //////////////////
        /// <summary>
        /// (Optional) Gets Item translations for the given Admin language ID.
        /// <para> Overrides default Item Descriptions if provided </para>
        /// </summary>
        public int? LanguageID { get; set; }

        ///////////////////////
        // IChildItemRequest //
        ///////////////////////
        /// <summary>
        /// (Optional) Gets all group members for group master items if enabled
        /// <para> group members are populated into the parent item </para>
        /// </summary>
        public bool IncludeGroupMembers { get; set; }
        /// <summary>
        /// (Optional) Gets all static kit children for static kit master items  if enabled
        /// <para> static kit children are populated into the parent item </para>
        /// </summary>
        public bool IncludeStaticKitChildren { get; set; }
        /// <summary>
        /// (Optional) Gets all dynamic kit children for dynamic kit master items  if enabled
        /// <para> Dynamic kit children are populated into the parent item </para>
        /// </summary>
        public bool IncludeDynamicKitChildren { get; set; }
        
        //////////////////////////////
        // IItemDetailFilterRequest //
        //////////////////////////////
        /// <summary>
        /// (Optional) Set to true to include LongDescription 1-4 for all items and children levels
        /// </summary>
        public bool IncludeShortDescriptions { get; set; }
        /// <summary>
        /// (Optional) Set to true to include LongDescription 1-4 for all items and children levels
        /// </summary>
        public bool IncludeLongDescriptions { get; set; }
        /// <summary>
        /// (Optional) Set to true to include Field 1-10 for all items and children levels
        /// </summary>
        public bool IncludeExtendedFields { get; set; }
        /// <summary>
        /// (Optional) Set to true to include OtherPrice 1-10 for all items and children levels
        /// </summary>
        public bool IncludeExtendedlItemPrices { get; set; }

    }
}