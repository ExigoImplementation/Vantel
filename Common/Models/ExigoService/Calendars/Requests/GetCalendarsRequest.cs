using System;
using System.Collections.Generic;

namespace ExigoService
{
    public class GetCalendarsRequest
    {
        public int? CustomerID { get; set; }
        public bool IncludeCalendarSubscriptions { get; set; }
        public IEnumerable<Guid> CalendarIDs { get; set; }
    }
}