using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public class ShoppingCartKitItem : IComparable<ShoppingCartKitItem>, IEquatable<ShoppingCartKitItem>
    {
        public ShoppingCartKitItem() {
            this.CategoryID = 0;
            this.ItemCode = string.Empty;
            this.Quantity = 0;
        }
        public ShoppingCartKitItem(IShoppingCartItem item)
        {
            this.ItemCode = item.ItemCode;
            this.Quantity = item.Quantity;
        }
        public string ItemCode { get; set; }
        /// <summary>
        /// Quantity of the children within a single kit. 
        /// This is used as a configuration relative to the parent not a true cart quantity. 
        /// Total quantity of children is calculated in GetOrderDetails(List<IShoppingCartItem> cartItems)
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// (optional) Dynamic Kit Category ID
        /// </summary>
        public int CategoryID { get; set; }

        /// <summary>
        /// IComparable, used with .Sort()
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(ShoppingCartKitItem next)
        {
            if (next == null){ return 1; }
            var compare = 0;
            // order by category ID
            compare = next.CategoryID.CompareTo(this.CategoryID);
            if (compare != 0){ return compare; }
            // then by itemcode
            compare = next.ItemCode.ToLower().CompareTo(this.ItemCode.ToLower());
            return compare;
        }
        /// <summary>
        /// IEquatable, Used to compare KitItems to each other.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ShoppingCartKitItem other)
        {
            if (other == null) { return false; }
            if (this.CategoryID != other.CategoryID) { return false; }
            if (this.ItemCode != other.ItemCode) { return false; }
            if (this.Quantity != other.Quantity) { return false; }
            return true;
               
        }
    }
}