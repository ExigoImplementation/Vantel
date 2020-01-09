using Backoffice.ViewModels;
using Common;
using ExigoService;
using ExigoWeb.Kendo;
using System;
using System.Linq;
using System.Web.Mvc;
using Dapper;
using System.Collections.Generic;


namespace Backoffice.Controllers
{
    [RoutePrefix("events")]
    public class EventsController : Controller
    {
        #region Views

        public ActionResult EventsMap()
        {
            var model = new EventsMapViewModel();
            model.CalendarFilters = new CalendarViewModel(Identity.Current.CustomerID);
            model.Events = ExigoDAL.GetCalendarEvents(new GetCalendarEventsRequest()
            {
                CustomerID = Identity.Current.CustomerID
            });
            return View(model);
        }

        /// <summary>
        /// Calendar View
        /// </summary>
        /// <returns>View with CalendarViewModel</returns>
        [Route("calendar")]
        public ActionResult Calendar()
        {
            return View(new CalendarViewModel(Identity.Current.CustomerID));
        }

        public ActionResult Create()
        {
            return View(new CreateEventViewModel());
        }

        [Route("calendar/subscriptions")]
        public ActionResult Subscriptions()
        {
            var model = new SubscriptionsViewModel();

            // Get the Calendar Subscriptions for the provided Customer ID
            var calendars = ExigoDAL.GetCustomerCalendarSubscriptions(Identity.Current.CustomerID);

            // Get the CustomerIDs for each of the Calendar Subscriptions
            var customerIDs = calendars.Select(c => c.CustomerID).Distinct().ToList();
            var customerCalendarSubscription = new List<CalendarSubscriptionCustomer>();

            using (var context = ExigoDAL.Sql())
            {
                customerCalendarSubscription = context.Query<CalendarSubscriptionCustomer>(@"
                     SELECT 
                        c.CustomerID
                        ,c.FirstName
                        ,c.LastName                        
                        
                    FROM 
                        Customers c
                        
                    WHERE 
                        c.CustomerID IN @customerids",
                    new
                    {
                        customerids = customerIDs
                    }).ToList();

                //Apply the correct calendars to the customers
                foreach (var customer in customerCalendarSubscription)
                {
                    customer.Calendars = calendars.Where(c => c.CustomerID == customer.CustomerID).ToList();
                }

                model.CustomerCalendarSubscriptions = customerCalendarSubscription;
            }

            return View(model);
        }

        public ActionResult EventMap(Guid calendarEventID)
        {
            var calendarEvent = ExigoDAL.GetCalendarEvents(new GetCalendarEventsRequest { EventID = calendarEventID }).FirstOrDefault();
            return View(calendarEvent);
        }

        /// <summary>
        /// Send the User to Google Calendar with a prefilled out form containing their Event's data.
        /// </summary>
        /// <param name="calendarEventID">The Calendar Event ID for the event data to send.</param>
        /// <returns>Attempts to Redirect the User to google, should it fail redirects them to the Calendar view.</returns>
        public ActionResult Sync(Guid calendarEventID)
        {
            try
            {


                // Retrieve the Calendar Event from the context
                CalendarEvent calendarEvent = new CalendarEvent();

                using (var context = ExigoDAL.Sql())
                {
                    calendarEvent = context.Query<CalendarEvent>(@"
                                    select 
	                                    Title,
	                                    Start,
	                                    [End],
	                                    Description,
	                                    Address1,
	                                    Address2,
	                                    City,
	                                    State,
	                                    Country,
	                                    Zip
                                    from ExigoWebContext.CalendarEvents
                                    where CalendarEventID = @calendarEventID
                                    ", new
                    {
                        calendarEventID
                    }).FirstOrDefault();
                }

                // Populate our location object on the Calendar Event record
                calendarEvent.PopulateLocation();

                // Format the Start Date
                var startDate = calendarEvent.Start.ToString("o").Replace("-", "").Replace(":", "");
                // Remove the Timezone
                startDate = startDate.Substring(0, startDate.IndexOf("."));

                // Format the End Date
                var endDate = calendarEvent.End.ToString("o").Replace("-", "").Replace(":", "");
                // Remove the Timezone
                endDate = endDate.Substring(0, endDate.IndexOf("."));

                // Format the URL for Google Calendar's Create an Event 
                var location = string.Format("https://www.google.com/calendar/render?action=TEMPLATE&text={0}&dates={1}/{2}&details={3}&location={4}&sf=true&output=xml", calendarEvent.Title, startDate, endDate, calendarEvent.Description, calendarEvent.Location);

                // Redirect the user
                return Redirect(location);
            }
            catch (Exception)
            {
                // Should anything fail redirect the user to the Calendar view
                return RedirectToAction("Calendar");
            }
        }

        #endregion

        #region AJAX

        /// <summary>
        /// Get Events for the Kendo Scheduler
        /// </summary>
        /// <returns>JSON Net Result</returns>
        public ActionResult GetEvents(KendoGridRequest request)
        {
            // Establish the query
            var query = ExigoDAL.GetCalendarEvents(new GetCalendarEventsRequest()
            {
                CustomerID = Identity.Current.CustomerID,
                IncludeCalendarSubscriptions = true
            }).AsQueryable();


            // Fetch the data
            var context = new KendoGridDataContext();
            return context.Query(request, query, c => new
            {
                c.ID,
                c.Title,
                c.Description,
                Start = c.Start.ToString("o"),
                End = c.End.ToString("o"),
                c.StartTimezone,
                c.EndTimezone,
                c.RecurrenceID,
                c.RecurrenceRule,
                c.RecurrenceException,
                c.IsAllDay,
                c.CalendarID,
                c.CalendarEventTypeID,
                c.CalendarEventPrivacyTypeID,
                c.CreatedBy,
                CreatedDate = c.CreatedDate.ToString("s"),
                Tags = string.Join(", ", c.Tags),
                Location_Address1 = c.Location.Address1,
                Location_Address2 = c.Location.Address2,
                Location_City = c.Location.City,
                Location_State = c.Location.State,
                Location_Zip = c.Location.Zip,
                Location_Country = c.Location.Country,
                c.SpeakerID,
                c.Phone,
                c.Flyer,
                c.Cost,
                c.ConferenceNumber,
                c.ConferencePIN,
                c.Url
            });
        }

        /// <summary>
        /// Registers Event in the Extended DB Table.
        /// </summary>
        /// <param name="request">The Event to Create.</param>
        /// <returns>JSON Net Result success(bool)</returns>
        [HttpPost]
        public ActionResult SaveEvent(CalendarEvent request)
        {
            try
            {
                if (!GlobalUtilities.CalendarPermissions(Identity.Current.CustomerTypeID))
                {
                    return new JsonNetResult(new
                    {
                        success = false,
                        message = Resources.Common.InsufficientPrivileges
                    });
                }

                if (request.Title.IsNullOrEmpty() && request.Title != Resources.Common.Title)
                {
                    return new JsonNetResult(new
                    {
                        success = false,
                        message = Resources.Common.EventTitleRequired
                    });
                }

                ExigoDAL.SaveCalendarEvent(request);

                return new JsonNetResult(new
                {
                    success = true,
                    nextLocation = Url.Action("Calendar", "Events")
                });
            }
            catch (Exception ex)
            {
                return new JsonNetResult(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        /// <summary>
        /// Deletes Event in the Extended DB Table.
        /// </summary>
        /// <param name="calendarEventID">The ID of the Calendar Event to delete.</param>
        /// <returns>JSON Net Result success(bool)</returns>
        [HttpPost]
        public ActionResult DeleteEvent(Guid calendarEventID)
        {
            try
            {
                ExigoDAL.DeleteCalendarEvent(calendarEventID);

                return new JsonNetResult(new
                {
                    success = true,
                });
            }
            catch (Exception ex)
            {
                return new JsonNetResult(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        /// <summary>
        /// Searches for Available Subscriptions using the Query as a Filter for which distributors to display.
        /// </summary>
        /// <param name="query">A filter e.g. Name or Customer ID</param>
        /// <returns>JSON Net Result success(bool), html(Partials/_SubscriptionResults)</returns>
        [HttpPost]
        public ActionResult SearchSubscriptions(string query)
        {
            try
            {
                return new JsonNetResult(new
                {
                    success = true,
                    html = this.RenderPartialViewToString("Partials/_SubscriptionResults", ExigoDAL.SearchSubscriptions(query, Identity.Current.CustomerID))
                });
            }
            catch (Exception ex)
            {
                return new JsonNetResult(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        /// <summary>
        /// Subscribes the Current User to another user's calendar.
        /// </summary>
        /// <param name="id">The Calendar ID of the calendar the current user is subscribing to.</param>
        /// <returns>JSON Net Result success(bool) or Redirects to Subscriptions.</returns>
        [HttpPost]
        public ActionResult SubscribeToDistributorCalendar(Guid id)
        {

            ExigoDAL.SubscribeToCustomerCalendar(Identity.Current.CustomerID, id);

            if (Request.IsAjaxRequest())
            {
                return new JsonNetResult(new
                {
                    success = true,
                });
            }

            return RedirectToAction("Subscriptions");
        }

        /// <summary>
        /// Unsubscribes the Current User from another user's calendar.
        /// </summary>
        /// <param name="id">The Calendar ID of the calendar the current user is unsubscribing to.</param>
        /// <returns>JSON Net Result success(bool) or Redirects to Subscriptions.</returns>
        [HttpPost]
        public ActionResult UnsubscribeFromDistributorCalendar(Guid id)
        {
            ExigoDAL.UnsubscribeFromCustomerCalendar(Identity.Current.CustomerID, id);

            if (Request.IsAjaxRequest())
            {
                return new JsonNetResult(new
                {
                    success = true,
                });
            }

            return RedirectToAction("subscriptions");
        }

        #endregion
    }
}