using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ExigoService;

namespace Backoffice.ViewModels
{
    public class CreateEventViewModel
    {
        /// <summary>
        /// Default Constructor.
        /// Initializes Event Types and Privacy Types
        /// </summary>
        public CreateEventViewModel()
        {
            this.Request = new CalendarEvent(Identity.Current.CustomerID);
            this.EventTypes = ExigoDAL.GetCalendarEventTypes();
            this.PrivacyTypes = ExigoDAL.GetCalendarEventPrivacyTypes();
            GetCalendarID();
        }

        /// <summary>
        /// The Calendar Event that will be created.
        /// </summary>
        public CalendarEvent Request { get; set; }

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

        public void GetCalendarID()
        {
            var mainCalendar = ExigoDAL.GetCalendars(new GetCalendarEventsRequest()
            {
                CustomerID = Identity.Current.CustomerID
            }).Where(cal => cal.CustomerID == Identity.Current.CustomerID).FirstOrDefault();

            if (mainCalendar != null)
            {
                this.Request.CalendarID = mainCalendar.CalendarID;
            }
        }
    }
}