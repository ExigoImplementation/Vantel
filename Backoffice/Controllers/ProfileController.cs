using Backoffice.Services;
using Backoffice.ViewModels;
using Common;
using Common.Services;
using Dapper;
using ExigoService;
using ExigoWeb.Kendo;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Backoffice.Controllers
{
    [RoutePrefix("profile")]
    [Route("{action=index}")]
    public class ProfileController : Controller
    {
        public ActionResult Index(string token)
        {
            var model = new ProfileViewModel();
            var id = Convert.ToInt32(Security.Decrypt(token, Identity.Current.CustomerID));

            if (id == 0 || (id != Identity.Current.CustomerID && !ExigoDAL.IsCustomerInlDownline(Identity.Current.CustomerID, id)))
            {
                id = Identity.Current.CustomerID;
            }

            model.Customer = Customers.GetCustomer(id);
            model.Volumes = ExigoDAL.GetCustomerVolumes(new GetCustomerVolumesRequest
            {
                CustomerID = id,
                PeriodTypeID = PeriodTypes.Default
            });

            if (model.Volumes == null) model.Volumes = new VolumeCollection();

            if (model.Customer.EnrollerID > 0)
            {
                model.Customer.Enroller = Customers.GetCustomer(Convert.ToInt32(model.Customer.EnrollerID));

                if (model.Customer.EnrollerID == model.Customer.SponsorID)
                {
                    model.Customer.Sponsor = model.Customer.Enroller;
                }
            }
            if (model.Customer.SponsorID > 0 && model.Customer.SponsorID != model.Customer.EnrollerID)
            {
                model.Customer.Sponsor = Customers.GetCustomer(Convert.ToInt32(model.Customer.SponsorID));
            }

            if (model.Customer.RankID == 0)
            {
                model.Customer.RankID = model.Volumes.PayableAsRank.RankID;
            }

            if (model.Customer.EnrollerID != Identity.Current.CustomerID && model.Customer.CustomerID != Identity.Current.CustomerID)
            {
                model.IsInEnrollerTree = ExigoDAL.IsCustomerInEnrollerDownline(Identity.Current.CustomerID, model.Customer.CustomerID);
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("Partials/_Profile", model);
            }
            else
            {
                return View(model);
            }
        }

        [Route("popoversummary/{id:int=0}")]
        public ActionResult PopoverSummary(int id)
        {
            var model = new ProfileViewModel();

            if (id == 0) id = Identity.Current.CustomerID;

            model.Customer = Customers.GetCustomer(id);

            if (model.Customer.RankID == 0)
            {
                var volumes = ExigoDAL.GetCustomerVolumes(new GetCustomerVolumesRequest
                {
                    CustomerID = id,
                    PeriodTypeID = PeriodTypes.Default
                });

                model.Customer.RankID = volumes.PayableAsRank.RankID;
            }

            if (Request.IsAjaxRequest()) return PartialView("Partials/_ProfilePopover", model);
            else return View(model);
        }

        [Route("activity/{id:int}")]
        public ActionResult Activity(int id)
        {
            var model = new List<CustomerWallItem>();

            model = ExigoDAL.GetCustomerRecentActivity(new GetCustomerRecentActivityRequest
            {
                CustomerID = id
            }).OrderByDescending(c => c.EntryDate).ToList();

            return PartialView("Partials/Activity", model);
        }

        [Route("rankadvancement/{id:int}")]
        public ActionResult RankAdvancement(int id)
        {

            var currentPeriod = ExigoDAL.GetCurrentPeriod(PeriodTypes.Default);

            int result;

            using (var context = ExigoDAL.Sql())
            {
                result = context.Query<int>(@"
                    SELECT 
                        ISNULL(pv.PaidRankID, 1)	
                    FROM
	                    PeriodVolumes pv		                            
                    WHERE pv.CustomerID = @customerid
	                    AND pv.PeriodTypeID = @periodtypeid
	                    AND pv.PeriodID = @periodid
                ", new
                {
                    customerid = id,
                    periodtypeid = currentPeriod.PeriodTypeID,
                    periodid = currentPeriod.PeriodID
                }).FirstOrDefault();
            }

            var paidRankID = 1;

            if (result > 0) paidRankID = result;

            var ranks = RankService.GetRanks().ToList();

            if (ranks.Last().RankID != paidRankID)
            {
                paidRankID = ranks.OrderBy(c => c.RankID).Where(c => c.RankID > paidRankID).FirstOrDefault().RankID;
            }

            var model = RankQualificationService.GetCustomerRankQualifications(new GetCustomerRankQualificationsRequest
            {
                CustomerID = id,
                PeriodTypeID = PeriodTypes.Default,
                RankID = paidRankID
            });

            return PartialView("Partials/RankAdvancement", model);
        }

        [Route("volumes/{id:int}")]
        public ActionResult VolumesList(int id, KendoGridRequest request = null)
        {
            if (Request.HttpMethod.ToUpper() == "GET") return PartialView("Partials/VolumesList");



            using (var context = new KendoGridDataContext(ExigoDAL.Sql()))
            {
                return context.Query(request, @"
                    SELECT  
                          p.PeriodID
                        , p.StartDate
                        , p.EndDate
                        , p.PeriodDescription
                        , r.RankDescription
                        , pv.PaidRankID
                        , pv.Volume1
                        , pv.Volume2
                        , pv.Volume3
                        , pv.Volume4
                        , pv.Volume5
                    FROM Customers c
                    INNER JOIN Periods p
                        ON p.EndDate > c.CreatedDate
                        AND p.PeriodTypeID = @periodtype
                        AND p.StartDate <= GETDATE()
                    INNER JOIN PeriodVolumes pv
                        ON pv.PeriodID = p.PeriodID
                        AND pv.PeriodTypeID = p.PeriodTypeID
                        AND pv.CustomerID = c.CustomerID
                    LEFT JOIN Ranks r 
                        ON r.RankID = pv.PaidRankID
                        WHERE c.CustomerID = @customerid
            ", new
                {
                    customerid = id,
                    periodtype = PeriodTypes.Default
                }).Tokenize("CustomerID");
            }
        }

        [Route("orders/{id:int}")]
        public ActionResult OrdersList(int id, KendoGridRequest request = null)
        {
            if (Request.HttpMethod.ToUpper() == "GET") return PartialView("Partials/OrdersList");


            // Establish the query
            using (var context = new KendoGridDataContext(ExigoDAL.Sql()))
            {
                return context.Query<OrdersViewModel>(request, @"
                        Select 
                              o.OrderDate
                            , CountryCode = o.Country
                            , o.CurrencyCode
                            , o.OrderID
                            , o.SubTotal
                            , o.BusinessVolumeTotal
                            , o.CommissionableVolumeTotal
                        FROM Orders o
                        WHERE o.CustomerID = @customerid
                            AND o.OrderStatusID >= @orderstatus", 
                        new
                        {
                            customerid = id,
                            orderstatus = OrderStatuses.Accepted
                        });
            }
        }

        [Route("autoorders/{id:int}")]
        public ActionResult AutoOrdersList(int id, KendoGridRequest request = null)
        {
            if (Request.HttpMethod.ToUpper() == "GET") return PartialView("Partials/AutoOrdersList");

            using (var context = new KendoGridDataContext(ExigoDAL.Sql()))
            {
                return context.Query<OrdersViewModel>(request, @"
                        Select ao.AutoOrderID
                            , CountryCode = ao.Country
                            , ao.CurrencyCode
                            , ao.LastRunDate
                            , ao.NextRunDate
                            , ao.SubTotal
                            , ao.BusinessVolumeTotal
                            , ao.CommissionableVolumeTotal                        
                        From AutoOrders ao                        
                        Where ao.CustomerID = @customerid
                            and ao.AutoOrderStatusID = @autoorderstatus
                    ", new
                {
                    customerid = id,
                    autoorderstatus = 0
                });

            }
        }

    }
}