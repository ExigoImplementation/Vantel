using ExigoService;

namespace Common.Api.ExigoWebService
{
    public partial class ShipMethodResponse : IShipMethod
    {
        public ShipMethodResponse() { }

        public static explicit operator ExigoService.ShipMethod(ShipMethodResponse shipmethod)
        {
            var model = new ExigoService.ShipMethod();
            if (shipmethod == null) return model;

            model.ShipMethodID          = shipmethod.ShipMethodID;
            model.ShipMethodDescription = shipmethod.Description;
            model.Price                 = shipmethod.ShippingAmount;

            return model;
        }

        public string ShipMethodDescription { get; set; }
        public decimal Price { get; set; }
        public bool Selected { get; set; }
    }
}