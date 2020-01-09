using ExigoService;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;

namespace Backoffice.ViewModels
{
    /// <summary>
    /// Calendar View Model designed for ManageEvent.cshtml
    /// </summary>
    public class CalendarViewModel
    {
        /// <summary>
        /// Default Constructor.
        /// Constructs any properties that are objects to avoid Null Reference Exception.
        /// Intializes Time Zones to the result of Exigo.GetTimeZones().
        /// Initializes Privacy Types to Exigo.GetCalendarPrivacyTypes();
        /// </summary>
        public CalendarViewModel()
        {
            this.Calendars = new List<Calendar>();
            this.EventTypes = new List<CalendarEventType>();
            this.PrivacyTypes = ExigoDAL.GetCalendarEventPrivacyTypes();
        }

        /// <summary>
        /// Constructs the View Model.
        /// Initializes Calendars to Exigo.GetCalendars() using the provided CustomerID and includes calendar subscriptions.
        /// Initializes Event Types to Exigo.GetCalendarEventTypes().
        /// Intializes Time Zones to the result of Exigo.GetTimeZones().
        /// Initializes Privacy Types to Exigo.GetCalendarPrivacyTypes();
        /// </summary>
        /// <param name="customerID">The CustomerID to pull Calendars for.</param>
        public CalendarViewModel(int customerID)
        {
            this.Calendars = ExigoDAL.GetCalendars(new GetCalendarsRequest
            {
                CustomerID = customerID,
                IncludeCalendarSubscriptions = true
            });

            this.EventTypes = ExigoDAL.GetCalendarEventTypes();
            this.PrivacyTypes = ExigoDAL.GetCalendarEventPrivacyTypes();
        }

        /// <summary>
        /// Constructs the View Model.
        /// Initializes Calendars to Exigo.GetCalendars() using the provided CustomerID and the boolean.
        /// Initializes Event Types to Exigo.GetCalendarEventTypes().
        /// </summary>
        /// <param name="customerID">The CustomerID to pull Calendars for.</param>
        /// <param name="includeCalendarSubscriptions">True: Pull Calendars CustomerID is Subscribed to. False: Don't Pull Calendars CustomerID is subscribed to.</param>
        public CalendarViewModel(int customerID, bool includeCalendarSubscriptions)
        {
            this.Calendars = ExigoDAL.GetCalendars(new GetCalendarsRequest
            {
                CustomerID = customerID,
                IncludeCalendarSubscriptions = includeCalendarSubscriptions
            });

            this.EventTypes = ExigoDAL.GetCalendarEventTypes();
        }

        /// <summary>
        /// An Enumerable of ExigoService.Calendars.
        /// </summary>
        public IEnumerable<Calendar> Calendars { get; set; }

        /// <summary>
        /// An Enumerable of ExigoService.CalendarEventTypes.
        /// </summary>
        public IEnumerable<CalendarEventType> EventTypes { get; set; }

        /// <summary>
        /// An Enumerable of ExigoService.ExigoWeb.CalendarEventPrivacyTypes.
        /// </summary>
        public IEnumerable<CalendarEventPrivacyType> PrivacyTypes { get; set; }

        private IEnumerable<CalendarSpeaker> _Speakers { get; set; }
        /// <summary>
        /// An Enumerable of Speaker Names.
        /// </summary>
        public IEnumerable<CalendarSpeaker> Speakers
        {
            get
            {
                if (_Speakers != null)
                {
                    return _Speakers;
                }

                _Speakers = ExigoDAL.GetCalendarEventSpeakers();

                return _Speakers;
            }
        }

        private IEnumerable<Country> _Countries { get; set; }
        /// <summary>
        /// An Enumerable of Coutnries.
        /// </summary>
        public IEnumerable<Country> Countries
        {
            get
            {
                if (_Countries != null)
                {
                    return _Countries;
                }

                _Countries = ExigoDAL.GetCountries();

                return _Countries;
            }
        }

        private IEnumerable<string> _Tags { get; set; }
        /// <summary>
        /// An Enumerable of Tags.
        /// </summary>
        public IEnumerable<string> Tags
        {
            get
            {
                if (_Tags != null)
                {
                    return _Tags;
                }

                _Tags = ExigoDAL.GetTags();

                return _Tags;
            }
        }

    }
}