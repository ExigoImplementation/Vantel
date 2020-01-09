using ExigoService;

namespace ReplicatedSite.ViewModels
{
    public class EnrollmentCompleteViewModel
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public int AutoOrderID { get; set; }

        public Order Order { get; set; }
        public AutoOrder AutoOrder { get; set; }
    }
}