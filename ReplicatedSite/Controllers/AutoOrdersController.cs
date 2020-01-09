using System;
using ExigoService;
using System.Linq;
using System.Web.Mvc;
using Common;
using Common.Api.ExigoWebService;
using ReplicatedSite.ViewModels;
using System.Collections.Generic;

namespace ReplicatedSite.Controllers
{
    [Authorize]
    [RoutePrefix("{webalias}/auto-order")]
    public class AutoOrdersController : BaseController
    {
        [Route("AutoOrders")]
        public ActionResult Index()
        {
            return RedirectToAction("AutoOrderList");
        }

        [Route("about-auto-order")]
        public ActionResult NoActiveAutoOrdersFound()
        {
            return View();
        }
        
        [Route("auto-order-list")]
        public ActionResult AutoOrderList()
        {
            var viewModel = new AutoOrderListViewModel();

            viewModel.AutoOrders = ExigoDAL.GetCustomerAutoOrders(Identity.Customer.CustomerID, includePaymentMethods: false);

            if (!viewModel.AutoOrders.Any())
            {
                return RedirectToAction("NoActiveAutoOrdersFound");
            }

            return View(viewModel);
        }
        
        [Route("manage-auto-order/{id:int=0}")]
        public ActionResult ManageAutoOrder(int id)
        {
            var viewModel     = new ManageAutoOrderViewModel();
            var customerID    = Identity.Customer.CustomerID;
            var customer      = Customers.GetCustomer(customerID);
            var autoOrder     = ExigoDAL.GetCustomerAutoOrders(customerID, id).FirstOrDefault();
            var market        = GlobalSettings.Markets.AvailableMarkets.Where(c => c.Countries.Contains(Identity.Customer.Country)).FirstOrDefault();
            if (market == null) market = GlobalSettings.Markets.AvailableMarkets.FirstOrDefault(v => v.IsDefault);
            var configuration = market.GetConfiguration().AutoOrders;
            var isExistingAutoOrder = id != 0;

            if (isExistingAutoOrder)
            {
                viewModel.AutoOrder = autoOrder;
                viewModel.AutoOrder.StartDate = ExigoDAL.GetNextAvailableAutoOrderStartDate(viewModel.AutoOrder.NextRunDate ?? DateTime.Now);

                // Fill in any blanks in the recipient
                viewModel.AutoOrder.ShippingAddress.FirstName  = GlobalUtilities.Coalesce(viewModel.AutoOrder.ShippingAddress.FirstName, customer.FirstName);
                viewModel.AutoOrder.ShippingAddress.MiddleName = GlobalUtilities.Coalesce(viewModel.AutoOrder.ShippingAddress.MiddleName, customer.MiddleName);
                viewModel.AutoOrder.ShippingAddress.LastName   = GlobalUtilities.Coalesce(viewModel.AutoOrder.ShippingAddress.LastName, customer.LastName);
                viewModel.AutoOrder.ShippingAddress.Company    = GlobalUtilities.Coalesce(viewModel.AutoOrder.ShippingAddress.Company, customer.Company);
                viewModel.AutoOrder.ShippingAddress.Email      = GlobalUtilities.Coalesce(viewModel.AutoOrder.ShippingAddress.Email, customer.Email);
                viewModel.AutoOrder.ShippingAddress.Phone      = GlobalUtilities.Coalesce(viewModel.AutoOrder.ShippingAddress.Phone, customer.PrimaryPhone, customer.SecondaryPhone, customer.MobilePhone);
            }
            else
            {
                var customerShippingAddress = customer.ShippingAddresses.Where(a => a.IsComplete && a.Country == Identity.Customer.Country).FirstOrDefault();

                viewModel.AutoOrder = new AutoOrder()
                {
                    FrequencyTypeID        = FrequencyTypes.Monthly,
                    CurrencyCode           = configuration.CurrencyCode,
                    AutoOrderPaymentTypeID = AutoOrderPaymentTypes.PrimaryCreditCardOnFile,
                    StartDate              = ExigoDAL.GetNextAvailableAutoOrderStartDate(DateTime.Now),
                    ShippingAddress        = (customerShippingAddress != null) ? customerShippingAddress : new ShippingAddress() { Country = Identity.Customer.Country },
                    ShipMethodID           = configuration.DefaultShipMethodID
                };

                viewModel.AutoOrder.ShippingAddress.Phone = GlobalUtilities.Coalesce(viewModel.AutoOrder.ShippingAddress.Phone, customer.PrimaryPhone, customer.SecondaryPhone, customer.MobilePhone);
                viewModel.UsePointAccount = false;
            }

            // Get Available Ship Methods, if we are managing an existing auto order
            if (isExistingAutoOrder)
            {
                var calculateOrderResponse = ExigoDAL.CalculateOrder(new OrderCalculationRequest
                {
                    Address           = viewModel.AutoOrder.ShippingAddress,
                    ShipMethodID      = viewModel.AutoOrder.ShipMethodID,
                    ReturnShipMethods = true,
                    Configuration     = Identity.Customer.Market.Configuration.AutoOrders,
                    CustomerID        = Identity.Customer.CustomerID,
                    Items             = viewModel.AutoOrder.Details.Select(i => new ShoppingCartItem { ItemCode = i.ItemCode, Quantity = i.Quantity })
                });
                viewModel.AvailableShipMethods = calculateOrderResponse.ShipMethods.ToList();
            }
            else
            {
                // Give the View a default ship method for display only
                viewModel.AvailableShipMethods = new List<ShipMethod> { new ShipMethod { Price = 0, Selected = true, ShipMethodDescription = "---", ShipMethodID = 0 } };
            }

            InflateManageAutoOrderViewModel(customerID, market, configuration, ref viewModel);

            return View(viewModel);
        }

        public JsonNetResult CalculateAutoOrder(ManageAutoOrderViewModel model)
        {
            try
            {
                var calculateOrderResponse = ExigoDAL.CalculateOrder(new OrderCalculationRequest
                {
                    Address = model.AutoOrder.ShippingAddress,
                    ShipMethodID = model.AutoOrder.ShipMethodID,
                    ReturnShipMethods = true,
                    Configuration = Identity.Customer.Market.Configuration.AutoOrders,
                    CustomerID = Identity.Customer.CustomerID,
                    Items = model.AutoOrder.Details.Select(i => new ShoppingCartItem { ItemCode = i.ItemCode, Quantity = i.Quantity })
                });

                return new JsonNetResult(new
                {
                    success = true,
                    shipmethods = calculateOrderResponse.ShipMethods.ToList()
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

        [Route("manage-auto-order/{id:int=0}"), HttpPost]
        public ActionResult ManageAutoOrder(int id, ManageAutoOrderViewModel viewModel)
        {
            var customerID = Identity.Customer.CustomerID;
            var apiRequests = new List<ApiRequest>();
            var customer = Customers.GetCustomer(customerID);
            var market = GlobalSettings.Markets.AvailableMarkets.Where(c => c.Countries.Contains(Identity.Customer.Country)).FirstOrDefault();
            if (market == null) market = GlobalSettings.Markets.AvailableMarkets.FirstOrDefault(v => v.IsDefault);
            var configuration = market.GetConfiguration().AutoOrders;
            var warehouseID = configuration.WarehouseID;
            var isExistingAutoOrder = id != 0;
            var paymentMethods = ExigoDAL.GetCustomerPaymentMethods(new GetCustomerPaymentMethodsRequest() { CustomerID = Identity.Customer.CustomerID, ExcludeIncompleteMethods = true });



            // Remove all items that have no quantity.
            viewModel.AutoOrder.Details = viewModel.AutoOrder.Details.Where(d => d.Quantity > 0).ToList();
            if (!viewModel.AutoOrder.Details.Any())
            {
                ModelState.AddModelError("Result", "Please select at least one product for your Auto Order.");
            }



            if (ModelState.Keys.Contains("Result"))
            {
                InflateManageAutoOrderViewModel(customerID, market, configuration, ref viewModel);

                return View(viewModel);
            }

            // Save New Credit Card
            var isUsingNewCard = viewModel.AutoOrder.AutoOrderPaymentTypeID == 0;
            var hasPrimaryCard = paymentMethods.Where(v => v.IsComplete).Count() > 0;
            if (isUsingNewCard)
            {
                var saveCCRequest = new SetAccountCreditCardTokenRequest(viewModel.NewCreditCard);

                // If there is one or more available payment type, save the card in the secondary card slot
                if (hasPrimaryCard)
                {
                    saveCCRequest.CreditCardAccountType = AccountCreditCardType.Secondary;
                    viewModel.AutoOrder.AutoOrderPaymentTypeID = AutoOrderPaymentTypes.SecondaryCreditCardOnFile;
                }
                else
                {
                    viewModel.AutoOrder.AutoOrderPaymentTypeID = AutoOrderPaymentTypes.PrimaryCreditCardOnFile;
                }
                saveCCRequest.CustomerID = customerID;
                apiRequests.Add(saveCCRequest);
            }


            // Prepare the auto order
            var autoOrder = viewModel.AutoOrder;
            var createAutoOrderRequest = new CreateAutoOrderRequest(autoOrder)
            {
                PriceType = configuration.PriceTypeID,
                WarehouseID = warehouseID,
                Notes = !string.IsNullOrEmpty(autoOrder.Notes)
                                ? autoOrder.Notes
                                : string.Format("Created with the API Auto-Delivery manager at \"{0}\" on {1:u} at IP {2} using {3} {4} ({5}).",
                                    Request.Url.AbsoluteUri,
                                    DateTime.Now.ToUniversalTime(),
                                    GlobalUtilities.GetClientIP(),
                                    HttpContext.Request.Browser.Browser,
                                    HttpContext.Request.Browser.Version,
                                    HttpContext.Request.Browser.Platform),
                CustomerID = customerID
            };

            apiRequests.Add(createAutoOrderRequest);

            try
            {
                // Process the transaction
                var transaction = new TransactionalRequest { TransactionRequests = apiRequests.ToArray() };
                var response = ExigoDAL.WebService().ProcessTransaction(transaction);

                return RedirectToAction("AutoOrderList", new { success = "1" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Result", "We were unable to save your Auto-Delivery: " + ex.Message);

                InflateManageAutoOrderViewModel(customerID, market, configuration, ref viewModel);

                return View(viewModel);
            }
        }

        [NonAction]
        private void InflateManageAutoOrderViewModel(int customerID, IMarket market, IOrderConfiguration configuration, ref ManageAutoOrderViewModel viewModel)
        {
            viewModel.AvailableStartDates = Enumerable.Range(1, 27).Select(day =>
            {
                var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day).BeginningOfDay();
                if (date < DateTime.Now.BeginningOfDay()) date = date.AddMonths(1);

                return date;

            }).OrderBy(d => d.Day).ToList();

            viewModel.AvailableProducts = ExigoDAL.GetItems(new ExigoService.GetItemsRequest()
            {
                Configuration = configuration,
                LanguageID = CurrentLanguage.LanguageID,
                CategoryID = configuration.CategoryID,
                PriceTypeID = PriceTypes.Wholesale,
                IncludeAllChildCategories = true,
                IncludeDynamicKitChildren = false
            }).ToList();

            viewModel.AvailablePaymentMethods = ExigoDAL.GetCustomerPaymentMethods(customerID)
                .Where(p => p.IsValid)
                .Where(p => p is IAutoOrderPaymentMethod)
                .ToList();

            if (viewModel.AvailablePaymentMethods != null && viewModel.AvailablePaymentMethods.Count() == 1)
            {
                viewModel.NewCreditCard.Type = CreditCardType.Secondary;
            }
        }

        public ActionResult DeleteAutoOrder(int id)
        {
            try
            {
                var customerID = Identity.Customer.CustomerID;

                ExigoDAL.DeleteCustomerAutoOrder(customerID, id);

                return RedirectToAction("AutoOrderList", new { success = "1" });
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return RedirectToAction("AutoOrderList", new { success = "0" });
            }

        }
    }
}

