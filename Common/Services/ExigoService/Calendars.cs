using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using Dapper;

namespace ExigoService
{
    public static partial class ExigoDAL
    {

        #region Get

        public static IEnumerable<Calendar> GetCalendars(GetCalendarsRequest request)
        {
            var calendars = new List<Calendar>();
            var cacheKey = "GetCalendars_{0}_{1}".FormatWith(request.CustomerID ?? 0,
                request.IncludeCalendarSubscriptions);
            var cache = (HttpContext.Current != null) ? HttpContext.Current.Cache : null;

            // Check the cache to see if we've already made this call recently
            if (cache == null || cache[cacheKey] == null)
            {
                GlobalUtilities.RunAsyncTasks(

                    // Add the customer's personal calendars
                    () =>
                    {
                        if (request.CustomerID != null)
                        {
                            var apiCalendars = new List<Calendar>();
                            using (var ctx = ExigoDAL.Sql())
                            {
                                apiCalendars = ctx.Query<Calendar>(@"
                                                select
                                                    CalendarID = c.CalendarID,
                                                    CustomerID = c.CustomerID,
                                                    Description = c.Description,
                                                    CalendarTypeID = c.CalendarTypeID,
                                                    CreatedDate = c.CreatedDate
                                                from ExigoWebContext.Calendars c
                                                where c.CustomerID = @customerID
                                                ", new { customerID = request.CustomerID }).ToList();
                            }

                            foreach (var apiCalendar in apiCalendars)
                            {
                                calendars.Add(apiCalendar);
                            }
                        }
                    },

                    // Include the customer's calendar subscriptions if applicable
                    () =>
                    {
                        if (request.CustomerID != null && request.IncludeCalendarSubscriptions)
                        {
                            var calendarSubscriptions = GetCustomerCalendarSubscriptions((int)request.CustomerID);
                            var calendarSubscriptionsIDs = calendarSubscriptions.Select(c => c.CalendarID).ToList();

                            if (calendarSubscriptionsIDs.Count > 0)
                            {
                                var apiCalendars = new List<Calendar>();
                                using (var ctx = ExigoDAL.Sql())
                                {
                                    apiCalendars = ctx.Query<Calendar>(@"
                                                select
                                                    CalendarID = c.CalendarID,
                                                    CustomerID = c.CustomerID,
                                                    Description = c.Description,
                                                    CalendarTypeID = c.CalendarTypeID,
                                                    CreatedDate = c.CreatedDate
                                                from ExigoWebContext.Calendars c
                                                where c.CalendarID in @calendarSubscriptionsIDs
                                                ", new { calendarSubscriptionsIDs }).ToList();
                                }


                                foreach (var apiCalendar in apiCalendars)
                                {
                                    calendars.Add(apiCalendar);
                                }
                            }
                        }
                    },

                    // Include any additional requested calendars
                    () =>
                    {
                        if (request.CalendarIDs != null && request.CalendarIDs.Any())
                        {
                            var apiCalendars = new List<Calendar>();
                            using (var ctx = ExigoDAL.Sql())
                            {
                                apiCalendars = ctx.Query<Calendar>(@"
                                                select
                                                    CalendarID = c.CalendarID,
                                                    CustomerID = c.CustomerID,
                                                    Description = c.Description,
                                                    CalendarTypeID = c.CalendarTypeID,
                                                    CreatedDate = c.CreatedDate
                                                from ExigoWebContext.Calendars c
                                                where c.CalendarID in @calendarIDs
                                                ", new { calendarIDs = request.CalendarIDs.ToList() }).ToList();
                            }

                            foreach (var apiCalendar in apiCalendars)
                            {
                                calendars.Add(apiCalendar);
                            }
                        }
                    }
                    );


                // If we asked for a specific customer's calendars, and none of the calendars belong to that customer, create a default calendar and add it to the collection.
                if (request.CustomerID != null && calendars.Any(c => c.CustomerID == (int)request.CustomerID) == false)
                {
                    var defaultCalendar = CreateDefaultCalendar((int)request.CustomerID);
                    calendars.Add(defaultCalendar);
                }

                if (cache != null)
                {
                    cache.Insert(cacheKey, calendars,
                        null,
                        DateTime.Now.AddMinutes(5),
                        Cache.NoSlidingExpiration,
                        CacheItemPriority.Normal,
                        null);
                }
            }
            else
            {
                calendars = (List<Calendar>)cache[cacheKey];
            }


            // Return the calendars
            foreach (var calendar in calendars)
            {
                yield return calendar;
            }
        }

        /// <summary>
        /// Retrieves the Calendar Subscriptions for a specific Customer.
        /// </summary>
        /// <param name="customerID">The Customer ID of the Calendar Subscriptions to get.</param>
        /// <returns>List of ExigoService.Calendar Objects</returns>
        public static IEnumerable<Calendar> GetCustomerCalendarSubscriptions(int customerID)
        {
            // Create a List of Calendars
            var calendars = new List<Calendar>();

            using (var ctx = ExigoDAL.Sql())
            {
                // Set the Calendars to the Results from the DB
                calendars = ctx.Query<Calendar>(@"
                            SELECT  CalendarID = cs.CalendarID,
                                    CustomerID = c.CustomerID,
                                    Description = c.Description,
                                    CalendarTypeID = c.CalendarTypeID,
                                    CreatedDate = c.CreatedDate
                            FROM ExigoWebContext.CalendarSubscriptions cs
                            INNER JOIN ExigoWebContext.Calendars c 
                                ON c.CalendarID = cs.CalendarID
                            WHERE cs.CustomerID = @customerID
                            ", new { customerID }).ToList();
            }

            // Return the Calendars
            return calendars;
        }

        /// <summary>
        /// Retrieves Enumerable of Calendar Event from the ExigoWebContext
        /// </summary>
        /// <param name="request">The request used to identify which Calendar Events to retrieve.</param>
        /// <returns>Enumerable of Calendar Events</returns>
        public static IEnumerable<CalendarEvent> GetCalendarEvents(GetCalendarEventsRequest request)
        {
            // Create the collection of events
            var calendarEvents = new List<CalendarEvent>();

            // Get a List of Calendars using the request passed in
            var calendarIDs = GetCalendars(request).Select(cal => cal.CalendarID).ToList();

            // Logic to get single Event data, if Event ID is provided
            var calendarIDSQL = (calendarIDs.Count > 0) ? "WHERE ce.CalendarID IN @calendarIDs" : "";
            var singleEventSQL = (request.EventID != Guid.Empty) ? ((calendarIDs.Count > 0) ? " AND" : " WHERE") + " ce.CalendarEventID = '{0}'".FormatWith(request.EventID) : "";
            var whereClause = calendarIDSQL + singleEventSQL;

            // Set the SQL Command
            string sql = @"
                            SELECT 
                                [CalendarEventID],
                                [CalendarID],
                                [CalendarEventPrivacyTypeID],
                                [CreatedBy],
                                [CreatedDate],
                                [Description],
                                [Start],
                                [End],
                                [StartTimezone],
                                [EndTimezone],
                                [IsAllDay],
                                [Title],
                                [Phone],
                                [Flyer],
                                [Cost],
                                [ConferenceNumber],
                                [ConferencePIN],
                                [Url],
                                [RecurrenceException],
                                [RecurrenceID],
                                [RecurrenceRule],
                                [SpeakerName],
                                SpeakerID = [CalendarSpeakerID],
                                [CalendarEventTypeID],
                                [Address1],
                                [Address2],
                                [City],
                                [State],
                                [Country],
                                [Zip]
                            FROM ExigoWebContext.CalendarEvents ce
                            " + whereClause + @"

                            SELECT * 
                            FROM ExigoWebContext.CalendarEventTags cet
                                INNER JOIN ExigoWebContext.Tags t
                                        ON t.TagID = cet.TagID
                            ";

            // Establish a SQL Connection
            using (var ctx = ExigoDAL.Sql())
            {
                // Exceute the SQL Command
                using (var query = ctx.QueryMultiple(sql, new { calendarIDs }))
                {
                    // Get each Calendar Event and Set the Address and Tags
                    calendarEvents = query.Read<CalendarEvent, Address, CalendarEvent>((calendarevent, address) =>
                    {
                        calendarevent.Location = address ?? new Address();
                        calendarevent.Tags = new List<string>();

                        return calendarevent;
                    }, "Address1").ToList();

                    // If we got any Events back
                    if (calendarEvents.Any())
                    {
                        // Read the Tags part of the Command 
                        var tags = query.Read<CalendarEventTag, Tag, CalendarEventTag>((cet, tag) =>
                        {
                            cet.Tag = tag;
                            return cet;
                        }, "TagID").ToList();

                        // Go through each Calendar Event returned and set the Tags to the appropriate Tags
                        foreach (var calEvent in calendarEvents)
                        {
                            calEvent.Tags = tags.Where(t => t.CalendarEventID == calEvent.CalendarEventID && t.Tag.Name != "").ToList().Select(t => t.Tag.Name).ToList();
                        }
                    }
                }
            }

            calendarEvents = calendarEvents.Where(c => c.CreatedBy == request.CustomerID || c.CalendarEventPrivacyTypeID == 1).ToList();
            
            // Return the Calendar Events
            return calendarEvents;
        }

        /// <summary>
        /// Get all Calendar Event Privacy Types
        /// </summary>
        /// <returns>An Enumerable of CalendarEventPrivacyType</returns>
        public static IEnumerable<CalendarEventPrivacyType> GetCalendarEventPrivacyTypes()
        {
            // Set the SQL Command
            const string sql = @" SELECT * FROM ExigoWebContext.CalendarEventPrivacyTypes ";

            // Create a Collection of Calendar Event Privacy Types
            var privacyTypes = new List<CalendarEventPrivacyType>();

            // Create a SQL Connetion
            using (var ctx = ExigoDAL.Sql())
            {
                // Execute the SQL Command and set the results to the Collection constructed above
                privacyTypes = ctx.Query<CalendarEventPrivacyType>(sql).ToList();
            }

            // Return the Privacy Types
            return privacyTypes;
        }

        /// <summary>
        /// Get all Calendar Event Types
        /// </summary>
        /// <returns>An Enumerable of CalendarEventType</returns>
        public static IEnumerable<CalendarEventType> GetCalendarEventTypes()
        {
            // Set the SQL Command
            const string sql = @" SELECT * FROM ExigoWebContext.CalendarEventTypes ";

            // Create a Collection of Calendar Event Types
            var eventTypes = new List<CalendarEventType>();

            // Create a SQL Connetion
            using (var ctx = ExigoDAL.Sql())
            {
                // Execute the SQL Command and set the results to the Collection constructed above
                eventTypes = ctx.Query<CalendarEventType>(sql).ToList();
            }

            // Return the Event Types
            return eventTypes;
        }

        /// <summary>
        /// Retrieves an enumerable of Speaker Names
        /// </summary>
        /// <param name="calendarIds">If provided, the Calendar ID(s) to pull speaker names from.</param>
        /// <returns>An enumerable of string that represent Speaker's Names at events</returns>
        public static IEnumerable<CalendarSpeaker> GetCalendarEventSpeakers()
        {
            // Set the SQL Command
            const string sql = @" SELECT * FROM ExigoWebContext.CalendarSpeakers ce WHERE ce.Description <> '' ";

            // Create a Collection of CalendarSpeaker to hold Speaker Names
            var speakers = new List<CalendarSpeaker>();

            // Create a SQL Connetion
            using (var ctx = ExigoDAL.Sql())
            {
                // Execute the SQL Command and set the results to the Collection constructed above
                speakers = ctx.Query<CalendarSpeaker>(sql).ToList();
            }

            // Return the Speaker Names colletion
            return speakers;
        }

        /// <summary>
        /// Retrieves an enumerable of Countries
        /// </summary>
        /// <param name="calendarIds">If provided, the Calendar ID(s) to pull countries from.</param>
        /// <returns>An enumerable of string that represent Countries where events are located.</returns>
        public static IEnumerable<Country> GetCalendarEventCountries()
        {
            return GetCountries();
        }

        /// <summary>
        /// Search Calendar Subscriptions using the conditional paramater as a filter.
        /// </summary>
        /// <param name="conditional">The condition to use for filtering e.g. "John" .Where(cal => cal.FirstName.Contains("John").</param>
        /// <param name="customerID">The Customer ID of the current user. This prevents the user from subscribing to themselves.</param>
        /// <returns>An Enumerable of Calendar Subscription Customer.</returns>
        public static IEnumerable<Calendar> SearchSubscriptions(string conditional, int customerID)
        {

            // Set the Base SQL Command
            var sql = @" 
                        SELECT 
                            ca.CalendarID,
                            ca.CustomerID,
                            ca.Description,
                            ca.CalendarTypeID,
                            ca.CreatedDate,
                            c.CustomerID as 'CusID',
                            c.FirstName,
                            c.LastName,
                            c.Company
                        FROM ExigoWebContext.Calendars ca 
                            INNER JOIN Customers c
                                on c.CustomerID = ca.CustomerID 
                        WHERE ca.CalendarID NOT IN 
                            (SELECT cs.CalendarID FROM ExigoWebContext.CalendarSubscriptions cs INNER JOIN ExigoWebContext.Calendars c ON c.CalendarID = cs.CalendarID WHERE cs.CustomerID = @customerid)
                        AND
                        ";

            // If the conditional passed in can be parsed as an Int then the user is filtering on CustomerID
            // However, if the conditional can't be parsed as an Int then the user is filtering on Customer Name
            sql += conditional.CanBeParsedAs<int>()
                ? string.Format("ca.CustomerID = {0}", conditional)
                : string.Format("(FirstName LIKE '%{0}%' OR LastName LIKE '%{0}%')", conditional);

            // Create a Collection of Calendars
            var calendarSubscriptions = new List<Calendar>();

            // Establish a SQL Connection
            using (var ctx = ExigoDAL.Sql())
            {
                // Excecute the SQL Command and set the results to the Collection constructed above
                calendarSubscriptions = ctx.Query<Calendar, Customer, Calendar>(sql, (cal, cus) =>
                {
                    cal.Customer = cus;
                    return cal;
                }
                , param: new { customerid = customerID }
                , splitOn: "CusID"
                ).ToList();
            }

            // Using the query, select the Calendars and Group By Customer. Then Create Calendar Subscription Customers for each row and return those items.
            return
               calendarSubscriptions;
        }

        /// <summary>
        /// Retrieves an enumerable of Tags.
        /// </summary>
        /// <returns>An enumerable of Tag Names.</returns>
        public static IEnumerable<string> GetTags()
        {
            // Set the SQL Command
            const string sql = @" SELECT Name FROM ExigoWebContext.Tags ";

            // Create a collection of string to hold Tag Names
            var tags = new List<string>();

            // Establish a SQL Command
            using (var ctx = ExigoDAL.Sql())
            {
                // Execute the SQL Command and set the results to the Collection constructed above
                tags = ctx.Query<string>(sql).ToList();
            }

            // Return the collection of Tag Name
            return tags;
        }

        /// <summary>
        /// Creates Default Calendar Subscriptions for the specified Customer.
        /// </summary>
        /// <param name="customerID">The customer ID to create default calendar subscriptions for.</param>
        public static void CreateDefaultCalendarSubscriptions(int customerID)
        {
            // Ensure the corporate calendar subscription exists
            var corporateCalendar = GetCalendars(new GetCalendarsRequest
            {
                CustomerID = GlobalSettings.Company.CorporateCalendarAccountID
            }).First();

            SubscribeToCustomerCalendar(customerID, corporateCalendar.CalendarID);

            // Create a EnrollerID Global Scope Variable
            int enrollerID;

            // Establish a SQL Connection
            using (var ctx = ExigoDAL.Sql())
            {
                // Execute a SQL Command and set the result to the EnrollerID declared above
                enrollerID = ctx.Query<int>(@" SELECT EnrollerID = ISNULL(EnrollerID, -1) FROM Customers c WHERE c.CustomerID = @customerID", new { customerID }).FirstOrDefault();
            }

            // If the EnrollerID is invalid exit
            if (enrollerID < 1 || enrollerID == GlobalSettings.Company.CorporateCalendarAccountID)
            {
                return;
            }

            // Retrieve a Calendar for the EnrollerID
            var calendars = GetCalendars(new GetCalendarsRequest
            {
                CustomerID = enrollerID,
                IncludeCalendarSubscriptions = false
            });

            // If they do not have a calendar, create them one
            var calendar = calendars.Any() ? calendars.Where(c => c.CalendarTypeID == 1).OrderBy(c => c.CreatedDate).FirstOrDefault() : CreateDefaultCalendar(enrollerID);

            // Subscribe to the enroller's calendar
            SubscribeToCustomerCalendar(customerID, calendar.CalendarID);
        }

        /// <summary>
        /// Ensure Calendar and Calendar Subscriptions are valid for a specified customer.
        /// </summary>
        /// <param name="customerID">The CustomerID to validate Calendar and Calendar Subscriptions for.</param>
        /// <returns>The Calendar for the Current Customer</returns>
        public static Calendar EnsureCalendar(int customerID)
        {
            // Get the provided customer's personal calendar count to see if they have one
            var calendars = GetCalendars(new GetCalendarsRequest
            {
                CustomerID = customerID,
                IncludeCalendarSubscriptions = false
            });

            // If we didn't find any calendars, create a default calendar and return it.
            // Otherwise, return the first calendar we found.
            var calendar = calendars.Any() ? calendars.FirstOrDefault() : CreateDefaultCalendar(customerID);

            // Ensure that we have at least one subscription, which is to our enroller
            var subscriptions = GetCustomerCalendarSubscriptions(customerID);

            // If we don't have any Subscriptions create the default one
            if (!subscriptions.Any() && !(customerID == GlobalSettings.Company.CorporateCalendarAccountID))
            {
                CreateDefaultCalendarSubscriptions(customerID);
            }

            return calendar;
        }

        #endregion

        #region Save

        /// <summary>
        /// Creates a Calendar
        /// </summary>
        /// <param name="request">Request encompassing, the CustomerID to create a Calendar for, the Description of the Calendar, and more related information.</param>
        /// <returns>The Newly Created Calendar</returns>
        public static Calendar CreateCalendar(CreateCalendarRequest request)
        {
            // Create the calendar
            var calendar = new Calendar()
            {
                CalendarID = Guid.NewGuid(),
                CustomerID = request.CustomerID,
                Description = request.Description,
                CalendarTypeID = 1,
                CreatedDate = DateTime.Now
            };

            // Establish a SQL Connection
            using (var ctx = ExigoDAL.Sql())
            {
                // Save the Calendar to the DB
                ctx.Execute(@" 
                    INSERT INTO ExigoWebContext.Calendars (CalendarID, CustomerID, Description, CalendarTypeID, CreatedDate) VALUES (@calendarID, @customerID, @description, @calendarTypeID, @createdDate)",
                new
                {
                    calendarID = calendar.CalendarID,
                    customerID = calendar.CustomerID,
                    description = calendar.Description,
                    calendarTypeID = calendar.CalendarTypeID,
                    createdDate = calendar.CreatedDate
                });
            }

            // Return the saved calendar
            return calendar;
        }

        /// <summary>
        /// Call Create Calendar with Defaults
        /// </summary>
        /// <param name="customerID">CustomeID to create calendar for.</param>
        /// <returns>Customer's New Calendar</returns>
        public static Calendar CreateDefaultCalendar(int customerID)
        {
            return CreateCalendar(new CreateCalendarRequest
            {
                CustomerID = customerID,
                Description = "Personal Calendar"
            });
        }

        /// <summary>
        /// Saves Calendar Event to ExigoWebContext
        /// </summary>
        /// <param name="model">Calendar Event to Save.</param>
        /// <returns>Calendar Event</returns>
        public static CalendarEvent SaveCalendarEvent(CalendarEvent model)
        {
            // Default the SQL Command to Updating an Existing Event
            var sql = @"
                        UPDATE ExigoWebContext.CalendarEvents 
                        SET
                            CalendarID = @calendarID, 
                            CalendarEventTypeID = @calendarEventTypeID, 
                            CalendarEventPrivacyTypeID = @calendarEventPrivacyTypeID, 
                            CreatedBy = @createdBy, CreatedDate = @createdDate, 
                            Description = @description, 
                            Start = @start, 
                            [End] = @end, 
                            StartTimezone = @startTimezone, 
                            EndTimezone = @endTimezone, 
                            IsAllDay = @isAllDay, 
                            Title = @title, 
                            Address1 = @address1, 
                            Address2 = @address2, 
                            City = @city, 
                            State = @state, 
                            Zip = @zip, 
                            Country = @country, 
                            CalendarSpeakerID = @speakersID, 
                            Phone = @phone, 
                            Flyer = @flyer, 
                            Cost = @cost, 
                            ConferenceNumber = @conferenceNumber, 
                            ConferencePIN = @conferencePIN, 
                            Url = @url, 
                            RecurrenceException = @recurrenceException, 
                            RecurrenceID = @recurrenceID, 
                            RecurrenceRule = @recurrenceRule
                        WHERE
                            CalendarEventID = @calendarEventID
                    ";

            // If the Calendar Event has no ID then we need to create one
            if (model.CalendarEventID == Guid.Empty)
            {
                // Create a New Calendar Event ID
                model.CalendarEventID = Guid.NewGuid();

                // Change the SQL Command to Saving an Event
                sql = @"
                        INSERT INTO ExigoWebContext.CalendarEvents
                            (CalendarEventID, CalendarID, CalendarEventTypeID, CalendarEventPrivacyTypeID, CreatedBy, CreatedDate, Description, Start, [End], StartTimezone, EndTimezone, IsAllDay, Title, Address1, Address2, City, State, Zip, Country, CalendarSpeakerID, Phone, Flyer, Cost, ConferenceNumber, ConferencePIN, Url, RecurrenceException, RecurrenceID, RecurrenceRule)
                        VALUES 
                            (@calendarEventID, @calendarID, @calendarEventTypeID, @calendarEventPrivacyTypeID, @createdBy, @createdDate, @description, @start, @end, @startTimezone, @endTimezone, @isAllDay, @title, @address1, @address2, @city, @state, @zip, @country, @speakersID, @phone, @flyer, @cost, @conferenceNumber, @conferencePIN, @url, @recurrenceException, @recurrenceID, @recurrenceRule)
                    ";
            }

            // Establish a SQL Connection
            using (var ctx = ExigoDAL.Sql())
            {
                // Execute the SQL Command
                ctx.Execute(sql, new
                {
                    calendarEventID = model.CalendarEventID,
                    calendarID = model.CalendarID,
                    calendarEventTypeID = model.CalendarEventTypeID,
                    calendarEventPrivacyTypeID = model.CalendarEventPrivacyTypeID,
                    createdBy = model.CreatedBy,
                    createdDate = model.CreatedDate,
                    description = model.Description,
                    start = model.Start,
                    end = model.End,
                    startTimezone = model.StartTimezone,
                    endTimezone = model.EndTimezone,
                    isAllDay = model.IsAllDay,
                    title = model.Title,
                    address1 = model.Location.Address1,
                    address2 = model.Location.Address2,
                    city = model.Location.City,
                    state = model.Location.State,
                    zip = model.Location.Zip,
                    country = model.Location.Country,
                    speakersID = model.SpeakerID,
                    phone = model.Phone,
                    flyer = model.Flyer,
                    cost = model.Cost,
                    conferenceNumber = model.ConferenceNumber,
                    conferencePIN = model.ConferencePIN,
                    url = model.Url,
                    recurrenceException = model.RecurrenceException,
                    recurrenceID = model.RecurrenceID,
                    recurrenceRule = model.RecurrenceRule
                });
            }

            // Save Tags if applicable
            if (model.Tags.Any())
            {
                SaveCalendarEventTags(model.CalendarEventID, model.Tags);
            }

            return model;
        }

        /// <summary>
        /// Saves a list of Calendar Event Tags to ExigoWebContext.
        /// </summary>
        /// <param name="tags">The tags to save.</param>
        public static void SaveCalendarEventTags(Guid calendarEventID, IEnumerable<string> tags)
        {
            // Break up tags separated by commas into a list of unique strings
            IEnumerable<string> distinctTags = new List<string>();
            foreach (var newTag in tags)
            {
                // Split up any tags with commas
                var tempTags = newTag.Split(',').ToList();
                // Trim excess spaces and convert to lowercase for distinct comparison
                for (var i = 0; i < tempTags.Count(); i++)
                {
                    tempTags[i] = tempTags[i].Trim().ToLower();
                }
                // Add distinct tags from this tag
                distinctTags = distinctTags.Concat(tempTags.Distinct());

            }

            // ensure distinct tags between all tags.
            distinctTags = distinctTags.Distinct().ToList();

            // Get the Existing Tags
            var dBTags = GetTags();

            // Separate the Tags that do not already exist (new tags)
            var tagsToSave = distinctTags.Except(dBTags);

            // Save each new tag
            foreach (var tag in tagsToSave)
            {
                var newTag = new Tag()
                {
                    TagID = Guid.NewGuid(),
                    Name = tag
                };

                // Establish a SQL Connection
                using (var ctx = ExigoDAL.Sql())
                {
                    // Save the Tag
                    ctx.Execute(@"INSERT INTO ExigoWebContext.Tags (TagID, Name) VALUES (@tagID, @name) ",
                        new { tagID = newTag.TagID, name = newTag.Name });
                }
            }

            // Create a Global Tag Collection Variable
            var tagMappings = new List<Tag>();

            // Establish a SQL Connection
            using (var ctx = ExigoDAL.Sql())
            {
                // Retrieve the Tags from the Context (so that we have the full objects including the new ones we just saved)
                tagMappings =
                    ctx.Query<Tag>(@" SELECT [TagID], [Name] From ExigoWebContext.Tags WHERE Name IN @tags ", new { tags = distinctTags }).ToList();
                // Remove all existing tags that are no longer valid
                ctx.Execute(
                        @"DELETE FROM ExigoWebContext.CalendarEventTags WHERE CalendarEventID = @calendarEventID AND TagID NOT IN @tagIDs"
                        , new
                        {
                            calendarEventID = calendarEventID,
                            tagIDs = tagMappings.Select(t => t.TagID)
                        });
            }

            // Add each tag to the mapping table between CalendarEvents and Tags
            foreach (var tag in tagMappings)
            {
                var newCalendarEventTag = new CalendarEventTag()
                {
                    CalendarEventTagID = Guid.NewGuid(),
                    CalendarEventID = calendarEventID,
                    TagID = tag.TagID
                };

                // Establish a SQL Connection
                using (var ctx = ExigoDAL.Sql())
                {

                    // Save the Calendar Event Tag if it is not already associated with this event
                    ctx.Execute(
                        @"
                            IF NOT EXISTS (
                                SELECT 1 
                                FROM ExigoWebContext.CalendarEventTags 
                                WHERE 
                                    CalendarEventID = @calendarEventID 
                                    AND TagID = @tagID)
                            BEGIN
                                INSERT INTO ExigoWebContext.CalendarEventTags (CalendarEventTagID, CalendarEventID, TagID) 
                                VALUES (@calendarEventTagID, @calendarEventID, @tagID) 
                            END",
                        new
                        {
                            calendarEventTagID = newCalendarEventTag.CalendarEventTagID,
                            calendarEventID = newCalendarEventTag.CalendarEventID,
                            tagID = newCalendarEventTag.TagID
                        });
                }
            }
        }

        /// <summary>
        /// Creates a Calendar Subscription Record in the DB
        /// </summary>
        /// <param name="customerID">The CustomerID of the subscribee.</param>
        /// <param name="calendarID">The CalendarID of the subscription.</param>
        public static void SubscribeToCustomerCalendar(int customerID, Guid calendarID)
        {
            // Establish a SQL Connection
            using (var ctx = ExigoDAL.Sql())
            {
                // Insert a Calendar Subscription Record
                ctx.Execute(
                    @" INSERT INTO ExigoWebContext.CalendarSubscriptions (CustomerID, CalendarID) VALUES (@customerID, @calendarID) ",
                    new
                    {
                        customerID,
                        calendarID
                    });
            }

            // Clear Get Calendars Cache
            InvalidateCalendarCache("GetCalendars/{0}/{1}", customerID, true);
        }

        #endregion

        #region Delete

        /// <summary>
        /// Deletes Calendar Event from the ExigoWebContext.
        /// </summary>
        /// <param name="calendarEventID">The Guid of the Calendar Event to delete.</param>
        public static void DeleteCalendarEvent(Guid calendarEventID)
        {
            // Establish a SQL Connection
            using (var ctx = ExigoDAL.Sql())
            {
                // Delete the Calendar Event from the DB
                ctx.Execute("DELETE FROM ExigoWebContext.CalendarEvents WHERE CalendarEventID = @calendarEventID",
                    new { calendarEventID });
            }
        }

        /// <summary>
        /// Unsubscribes a Customer from a Calendar.
        /// </summary>
        /// <param name="calendarID">The Guid of the Calendar to delete.</param>
        public static void UnsubscribeFromCustomerCalendar(int customerID, Guid calendarID)
        {
            // Establish a SQL Connection
            using (var ctx = ExigoDAL.Sql())
            {
                // Delete the Calendar Subscription Record
                ctx.ExecuteReader(
                    @" DELETE FROM ExigoWebContext.CalendarSubscriptions WHERE CustomerID = @customerID AND CalendarID = @calendarID",
                    new { customerID, calendarID });
            }

            // Invalidate the Get Calendar Cache Key
            InvalidateCalendarCache("GetCalendars/{0}/{1}", customerID, true);
        }

        #endregion

        /// <summary>
        /// Invalidates the supplied Cache Key.
        /// </summary>
        /// <param name="cacheKeyFormat">The Cache Key to invalidate.</param>
        /// <param name="arguments">Any arguments that are used to format the Cache Key string.</param>
        private static void InvalidateCalendarCache(string cacheKeyFormat, params object[] arguments)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Remove(string.Format(cacheKeyFormat, arguments));
            }
        }

    }
}