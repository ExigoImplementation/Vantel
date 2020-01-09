namespace ExigoService
{
    public class GetItemListRequest
    {
        public IOrderConfiguration Configuration { get; set; }
        public int? CategoryID { get; set; }
    }
}