using System;

namespace ExigoService
{
    public class CustomerWallItem : ICustomerWallItem
    {
        public int CustomerWallItemID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerIDToken { get; set; }
        public DateTime EntryDate { get; set; }
        public string Text { get; set; }

        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
    }
}