using System.Collections.Generic;

namespace ExigoService
{
    public class MasterSortItem
    {
        public MasterSortItem()
        {
            this.ItemCode = string.Empty;
        }
        public int WebCategoryID { get; set; }
        public string ItemCode { get; set; }
        public int MasterSortID { get; set; }
    }
}
