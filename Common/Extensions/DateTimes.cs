using System;


public static class DateTimeExtensions
{
    /// <summary>
    /// Gets the first week day following a date.
    /// </summary> 
    /// <param name="date">The date.</param> 
    /// <param name="dayOfWeek">The day of week to return.</param> 
    /// <returns>The first dayOfWeek day following date, or date if it is on dayOfWeek.</returns> 
    public static DateTime GetNextWeekDay(this DateTime date, DayOfWeek dayOfWeek)
    {
        return date.AddDays((dayOfWeek < date.DayOfWeek ? 7 : 0) + dayOfWeek - date.DayOfWeek);
    }

    ///<summary>Gets the first week day following a date.</summary>
    ///<param name="date">The date.</param>
    ///<param name="dayOfWeek">The day of week to return.</param>
    ///<returns>The first dayOfWeek day following date, or date if it is on dayOfWeek.</returns>
    public static DateTime Next(this DateTime date, DayOfWeek dayOfWeek)
    {
        return date.AddDays((dayOfWeek < date.DayOfWeek ? 7 : 0) + dayOfWeek - date.DayOfWeek);
    }

    /// <summary>
    /// Get a DateTime that represents the beginning of the hour of the provided date.
    /// </summary>
    /// <param name="date">The date</param>
    /// <returns></returns>
    public static DateTime BeginningOfHour(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0, 0, date.Kind);
    }

    /// <summary>
    /// Get a DateTime that represents the end of the hour of the provided date.
    /// </summary>
    /// <param name="date">The date</param>
    /// <returns></returns>
    public static DateTime EndOfHour(this DateTime date)
    {
        return date.BeginningOfHour().AddHours(1).AddSeconds(-1).AddTicks(-1);
    }

    /// <summary>
    /// Get a DateTime that represents the beginning of the day of the provided date.
    /// </summary>
    /// <param name="date">The date</param>
    /// <returns></returns>
    public static DateTime BeginningOfDay(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0, date.Kind);
    }

    /// <summary>
    /// Get a DateTime that represents the end of the day of the provided date.
    /// </summary>
    /// <param name="date">The date</param>
    /// <returns></returns>
    public static DateTime EndOfDay(this DateTime date)
    {
        return date.BeginningOfDay().AddDays(1).AddTicks(-1);
    }

    /// <summary>
    /// Get a DateTime that represents the beginning of the month of the provided date.
    /// </summary>
    /// <param name="date">The date</param>
    /// <returns></returns>
    public static DateTime BeginningOfMonth(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, 1, 0, 0, 0, 0, date.Kind);
    }

    /// <summary>
    /// Get a DateTime that represents the end of the month of the provided date.
    /// </summary>
    /// <param name="date">The date</param>
    /// <returns></returns>
    public static DateTime EndOfMonth(this DateTime date)
    {
        return date.BeginningOfMonth().AddMonths(1).AddTicks(-1);
    }

    /// <summary>
    /// Get a DateTime that represents the beginning of the year of the provided date.
    /// </summary>
    /// <param name="date">The date</param>
    /// <returns></returns>
    public static DateTime BeginningOfYear(this DateTime date)
    {
        return new DateTime(date.Year, 1, 1, 0, 0, 0, 0, date.Kind);
    }

    /// <summary>
    /// Get a DateTime that represents the end of the year of the provided date.
    /// </summary>
    /// <param name="date">The date</param>
    /// <returns></returns>
    public static DateTime EndOfYear(this DateTime date)
    {
        return date.BeginningOfYear().AddYears(1).AddTicks(-1);
    }

    /// <summary>
    /// Determines if the provided datetime is a weekend.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static bool IsWeekend(this DateTime date)
    {
        return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
    }

    /// <summary>
    /// Gets a string describing the amount of time that has passed since the provided date.
    /// </summary>
    /// <param name="date">The starting date</param>
    /// <param name="pastDate">A past date to compare</param>
    /// <returns>A string describing the amount of time that has passed since the provided date.</returns>
    public static string RelativeTimeSince(this DateTime date, DateTime pastDate)
    {
        var startDate = pastDate;
        var endDate = date;
        var diff = endDate.Subtract(startDate);
        var delta = Math.Abs(diff.TotalSeconds);

        if (delta < 60)
        {
            return diff.Seconds == 1 ? "One second ago" : diff.Seconds + " seconds ago";
        }
        if (delta < 120)
        {
            return "A minute ago";
        }
        if (delta < 2700) // 45 * 60
        {
            return diff.Minutes + " minutes ago";
        }
        if (delta < 5400) // 90 * 60
        {
            return "An hour ago";
        }
        if (delta < 86400) // 24 * 60 * 60
        {
            return diff.Hours + " hours ago";
        }
        if (delta < 172800) // 48 * 60 * 60
        {
            return "Yesterday";
        }
        if (delta < 7776000) // 30 * 24 * 60 * 60
        {
            return diff.Days + " days ago";
        }
        if (delta < 31104000) // 12 * 30 * 24 * 60 * 60
        {
            int months = Convert.ToInt32(Math.Floor((double)diff.Days / 30));
            return months <= 1 ? "One month ago" : months + " months ago";
        }

        var years = Convert.ToInt32(Math.Floor((double)diff.Days / 365));
        return years <= 1 ? "One year ago" : years + " years ago";
    }

    /// <summary>
    /// Gets a string describing the amount of time that has passed since the provided date.
    /// </summary>
    /// <param name="date">The starting date</param>
    /// <param name="pastDate">A past date to compare</param>
    /// <returns>A string describing the amount of time that has passed since the provided date.</returns>
    public static string RelativeTimeSince(this DateTime date, DateTime? pastDate)
    {
        if (pastDate.HasValue)
        {
            return date.RelativeTimeSince(pastDate.Value);
        }
        return string.Empty;
    }

    /// <summary>
    /// Gets a string describing the days that have passed since the provided date.
    /// </summary>
    /// <param name="date">The starting date</param>
    /// <param name="pastDate">A past date to compare</param>
    /// <returns>A string describing the amount of time that has passed since the provided date.</returns>
    public static string RelativeDateSince(this DateTime date, DateTime pastDate)
    {
        var startDate = pastDate;
        var endDate = date;
        var diff = endDate.Subtract(startDate);
        var delta = Math.Abs(diff.TotalSeconds);

        if (delta < 86400) // 24 * 60 * 60
        {
            return "Today";
        }
        if (delta < 172800) // 48 * 60 * 60
        {
            return "Yesterday";
        }
        if (delta < 7776000) // 30 * 24 * 60 * 60
        {
            return diff.Days + " days ago";
        }
        if (delta < 31104000) // 12 * 30 * 24 * 60 * 60
        {
            int months = Convert.ToInt32(Math.Floor((double)diff.Days / 30));
            return months <= 1 ? "One month ago" : months + " months ago";
        }

        var years = Convert.ToInt32(Math.Floor((double)diff.Days / 365));
        return years <= 1 ? "One year ago" : years + " years ago";
    }

    /// <summary>
    /// Gets a string describing the days that have passed since the provided date.
    /// </summary>
    /// <param name="date">The starting date</param>
    /// <param name="pastDate">A past date to compare</param>
    /// <returns>A string describing the amount of time that has passed since the provided date.</returns>
    public static string RelativeDateSince(this DateTime date, DateTime? pastDate)
    {
        if (pastDate.HasValue)
        {
            return date.RelativeDateSince(pastDate.Value);
        }
        return string.Empty;
    }

    /// <summary>
    /// Gets a string describing the amount of time between this date and the provided future date.
    /// </summary>
    /// <param name="date">The starting date</param>
    /// <param name="futureDate">A future date to compare</param>
    /// <returns>A string describing the amount of time between this date and the provided future date.</returns>
    public static string RelativeTimeUntil(this DateTime date, DateTime futureDate)
    {
        var startDate = date;
        var endDate = futureDate;
        var diff = endDate.Subtract(startDate);
        var delta = Math.Abs(diff.TotalSeconds);

        if (delta < 60)
        {
            return diff.Seconds == 1 ? "One second" : diff.Seconds + " seconds";
        }
        if (delta < 120)
        {
            return "One minute";
        }
        if (delta < 2700) // 45 * 60
        {
            return diff.Minutes + " minutes";
        }
        if (delta < 5400) // 90 * 60
        {
            return "One hour";
        }
        if (delta < 86400) // 24 * 60 * 60
        {
            return diff.Hours + " hours";
        }
        if (delta < 172800) // 48 * 60 * 60
        {
            return "Tomorrow";
        }
        if (delta < 7776000) // 30 * 24 * 60 * 60
        {
            return diff.Days + " days";
        }
        if (delta < 31104000) // 12 * 30 * 24 * 60 * 60
        {
            int months = Convert.ToInt32(Math.Floor((double)diff.Days / 30));
            return months <= 1 ? "One month" : months + " months ago";
        }

        var years = Convert.ToInt32(Math.Floor((double)diff.Days / 365));
        return years <= 1 ? "One year" : years + " years ago";
    }

    /// <summary>
    /// Gets a string describing the amount of time between this date and the provided future date.
    /// </summary>
    /// <param name="date">The starting date</param>
    /// <param name="futureDate">A future date to compare</param>
    /// <returns>A string describing the amount of time between this date and the provided future date.</returns>
    public static string RelativeTimeUntil(this DateTime date, DateTime? futureDate)
    {
        if (futureDate.HasValue)
        {
            return date.RelativeTimeUntil(futureDate.Value);
        }
        return string.Empty;
    }

    /// <summary>
    /// Gets a string describing the days between this date and the provided future date.
    /// </summary>
    /// <param name="date">The starting date</param>
    /// <param name="futureDate">A future date to compare</param>
    /// <returns>A string describing the amount of time between this date and the provided future date.</returns>
    public static string RelativeDateUntil(this DateTime date, DateTime futureDate)
    {
        var startDate = date;
        var endDate = futureDate;
        var diff = endDate.Subtract(startDate);
        var delta = Math.Abs(diff.TotalSeconds);

        if (delta < 86400) // 24 * 60 * 60
        {
            return "Today";
        }
        if (delta < 172800) // 48 * 60 * 60
        {
            return "Tomorrow";
        }
        if (delta < 7776000) // 30 * 24 * 60 * 60
        {
            return diff.Days + " days";
        }
        if (delta < 31104000) // 12 * 30 * 24 * 60 * 60
        {
            int months = Convert.ToInt32(Math.Floor((double)diff.Days / 30));
            return months <= 1 ? "One month" : months + " months";
        }

        var years = Convert.ToInt32(Math.Floor((double)diff.Days / 365));
        return years <= 1 ? "One year" : years + " years";
    }

    /// <summary>
    /// Gets a string describing the days between this date and the provided future date.
    /// </summary>
    /// <param name="date">The starting date</param>
    /// <param name="futureDate">A future date to compare</param>
    /// <returns>A string describing the amount of time between this date and the provided future date.</returns>
    public static string RelativeDateUntil(this DateTime date, DateTime? futureDate)
    {
        if (futureDate.HasValue)
        {
            return date.RelativeDateUntil(futureDate.Value);
        }
        return string.Empty;
    }

    /// <summary>
    /// Converts the DateTime to Exigo's time zone, which is Central Standard Time.
    /// </summary>
    /// <param name="dateToBeConverted"></param>
    /// <returns></returns>
    public static DateTime ToExigoTime(this DateTime dateToBeConverted)
    {
        var DateinUTC = TimeZoneInfo.ConvertTimeToUtc(dateToBeConverted);
        TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
        return TimeZoneInfo.ConvertTimeFromUtc(DateinUTC, cstZone);
    }

    /// <summary>
    /// Converts the DateTime to CST time zone
    /// </summary>
    /// <param name="dateToBeConverted"></param>
    /// <returns></returns>
    public static DateTime ToCST(this DateTime dateToBeConverted)
    {
        var DateinUTC = TimeZoneInfo.ConvertTimeToUtc(dateToBeConverted);
        TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
        return TimeZoneInfo.ConvertTimeFromUtc(DateinUTC, cstZone);
    }
}