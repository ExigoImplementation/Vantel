using System;
using System.Collections.Generic;

namespace ExigoService
{
    public interface ICalendarEvent
    {
        #region Properties

        /// <summary>
        /// The event's identification Guid.
        /// </summary>
        Guid CalendarEventID { get; set; }

        /// <summary>
        /// The calendar's identification Guid of which the event belongs to.
        /// </summary>
        Guid CalendarID { get; set; }

        /// <summary>
        /// The event's type, represented by integer.
        /// </summary>
        Guid CalendarEventTypeID { get; set; }

        /// <summary>
        /// The event's privacy, represented by integer.
        /// </summary>
        int CalendarEventPrivacyTypeID { get; set; }

        /// <summary>
        /// The Customer ID of the user that created the event.
        /// </summary>
        int CreatedBy { get; set; }

        /// <summary>
        /// The DateTime, in UTC, the event was created.
        /// </summary>
        DateTime CreatedDate { get; set; }

        /// <summary>
        /// An Enumerable of string that represent Tag names describing the Calendar Event.
        /// </summary>
        List<string> Tags { get; set; }

        #endregion
    }
}