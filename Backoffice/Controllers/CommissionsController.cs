using Backoffice.Services;
using Backoffice.ViewModels;
using Common;
using Common.Services;
using ExigoService;
using ExigoWeb.Kendo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Backoffice.Controllers
{
    public class CommissionsController : Controller
    {
        #region Commissions
        [Route("~/commissions/{runid:int=0}/{periodid:int=0}")]
        public ActionResult CommissionDetail(int runid, int periodid)
        {
            var model = new CommissionDetailViewModel();

            // View Requests
            if (!Request.IsAjaxRequest())
            {
                model.CommissionPeriods = ExigoDAL.GetCommissionPeriodList(Identity.Current.CustomerID, true);
                return View("CommissionDetail", model);
            }

            // AJAX requests
            else
            {
                // Real-time commissions
                if (runid == 0)
                {
                    model.Commissions = ExigoDAL.GetCustomerRealTimeCommissions(new GetCustomerRealTimeCommissionsRequest
                    {
                        CustomerID = Identity.Current.CustomerID,
                        GetPeriodVolumes = true
                    });

                    // Check Period ID
                    if (periodid > 0)
                    {
                        model.PeriodID = periodid;
                        model.Commissions = model.Commissions.Where(c => c.Period.PeriodID == periodid);
                    }

                    return PartialView("_RealTimeCommissionDetail", model);
                }

                // Historical Commissions
                else
                {
                    model.Commissions = new List<ICommission>() { ExigoDAL.GetCustomerHistoricalCommission(Identity.Current.CustomerID, runid) };
                    return PartialView("_HistoricalCommissionDetail", model);
                }
            }
        }

        [HttpPost]
        public JsonNetResult GetRealTimeBonusDetails(KendoGridRequest request)
        {
            try
            {
                // Have to include this as part of the url via a query string, to avoid hacking the kendo request object that is Http POSTed - Mike M.
                var periodID = Request.QueryString["periodID"] != null ? Convert.ToInt32(Request.QueryString["periodID"]) : 0;
                var results = new List<RealTimeCommissionBonusDetail>();


                // Get the commission record(s)
                var context = ExigoDAL.WebService();
                var realtimeresponse = context.GetRealTimeCommissions(new Common.Api.ExigoWebService.GetRealTimeCommissionsRequest
                {
                    CustomerID = Identity.Current.CustomerID
                });
                if (realtimeresponse.Commissions.Length == 0) return new JsonNetResult();


                // Get the bonuses (I know, this is brutal, but our UI depends on it)
                var commissionsList = periodID > 0 ? realtimeresponse.Commissions.Where(c => c.PeriodID == periodID).ToList() : realtimeresponse.Commissions.ToList();
                foreach (var commission in commissionsList)
                {
                    var bonuses = commission.Bonuses
                        .Select(c => new CommissionBonus()
                        {
                            BonusID = c.BonusID,
                            BonusDescription = c.Description
                        }).Distinct();

                    foreach (var bonusID in commission.Bonuses.Select(c => c.BonusID).Distinct())
                    {
                        var bonus = bonuses.Where(c => c.BonusID == bonusID).FirstOrDefault();

                        // Get the details for this bonus
                        var details = context.GetRealTimeCommissionDetail(new Common.Api.ExigoWebService.GetRealTimeCommissionDetailRequest
                        {
                            CustomerID = commission.CustomerID,
                            PeriodType = commission.PeriodType,
                            PeriodID = commission.PeriodID,
                            BonusID = bonusID
                        }).CommissionDetails;


                        // Get the period details for this period
                        var period = ExigoDAL.GetPeriods(new GetPeriodsRequest
                        {
                            PeriodTypeID = commission.PeriodType,
                            PeriodIDs = new List<int> { commission.PeriodID }
                        }).FirstOrDefault();


                        // Format and save each bonus
                        foreach (var detail in details)
                        {
                            var typedDetail = (CommissionBonusDetail)detail;
                            var result = GlobalUtilities.Extend(typedDetail, new RealTimeCommissionBonusDetail());

                            result.BonusID = bonus.BonusID;
                            result.BonusDescription = bonus.BonusDescription;
                            result.PeriodDescription = period.PeriodDescription;
                            results.Add(result);
                        }
                    }
                }


                // Filtering
                foreach (var filter in request.FilterObjectWrapper.FilterObjects)
                {
                    results = results.AsQueryable().Where(filter.Field1, filter.Operator1, filter.Value1).ToList();
                }

                // Sorting
                foreach (var sort in request.SortObjects)
                {
                    results = results.AsQueryable().OrderBy(sort.Field, sort.Direction).ToList();
                }


                // Return the data
                return new JsonNetResult(new
                {
                    data = results
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

        [HttpPost]
        public JsonNetResult GetHistoricalBonusDetails(KendoGridRequest request, int runid)
        {
            try
            {
                // Fetch the data
                using (var context = new KendoGridDataContext(ExigoDAL.Sql()))
                {
                    var data = context.Query(request, @"
                    SELECT 
	                    cd.BonusID
                        ,b.BonusDescription
                        ,cd.FromCustomerID
                        ,FromCustomerName = c.FirstName + ' ' + c.LastName
                        ,cd.Level
                        ,cd.PaidLevel
                        ,cd.SourceAmount
                        ,cd.Percentage
                        ,cd.CommissionAmount 
	
                    FROM
	                    CommissionDetails cd	
	                    INNER JOIN Customers c 
		                    ON c.CustomerID = cd.FromCustomerID
	                    INNER JOIN Bonuses b 
		                    ON b.BonusID = cd.BonusID

                    WHERE
	                    cd.CustomerID = @customerid
	                    AND cd.CommissionRunID = @runid
                ", new
                    {
                        customerid = Identity.Current.CustomerID,
                        runid = runid
                    });


                    // Return the data
                    return new JsonNetResult(new
                    {
                        data = data.Data
                    });
                }
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
        #endregion

        #region Rank
        [Route("~/rank")]
        public ActionResult Rank()
        {
            var model = new RankViewModel();

            //Pull in all the ranks
            model.Ranks = RankService.GetRanks().OrderBy(c => c.RankID);

            var currentperiod = ExigoDAL.GetCurrentPeriod(PeriodTypes.Default);

            //Get the current rank from the current paid as rank from the current commission period
            model.CurrentRank = ExigoDAL.GetCustomerVolumes(new GetCustomerVolumesRequest
            {
               CustomerID   = Identity.Current.CustomerID,
               PeriodTypeID = currentperiod.PeriodTypeID,
               PeriodID     = currentperiod.PeriodID
            }).PayableAsRank;

            //Get the next rank so we can jump to the next qualification
            model.NextRank = model.Ranks.OrderBy(c => c.RankID).Where(c => c.RankID > model.CurrentRank.RankID).FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        public JsonNetResult GetRankQualifications(int rankID = 0)
        {
            try
            {
                var response = RankQualificationService.GetCustomerRankQualifications(new GetCustomerRankQualificationsRequest
                {
                    CustomerID = Identity.Current.CustomerID,
                    PeriodTypeID = PeriodTypes.Default,
                    RankID = rankID
                });

                var html = (!response.IsUnavailable) ? this.RenderPartialViewToString("_RankQualificationDetail", response) : "";

                return new JsonNetResult(new
                {
                    result = response,
                    html = html
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

        [HttpPost]
        public JsonNetResult GetRankQualificationsDashboard(int rankID = 0)
        {
            try
            {
                var response = RankQualificationService.GetCustomerRankQualifications(new GetCustomerRankQualificationsRequest
                {
                    CustomerID = Identity.Current.CustomerID,
                    PeriodTypeID = PeriodTypes.Default,
                    RankID = rankID
                });

                var html = (!response.IsUnavailable) ? this.RenderPartialViewToString("_RankQualificationDetailDashboard", response) : "";

                return new JsonNetResult(new
                {
                    result = response,
                    html = html
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
        #endregion

        #region Volumes
        [Route("~/volumes")]
        public ActionResult VolumeList(KendoGridRequest request = null)
        {
            if (Request.HttpMethod.ToUpper() == "GET") return View();

            // Fetch the data
            using (var context = new KendoGridDataContext(ExigoDAL.Sql()))
            {
                var results = context.Query(request, @"
                    SELECT 
	                    pv.PeriodID
                        ,p.StartDate
                        ,p.EndDate
                        ,p.PeriodDescription
                        ,pv.PaidRankID
                        ,PaidRankDescription = ''
                        ,pv.Volume1
                        ,pv.Volume2
                        ,pv.Volume3 
	
                    FROM
	                    PeriodVolumes pv	
	                    INNER JOIN Periods p
		                    ON p.PeriodID = pv.PeriodID
	                    INNER JOIN Ranks r 
		                    ON r.RankID = pv.PaidRankID

                    WHERE
	                    pv.CustomerID = @customerid
	                    AND pv.PeriodTypeID = @periodtypeid
	                    AND p.StartDate <= @startdate
                ", new
                 {
                     customerid = Identity.Current.CustomerID,
                     periodtypeid = PeriodTypes.Default,
                     startdate = DateTime.Now.ToCST()
                 });

                // get the translated paid rank description
                foreach (var item in results.Data)
                {
                    item.PaidRankDescription = CommonResources.Ranks(item.PaidRankID, CommonResourceFormat.Default);
                }

                return results;
            }
        }
        #endregion
    }
}