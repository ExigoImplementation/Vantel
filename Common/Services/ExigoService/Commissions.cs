using System.Collections.Generic;
using System.Linq;
using Common.Models.Dashboard;
using Dapper;

namespace ExigoService
{
    public static partial class ExigoDAL
    {
        public static IEnumerable<HistoricalCommission> GetCommissionList(int customerID)
        {
            // Historical Commissions
            var historicalCommissions = GetHistoricalCommissionList(customerID);

            if (historicalCommissions == null) return null;

            return historicalCommissions;
        }
        public static IEnumerable<HistoricalCommission> GetHistoricalCommissionList(int customerID)
        {
            // Historical Commissions
            var commissions = new List<HistoricalCommission>();
            using (var context = ExigoDAL.Sql())
            {
                commissions = context.Query<HistoricalCommission>(@"
                                SELECT c.CommissionRunID
                                  ,c.CustomerID
                                  ,c.CurrencyCode
                                  ,c.Earnings
                                  ,c.PreviousBalance
                                  ,c.BalanceForward
                                  ,c.Fee
                                  ,c.Total
                                  ,cr.CommissionRunDescription
                                  ,cr.PeriodTypeID
                                  ,cr.PeriodID
                                  ,cr.RunDate
                                  ,cr.AcceptedDate
                                  ,cr.CommissionRunStatusID
                                  ,cr.HideFromWeb
                                  ,cr.PlanID
                                  ,p.PeriodDescription
                                  ,p.StartDate
                                  ,p.EndDate
	                              ,prs.PeriodTypeID
                                  ,prs.PaidRankID
                                  ,prs.Score
                            FROM Commissions c
	                            LEFT JOIN CommissionRuns cr
		                            ON c.CommissionRunID = cr.CommissionRunID
	                            LEFT JOIN Periods p
		                            ON cr.PeriodID = p.PeriodID
	                            LEFT JOIN PeriodRankScores prs
		                            ON cr.PeriodID = prs.PeriodID
	                            LEFT JOIN Ranks r
		                            ON r.RankID = prs.PaidRankID
                            WHERE c.CustomerID = @customerID
                            ORDER BY cr.CommissionRunID DESC
                    ", new
                     {
                         customerID
                     }).ToList();
            }

            if (commissions == null) return null;

            return commissions;
        }

        public static IEnumerable<ICommission> GetCommissionPeriodList(int customerID, bool getRealTime = false)
        {
            // Historical Commissions
            var commissions = new List<ICommission>();
            using (var context = ExigoDAL.Sql())
            {
                commissions.AddRange(context.Query<HistoricalCommission, Period, HistoricalCommission>(@"
                                SELECT c.CommissionRunID
                                      ,c.CustomerID
                                      ,c.CurrencyCode
                                      ,c.Earnings
                                      ,c.PreviousBalance
                                      ,c.BalanceForward
                                      ,c.Fee
                                      ,c.Total
                                      ,cr.CommissionRunDescription
                                      ,cr.PeriodTypeID
                                      ,cr.PeriodID
                                      ,cr.RunDate
                                      ,cr.AcceptedDate
                                      ,cr.CommissionRunStatusID
                                      ,cr.HideFromWeb
                                      ,cr.PlanID
                                      ,p.PeriodID
                                      ,p.PeriodDescription
                                      ,p.PeriodTypeID
                                      ,p.StartDate
                                      ,p.EndDate
                                      ,p.AcceptedDate
                                FROM Commissions c
	                                LEFT JOIN CommissionRuns cr
		                                ON c.CommissionRunID = cr.CommissionRunID
	                                LEFT JOIN Periods p
		                                ON cr.PeriodID = p.PeriodID
		                                AND cr.PeriodTypeID = p.PeriodTypeID
                                WHERE c.CustomerID = @customerID
                                ORDER BY cr.CommissionRunID DESC
                    ", (hc, p) =>
                {
                    hc.Period = p;
                    return hc;
                }
                        , splitOn: "PeriodID"
                    , param: new
                    {
                        customerID
                    }).ToList());
            }

            if (getRealTime)
            {
                var realTimeCommissions = ExigoDAL.GetCustomerRealTimeCommissions(new GetCustomerRealTimeCommissionsRequest
                {
                    CustomerID = customerID,
                    GetPeriodVolumes = false
                });

                if (realTimeCommissions.Count() > 0)
                {
                    commissions.AddRange(realTimeCommissions);
                }
            }


            if (commissions == null) return null;

            return commissions;
        }


        public static HistoricalCommission GetCustomerHistoricalCommission(int customerID, int commissionRunID, bool includeVolumes = true)
        {
            // Get the commission record
            var commission = new HistoricalCommission();
            using (var context = ExigoDAL.Sql())
            {
                commission = context.Query<HistoricalCommission, Rank, Period, HistoricalCommission>(@"
                                SELECT c.CommissionRunID
                                  ,c.CustomerID
                                  ,c.CurrencyCode
                                  ,c.Earnings
                                  ,c.PreviousBalance
                                  ,c.BalanceForward
                                  ,c.Fee
                                  ,c.Total
                                  ,cr.CommissionRunDescription
                                  ,cr.PeriodTypeID
                                  ,cr.RunDate
                                  ,cr.CommissionRunStatusID
                                  ,cr.HideFromWeb
                                  ,cr.PlanID
                                  ,RankID = pv.PaidRankID
                                  ,r.RankDescription
                                  ,cr.PeriodID
                                  ,p.PeriodDescription
	                              ,p.PeriodTypeID
                                  ,p.StartDate
                                  ,p.EndDate
                                  ,cr.AcceptedDate
                            FROM Commissions c
	                            LEFT JOIN CommissionRuns cr
		                            ON c.CommissionRunID = cr.CommissionRunID
	                            LEFT JOIN Periods p
		                            ON cr.periodid = p.periodid
									and cr.periodtypeid = p.periodtypeid
                                LEFT JOIN PeriodVolumes pv 
                                    ON pv.periodid = p.periodid
                                    and pv.periodtypeid = p.periodtypeid
                                    and pv.customerid = c.customerid
	                            LEFT JOIN Ranks r
		                            ON r.RankID = pv.PaidRankID
                            WHERE c.CustomerID = @CustomerID
                                AND c.CommissionRunID = @CommissionRunID
                            ORDER BY cr.CommissionRunID DESC
                    ", (hc, r, p) =>
                    {
                        hc.Period = p;
                        hc.PaidRank = r;
                        return hc;
                    }, splitOn: "RankID,PeriodID",
                    param: new
                    {
                        CustomerID = customerID,
                        CommissionRunID = commissionRunID
                    }).FirstOrDefault();
            }


            if (commission == null) return null;
            var result = commission;


            // Get the volumes
            if (includeVolumes)
            {
                result.Volumes = GetCustomerVolumes(new GetCustomerVolumesRequest
                {
                    CustomerID = customerID,
                    PeriodID = result.Period.PeriodID,
                    PeriodTypeID = result.Period.PeriodTypeID
                });
            }
            else
            {
                result.Volumes = new VolumeCollection();
            }
            return result;
        }
        public static IEnumerable<RealTimeCommission> GetCustomerRealTimeCommissions(GetCustomerRealTimeCommissionsRequest request)
        {
            var results = new List<RealTimeCommission>();


            // Get the commission record
            var realtimeresponse = ExigoDAL.WebService().GetRealTimeCommissions(new Common.Api.ExigoWebService.GetRealTimeCommissionsRequest
            {
                CustomerID = request.CustomerID
            });
            if (realtimeresponse.Commissions.Length == 0) return results;


            // Get the unique periods for each of the commission results
            if (request.GetPeriodVolumes)
            {
                var periods = new List<Period>();
                var periodRequests = new List<GetPeriodsRequest>();
                foreach (var commissionResponse in realtimeresponse.Commissions)
                {
                    var periodID = commissionResponse.PeriodID;
                    var periodTypeID = commissionResponse.PeriodType;

                    var req = periodRequests.Where(c => c.PeriodTypeID == periodTypeID).FirstOrDefault();
                    if (req == null)
                    {
                        periodRequests.Add(new GetPeriodsRequest()
                        {
                            PeriodTypeID = periodTypeID,
                            PeriodIDs = new List<int> { periodID }
                        });
                    }
                    else
                    {
                        var ids = req.PeriodIDs.ToList();
                        ids.Add(periodID);
                        req.PeriodIDs = ids.Distinct().ToList();
                    }
                }
                foreach (var req in periodRequests)
                {
                    var responses = GetPeriods(req);
                    foreach (var response in responses)
                    {
                        periods.Add(response);
                    }
                }


                // Get the volumes for each unique period
                var volumeCollections = new List<VolumeCollection>();
                foreach (var period in periods)
                {
                    volumeCollections.Add(GetCustomerVolumes(new GetCustomerVolumesRequest
                    {
                        CustomerID = request.CustomerID,
                        PeriodID = period.PeriodID,
                        PeriodTypeID = period.PeriodTypeID,
                        VolumesToFetch = request.VolumesToFetch
                    }));
                }

                // Process each commission response 
                try
                {
                    foreach (var commission in realtimeresponse.Commissions)
                    {
                        var typedCommission = (RealTimeCommission)commission;

                        typedCommission.Period = periods
                            .Where(c => c.PeriodTypeID == commission.PeriodType)
                            .Where(c => c.PeriodID == commission.PeriodID)
                            .FirstOrDefault();

                        typedCommission.Volumes = volumeCollections
                            .Where(c => c.Period.PeriodTypeID == typedCommission.Period.PeriodTypeID)
                            .Where(c => c.Period.PeriodID == typedCommission.Period.PeriodID)
                            .FirstOrDefault();

                        typedCommission.PaidRank = typedCommission.Volumes.PayableAsRank;

                        results.Add(typedCommission);
                    }

                    return results.OrderByDescending(c => c.Period.StartDate);
                }
                catch { return results; }
            }
            else
            {
                var periodInfo = ExigoDAL.GetPeriods(new GetPeriodsRequest
                {
                    PeriodIDs = realtimeresponse.Commissions.Select(p => p.PeriodID).ToList(),
                    PeriodTypeID = realtimeresponse.Commissions.FirstOrDefault().PeriodType
                });
                foreach (var commission in realtimeresponse.Commissions)
                {
                    var typedCommission = (RealTimeCommission)commission;
                    var period = periodInfo.FirstOrDefault(p => p.PeriodID == commission.PeriodID && p.PeriodTypeID == commission.PeriodType);

                    typedCommission.Period = new Period();

                    //typedCommission.Period.PeriodID = commission.PeriodID;
                    //typedCommission.Period.PeriodTypeID =commission.PeriodType;
                    //typedCommission.Period.PeriodDescription = commission.PeriodDescription;

                    if (period != null)
                    {
                        typedCommission.Period = period;
                    }

                    results.Add(typedCommission);
                }

                return results.OrderByDescending(c => c.Period.StartDate);
            }
        }

        public static int GetLastCommissionRunID(int periodTypeID)
        {
            using (var context = ExigoDAL.Sql())
            {
                return context.Query<int>(@"
                            Select Top 1 CommissionRunID From CommissionRuns
	                            Where PeriodTypeID = @periodTypeID
                            Order by PeriodID Desc
                    ", new 
                    {
                        periodTypeID
                    }).FirstOrDefault();
            }
        }

        public static CommissionCard GetDashboardCommission(int customerID, int periodTypeID)
        {
            var commissionRunID = GetLastCommissionRunID(periodTypeID);
            dynamic card;
            using (var context = ExigoDAL.Sql())
            {
                card = context.Query<CommissionCard>(@"
                            SELECT c.CommissionRunID
                                        ,c.CurrencyCode
                                        ,c.Total
                                        ,cr.CommissionRunDescription
                                FROM Commissions c
	                                LEFT JOIN CommissionRuns cr
		                                ON c.CommissionRunID = cr.CommissionRunID
                                WHERE c.CustomerID = customerID
                                    AND c.CommissionRunID = @commissionRunID
                                ORDER BY cr.CommissionRunID DESC
                    ", new
                {
                    customerID,
                    commissionRunID
                }).FirstOrDefault();
                if (card == null) card = new CommissionCard();
            }
            return card;
        }
    }
}
