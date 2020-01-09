namespace ExigoService
{
    public class GetTreeRequest
    {
        public int TopCustomerID { get; set; }
        public int Levels { get; set; }
        public int Legs { get; set; }
        public bool IncludeOpenPositions { get; set; }
        public bool IncludeNullPositions { get; set; }
    }
}