using System;

namespace ExigoService
{
    public class CalendarEventTag
    {
        public Guid CalendarEventTagID { get; set; }

        public Guid CalendarEventID { get; set; }

        public Guid TagID { get; set; }

        public Tag Tag { get; set; }
    }
}