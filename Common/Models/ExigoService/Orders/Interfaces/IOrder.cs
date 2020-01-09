using System;
using System.Collections.Generic;

namespace ExigoService
{
    public interface IOrder
    {
        int OrderID { get; set; }
        int CustomerID { get; set; }

        string CurrencyCode { get; set; }
        int WarehouseID { get; set; }
        int ShipMethodID { get; set; }
        int OrderStatusID { get; set; }
        int OrderTypeID { get; set; }
        int PriceTypeID { get; set; }
        string Notes { get; set; }

        int? AutoOrderID { get; set; }
        int? ReturnOrderID { get; set; }
        int? ParentOrderID { get; set; }
        int? TransferToCustomerID { get; set; }
        int DeclineCount { get; set; }

        DateTime CreatedDate { get; set; }
        DateTime ModifiedDate { get; set; }
        DateTime OrderDate { get; set; }
        DateTime? ShippedDate { get; set; }

        IEnumerable<OrderDetail> Details { get; set; }
        IEnumerable<IPayment> Payments { get; set; }

        ShippingAddress Recipient { get; set; }

        decimal Total { get; set; }
        decimal Subtotal { get; set; }
        decimal TaxTotal { get; set; }
        decimal ShippingTotal { get; set; }
        decimal DiscountTotal { get; set; }
        decimal DiscountPercent { get; set; }
        decimal WeightTotal { get; set; }
        decimal BVTotal { get; set; }
        decimal CVTotal { get; set; }

        // Detailed tax information left out for now

        IEnumerable<string> TrackingNumbers { get; }

        decimal Other1Total { get; set; }
        decimal Other2Total { get; set; }
        decimal Other3Total { get; set; }
        decimal Other4Total { get; set; }
        decimal Other5Total { get; set; }
        decimal Other6Total { get; set; }
        decimal Other7Total { get; set; }
        decimal Other8Total { get; set; }
        decimal Other9Total { get; set; }
        decimal Other10Total { get; set; }

        string Other11 { get; set; }
        string Other12 { get; set; }
        string Other13 { get; set; }
        string Other14 { get; set; }
        string Other15 { get; set; }
        string Other16 { get; set; }
        string Other17 { get; set; }
        string Other18 { get; set; }
        string Other19 { get; set; }
        string Other20 { get; set; }
    }
}