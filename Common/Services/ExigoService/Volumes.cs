using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using Dapper;
using System.Data;

namespace ExigoService
{
    public static partial class ExigoDAL
    {
        public static VolumeCollection GetCustomerVolumes(GetCustomerVolumesRequest request)
        {
            var periodID = request.PeriodID;
            var periodTypeID = request.PeriodTypeID;
            if (periodID == null)
            {
                periodID = ExigoDAL.GetCurrentPeriod(periodTypeID).PeriodID;
            }
            VolumeCollection volumes = new VolumeCollection();

            // Determine if we need to pull all Period Volumes or if we are passing in our own list of Volumes to fetch
            int totalVolumeBuckets = 200;
            string volumeSelectQuery = "";

            if (request.VolumesToFetch == null || request.VolumesToFetch.Count() == 0)
            {
                request.VolumesToFetch = new List<int>();
                for (int i = 1; i <= totalVolumeBuckets; i++)
                {
                    request.VolumesToFetch.Add(i);
                }
            }

            for (int i = 0, length = request.VolumesToFetch.Count(); i < length; i++)
            {
                var volumeBucket = request.VolumesToFetch[i];
                volumeSelectQuery = volumeSelectQuery + " , Volume{0} = isnull(pv.Volume{0}, 0)".FormatWith(volumeBucket);
            }


            using (var context = ExigoDAL.Sql())
            {
                volumes = context.Query<VolumeCollection, Period, Rank, Rank, VolumeCollection>(@"
                            Select 
                                c.CustomerID			                        
			                    , ModifiedDate = isnull(pv.ModifiedDate, '01/01/1900')
                                " + volumeSelectQuery + @"
		                        , PeriodID = p.PeriodID
			                    , PeriodTypeID = p.PeriodTypeID
		                        , PeriodDescription = p.PeriodDescription
		                        , StartDate = p.StartDate
		                        , EndDate = p.EndDate
		                        , RankID = isnull(pv.RankID,0)
		                        , RankDescription = isnull(r.RankDescription, '')
		                        , RankID = isnull(pv.PaidRankID,0)
		                        , RankDescription = isnull(pr.RankDescription, '')                                
	                        FROM Customers c
		                        LEFT JOIN PeriodVolumes pv
		                            ON pv.CustomerID = c.CustomerID
	                            LEFT JOIN Periods p
		                            ON pv.PeriodID = p.PeriodID
		                            AND pv.PeriodTypeID = p.PeriodTypeID
	                            LEFT JOIN Ranks r
		                            ON r.RankID = c.RankID
	                            LEFT JOIN Ranks pr
		                            ON pr.RankID = pv.PaidRankID
	                        WHERE pv.CustomerID = @CustomerID
		                        AND p.PeriodTypeID = @PeriodTypeID
                                AND p.PeriodID = @PeriodID
                    ", (vc, p, hr, pr) =>
                     {
                         vc.Period = p;
                         vc.HighestAchievedRankThisPeriod = hr;
                         vc.PayableAsRank = pr;
                         return vc;
                     }
                        , param: new
                        {
                            CustomerID = request.CustomerID,
                            PeriodTypeID = request.PeriodTypeID,
                            PeriodID = periodID
                        }
                        , splitOn: "PeriodID, RankID, RankID"
                     ).FirstOrDefault();
            }

            return (volumes != null) ? volumes : new VolumeCollection();
    
        }
    }
}
