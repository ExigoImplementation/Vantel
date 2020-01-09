using System;

namespace ExigoService
{
    public interface ICalendarEventType
    {
        Guid CalendarEventTypeID { get; set; }
        string CalendarEventTypeDescription { get; set; }
        string Color { get; set; }
    }
}