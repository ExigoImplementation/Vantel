using System;
using System.Globalization;
using System.Linq;

namespace Common
{
    public static partial class GlobalUtilities
    {
        /// <summary>
        /// Formats the time difference into text similar to what you would see in a social network.
        /// </summary>
        /// <param name="startDateTime">The DateTime of the event. This date is usually in the past.</param>
        /// <param name="endDateTime">The DateTime you want to compare your event DateTime to. Usually DateTime.Now.</param>
        /// <returns>A formatted string description explaining the time difference in simple terms.</returns>
        public static string GetFormattedTimeDifference(DateTime startDateTime, DateTime endDateTime)
        {
            var startDate = startDateTime;
            var endDate = endDateTime;
            var diff = endDate.Subtract(startDate);

            // Start with the smallest increments, and go up from there.
            var description = string.Empty;
            if (Math.Round(diff.TotalSeconds, 0) == 0) description = string.Format("Just now");
            else if (Math.Round(diff.TotalSeconds, 0) == 1) description = string.Format("{0:0} second ago", diff.TotalSeconds);
            else if (Math.Round(diff.TotalSeconds, 0) < 60) description = string.Format("{0:0} seconds ago", diff.TotalSeconds);
            else if (Math.Round(diff.TotalMinutes, 0) == 1) description = string.Format("{0:0} minute ago", diff.TotalMinutes);
            else if (Math.Round(diff.TotalMinutes, 0) < 60) description = string.Format("{0:0} minutes ago", diff.TotalMinutes);
            else if (Math.Round(diff.TotalHours, 0) == 1) description = string.Format("{0:0} hour ago", diff.TotalHours);
            else if (Math.Round(diff.TotalHours, 0) < 24) description = string.Format("{0:0} hours ago", diff.TotalHours);
            else if (Math.Round(diff.TotalDays, 0) == 1) description = string.Format("{0:0} day ago", diff.TotalDays);
            else if (Math.Round(diff.TotalDays, 0) < 7) description = string.Format("{0:0} days ago", diff.TotalDays);
            else description = string.Format("{0:MMMM d, yyyy}", startDate);

            return description;
        }

        /// <summary>
        /// Get a DateTime that represents the provided day of the week in the provided numerical week of the provided date. Example: 2nd Thursday.
        /// </summary>
        /// <param name="date">The date</param>
        /// <param name="nthWeek">The numerical week to get the DateTime in</param>
        /// <param name="dayOfWeek">The day of the week you want the DateTime to be in</param>
        /// <returns></returns>
        public static DateTime GetNthWeekofMonth(DateTime date, int nthWeek, DayOfWeek dayOfWeek)
        {
            return date.Next(dayOfWeek).AddDays((nthWeek - 1) * 7);
        }

        /// <summary>
        /// Parses a JavaScript Date() string into a C# DateTime object.
        /// </summary>
        /// <param name="datetime">The JavaScript Date() string to parse.</param>
        /// <returns>The parsed C# DateTime</returns>
        public static DateTime ParseJavaScriptDate(string datetime)
        {
            return DateTime.ParseExact(datetime.Substring(0, 24),
                "ddd MMM d yyyy HH:mm:ss",
                CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Attempts to parse a JavaScript Date() string into a C# DateTime object.
        /// </summary>
        /// <param name="datetime">The JavaScript Date() string to parse.</param>
        /// <returns>Whether the parsing was successful</returns>
        public static bool TryParseJavaScriptDate(string datetime, out DateTime result)
        {
            try
            {
                result = ParseJavaScriptDate(datetime);
                return true;
            }
            catch
            {
                result = DateTime.MinValue;
                return false;
            }
        }

        /// <summary>
        /// Converts Timezone from IANA format to .NET Timezone.
        /// </summary>
        /// <param name="ianaZoneId">Timezone ID in IANA/Olson Format.</param>
        /// <returns>.NET Timezone.</returns>
        public static string IanaToNet(string ianaZoneId)
        {
            // Create a String Array of UTC Zones
            string[] utcZones = { "Etc/UTC", "Etc/UCT", "Etc/GMT" };

            // If our list of UTC Zones contains the ianaZoneId then return UTC
            if (utcZones.Contains(ianaZoneId, StringComparer.Ordinal))
            {
                return "UTC";
            }

            // Create Local Copy of Time Zone Database (IANA/Olson)
            var tzdbSource = NodaTime.TimeZones.TzdbDateTimeZoneSource.Default;

            // Resolve any link, since the CLDR doesn't necessarily use canonical IDs
            var links = tzdbSource.CanonicalIdMap.Where(tz => tz.Value.Equals(ianaZoneId, StringComparison.Ordinal)).Select(tz => tz.Key);

            // Resolve canonical zones, and include original zone as well
            var possibleZones = tzdbSource.CanonicalIdMap.ContainsKey(ianaZoneId) ? links.Concat(new[] { tzdbSource.CanonicalIdMap[ianaZoneId], ianaZoneId }) : links;

            // Map the windows zone
            var mappings = tzdbSource.WindowsMapping.MapZones;
            var item = mappings.FirstOrDefault(x => x.TzdbIds.Any(possibleZones.Contains));

            // Return our Result for the Mappings
            return (item == null) ? null : item.WindowsId;
        }

        /// <summary>
        /// Converts Timezone from .NET Timezone to IANA/Olson Format.
        /// </summary>
        /// <param name="netZone">Timezone in .Net Format.</param>
        /// <returns>IANA/Olson Timezone.</returns>
        public static string NetToIana(string netZone)
        {
            // If our netZone is UTC then return Etc/UTC
            if (netZone.Equals("UTC", StringComparison.Ordinal))
            {
                return "Etc/UTC";
            }

            // Create Local Copy of Time Zone Database (IANA/Olson)
            var tzdbSource = NodaTime.TimeZones.TzdbDateTimeZoneSource.Default;

            // Get the .NET Timezone ID for the netZone
            var tzi = TimeZoneInfo.FindSystemTimeZoneById(netZone);

            // If we didn't find our result then stop here
            if (tzi == null)
            {
                return null;
            }

            // Locate the IANA/Olson Id for netZone's ID
            var tzid = tzdbSource.MapTimeZoneId(tzi);

            // Return Canonical ID that matches our netZone
            return (tzid == null) ? null : tzdbSource.CanonicalIdMap[tzid];
        }

    }
}