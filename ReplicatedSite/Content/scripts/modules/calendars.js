// Calendars module
define(["moment"], function (moment) {

    var module = {
        getDayOfWeekName: function (day) {
            switch (Number(day)) {
                case 0: return moment.weekdays()[day];
                case 1: return moment.weekdays()[day];
                case 2: return moment.weekdays()[day];
                case 3: return moment.weekdays()[day];
                case 4: return moment.weekdays()[day];
                case 5: return moment.weekdays()[day];
                case 6: return moment.weekdays()[day];
                default: return day;
            }
        },
        getOrdinalWeekNumber: function (weekNumber) {
            switch (weekNumber) {
                case 1: return "first";
                case 2: return "second";
                case 3: return "third";
                case 4: return "fourth";
                case 5: return "fifth";
                case 6: return "sixth";
                default: return weekNumber;
            }
        },
        getCalendarEventRecurrenceSummary: function (request) {

            var result = "";

            // Format our start date in Moment to make this easier
            request.StartDate = new moment(request.StartDate);

            // If we aren't repeating the event, stop here
            if (!request.CalendarEventRecurrenceTypeID || request.CalendarEventRecurrenceTypeID == '0') return "None";
            request.CalendarEventRecurrenceTypeID = Number(request.CalendarEventRecurrenceTypeID);


            var repeatType = "";
            switch (request.CalendarEventRecurrenceTypeID) {
                case 1:
                    if (request.CalendarEventRecurrenceInterval == 1) result = "Daily";
                    else result = "Every {0} days".format(request.CalendarEventRecurrenceInterval);
                    break;

                case 2:
                    if (request.CalendarEventRecurrenceInterval == 1) result = "Weekly";
                    else result = "Every {0} weeks".format(request.CalendarEventRecurrenceInterval);

                    request.CalendarWeeklyEventRecurrenceDays = request.CalendarWeeklyEventRecurrenceDays || request.StartDate.day();
                    var repeatingDays = request.CalendarWeeklyEventRecurrenceDays.split(',');
                    var repeatingDayNames = [];
                    for (var i = 0; i < repeatingDays.length; i++) {
                        repeatingDayNames.push(module.getDayOfWeekName(Number(repeatingDays[i])) + "s");
                    }

                    // Just say 'weekends' or 'weekdays' if applicable
                    if (request.CalendarWeeklyEventRecurrenceDays == '1,2,3,4,5') result += ", every weekday";
                    else result += " on {0}".format(repeatingDayNames.join(', '));
                    break;

                case 3:
                    if (request.CalendarEventRecurrenceInterval == 1) result = "Monthly";
                    else result = "Every {0} months".format(request.CalendarEventRecurrenceInterval);

                    request.CalendarMonthlyEventRecurrenceTypeID = request.CalendarMonthlyEventRecurrenceTypeID || 1;

                    switch (request.CalendarMonthlyEventRecurrenceTypeID) {
                        case 1:
                            result += " on day {0}".format(request.StartDate.format('D'));
                            break;

                        case 2:
                            var dayName = module.getDayOfWeekName(request.StartDate.day());
                            var nthOfMonth = module.getOrdinalWeekNumber(Math.ceil(request.StartDate.date() / 7));

                            result += " on the {0} {1}".format(nthOfMonth, dayName);
                            break;
                    }
                    break;

                case 4:
                    if (request.CalendarEventRecurrenceInterval == 1) result = "Annually";
                    else result = "Every {0} years".format(request.CalendarEventRecurrenceInterval);

                    result += " on {0}".format(request.StartDate.format('MMMM D'));
                    break;
            }

            if (request.CalendarEventRecurrenceMaxInstances) {
                if (request.CalendarEventRecurrenceMaxInstances == 1) result = "Once";
                else result += ", {0} times".format(request.CalendarEventRecurrenceMaxInstances);
            }

            if (request.CalendarEventRecurrenceEndDate) {
                result += " until {0}".format(new moment(request.CalendarEventRecurrenceEndDate).format('MMMM Do, YYYY'));
            }


            return result;
        }
    };


    return module;
});