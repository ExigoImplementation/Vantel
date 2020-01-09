using ExigoService;
using ReplicatedSite.Models;
using System.Collections.Generic;

namespace ReplicatedSite.ViewModels
{
    public class EnrollmentReviewViewModel : IEnrollmentViewModel
    {       
        public IEnumerable<Item> Items { get; set; }
        public OrderCalculationResponse Totals { get; set; }
        public IEnumerable<IShipMethod> ShipMethods { get; set; }
        public int ShipMethodID { get; set; }
        public EnrollmentPropertyBag PropertyBag { get; set; }
        public string[] Errors { get; set; }
    }
}