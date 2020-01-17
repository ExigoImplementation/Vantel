using Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExigoService
{
    public class ShoppingCartItem : IShoppingCartItem, IEquatable<ShoppingCartItem>
    {
        public ShoppingCartItem()
        {
            ID                  = Guid.NewGuid();
            ItemCode            = string.Empty;
            Quantity            = 0;
            ParentItemCode      = string.Empty;
            GroupMasterItemCode = string.Empty;
            Type                = ShoppingCartItemType.Order;
        }
        public ShoppingCartItem(IShoppingCartItem item)
        {
            ID                  = (item.ID != Guid.Empty) ? item.ID : Guid.NewGuid();
            ItemCode            = GlobalUtilities.Coalesce(item.ItemCode);
            Quantity            = item.Quantity;
            ParentItemCode = GlobalUtilities.Coalesce(item.ParentItemCode);
            GroupMasterItemCode = GlobalUtilities.Coalesce(item.GroupMasterItemCode);
            Type                = item.Type;
            Children            = item.Children;
        }

        public Guid ID
        {
            get
            {
                if (_id == null)
                {
                    _id = Guid.NewGuid();
                }
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        private Guid _id;

        public string ItemCode { get; set; }
        public decimal Quantity { get; set; }
        public string ParentItemCode { get; set; }
        public string GroupMasterItemCode { get; set; }
        public ShoppingCartItemType Type { get; set; }

        /// <summary>
        /// Child Items relating to this parent. 
        /// <para>
        /// Children are treated as an item config for the parent:
        /// These should only represent the quantity of children for 1 parent regardless of the parent's current quantity. (this allows comparison between kits)
        /// Populating the full child quantity is done dynamically for order submission and for display.
        /// </para>
        /// </summary>
        public List<ShoppingCartKitItem> Children { get; set; }
        public bool HasChildren { get { return this.Children != null && this.Children.Any(); } }

        /// <summary>
        /// Compare itemcode, type, and children config against another item
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ShoppingCartItem other)
        {
            if (other == null) { return false; }
            if (this.ItemCode != other.ItemCode) { return false; }
            if (this.Type != other.Type) { return false; }
            // compare child configs
            if (this.HasChildren != other.HasChildren) { return false; }
            if (this.HasChildren)
            {
                var a = this.Children;
                var b = other.Children;
                // compare total children
                if (a.Count() != b.Count()) { return false; }
                // order the children for comparison
                a.Sort();
                b.Sort();
                // check each child
                for (int i = 0; i < a.Count(); i++)
                {
                    // compare each child
                    if (!a[i].Equals(b[i])) { return false; }
                }
            }
            return true;

        }
    }
}