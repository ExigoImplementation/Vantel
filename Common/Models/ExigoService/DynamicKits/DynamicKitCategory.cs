using System.Collections.Generic;

namespace ExigoService
{
    public class DynamicKitCategory 
    {
        public DynamicKitCategory()
        {
            this.Items = new List<Item>();
        }
        public string MasterItemCode { get; set; }
        public int DynamicKitCategoryID { get; set; }
        public string DynamicKitCategoryDescription { get; set; }
        public decimal Quantity { get; set; }

        /// <summary>
        /// Category Item quantities are never set relative to cart items
        /// </summary>
        public List<Item> Items { get; set; }
    }
}