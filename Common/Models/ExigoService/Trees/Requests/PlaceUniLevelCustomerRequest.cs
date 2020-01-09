namespace ExigoService
{
    public class PlaceUniLevelCustomerRequest
    {
        public int CustomerID { get; set; }
        public int ToSponsorID { get; set; }
        public string Reason { get; set; }

        public int? Placement { get; set; }
        public bool FindNextAvailablePlacement { get; set; }
        public int? BuildTypeID { get; set; }
    }
}