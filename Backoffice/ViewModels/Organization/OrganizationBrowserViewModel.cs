using ExigoService;
using System.Collections.Generic;

namespace Backoffice.ViewModels
{
    public class OrganizationBrowserViewModel
    {
        public Customer TopCustomer { get; set; }
        public List<Customer> Customers { get; set; }
    }
}