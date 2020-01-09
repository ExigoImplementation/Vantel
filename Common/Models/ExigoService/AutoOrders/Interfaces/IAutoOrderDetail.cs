namespace ExigoService
{
    public interface IAutoOrderDetail
    {
        int AutoOrderDetailID { get; set; }
        int AutoOrderID { get; set; }

        int ItemID { get; set; }
        int? ParentItemID { get; set; }
        string ItemCode { get; set; }
        string ItemDescription { get; set; }
        decimal Quantity { get; set; }
        decimal PriceEach { get; set; }
        decimal PriceTotal { get; set; }
        bool IsVirtual { get; set; }

        decimal BVEach { get; set; }
        decimal BV { get; set; }
        decimal CVEach { get; set; }
        decimal CV { get; set; }

        decimal? PriceEachOverride { get; set; }
        decimal? TaxableEachOverride { get; set; }
        decimal? ShippingPriceEachOverride { get; set; }
        decimal? BVEachOverride { get; set; }
        decimal? CVEachOverride { get; set; }

        string ImageUrl { get; set; }
    }
}