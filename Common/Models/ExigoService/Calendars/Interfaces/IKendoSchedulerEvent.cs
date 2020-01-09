using System;

namespace ExigoService
{
    /// <summary>
    /// Fields that are used by standard Kendo Scheduler Events ("http://docs.telerik.com/kendo-ui/api/javascript/data/schedulerevent#events")
    /// </summary>
    public interface IKendoSchedulerEvent
    {
        /// <summary>
        /// The mandatory unique identifier of the event.
        /// </summary>
        string ID { get; set; }

        /// <summary>
        /// The optional event description.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// The date at which the scheduler event starts. The start date is mandatory.
        /// </summary>
        DateTime Start { get; set; }

        /// <summary>
        /// The date at which the scheduler event ends. The end date is mandatory.
        /// </summary>
        DateTime End { get; set; }

        /// <summary>
        /// The timezone of the start date. If not specified the schema's timezone will be used.
        /// </summary>
        string StartTimezone { get; set; }

        /// <summary>
        /// The timezone of the end date. If not specified the schema's timezone will be used.
        /// </summary>
        string EndTimezone { get; set; }

        /// <summary>
        /// If set to true the event is "all day". By default events are not all day.
        /// </summary>
        bool IsAllDay { get; set; }

        /// <summary>
        /// The recurrence exceptions. A list of semi-colon separated dates formatted using the yyyyMMddTHHmmssZ format string.
        /// </summary>
        string RecurrenceException { get; set; }

        /// <summary>
        /// The id of the recurrence parent event. Required for events that are recurrence exceptions.
        /// </summary>
        string RecurrenceID { get; set; }

        /// <summary>
        /// The recurrence rule describing the recurring pattern of the event. The format follows the iCal specification. ("http://tools.ietf.org/html/rfc5545")
        /// </summary>
        string RecurrenceRule { get; set; }

        /// <summary>
        /// The title of the event which is displayed by the scheduler widget.
        /// </summary>
        string Title { get; set; }
    }
}