using System;
using System.Collections.Generic;

namespace ExigoService
{
    public interface IShoppingCartItem
    {
        Guid ID { get; set; }
        string ItemCode { get; set; }
        decimal Quantity { get; set; }

        /// <summary>
        /// Master Item code of group member. Group member links to this item when inspecting.
        /// </summary>
        string GroupMasterItemCode { get; set; }

        /// <summary>
        /// Item Type
        /// </summary>
        ShoppingCartItemType Type { get; set; }

        List<ShoppingCartKitItem> Children { get; set; }

    }
}