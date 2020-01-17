using Common;
using Common.Api.ExigoWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Serilog;

namespace ExigoService
{
    public static partial class ExigoDAL
    {
        /// <summary>
        /// Web Service Get Orders call
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static List<Order> GetCustomerOrders(GetCustomerOrdersRequest request)
        {
            var orders = new List<Order>();
            try
            {
                if (request.CustomerID == 0 && request.PartyID == 0 && request.OrderID == 0)
                {
                    throw new ArgumentException("CustomerID is required or Order ID is required.");
                }

                var orequest = new GetOrdersRequest();

                // Apply the request variables
                if (request.CustomerID > 0)
                {
                    orequest.CustomerID = request.CustomerID;
                }
                orequest.BatchSize = 10000;

                if (request.OrderID != null)
                {
                    orequest.OrderID = request.OrderID;
                }
                if (request.StartDate != null)
                {
                    orequest.OrderDateStart = request.StartDate;
                }
                if (request.PartyID > 0)
                {
                    orequest.PartyID = request.PartyID;
                }

                var response = ExigoDAL.WebService().GetOrders(orequest);
                var responseOrders = new List<Order>();

                response.Orders.ToList().ForEach(o =>
                {
                    var _order = (Order)o;
                    var includeInOrderList = false;

                    // Apply additional filters that are not available in our Get Orders Request
                    if (request.OrderStatuses.Length > 0 || request.OrderTypes.Length > 0)
                    {
                        if (request.OrderStatuses.Length > 0 && request.OrderStatuses.Contains(_order.OrderStatusID))
                        {
                            includeInOrderList = true;
                        }
                        if (request.OrderTypes.Length > 0 && request.OrderTypes.Contains(_order.OrderTypeID))
                        {
                            includeInOrderList = true;
                        }
                    }
                    else
                    {
                        includeInOrderList = true;
                    }

                    if (includeInOrderList)
                    {
                        if (request.IncludeOrderDetails)
                        {
                            // Go ahead and get our list of Item Images and details that do not come back with the API call
                            var _orders = response.Orders.ToList();
                            var itemCodes = new List<string>();
                            foreach (var order in response.Orders)
                            {
                                foreach (var detail in order.Details)
                                {
                                    itemCodes.Add(detail.ItemCode);
                                }
                            }

                            // Temp List of Item that contains only the fields we need to add to the Order Detail items
                            var additionalItemDetails = new List<Item>();
                            using (var context = ExigoDAL.Sql())
                            {
                                additionalItemDetails = context.Query<Item>(@"
                                                    select distinct ItemCode,
                                                        SmallImageUrl = @imageUrlPrefix + SmallImageName,
                                                        IsVirtual
                                                    from Items 
                                                    where ItemCode in @itemCodes
                                                    ", new
                                {
                                    itemCodes,
                                    imageUrlPrefix = GlobalUtilities.GetProductImagePath()
                                }).ToList();
                            }

                            var _details = new List<OrderDetail>();

                            o.Details.ToList().ForEach(d =>
                            {
                                var detail = (OrderDetail)d;
                                var additionalDetail = additionalItemDetails.FirstOrDefault(i => i.ItemCode == d.ItemCode);

                                if (additionalDetail != null)
                                {
                                    detail.ImageUrl = additionalDetail.SmallImageUrl;
                                    detail.IsVirtual = additionalDetail.IsVirtual;
                                }

                                _details.Add(detail);
                            });

                            _order.Details = _details;
                        }

                        if (request.IncludePayments)
                        {
                            var _payments = new List<Payment>();

                            o.Payments.ToList().ForEach(p =>
                            {
                                var payment = new Payment();

                                payment.Amount = p.Amount;
                                payment.AuthorizationCode = p.AuthorizationCode;
                                payment.BillingName = p.BillingName;
                                payment.CreditCardNumber = p.CreditCardNumberDisplay;
                                payment.CreditCardTypeID = p.CreditCardType;
                                payment.CurrencyCode = p.CurrencyCode;
                                payment.CustomerID = p.CustomerID;
                                payment.Memo = p.Memo;
                                payment.OrderID = Convert.ToInt32(p.OrderID);
                                payment.PaymentDate = p.PaymentDate;
                                payment.PaymentID = p.PaymentID;
                                payment.PaymentTypeID = (int)p.PaymentType;

                                _payments.Add(payment);
                            });

                            _order.Payments = _payments;
                        }

                        responseOrders.Add(_order);
                    }
                });

                orders = responseOrders
                    .OrderBy(c => c.OrderDate)
                    .Skip(request.Skip)
                    .Take(request.Take)
                    .ToList();
            }
            catch
            {
                return null;
            }

            return orders;
        }

        /// <summary>
        /// SQL Get Orders call. Uses Web Service as a fail safe.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static GetCustomerOrdersResponse GetCustomerOrders_SQL(GetCustomerOrdersRequest request)
        {
            var orders = new List<Order>();
            var model = new GetCustomerOrdersResponse();

            try
            {
                if (request.OrderID != null && request.OrderID <= 0 && request.CustomerID <= 0)
                {
                    throw new ArgumentOutOfRangeException("Customer ID and/or Order ID is required.");
                }

                var where = "";

                if (request.OrderID != null)
                {
                    where = "OrderID = @orderid AND CustomerID = @customerid";
                }
                else
                {
                    where = "CustomerID = @customerid";
                }

                if (request.OrderTypes.Count() != 0)
                {
                    where += " AND OrderTypeID in @ordertypes";
                }
                if (request.OrderStatuses.Count() != 0)
                {
                    where += " AND OrderStatusID in @orderstatuses";
                }

                // We only populate the Count if we do not already have it provided to us
                var getCount = request.TotalRowCount == 0;
                var countQuery = "";

                if (getCount)
                {
                    countQuery = String.Format(@"
                    SELECT Count(OrderID)
                    FROM [Orders]

                    Where {0} 
                ", where);
                }
                else
                {
                    model.OrderCount = request.TotalRowCount;
                }

                var query = String.Format(@"
                    ;SELECT 
                        [OrderID]
                        ,[CustomerID] 
                        ,[OrderStatusID]
                        ,[OrderTypeID]
                        ,[OrderDate]
                        ,[CurrencyCode]
                        ,[WarehouseID]
                        ,[ShipMethodID]
                        ,[PriceTypeID]
                        ,[Notes]
                        ,[Total]
                        ,[SubTotal]
                        ,[TaxTotal]
                        ,[ShippingTotal]
                        ,[DiscountTotal]
                        ,[DiscountPercent]
                        ,[WeightTotal]
                        ,[BVTotal] = [BusinessVolumeTotal]
                        ,[CVTotal] = [CommissionableVolumeTotal]
                        ,[TrackingNumber1]
                        ,[TrackingNumber2]
                        ,[TrackingNumber3]
                        ,[TrackingNumber4]
                        ,[TrackingNumber5]
                        ,[Other1Total]
                        ,[Other2Total]
                        ,[Other3Total]
                        ,[Other4Total]
                        ,[Other5Total]
                        ,[Other6Total]
                        ,[Other7Total]
                        ,[Other8Total]
                        ,[Other9Total]
                        ,[Other10Total]
                        ,[ShippingTax]
                        ,[OrderTax]
                        ,[FedTaxTotal]
                        ,[StateTaxTotal]
                        ,[FedShippingTax]
                        ,[StateShippingTax]
                        ,[CityShippingTax]
                        ,[CityLocalShippingTax]
                        ,[CountyShippingTax]
                        ,[CountyLocalShippingTax]
                        ,[Other11]
                        ,[Other12]
                        ,[Other13]
                        ,[Other14]
                        ,[Other15]
                        ,[Other16]
                        ,[Other17]
                        ,[Other18]
                        ,[Other19]
                        ,[Other20]
                        ,[IsCommissionable]
                        ,[AutoOrderID]
                        ,[ReturnOrderID]
                        ,[ReplacementOrderID]
                        ,[ParentOrderID]
                        ,[BatchID]
                        ,[DeclineCount]
                        ,[TransferToCustomerID]
                        ,[PartyID]
                        ,[WebCarrierID1]
                        ,[WebCarrierID2]
                        ,[WebCarrierID3]
                        ,[WebCarrierID4]
                        ,[WebCarrierID5]
                        ,[ShippedDate]
                        ,[CreatedDate]
                        ,[LockedDate]
                        ,[ModifiedDate]
                        ,[CreatedBy]
                        ,[ModifiedBy]
                        ,[SuppressPackSlipPrice]
                        ,[ReturnCategoryID]
                        ,[ReplacementCategoryID]
                        ,[FirstName]
                        ,[LastName]
                        ,[Company]
                        ,[Address1]
                        ,[Address2]
                        ,[City]
                        ,[State]
                        ,[Zip]
                        ,[Country]
                        ,[County]
                        ,[Email]
                        ,[Phone]
                    FROM [Orders]

                    Where {0} 
                        ORDER BY OrderDate DESC

	                    OFFSET     @skip ROWS       
	                    FETCH NEXT @take ROWS ONLY
                ", where);

                using (var context = ExigoDAL.Sql())
                {
                    context.Open();

                    if (getCount)
                    {
                        var count = context.Query<int>(countQuery,
                                    new
                                    {
                                        orderid = request.OrderID,
                                        orderstatuses = request.OrderStatuses,
                                        ordertypes = request.OrderTypes,
                                        customerid = request.CustomerID
                                    }).FirstOrDefault();

                        model.OrderCount = count;
                    }

                    orders = context.Query<Order, ShippingAddress, Order>(
                        query
                        , (o, sa) =>
                        {
                            o.Recipient = sa;
                            return o;
                        }

                    , param: new
                    {
                        orderid = request.OrderID,
                        orderstatuses = request.OrderStatuses,
                        ordertypes = request.OrderTypes,
                        customerid = request.CustomerID,
                        skip = request.Skip,
                        take = request.Take
                    }
                    , splitOn: "FirstName"
                    ).ToList();

                }
                if (request.IncludeOrderDetails)
                {
                    var orderDetails = new List<OrderDetail>();
                    using (var context = Sql())
                    {
                        List<int> orderIDs = orders.Select(order => order.OrderID).ToList();
                        orderDetails = context.Query<OrderDetail>(@"
                        SELECT 
                            [OrderID]
                            ,[OrderLine]
                            ,[ItemID]
                            ,[ItemCode]
                            ,[ItemDescription]
                            ,[Quantity]
                            ,[PriceEach]
                            ,[PriceTotal]
                            ,[Tax]
                            ,[WeightEach]
                            ,[Weight]
                            ,[BusinessVolumeEach]
                            ,[BusinessVolume]
                            ,[CommissionableVolumeEach]
                            ,[CommissionableVolume]
                            ,[ParentItemID]
                            ,[Taxable]
                            ,[FedTax]
                            ,[StateTax]
                            ,[CityTax]
                            ,[CityLocalTax]
                            ,[CountyTax]
                            ,[CountyLocalTax]
                            ,[ManualTax]
                            ,[IsStateTaxOverride]
                            ,[Reference1]
                        FROM [OrderDetails]

                        WHERE OrderID in @orderids;

                    ", new
                        {
                            orderids = orderIDs
                        }).ToList();
                    }

                    // Temp List of Item that contains only the fields we need to add to the Order Detail items
                    var imageUrls = new List<Item>();
                    List<string> detailItemCodes = new List<string>();
                    foreach (var order in orders)
                    {
                        foreach (var detail in orderDetails)
                        {
                            detailItemCodes.Add(detail.ItemCode);
                        }
                    }
                    foreach (var detail in orderDetails)
                    {
                        detailItemCodes.Add(detail.ItemCode);
                    }

                    using (var context = ExigoDAL.Sql())
                    {
                        imageUrls = context.Query<Item>(@"
                        SELECT DISTINCT 
                            [ItemCode]
                            ,[SmallImageUrl] = [SmallImageName]
                            ,[IsVirtual]
                        FROM [Items] 
                        WHERE ItemCode IN @itemcodes
                        ", new
                        {
                            itemcodes = detailItemCodes.Distinct().ToArray()
                        }).ToList();
                    }

                    foreach (var order in orders)
                    {
                        var details = orderDetails.Where(o => o.OrderID == order.OrderID);

                        foreach (var detail in orderDetails)
                        {
                            var imageUrl = imageUrls.Where(i => i.ItemCode == detail.ItemCode).FirstOrDefault();
                            if (imageUrl != null)
                            {
                                detail.ImageUrl = imageUrl.SmallImageUrl;
                            }

                        }
                        order.Details = details.ToList();
                    }

                }

                if (request.IncludePayments)
                {
                    var orderPayments = new List<Payment>();
                    List<int> orderIDs = orders.Select(order => order.OrderID).ToList();
                    using (var context = ExigoDAL.Sql())
                    {
                        orderPayments = context.Query<Payment>(@"
                        SELECT
                            [PaymentID]  
                            ,[CustomerID]
                            ,[OrderID]
                            ,[PaymentTypeID]
                            ,[PaymentDate]
                            ,[Amount]
                            ,[CurrencyCode]
                            ,[WarehouseID]
                            ,[BillingName]
                            ,[CreditCardTypeID]
                            ,[CreditCardNumber]
                            ,[AuthorizationCode]
                            ,[Memo]
                            ,[BillingAddress1]
                            ,[BillingAddress2]
                            ,[BillingCity]
                            ,[BillingState]
                            ,[BillingZipAddress]
                            ,[BillingCountry]
                        FROM [Payments] 
                        WHERE OrderID in @orderids;
                        
                        ", new
                        {
                            orderids = orderIDs
                        }).ToList();
                    }


                    foreach (var order in orders)
                    {
                        var payment = orderPayments.Where(o => o.OrderID == order.OrderID);
                        order.Payments = payment;
                    }

                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error Getting Customer Orders: {Message}", ex.Message);
                orders = GetCustomerOrders(request);
            }
            orders = GetTrackingUrl(orders);
            model.Orders = orders;

            return model;
        }

        public static void CancelOrder(int orderID)
        {
            ExigoDAL.WebService().ChangeOrderStatus(new ChangeOrderStatusRequest
            {
                OrderID = orderID,
                OrderStatus = OrderStatusType.Canceled
            });
        }

        public static OrderCalculationResponse CalculateOrder(OrderCalculationRequest request)
        {
            var result = new OrderCalculationResponse();
            if (request.Items.Count() == 0 || request.Address == null || request.Address.Country.IsNullOrEmpty() || request.Address.State.IsNullOrEmpty()) return result;
            if (request.ShipMethodID == 0) request.ShipMethodID = request.Configuration.DefaultShipMethodID;


            var apirequest = new CalculateOrderRequest();

            apirequest.WarehouseID = request.Configuration.WarehouseID;
            apirequest.CurrencyCode = request.Configuration.CurrencyCode;
            apirequest.PriceType = request.Configuration.PriceTypeID;
            apirequest.ShipMethodID = request.ShipMethodID;
            apirequest.ReturnShipMethods = request.ReturnShipMethods;
            apirequest.City = request.Address.City;
            apirequest.State = request.Address.State;
            apirequest.Zip = request.Address.Zip;
            apirequest.Country = request.Address.Country;
            apirequest.Details = request.Items.Select(c => new OrderDetailRequest(c)).ToArray();

            var apiresponse = ExigoDAL.WebService().CalculateOrder(apirequest);

            result.Subtotal = apiresponse.SubTotal;
            result.Shipping = apiresponse.ShippingTotal;
            result.Tax = apiresponse.TaxTotal;
            result.Discount = apiresponse.DiscountTotal;
            result.Total = apiresponse.Total;
            result.Details = apiresponse.Details.Select(c => new OrderDetail
            {
                ItemCode = c.ItemCode,
                PriceEach = c.PriceEach,
                ItemDescription = c.Description,
                Quantity = c.Quantity,
                ParentItemCode = c.ParentItemCode
            }).ToList();


            // Assemble the ship methods, if requested
            if (request.ReturnShipMethods)
            {
                var shipMethods = new List<ShipMethod>();
                if (apiresponse.ShipMethods != null && apiresponse.ShipMethods.Length > 0)
                {
                    var webShipMethods = new List<ShipMethod>();

                    using (var sql = ExigoDAL.Sql())
                    {
                        webShipMethods = sql.Query<ShipMethod>(@"
                                        SELECT 
                                            [ShipMethodID]
                                            ,[ShipMethodDescription]
                                            ,[WarehouseID]
                                            ,[ShipCarrierID]
                                            ,[DisplayOnWeb]
                                        FROM [dbo].[ShipMethods]
                                        WHERE DisplayOnWeb = 1 
                                            AND WarehouseID = @wid",
                                            new
                                            {
                                                wid = request.Configuration.WarehouseID
                                            }).ToList();
                    }

                    if (webShipMethods.Count() > 0)
                    {
                        var webShipMethodIds = webShipMethods.Select(s => s.ShipMethodID);
                        foreach (var shipMethod in apiresponse.ShipMethods.Where(x => webShipMethodIds.Contains(x.ShipMethodID)))
                        {
                            shipMethods.Add((ShipMethod)shipMethod);
                        }

                        if (shipMethods.Any())
                        {
                            // Ensure that at least one ship method is selected
                            var shipMethodID = (request.ShipMethodID != 0) ? request.ShipMethodID : request.Configuration.DefaultShipMethodID;
                            if (shipMethods.Any(c => c.ShipMethodID == shipMethodID))
                            {
                                shipMethods.First(c => c.ShipMethodID == shipMethodID).Selected = true;
                            }
                            else
                            {
                                shipMethods.First().Selected = true;
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Error: You need at least one Ship Method set up as DisplayOnWeb.");
                    }
                }
                result.ShipMethods = shipMethods.AsEnumerable();
            }


            return result;
        }

        public static List<Order> GetTrackingUrl(List<Order> orders)
        {
            var shipCarriers = new List<ShipCarriers>();
            var shipMethods = orders.Select(v => v.ShipMethodID).Distinct().ToList();
            var trackingUrls = new List<string>();
            using (var context = ExigoDAL.Sql())
            {
                shipCarriers = context.Query<ShipCarriers>(@"
                    SELECT 
                          s.ShipMethodID
                        , sc.TrackingUrl 
                    FROM ShipMethods AS s
                        INNER JOIN ShipCarriers AS sc
                        ON sc.ShipCarrierID = s.ShipCarrierID
                    WHERE s.ShipMethodID IN @shipmethodids
                        
                        ", new
                {
                    shipmethodids = shipMethods
                }).ToList();
            }

            foreach (var order in orders)
            {
                order.TrackingURL = shipCarriers.Where(v => order.ShipMethodID == v.ShipMethodID).FirstOrDefault().TrackingUrl;
            }

            return orders;
        }

        public class ShipCarriers
        {
            public string TrackingUrl;
            public int ShipMethodID;
        }
    }
}