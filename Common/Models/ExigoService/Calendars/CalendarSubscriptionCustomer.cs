using System.Collections.Generic;

namespace ExigoService
{
    public class CalendarSubscriptionCustomer : Customer
    {
        public CalendarSubscriptionCustomer() : base()
        {
            this.Calendars = new List<Calendar>();
        }

        public List<Calendar> Calendars { get; set; }
    }
}