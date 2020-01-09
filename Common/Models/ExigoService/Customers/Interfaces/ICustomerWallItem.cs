using System;

namespace ExigoService
{
    public interface ICustomerWallItem
    {
        int CustomerWallItemID { get; set; }
        int CustomerID { get; set; }
        DateTime EntryDate { get; set; }
        string Text { get; set; }

        string Field1 { get; set; }
        string Field2 { get; set; }
        string Field3 { get; set; }
    }
}