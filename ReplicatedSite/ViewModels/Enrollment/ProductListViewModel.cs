using ExigoService;
using System.Collections.Generic;

namespace ReplicatedSite.ViewModels
{
    public class EnrollmentProductListViewModel
    {
        public IEnumerable<Item> OrderItems { get; set; }
        public IEnumerable<Item> AutoOrderItems { get; set; }        
    }
}