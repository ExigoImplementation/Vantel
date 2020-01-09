using System;

namespace ExigoService
{
    public interface ICalendar
    {
        Guid CalendarID { get; set; }
        int CustomerID { get; set; }
        string Description { get; set; }
        int CalendarTypeID { get; set; }
        DateTime CreatedDate { get; set; }
    }
}