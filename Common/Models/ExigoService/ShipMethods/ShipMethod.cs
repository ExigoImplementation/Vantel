namespace ExigoService
{
    public class ShipMethod : IShipMethod
    {
        public int ShipMethodID { get; set; }
        public string ShipMethodDescription { get; set; }
        public decimal Price { get; set; }
        public bool Selected { get; set; }
        public string FormattedPrice { get { return Price.ToString("C"); } }
    }
}