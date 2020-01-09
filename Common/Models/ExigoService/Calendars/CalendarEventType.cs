using System;

namespace ExigoService
{
    public class CalendarEventType : ICalendarEventType
    {
        public Guid CalendarEventTypeID { get; set; }
        public string CalendarEventTypeDescription { get; set; }
        public string Color { get; set; }
    }
}