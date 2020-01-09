using ExigoService;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Backoffice.ViewModels
{   
    public class SubscriptionsViewModel
    {      
        public SubscriptionsViewModel()
        {
            this.CustomerCalendarSubscriptions = new List<CalendarSubscriptionCustomer>();
        }    

              
        public List<CalendarSubscriptionCustomer> CustomerCalendarSubscriptions { get; set; }
              
        [Required(ErrorMessageResourceName = "FieldIsRequired", ErrorMessageResourceType = typeof(Common.Resources.Models))]
        public string Query { get; set; }
    }
}