using System;
using System.Collections.Generic;
using System.Linq;

namespace ExigoService
{
    public partial class AutoOrder : IAutoOrder
    {
        public AutoOrder()
        {
            ShippingAddress = new ShippingAddress();
            Details = new List<AutoOrderDetail>();
        }

        public int AutoOrderID { get; set; }
        public int CustomerID { get; set; }

        public string Description { get; set; }
        public int AutoOrderStatusID { get; set; }
        public int FrequencyTypeID { get; set; }
        public string CurrencyCode { get; set; }
        public int WarehouseID { get; set; }
        public int ShipMethodID { get; set; }
        public int AutoOrderPaymentTypeID { get; set; }
        public int AutoOrderProcessTypeID { get; set; }
        public string Notes { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? StopDate { get; set; }
        public DateTime? LastRunDate { get; set; }
        public DateTime? NextRunDate { get; set; }
        public DateTime? CancelledDate { get; set; }

        public ShippingAddress ShippingAddress { get; set; }

        public IPaymentMethod PaymentMethod { get; set; }

        public List<AutoOrderDetail> Details { get; set; }

        public decimal Total { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TaxTotal { get; set; }
        public decimal ShippingTotal { get; set; }
        public decimal DiscountTotal { get; set; }        

        public decimal BVTotal { get; set; }
        public string FormattedBVTotal { get { return string.Format("{0:N0} {1}", BVTotal, "BV"); } }

        public decimal CVTotal { get; set; }
        public string FormattedCVTotal { get { return string.Format("{0:N0} {1}", CVTotal, "CV"); } }

        public string Other11 { get; set; }
        public string Other12 { get; set; }
        public string Other13 { get; set; }
        public string Other14 { get; set; }
        public string Other15 { get; set; }
        public string Other16 { get; set; }
        public string Other17 { get; set; }
        public string Other18 { get; set; }
        public string Other19 { get; set; }
        public string Other20 { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }



        public string GetDescription()
        {
            if (this.Description.IsNotNullOrEmpty()) return this.Description;
            else
            {
                if (this.IsVirtualAutoOrder) return this.FrequencyTypeDescription + " Subscription Renewal";
                else return this.FrequencyTypeDescription + " Auto-ship";
            }
        }
        public string FrequencyTypeDescription
        {
            get
            {
                switch (this.FrequencyTypeID)
                {
                    default: return "Unknown";
                    case 1: return "Weekly";
                    case 2: return "Bi-Weekly";
                    case 3: return "Monthly";
                    case 4: return "Quarterly";
                    case 5: return "Bi-Yearly";
                    case 6: return "Yearly";
                    case 7: return "Bi-Monthly";
                    case 8: return "4-Week";
                    case 9: return "6-Week";
                }
            }
        }
        public bool IsActive
        {
            get { return this.AutoOrderStatusID == 0; }
        }
        public bool IsCancelled
        {
            get { return !this.IsActive; }
        }
        public bool IsVirtualAutoOrder
        {
            get { return this.Details != null && this.Details.All(d => d.IsVirtual); }
        }
        public bool IsBackupAutoOrder
        {
            get { return this.AutoOrderProcessTypeID == 2; }
        }
        public bool HasStarted
        {
            get { return this.StartDate <= DateTime.Now; }
        }
        public bool HasProcessedBefore
        {
            get { return this.LastRunDate != null; }
        }
        public bool WillBeStopped
        {
            get { return this.StopDate != null; }
        }
        public bool HasStopped
        {
            get { return this.WillBeStopped && ((DateTime)this.StopDate) < DateTime.Now; }
        }
        public bool HasValidPaymentMethod
        {
            get { return this.PaymentMethod != null && this.PaymentMethod.IsValid; }
        }
        public bool HasValidShippingAddress
        {
            get { return this.ShippingAddress.IsComplete; }
        }
    }
}