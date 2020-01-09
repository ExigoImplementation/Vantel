using Common.Api.ExigoWebService;
using System;

namespace Common
{
    public static partial class GlobalUtilities
    {
        /// <summary>
        /// Gets the start date for an autoOrders with the provided frequency type.
        /// </summary>
        /// <param name="frequency">How often the autoOrder will run</param>
        /// <returns>The start date for an autoOrder</returns>
        public static DateTime GetAutoOrderStartDate(FrequencyType frequency)
        {
            DateTime autoOrderStartDate = DateTime.Now.ToCST().Date;

            switch (frequency)
            {
                case FrequencyType.Weekly: autoOrderStartDate         = autoOrderStartDate.AddDays(7); break;
                case FrequencyType.BiWeekly: autoOrderStartDate       = autoOrderStartDate.AddDays(14); break;
                case FrequencyType.EveryFourWeeks: autoOrderStartDate = autoOrderStartDate.AddDays(28); break;
                case FrequencyType.Monthly: autoOrderStartDate        = autoOrderStartDate.AddMonths(1); break;
                case FrequencyType.BiMonthly: autoOrderStartDate      = autoOrderStartDate.AddMonths(2); break;
                case FrequencyType.Quarterly: autoOrderStartDate      = autoOrderStartDate.AddMonths(3); break;
                case FrequencyType.SemiYearly: autoOrderStartDate     = autoOrderStartDate.AddMonths(6); break;
                case FrequencyType.Yearly: autoOrderStartDate         = autoOrderStartDate.AddYears(1); break;
                default: break;
            }

            // Ensure we are not returning a day of 29, 30 or 31.
            autoOrderStartDate = GetNextAvailableAutoOrderStartDate(autoOrderStartDate);

            return autoOrderStartDate;
        }

        /// <summary>
        /// Gets the next available date for an autoOrder starting with the provided date.
        /// </summary>
        /// <param name="date">The original start date</param>
        /// <returns>The nearest available start date for an autoOrder</returns>
        public static DateTime GetNextAvailableAutoOrderStartDate(DateTime date)
        {
            // Ensure we are not returning a day of 29, 30 or 31.
            if (date.Day > 28)
            {
                date = new DateTime(date.AddMonths(1).Year, date.AddMonths(1).Month, 1).Date;
            }

            return date;
        }
    }
}