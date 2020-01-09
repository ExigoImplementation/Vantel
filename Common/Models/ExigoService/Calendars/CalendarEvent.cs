using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Common;

namespace ExigoService
{
    public class CalendarEvent : ICalendarEvent, IKendoSchedulerEvent
    {
        #region Constructors

        /// <summary>
        /// Default Constructor.
        /// Created Date set to time of Construction.
        /// Start Date set to an Hour from construction.
        /// End Date set to an Hour from Start Date.
        /// Time Zone set to Local Time Zone.
        /// Location set to newly constructed Address object.
        /// </summary>
        public CalendarEvent()
        {
            this.CalendarEventID = Guid.Empty;
            this.CreatedDate = DateTime.Now;
            this.Start = DateTime.Now.AddHours(1).BeginningOfHour();
            this.End = this.Start.AddHours(1);
            this.Location = new Address(GlobalSettings.Company.Address.Country, GlobalSettings.Company.Address.State);
            this.Tags = new List<string>();
        }

        /// <summary>
        /// Constructs Calendar Event using the Default Constructor. Then sets the Created By property using the parameter passed.
        /// <param name="createdBy">The ID of the User creating the Event.</param>
        /// </summary>
        public CalendarEvent(int createdBy)
            : this()
        {
            this.CreatedBy = createdBy;
        }
        #endregion

        #region Properties

        /// <summary>
        /// The event's identification Guid.
        /// </summary>
        public Guid CalendarEventID { get; set; }

        /// <summary>
        /// The calendar's identification Guid of which the event belongs to.
        /// </summary>
        public Guid CalendarID { get; set; }

        /// <summary>
        /// The event's type, represented by integer.
        /// </summary>
        [Display(ResourceType = typeof(Common.Resources.Models), Name = "Type")]
        public Guid CalendarEventTypeID { get; set; }

        /// <summary>
        /// The event's privacy, represented by integer.
        /// </summary>
        [Display(ResourceType = typeof(Common.Resources.Models), Name = "Privacy")]
        public int CalendarEventPrivacyTypeID { get; set; }

        /// <summary>
        /// The Customer ID of the user that created the event.
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// The DateTime, in UTC, the event was created.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// The mandatory unique identifier of the event.
        /// </summary>
        public string ID
        {
            get
            {
                return this.CalendarEventID.ToString();
            }
            set
            {
                if (value != null)
                {
                    this.CalendarEventID = new Guid(value);
                }
            }
        }

        /// <summary>
        /// The optional event description.
        /// </summary>
        [Display(ResourceType = typeof(Common.Resources.Models), Name = "Description")]
        [Required(ErrorMessageResourceType = typeof(Common.Resources.Models), ErrorMessageResourceName = "DescriptionRequired")]
        public string Description { get; set; }

        /// <summary>
        /// The date at which the scheduler event starts. The start date is mandatory.
        /// </summary>
        [Display(ResourceType = typeof(Common.Resources.Models), Name = "Start")]
        public DateTime Start { get; set; }

        /// <summary>
        /// The date at which the scheduler event ends. The end date is mandatory.
        /// </summary>
        [Display(ResourceType = typeof(Common.Resources.Models), Name = "End")]
        public DateTime End { get; set; }

        /// <summary>
        /// The timezone of the start date. If not specified the schema's timezone will be used.
        /// </summary>
        [Display(ResourceType = typeof(Common.Resources.Models), Name = "StartTimezone")]
        public string StartTimezone { get; set; }

        /// <summary>
        /// The timezone of the end date. If not specified the schema's timezone will be used.
        /// </summary>
        [Display(ResourceType = typeof(Common.Resources.Models), Name = "EndTimezone")]
        public string EndTimezone { get; set; }

        /// <summary>
        /// If set to true the event is "all day". By default events are not all day.
        /// </summary>
        [Display(ResourceType = typeof(Common.Resources.Models), Name = "IsAllDay")]
        public bool IsAllDay { get; set; }

        /// <summary>
        /// The recurrence exceptions. A list of semi-colon separated dates formatted using the yyyyMMddTHHmmssZ format string.
        /// </summary>
        public string RecurrenceException { get; set; }

        /// <summary>
        /// The id of the recurrence parent event. Required for events that are recurrence exceptions.
        /// </summary>
        public string RecurrenceID { get; set; }

        /// <summary>
        /// The recurrence rule describing the recurring pattern of the event. The format follows the iCal specification. ("http://tools.ietf.org/html/rfc5545")
        /// </summary>
        [Display(ResourceType = typeof(Common.Resources.Models), Name = "RecurrenceRule")]
        public string RecurrenceRule { get; set; }

        /// <summary>
        /// The title of the event which is displayed by the scheduler widget.
        /// </summary>
        [Display(ResourceType = typeof(Common.Resources.Models), Name = "Title")]
        [Required(ErrorMessageResourceType = typeof(Common.Resources.Models), ErrorMessageResourceName = "TitleRequired")]
        public string Title { get; set; }

        /// <summary>
        /// An Enumerable of string that represent Tag names describing the Calendar Event.
        /// </summary>
        [Display(ResourceType = typeof(Common.Resources.Models), Name = "Tags")]
        public List<string> Tags { get; set; }

        /// <summary>
        /// The Location of the event, represented by the Address object for isolating parts of the location e.g. City, State, Zip.
        /// </summary>
        [Display(ResourceType = typeof(Common.Resources.Models), Name = "Location")]
        public Address Location { get; set; }

        /// <summary>
        /// The SpeakerID for the event, should there be one.
        /// </summary>
        [Display(ResourceType = typeof(Common.Resources.Models), Name = "SpeakersName")]
        public Guid SpeakerID { get; set; }

        /// <summary>
        /// A Phone Number that can be used to contact a designated person for further information about the event.
        /// </summary>
        [Display(ResourceType = typeof(Common.Resources.Models), Name = "Phone")]
        public string Phone { get; set; }

        /// <summary>
        /// A Url that links to a flyer for the event.
        /// </summary>
        [Display(ResourceType = typeof(Common.Resources.Models), Name = "Flyer")]
        public string Flyer { get; set; }

        /// <summary>
        /// The amount of money it costs to attend/participate in the event.
        /// </summary>
        [Display(ResourceType = typeof(Common.Resources.Models), Name = "Cost")]
        public string Cost { get; set; }

        /// <summary>
        /// The Phone Number to call to be placed on the Conference Call with other attendees. Typically used for VoIP-enabled meetings.
        /// </summary>
        [Display(ResourceType = typeof(Common.Resources.Models), Name = "ConferenceNumber")]
        public string ConferenceNumber { get; set; }

        /// <summary>
        /// The PIN Number that is necessary to gain access to the Conference Call. 
        /// Most VoIP cients require a PIN/passcode in order to connect to the conference line.
        /// </summary>
        [Display(ResourceType = typeof(Common.Resources.Models), Name = "ConferencePIN")]
        public string ConferencePIN { get; set; }

        /// <summary>
        /// A Url related to the event. This could be the location of the event (online event), a website with details about the event, or used for other purposes.
        /// </summary>
        [Display(ResourceType = typeof(Common.Resources.Models), Name = "Url")]
        public string Url { get; set; }

        // Address Properties (for SQL version)
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        #endregion

        #region Methods
        public void PopulateLocation()
        {
            this.Location = new Address
            {
                Address1 = this.Address1,
                Address2 = this.Address2,
                City = this.City,
                State = this.State,
                Country = this.Country,
                Zip = this.Zip
            };
        }
        #endregion
    }
}