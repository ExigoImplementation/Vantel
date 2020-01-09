using ReplicatedSite.ViewModels;
using Common;
using Common.Services;
using ExigoService;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ReplicatedSite.Controllers
{
    [Authorize]
    [RoutePrefix("{webalias}")]
    public class OrdersController : BaseController
    {
        public int RowCount = 10;

        [Route("orders")]
        public ActionResult OrderList(int page = 1, int count = 0)
        {
            var model = new GetCustomerOrdersResponse();

            try
            {
                model = ExigoDAL.GetCustomerOrders_SQL(new GetCustomerOrdersRequest
                {
                    CustomerID = Identity.Customer.CustomerID,
                    Page = page,
                    RowCount = RowCount,
                    LanguageID = CurrentLanguage.LanguageID,
                    IncludeOrderDetails = true,
                    TotalRowCount = count
                });

                // Check for brand new Orders in our NewOrders cookie
                #region New Order Logic
                // Create a cookie to store our newest Order ID to ensure it shows on the Order History page
                var orderIDCookie = Request.Cookies["NewOrder_{0}".FormatWith(Identity.Customer.CustomerID)];

                if (orderIDCookie != null && orderIDCookie.Value.CanBeParsedAs<int>())
                {
                    int newOrderID = Convert.ToInt32(orderIDCookie.Value);

                    if (model.Orders.Any(c => c.OrderID == newOrderID))
                    {
                        Response.Cookies["NewOrder_{0}".FormatWith(Identity.Customer.CustomerID)].Expires = DateTime.Now.AddDays(-1);
                    }
                    else
                    {
                        var newApiOrder = ExigoDAL.GetCustomerOrders(new GetCustomerOrdersRequest
                        {
                            CustomerID = Identity.Customer.CustomerID,
                            IncludeOrderDetails = true,
                            LanguageID = CurrentLanguage.LanguageID,
                            OrderID = newOrderID,
                            TotalRowCount = 1
                        }).FirstOrDefault();

                        if (newApiOrder != null)
                        {
                            model.Orders.Add(newApiOrder);
                            RowCount = RowCount + 1;
                        }
                    }
                }
                #endregion

                model.RowCount = RowCount;
                model.Page = page;

                if (Request.IsAjaxRequest())
                {
                    model.Page = page++;
                    var orderNodes = this.RenderPartialViewToString("Partials/_OrderListRows", model);
                    var pagination = this.RenderPartialViewToString("Partials/_OrderListPagination", model);

                    return new JsonNetResult(new
                    {
                        success = true,
                        orderNodes,
                        pagination
                    });
                }
            }
            catch (Exception ex)
            {
                if (Request.IsAjaxRequest())
                {
                    return new JsonNetResult(new
                    {
                        success = false,
                        message = ex.Message
                    });
                }
            }

            return View("OrderList", model);

        }

        [Route("orders/cancelled")]
        public ActionResult CancelledOrdersList(int page = 1, int count = 0)
        {
            var model = new GetCustomerOrdersResponse();

            try
            {
                model = ExigoDAL.GetCustomerOrders_SQL(new GetCustomerOrdersRequest
                {
                    CustomerID = Identity.Customer.CustomerID,
                    Page = page,
                    RowCount = RowCount,
                    LanguageID = CurrentLanguage.LanguageID,
                    IncludeOrderDetails = true,
                    OrderStatuses = new int[] {
                        OrderStatuses.Cancelled
                    },
                    TotalRowCount = count
                });

                model.RowCount = RowCount;
                model.Page = page;


                if (Request.IsAjaxRequest())
                {
                    model.Page = page++;
                    var orderNodes = this.RenderPartialViewToString("Partials/_OrderListRows", model);
                    var pagination = this.RenderPartialViewToString("Partials/_OrderListPagination", model);

                    return new JsonNetResult(new
                    {
                        success = true,
                        orderNodes,
                        pagination
                    });
                }
            }
            catch (Exception ex)
            {
                if (Request.IsAjaxRequest())
                {
                    return new JsonNetResult(new
                    {
                        success = false,
                        message = ex.Message
                    });
                }
            }

            return View("OrderList", model);
        }

        [Route("orders/open")]
        public ActionResult OpenOrdersList(int page = 1, int count = 0)
        {
            var model = new GetCustomerOrdersResponse();

            try
            {
                model = ExigoDAL.GetCustomerOrders_SQL(new GetCustomerOrdersRequest
                {
                    CustomerID = Identity.Customer.CustomerID,
                    Page = page,
                    RowCount = RowCount,
                    LanguageID = CurrentLanguage.LanguageID,
                    IncludeOrderDetails = true,
                    OrderStatuses = new int[] {
                        OrderStatuses.Incomplete,
                        OrderStatuses.Pending,
                        OrderStatuses.CCDeclined,
                        OrderStatuses.ACHDeclined,
                        OrderStatuses.CCPending,
                        OrderStatuses.ACHPending,
                        OrderStatuses.PendingInventory,
                        OrderStatuses.Accepted
                    },
                    TotalRowCount = count
                });

                model.RowCount = RowCount;
                model.Page = page;


                if (Request.IsAjaxRequest())
                {
                    model.Page = page++;
                    var orderNodes = this.RenderPartialViewToString("Partials/_OrderListRows", model);
                    var pagination = this.RenderPartialViewToString("Partials/_OrderListPagination", model);

                    return new JsonNetResult(new
                    {
                        success = true,
                        orderNodes,
                        pagination
                    });
                }
            }
            catch (Exception ex)
            {
                if (Request.IsAjaxRequest())
                {
                    return new JsonNetResult(new
                    {
                        success = false,
                        message = ex.Message
                    });
                }
            }
            return View("OrderList", model);
        }

        [Route("orders/shipped")]
        public ActionResult ShippedOrdersList(int page = 1, int count = 0)
        {
            var model = new GetCustomerOrdersResponse();

            try
            {
                model = ExigoDAL.GetCustomerOrders_SQL(new GetCustomerOrdersRequest
                {
                    CustomerID = Identity.Customer.CustomerID,
                    Page = page,
                    RowCount = RowCount,
                    LanguageID = CurrentLanguage.LanguageID,
                    IncludeOrderDetails = true,
                    OrderStatuses = new int[] { OrderStatuses.Shipped },
                    TotalRowCount = count
                });

                model.RowCount = RowCount;
                model.Page = page;

                if (Request.IsAjaxRequest())
                {
                    model.Page = page++;
                    var orderNodes = this.RenderPartialViewToString("Partials/_OrderListRows", model);
                    var pagination = this.RenderPartialViewToString("Partials/_OrderListPagination", model);

                    return new JsonNetResult(new
                    {
                        success = true,
                        orderNodes,
                        pagination
                    });
                }
            }
            catch (Exception ex)
            {
                if (Request.IsAjaxRequest())
                {
                    return new JsonNetResult(new
                    {
                        success = false,
                        message = ex.Message
                    });
                }
            }
            return View("OrderList", model);
        }

        [Route("orders/declined")]
        public ActionResult DeclinedOrdersList(int page = 1, int count = 0)
        {
            var model = new GetCustomerOrdersResponse();

            try
            {
                model = ExigoDAL.GetCustomerOrders_SQL(new GetCustomerOrdersRequest
                {
                    CustomerID = Identity.Customer.CustomerID,
                    Page = page,
                    RowCount = RowCount,
                    LanguageID = CurrentLanguage.LanguageID,
                    IncludeOrderDetails = true,
                    OrderStatuses = new int[]
                    {
                        OrderStatuses.Incomplete,
                        OrderStatuses.CCDeclined,
                        OrderStatuses.ACHDeclined
                    },
                    TotalRowCount = count
                });

                model.RowCount = RowCount;
                model.Page = page;

                if (Request.IsAjaxRequest())
                {
                    model.Page = page++;
                    var orderNodes = this.RenderPartialViewToString("Partials/_OrderListRows", model);
                    var pagination = this.RenderPartialViewToString("Partials/_OrderListPagination", model);

                    return new JsonNetResult(new
                    {
                        success = true,
                        orderNodes,
                        pagination
                    });
                }
            }
            catch (Exception ex)
            {
                if (Request.IsAjaxRequest())
                {
                    return new JsonNetResult(new
                    {
                        success = false,
                        message = ex.Message
                    });
                }
            }

            return View("OrderList", model);
        }

        [Route("orders/returns")]
        public ActionResult ReturnedOrdersList(int page = 1, int count = 0)
        {
            var model = new GetCustomerOrdersResponse();

            try
            {
                model = ExigoDAL.GetCustomerOrders_SQL(new GetCustomerOrdersRequest
                {
                    CustomerID = Identity.Customer.CustomerID,
                    Page = page,
                    RowCount = RowCount,
                    LanguageID = CurrentLanguage.LanguageID,
                    IncludeOrderDetails = true,
                    OrderTypes = new int[]
                    {
                        OrderTypes.ReturnOrder
                    },
                    TotalRowCount = count
                });

                model.RowCount = RowCount;
                model.Page = page;


                if (Request.IsAjaxRequest())
                {
                    model.Page = page++;
                    var orderNodes = this.RenderPartialViewToString("Partials/_OrderListRows", model);
                    var pagination = this.RenderPartialViewToString("Partials/_OrderListPagination", model);

                    return new JsonNetResult(new
                    {
                        success = true,
                        orderNodes,
                        pagination
                    });
                }
            }
            catch (Exception ex)
            {
                if (Request.IsAjaxRequest())
                {
                    return new JsonNetResult(new
                    {
                        success = false,
                        message = ex.Message
                    });
                }
            }

            return View("OrderList", model);
        }

        [Route("orders/search/{id:int}")]
        public ActionResult SearchOrdersList(int id)
        {
            ViewBag.IsSearch = true;
            var model = new GetCustomerOrdersResponse();

            model = ExigoDAL.GetCustomerOrders_SQL(new GetCustomerOrdersRequest
            {
                CustomerID = Identity.Customer.CustomerID,
                Page = 1,
                RowCount = RowCount,
                LanguageID = CurrentLanguage.LanguageID,
                IncludeOrderDetails = true,
                OrderID = id,
                TotalRowCount = 1
            });

            model.RowCount = RowCount;
            model.Page = 1;
            
            model.OrderCount = model.Orders.Count();

            return View("OrderList", model);
        }

        [Route("order/cancel")]
        public ActionResult CancelOrder(string token)
        {
            var orderID = Convert.ToInt32(Security.Decrypt(token, Identity.Customer.CustomerID));

            ExigoDAL.CancelOrder(orderID);

            return Redirect(Request.UrlReferrer.ToString());
        }

        [Route("order")]
        public ActionResult OrderDetail(string token)
        {
            var orderID = Convert.ToInt32(Security.Decrypt(token, Identity.Customer.CustomerID));
            var model = new GetCustomerOrdersResponse();
            ViewBag.IsSearch = true;

            model = ExigoDAL.GetCustomerOrders_SQL(new GetCustomerOrdersRequest
            {
                CustomerID = Identity.Customer.CustomerID,
                Page = 1,
                RowCount = RowCount,
                LanguageID = CurrentLanguage.LanguageID,
                IncludeOrderDetails = true,
                OrderID = orderID,
                TotalRowCount = 1
            });

            model.RowCount = RowCount;
            model.Page = 1;
            
            model.OrderCount = 1;


            return View("OrderList", model);
        }
        
        [Route("invoice")]
        public ActionResult OrderInvoice(string token)
        {
            var orderID = Convert.ToInt32(Security.Decrypt(token, Identity.Customer.CustomerID));
            
            var model = ExigoDAL.GetCustomerOrders_SQL(new GetCustomerOrdersRequest
            {
                CustomerID = Identity.Customer.CustomerID,
                OrderID = orderID,
                IncludeOrderDetails = true,
                LanguageID = CurrentLanguage.LanguageID,
                IncludePayments = true
            }).Orders.FirstOrDefault();

            return View("OrderInvoice", model);
        }
    }
}