using Common;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using Dapper;
using System;
using System.Web.Caching;

namespace ExigoService
{
    public static partial class ExigoDAL
    {
        private static IEnumerable<Period> GetAllPeriods(int periodTypeID)
        {
            var cacheKey = GlobalSettings.Exigo.Api.CompanyKey + "AllPeriods_" + periodTypeID.ToString();

            if (!MemoryCache.Default.Contains(cacheKey))
            {
                using (var context = ExigoDAL.Sql())
                {
                    var periods = context.Query<Period>(@"
                    SELECT
                        PeriodTypeID
                        , PeriodID
                        , PeriodDescription
                        , StartDate
                        , EndDate
                        , AcceptedDate
                    FROM
                        Periods
                    WHERE 
                        PeriodTypeID = @PeriodTypeID
                    ", new
                    {
                        PeriodTypeID = periodTypeID
                    }).ToList();
                    MemoryCache.Default.Add(cacheKey, periods, DateTime.Now.AddMinutes(GlobalSettings.Exigo.CacheTimeout));
                    foreach (var period in periods)
                    {
                        yield return period;
                    }
                }
            }
            var cachePeriods = MemoryCache.Default.Get(cacheKey) as IEnumerable<Period>;
            if(cachePeriods == null) yield break;

            foreach (var period in cachePeriods)
            {
                yield return period;
            }

        }

        public static List<Period> GetPeriods(GetPeriodsRequest request)
        {
            if (request.PeriodTypeID <= 0)
            {
                throw new ArgumentException("periodTypeID is not given", nameof(request.PeriodTypeID));
            }

            var periods = GetAllPeriods(request.PeriodTypeID).AsQueryable();

            // (optional) Filter by the customer.
            // If the customer is provided, only periods the customer was a part of will be returned.
            if (request.CustomerID.HasValue)
            {
                var customerCreatedDate = GetCustomerCreatedDate(request.CustomerID.Value);
                periods = periods.Where(c => c.EndDate >= customerCreatedDate);
            }

            // (optional) Filter by requested period IDs
            if (request.PeriodIDs != null &&  request.PeriodIDs.Any())
            {
                periods = periods.Where(p => request.PeriodIDs.Contains(p.PeriodID));
            }

            // (optional) Filter by periods that have been accepted
            if (request.OnlyAcceptedPeriods)
            {
                periods = periods.Where(p => p.AcceptedDate != null);
            }

            return periods.ToList();
        }
        public static Period GetCurrentPeriod(int periodTypeID)
        {
            if (periodTypeID <= 0)
            {
                throw new ArgumentException("periodTypeID is not given", nameof(periodTypeID));
            }

            var cacheKey = GlobalSettings.Exigo.Api.CompanyKey + "CurrentPeriod_" + periodTypeID.ToString();

            if (!MemoryCache.Default.Contains(cacheKey))
            {
                // Get the last period that started before today. 
                // This prevents site from breaking when the periods table runs out.
                var period = GetAllPeriods(periodTypeID)
                    .Where(x => x.StartDate < DateTime.Now.ToCST())// get periods that started before now (period dates are in CST timezone)
                    .OrderBy(p => p.StartDate) // order periods in ascending order (1,2,3,4....)
                    .LastOrDefault(); // get the last (most recent) period

                if (period != null)
                {
                    MemoryCache.Default.Add(cacheKey, period, DateTime.Now.AddMinutes(GlobalSettings.Exigo.CacheTimeout));
                }
                return period;
            }
            var data = MemoryCache.Default.Get(cacheKey) as Period;
            return data;
        }
    }
}
