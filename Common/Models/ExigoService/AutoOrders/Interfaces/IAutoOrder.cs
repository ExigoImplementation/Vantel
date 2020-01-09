using System;
using System.Collections.Generic;

namespace ExigoService
{
    public interface IAutoOrder
    {
        int AutoOrderID { get; set; }
        int CustomerID { get; set; }

        string Description { get; set; }
        int AutoOrderStatusID { get; set; }
        int FrequencyTypeID { get; set; }
        string CurrencyCode { get; set; }
        int WarehouseID { get; set; }
        int ShipMethodID { get; set; }
        int AutoOrderPaymentTypeID { get; set; }
        int AutoOrderProcessTypeID { get; set; }
        string Notes { get; set; }

        DateTime StartDate { get; set; }
        DateTime? StopDate { get; set; }
        DateTime? LastRunDate { get; set; }
        DateTime? NextRunDate { get; set; }
        DateTime? CancelledDate { get; set; }

        ShippingAddress ShippingAddress { get; set; }

        IPaymentMethod PaymentMethod { get; set; }

        List<AutoOrderDetail> Details { get; set; }

        decimal Total { get; set; }        
        decimal Subtotal { get; set; }        
        decimal TaxTotal { get; set; }        
        decimal ShippingTotal { get; set; }        
        decimal DiscountTotal { get; set; }        
        decimal BVTotal { get; set; }        
        decimal CVTotal { get; set; }        

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

        DateTime CreatedDate { get; set; }
        string CreatedBy { get; set; }
    }

}