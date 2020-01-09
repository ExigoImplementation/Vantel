namespace ExigoService
{
    public class GetItemDetailRequest
    {
        public IOrderConfiguration Configuration { get; set; }
        public string ItemCode { get; set; }
        public string CurrencyCode { get; set; }
        public int WarehouseID { get; set; }
        public int LanguageID { get; set; }
        public int PriceTypeID { get; set; }
    }
}