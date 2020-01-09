using ExigoService;

namespace Backoffice.ViewModels
{
    public class CustomerDetailsViewModel
    {
        public CustomerDetailsViewModel()
        {
            this.Customer = new Customer();
            this.Customer.MainAddress = new Address();
            this.PaidRank = new Rank();
            this.HighestRank = new Rank();
        }

        public Customer Customer { get; set; }
        public Rank PaidRank { get; set; }
        public Rank HighestRank { get; set; }
    }
}