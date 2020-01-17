using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public class PartyOrder
    {
        public PartyOrder()
        {
            Details = new List<OrderDetail>();
            ShippingAddress = new ShippingAddress();
        }

        public int OrderID { get; set; }
        public int CustomerID { get; set; }

        public string CurrencyCode { get; set; }
        public int WarehouseID { get; set; }
        public int ShipMethodID { get; set; }
        public int OrderStatusID { get; set; }
        public int OrderTypeID { get; set; }
        public int PriceTypeID { get; set; }
        public string Notes { get; set; }

        public int? AutoOrderID { get; set; }
        public int? ReturnOrderID { get; set; }
        public int? ParentOrderID { get; set; }
        public int? TransferToCustomerID { get; set; }
        public int DeclineCount { get; set; }

        public int PartyID { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ShippedDate { get; set; }

        public IEnumerable<OrderDetail> Details { get; set; }
        public IEnumerable<IPayment> Payments { get; set; }

        public ShippingAddress ShippingAddress { get; set; }

        public decimal Total { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TaxTotal { get; set; }
        public decimal ShippingTotal { get; set; }
        public decimal DiscountTotal { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal WeightTotal { get; set; }
        public decimal BVTotal { get; set; }
        public decimal CVTotal { get; set; }


        public string TrackingNumber1 { get; set; }
        public string TrackingNumber2 { get; set; }
        public string TrackingNumber3 { get; set; }
        public string TrackingNumber4 { get; set; }
        public string TrackingNumber5 { get; set; }
        public IEnumerable<string> TrackingNumbers
        {
            get
            {
                var trackingNumbers = new List<string>();
                if (TrackingNumber1.IsNotEmpty()) trackingNumbers.Add(TrackingNumber1);
                if (TrackingNumber2.IsNotEmpty()) trackingNumbers.Add(TrackingNumber2);
                if (TrackingNumber3.IsNotEmpty()) trackingNumbers.Add(TrackingNumber3);
                if (TrackingNumber4.IsNotEmpty()) trackingNumbers.Add(TrackingNumber4);
                if (TrackingNumber5.IsNotEmpty()) trackingNumbers.Add(TrackingNumber5);
                return trackingNumbers;
            }
        }

        public string TrackingURL { get; set; }

        public decimal Other1Total { get; set; }
        public decimal Other2Total { get; set; }
        public decimal Other3Total { get; set; }
        public decimal Other4Total { get; set; }
        public decimal Other5Total { get; set; }
        public decimal Other6Total { get; set; }
        public decimal Other7Total { get; set; }
        public decimal Other8Total { get; set; }
        public decimal Other9Total { get; set; }
        public decimal Other10Total { get; set; }

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

        public bool IsOpen
        {
            get { return OrderStatusID < 7; }
        }
        public bool HasDeclinedAtLeastOnce
        {
            get { return this.DeclineCount > 0; }
        }
        public bool HasShipped
        {
            get { return OrderStatusID == 9; }
        }
        public bool HasTrackingNumbers
        {
            get { return TrackingNumbers != null && TrackingNumbers.Count() > 0; }
        }
        public string OrderStatusDescription
        {
            get
            {
                var statusDescription = Common.Services.CommonResources.OrderStatuses(OrderStatusID);

                if (OrderStatusID != OrderStatuses.Shipped)
                {
                    return statusDescription;
                }
                else
                {
                    return statusDescription + ((this.ShippedDate != null) ? " " + ((DateTime)this.ShippedDate).ToShortDateString() : "");
                }
            }
        }
        public bool CanBeCancelled
        {
            get { return new int[] { 0, 1, 2, 3, 5, 6, 10 }.Contains(OrderStatusID); }
        }
    }
}