using System;

namespace ExigoService
{
    public class Calendar : ICalendar
    {
        public Guid CalendarID { get; set; }
        public int CustomerID { get; set; }
        public string Description { get; set; }
        public int CalendarTypeID { get; set; }
        public DateTime CreatedDate { get; set; }

        public Customer Customer { get; set; }
    }
}