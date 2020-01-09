using Backoffice.ViewModels;
using Common;
using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Dapper;
using Caching;
using Backoffice.Services;

namespace Backoffice.Controllers
{
    [RoutePrefix("")]
    public class DashboardController : Controller
    {
        // Widget cache timeout in minutes
        private const int WidgetCacheTimeout = 5;

        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        #region Ajax Calls
        public JsonNetResult GetCurrentCommissions()
        {
            var customerID = Identity.Current.CustomerID;

            try
            {
                var currentCommissions = Cache.Get("Dashboard_CurrentCommissionsCard_{0}".FormatWith(customerID),
                    TimeSpan.FromMinutes(WidgetCacheTimeout),
                    () =>
                        ExigoDAL.GetCustomerRealTimeCommissions(new GetCustomerRealTimeCommissionsRequest
                        {
                            CustomerID = customerID,
                            GetPeriodVolumes = false
                        }).ToList()
                    );


                var html = this.RenderPartialViewToString("Cards/CurrentCommissions", currentCommissions);

                return new JsonNetResult(new
                {
                    success = true,
                    html
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

        public JsonNetResult GetLastAcceptedCommissions()
        {
            var customerID = Identity.Current.CustomerID;

            try
            {
                var lastAcceptedCommissions = Cache.Get("Dashboard_LastAcceptedCommissionsCard_{0}".FormatWith(customerID),
                    TimeSpan.FromMinutes(WidgetCacheTimeout),
                    () =>
                        ExigoDAL.GetDashboardCommission(customerID, PeriodTypes.Default)
                    );


                var html = this.RenderPartialViewToString("Cards/LastAccepted", lastAcceptedCommissions);

                return new JsonNetResult(new
                {
                    success = true,
                    html
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

        public ActionResult GetRankAdvancementCard(int rankid)
        {
            var ranks = RankService.GetRanks().ToList();
            var customerID = Identity.Current.CustomerID;

            GetCustomerRankQualificationsResponse model = null;

            // Check to ensure that the rank we are checking is not the last rank.
            // If so, return a null. Our view will take care of nulls specially.
            if (ranks.Last().RankID != rankid)
            {
                var nextRankID = ranks.OrderBy(c => c.RankID).Where(c => c.RankID > rankid).FirstOrDefault().RankID;
                model = RankQualificationService.GetCustomerRankQualifications(new GetCustomerRankQualificationsRequest
                        {
                            CustomerID = Identity.Current.CustomerID,
                            PeriodTypeID = PeriodTypes.Default,
                            RankID = nextRankID
                        });
            }

            return PartialView("Cards/RankAdvancement", model);
        }

        // This method also get the "currentRankID" that is passed in the call to GetRankAdvancementCard
        public JsonNetResult GetVolumes()
        {
            var customerID = Identity.Current.CustomerID;

            try
            {
                var volumes = Cache.Get("Dashboard_VolumesCard_{0}".FormatWith(customerID),
                    TimeSpan.FromMinutes(WidgetCacheTimeout),
                    () =>
                        ExigoDAL.GetCustomerVolumes(new GetCustomerVolumesRequest
                        {
                            CustomerID = customerID,
                            PeriodTypeID = PeriodTypes.Default
                        })
                    );

                // Get the Current Rank that is used for the Rank Advancement call
                var currentRankID = (volumes != null) ? volumes.PayableAsRank.RankID : 0;

                var html = this.RenderPartialViewToString("Cards/Volumes", volumes);

                return new JsonNetResult(new
                {
                    success = true,
                    currentRankID,
                    html
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

        public JsonNetResult GetRecentOrders()
        {
            var customerID = Identity.Current.CustomerID;

            try
            {
                var orders = Cache.Get("Dashboard_OrdersCard_{0}".FormatWith(customerID),
                    TimeSpan.FromMinutes(WidgetCacheTimeout),
                    () =>
                           ExigoDAL.GetCustomerOrders_SQL(new GetCustomerOrdersRequest
                           {
                               CustomerID = customerID,
                               IncludeOrderDetails = false,
                               OrderStatuses = new int[] {
                            OrderStatuses.Accepted,
                            OrderStatuses.Printed,
                            OrderStatuses.Shipped
                        },
                               Page = 1,
                               RowCount = 4
                           }).Orders
                    );



                var html = this.RenderPartialViewToString("Cards/RecentOrders", orders);

                return new JsonNetResult(new
                {
                    success = true,
                    html
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

        public JsonNetResult GetCompanyNews()
        {
            var customerID = Identity.Current.CustomerID;

            try
            {
                var newsRequestResponse = Cache.Get("Dashboard__CompanyNews",
                    TimeSpan.FromMinutes(WidgetCacheTimeout),
                    () =>
                           ExigoDAL.GetCompanyNewsSQL(new GetCompanyNewsRequest
                           {
                               NewsDepartments = new List<int> { GlobalSettings.Backoffices.Reports.Dashboard.CompanyNews.Department }.ToArray()
                           })
                    );

                var companyNews = newsRequestResponse.GroupBy(v => v.NewsID).Select(o => o.First()).ToList();


                var html = this.RenderPartialViewToString("Cards/CompanyNews", companyNews);

                return new JsonNetResult(new
                {
                    success = true,
                    html
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

        public JsonNetResult GetNewestDistributors()
        {
            var customerID = Identity.Current.CustomerID;

            try
            {
                var newestDistributors = Cache.Get("Dashboard_NewestDistributors_{0}".FormatWith(customerID),
                    TimeSpan.FromMinutes(WidgetCacheTimeout),
                    () =>
                           ExigoDAL.GetNewestDistributors(new GetNewestDistributorsRequest
                           {
                               CustomerID = customerID,
                               RowCount = GlobalSettings.Backoffices.Reports.Dashboard.NewestDistributors.MaxResultSize,
                               CustomerStatuses = GlobalSettings.Backoffices.Reports.Dashboard.NewestDistributors.CustomerStatuses,
                               CustomerTypes = GlobalSettings.Backoffices.Reports.Dashboard.NewTeamMemebers.CustomerTypes,
                               Days = GlobalSettings.Backoffices.Reports.Dashboard.NewestDistributors.Days,
                           })
                    );



                var html = this.RenderPartialViewToString("Cards/NewestDistributors", newestDistributors);

                return new JsonNetResult(new
                {
                    success = true,
                    html
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

        public JsonNetResult GetRecentActivity()
        {
            var customerID = Identity.Current.CustomerID;

            try
            {
                var recentActivities = Cache.Get("Dashboard_RecentActivity_{0}".FormatWith(customerID),
                    TimeSpan.FromMinutes(WidgetCacheTimeout),
                    () =>
                        ExigoDAL.GetCustomerRecentActivity(new GetCustomerRecentActivityRequest
                        {
                            CustomerID = customerID,
                            Page = 1,
                            RowCount = 50
                        }).Tokenize()
                    );


                var html = this.RenderPartialViewToString("Cards/RecentActivity", recentActivities);

                return new JsonNetResult(new
                {
                    success = true,
                    html
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
    }
}