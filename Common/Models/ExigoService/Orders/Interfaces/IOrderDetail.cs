namespace ExigoService
{
    public interface IOrderDetail
    {
        int OrderDetailID { get; set; }
        int OrderID { get; set; }

        int ItemID { get; set; }
        string ItemCode { get; set; }
        string ItemDescription { get; set; }
        decimal Quantity { get; set; }
        decimal PriceEach { get; set; }
        decimal PriceTotal { get; set; }
        decimal Tax { get; set; }
        decimal WeightEach { get; set; }
        decimal Weight { get; set; }
        decimal BVEach { get; set; }
        decimal BV { get; set; }
        decimal CVEach { get; set; }
        decimal CV { get; set; }
        bool IsVirtual { get; set; }

        string ImageUrl { get; set; }

        decimal Other1Each { get; set; }
        decimal Other1 { get; set; }
        decimal Other2Each { get; set; }
        decimal Other2 { get; set; }
        decimal Other3Each { get; set; }
        decimal Other3 { get; set; }
        decimal Other4Each { get; set; }
        decimal Other4 { get; set; }
        decimal Other5Each { get; set; }
        decimal Other5 { get; set; }
        decimal Other6Each { get; set; }
        decimal Other6 { get; set; }
        decimal Other7Each { get; set; }
        decimal Other7 { get; set; }
        decimal Other8Each { get; set; }
        decimal Other8 { get; set; }
        decimal Other9Each { get; set; }
        decimal Other9 { get; set; }
        decimal Other10Each { get; set; }
        decimal Other10 { get; set; }
    }
}