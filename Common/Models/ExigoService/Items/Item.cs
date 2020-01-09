using System;
using System.Collections.Generic;

namespace ExigoService
{
    public class Item : IItem, IItemMasterSort, IShoppingCartItem
    {
        public Item()
        {
            this.GroupMembers = new List<ItemGroupMember>();
            this.DynamicKitCategories = new List<DynamicKitCategory>();
            this.StaticKitChildren = new List<Item>();
        }
        public int ItemID { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public int MasterSortID { get; set; }
        public decimal Weight { get; set; }
        public int ItemTypeID { get; set; }

        public string TinyImageUrl { get; set; }
        public string SmallImageUrl { get; set; }
        public string LargeImageUrl { get; set; }

        public string ShortDetail1 { get; set; }
        public string ShortDetail2 { get; set; }
        public string ShortDetail3 { get; set; }
        public string ShortDetail4 { get; set; }
        public string LongDetail1 { get; set; }
        public string LongDetail2 { get; set; }
        public string LongDetail3 { get; set; }
        public string LongDetail4 { get; set; }

        public bool IsVirtual { get; set; }
        public bool AllowOnAutoOrder { get; set; }

        // Group Members
        public bool IsGroupMaster { get; set; }
        public string GroupMembersDescription { get; set; }
        public List<ItemGroupMember> GroupMembers { get; set; }

        // Dynamic kit members
        public bool IsDynamicKitMaster { get; set; }
        /// <summary>
        /// Dynamic kit category's quantity is reflective of a single kit. This is to preserve cart logic
        /// Multiply the category qty by the master to get a total cart quantity.
        /// </summary>
        public List<DynamicKitCategory> DynamicKitCategories { get; set; }

        // Static Kit Members
        public List<Item> StaticKitChildren { get; set; }

        public int PriceTypeID { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Price { get; set; }
        public decimal BV { get; set; }
        public decimal CV { get; set; }
        public decimal OtherPrice1 { get; set; }
        public decimal OtherPrice2 { get; set; }
        public decimal OtherPrice3 { get; set; }
        public decimal OtherPrice4 { get; set; }
        public decimal OtherPrice5 { get; set; }
        public decimal OtherPrice6 { get; set; }
        public decimal OtherPrice7 { get; set; }
        public decimal OtherPrice8 { get; set; }
        public decimal OtherPrice9 { get; set; }
        public decimal OtherPrice10 { get; set; }

        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }
        public string Field6 { get; set; }
        public string Field7 { get; set; }
        public string Field8 { get; set; }
        public string Field9 { get; set; }
        public string Field10 { get; set; }

        public bool OtherCheck1 { get; set; }
        public bool OtherCheck2 { get; set; }
        public bool OtherCheck3 { get; set; }
        public bool OtherCheck4 { get; set; }
        public bool OtherCheck5 { get; set; }

        public int SortOrder { get; set; }

        // IShoppingCartItem Members
        public Guid ID { get; set; }
        public decimal Quantity { get; set; }
        public string GroupMasterItemCode { get; set; }
        public ShoppingCartItemType Type { get; set; }
        public List<ShoppingCartKitItem> Children { get; set; }

        public int CategoryID { get; set; }

        public string ItemUrl { get; set; }
        public string PriceString { get {
                return Price.ToString("c");
            }
        }
    }
}
