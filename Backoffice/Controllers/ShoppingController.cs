using Backoffice.Factories;
using Backoffice.Models;
using Backoffice.Providers;
using Backoffice.ViewModels;
using Common;
using Common.Api.ExigoWebService;
using Common.Providers;
using Common.Services;
using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Backoffice.Controllers
{
    [RoutePrefix("shopping")]
    public class ShoppingController : BaseController
    {
        #region Properties
        public string ShoppingCartName = Common.GlobalSettings.Globalization.CookieKey + "BackofficeShopping";

        public ShoppingCartItemsPropertyBag ShoppingCart
        {
            get
            {
                if (_shoppingCart == null)
                {
                    _shoppingCart = ExigoDAL.PropertyBags.Get<ShoppingCartItemsPropertyBag>(ShoppingCartName + "Cart");
                }
                return _shoppingCart;
            }
        }
        private ShoppingCartItemsPropertyBag _shoppingCart;

        public ShoppingCartCheckoutPropertyBag PropertyBag
        {
            get
            {
                if (_propertyBag == null)
                {
                    _propertyBag = ExigoDAL.PropertyBags.Get<ShoppingCartCheckoutPropertyBag>(ShoppingCartName + "PropertyBag");
                }
                return _propertyBag;
            }
        }
        private ShoppingCartCheckoutPropertyBag _propertyBag;

        public IOrderConfiguration OrderConfiguration
        {
            get
            {
                if (_orderConfiguration == null)
                {
                    _orderConfiguration = CurrentMarket.Configuration.Orders;
                }
                return _orderConfiguration;
            }
        }
        public IOrderConfiguration _orderConfiguration { get; set; }

        public IOrderConfiguration AutoOrderConfiguration
        {
            get
            {
                if (_autoOrderConfiguration == null)
                {
                    _autoOrderConfiguration = CurrentMarket.Configuration.AutoOrders;
                }
                return _autoOrderConfiguration;
            }
        }
        public IOrderConfiguration _autoOrderConfiguration { get; set; }

        public ILogicProvider LogicProvider
        {
            get
            {
                if (_logicProvider == null)
                {
                    _logicProvider = new ShoppingCartLogicProvider(this, ShoppingCart, PropertyBag);
                }
                return _logicProvider;
            }
        }
        private ILogicProvider _logicProvider;
        #endregion

        #region Items/Cart
        [Route("products")]
        public ActionResult ItemList()
        {
            var model = ShoppingViewModelFactory.Create<ItemListViewModel>(PropertyBag);

            model.Categories = ExigoDAL.GetChildWebCategories(new GetChildWebCategoriesRequest
            {
                TopCategoryID = OrderConfiguration.CategoryID,
                GetAllChildCategories = true,
                NestedType = NestedType.UniLevel
            });

            #region Parties
            // Party Info Logic
            if (PropertyBag.HasValidParty())
            {
                var partyService = new PartyService(Identity.Current.CustomerID);

                var customerID = Identity.Current.CustomerID;
                var isHostess = Identity.Current.IsHostess();

                var getPartyRequest = new ExigoService.GetPartiesRequest
                {
                    PartyID = PropertyBag.PartyID,
                    IncludeHostessDetails = true,
                    PartyStatusID = (int)PartyStatusTypes.Open
                };


                if (PropertyBag.IsHostessOrder())
                {
                    getPartyRequest.IncludeHostessRewards = true;

                    if (Identity.Current.IsHostess())
                    {
                        getPartyRequest.HostessID = customerID;
                    }
                    else
                    {
                        getPartyRequest.CustomerID = customerID;
                    }
                }

                ViewBag.ActiveParty = partyService.GetParties(getPartyRequest).FirstOrDefault();

                if (PropertyBag.IsGuestOrder())
                {
                    ViewBag.CurrentGuest = partyService.GetPartyGuests(PropertyBag.PartyID, PropertyBag.GuestID).FirstOrDefault();
                }
            }
            #endregion

            return View(model);
        }

        [Route("product/{itemcode}")]
        public ActionResult ItemDetail(string itemcode)
        {
            var model = ShoppingViewModelFactory.Create<ItemDetailViewModel>(PropertyBag);

            int priceTypeID = OrderConfiguration.PriceTypeID;

            #region Parties
            // Party Info Logic
            if (PropertyBag.HasValidParty())
            {
                var partyService = new PartyService(Identity.Current.CustomerID);

                var customerID = Identity.Current.CustomerID;
                var isHostess = Identity.Current.IsHostess();

                var getPartyRequest = new ExigoService.GetPartiesRequest
                {
                    PartyID = PropertyBag.PartyID,
                    IncludeHostessDetails = true,
                    PartyStatusID = (int)PartyStatusTypes.Open
                };


                if (PropertyBag.IsHostessOrder())
                {
                    getPartyRequest.IncludeHostessRewards = true;

                    if (Identity.Current.IsHostess())
                    {
                        getPartyRequest.HostessID = customerID;
                    }
                    else
                    {
                        getPartyRequest.CustomerID = customerID;
                    }
                }

                ViewBag.ActiveParty = partyService.GetParties(getPartyRequest).FirstOrDefault();

                if (PropertyBag.IsGuestOrder())
                {
                    priceTypeID = PriceTypes.Retail;
                    ViewBag.CurrentGuest = partyService.GetPartyGuests(PropertyBag.PartyID, PropertyBag.GuestID).FirstOrDefault();
                }
            }
            #endregion

            model.Item = ExigoDAL.GetItems(new ExigoService.GetItemsRequest
            {
                Configuration = OrderConfiguration,
                ItemCodes = new List<string> { itemcode },
                LanguageID = CurrentLanguage.LanguageID,
                PriceTypeID = priceTypeID,
                IncludeShortDescriptions = true,
                IncludeLongDescriptions = true,
                IncludeAllChildCategories = true,
                IncludeGroupMembers = true,
                IncludeStaticKitChildren = true,
                IncludeDynamicKitChildren = true
            }).FirstOrDefault();

            if (model.Item != null)
            {
                model.Item.Quantity = 1;
            }


            return View(model);
        }

        [Route("cart")]
        public ActionResult Cart()
        {
            var model = ShoppingViewModelFactory.Create<CartViewModel>(PropertyBag);
            int priceTypeID = OrderConfiguration.PriceTypeID;

            #region Parties
            // Party Info Logic
            if (PropertyBag.HasValidParty())
            {
                var partyService = new PartyService(Identity.Current.CustomerID);

                var customerID = Identity.Current.CustomerID;
                var isHostess = Identity.Current.IsHostess();

                var getPartyRequest = new ExigoService.GetPartiesRequest
                {
                    PartyID = PropertyBag.PartyID,
                    IncludeHostessDetails = true,
                    PartyStatusID = (int)PartyStatusTypes.Open
                };


                if (PropertyBag.IsHostessOrder())
                {
                    getPartyRequest.IncludeHostessRewards = true;

                    if (Identity.Current.IsHostess())
                    {
                        getPartyRequest.HostessID = customerID;
                    }
                    else
                    {
                        getPartyRequest.CustomerID = customerID;
                    }
                }

                ViewBag.ActiveParty = partyService.GetParties(getPartyRequest).FirstOrDefault();

                if (PropertyBag.IsGuestOrder())
                {
                    priceTypeID = PriceTypes.Retail;
                    ViewBag.CurrentGuest = partyService.GetPartyGuests(PropertyBag.PartyID, PropertyBag.GuestID).FirstOrDefault();
                }
            }
            #endregion

            // Get the cart items
            var cartItems = ShoppingCart.Items.ToList();
            if (cartItems.Where(el => el.Type == ShoppingCartItemType.Order).Any())
            {
                model.OrderItems = ExigoDAL.GetItems(new ExigoService.GetItemsRequest
                {
                    Configuration = OrderConfiguration,
                    ShoppingCartItems = ShoppingCart.Items.GetItemsOfType(ShoppingCartItemType.Order),
                    IgnoreCategoryRestrictions = true,
                    LanguageID = this.CurrentLanguage.LanguageID
                }).ToList();
            }
            else
            {
                model.OrderItems = new List<Item>();
            }

            if (cartItems.Where(el => el.Type == ShoppingCartItemType.AutoOrder).Any())
            {
                model.AutoOrderItems = ExigoDAL.GetItems(new ExigoService.GetItemsRequest
                {
                    Configuration = AutoOrderConfiguration,
                    ShoppingCartItems = ShoppingCart.Items.GetItemsOfType(ShoppingCartItemType.AutoOrder),
                    IgnoreCategoryRestrictions = true,
                    LanguageID = this.CurrentLanguage.LanguageID
                }).ToList();
            }
            else
            {
                model.AutoOrderItems = new List<Item>();
            }
            return View(model);
        }
        #endregion

        #region Shipping
        [Route("checkout/shipping")]
        public ActionResult Shipping()
        {
            var model = ShoppingViewModelFactory.Create<ShippingAddressesViewModel>(PropertyBag);

            model.Addresses = Customers.GetCustomerAddresses(Identity.Current.CustomerID)
                .Where(c => c.IsComplete)
                .Select(c => c as ShippingAddress);


            #region Parties
            // Party Info Logic
            if (PropertyBag.HasValidParty())
            {
                var partyService = new PartyService(Identity.Current.CustomerID);

                var customerID = Identity.Current.CustomerID;
                var isHostess = Identity.Current.IsHostess();

                var getPartyRequest = new ExigoService.GetPartiesRequest
                {
                    PartyID = PropertyBag.PartyID,
                    IncludeHostessDetails = true,
                    PartyStatusID = (int)PartyStatusTypes.Open
                };


                if (PropertyBag.IsHostessOrder())
                {
                    getPartyRequest.IncludeHostessRewards = true;

                    if (Identity.Current.IsHostess())
                    {
                        getPartyRequest.HostessID = customerID;
                    }
                    else
                    {
                        getPartyRequest.CustomerID = customerID;
                    }
                }

                ViewBag.ActiveParty = partyService.GetParties(getPartyRequest).FirstOrDefault();

                if (PropertyBag.IsGuestOrder())
                {
                    ViewBag.CurrentGuest = partyService.GetPartyGuests(PropertyBag.PartyID, PropertyBag.GuestID).FirstOrDefault();
                }
            }
            #endregion

            return View(model);
        }

        [HttpPost]
        [Route("checkout/shipping")]
        public ActionResult Shipping(ShippingAddress address)
        {
            // Attempt to validate the user's entered address if US address
            address = GlobalUtilities.ValidateAddress(address) as ShippingAddress;

            // Save the address to the customer's account if applicable
            if (Request.IsAuthenticated && address.AddressType == AddressType.New)
            {
                ExigoDAL.SetCustomerAddressOnFile(Identity.Current.CustomerID, address as Address);
            }

            PropertyBag.ShippingAddress = address;
            ExigoDAL.PropertyBags.Update(PropertyBag);

            return LogicProvider.GetNextAction();

        }
        #endregion

        #region AutoOrders
        [Route("checkout/autoorder")]
        public ActionResult AutoOrder()
        {
            var model = ShoppingViewModelFactory.Create<AutoOrderSettingsViewModel>(PropertyBag);

            // Ensure we have a valid frequency type
            if (!GlobalSettings.AutoOrders.AvailableFrequencyTypes.Contains(PropertyBag.AutoOrderFrequencyType))
            {
                PropertyBag.AutoOrderFrequencyType = GlobalSettings.AutoOrders.AvailableFrequencyTypes.FirstOrDefault();
            }

            // Ensure we have a valid start date based on the frequency
            if (PropertyBag.AutoOrderStartDate == DateTime.MinValue.ToCST())
            {
                PropertyBag.AutoOrderStartDate = GlobalUtilities.GetAutoOrderStartDate(PropertyBag.AutoOrderFrequencyType);
            }


            // Set our model
            model.AutoOrderStartDate = PropertyBag.AutoOrderStartDate;
            model.AutoOrderFrequencyType = PropertyBag.AutoOrderFrequencyType;

            return View(model);
        }

        [HttpPost]
        [Route("checkout/autoorder")]
        public ActionResult AutoOrder(DateTime autoOrderStartDate, FrequencyType autoOrderFrequencyType)
        {
            PropertyBag.AutoOrderStartDate = autoOrderStartDate;
            PropertyBag.AutoOrderFrequencyType = autoOrderFrequencyType;
            ExigoDAL.PropertyBags.Update(PropertyBag);

            return LogicProvider.GetNextAction();
        }
        #endregion

        #region Payments
        [Route("checkout/payment")]
        public ActionResult Payment()
        {
            var model = ShoppingViewModelFactory.Create<PaymentMethodsViewModel>(PropertyBag);

            model.PaymentMethods = ExigoDAL.GetCustomerPaymentMethods(new GetCustomerPaymentMethodsRequest
            {
                CustomerID = Identity.Current.CustomerID,
                ExcludeIncompleteMethods = true,
                ExcludeInvalidMethods = true
            });

            model.Addresses = Customers.GetCustomerAddresses(Identity.Current.CustomerID)
                .Where(c => c.IsComplete)
                .Select(c => c as ShippingAddress);

            #region Parties
            // Party Info Logic
            if (PropertyBag.HasValidParty())
            {
                var partyService = new PartyService(Identity.Current.CustomerID);

                var customerID = Identity.Current.CustomerID;
                var isHostess = Identity.Current.IsHostess();

                var getPartyRequest = new ExigoService.GetPartiesRequest
                {
                    PartyID = PropertyBag.PartyID,
                    IncludeHostessDetails = true,
                    PartyStatusID = (int)PartyStatusTypes.Open
                };


                if (PropertyBag.IsHostessOrder())
                {
                    getPartyRequest.IncludeHostessRewards = true;

                    if (Identity.Current.IsHostess())
                    {
                        getPartyRequest.HostessID = customerID;
                    }
                    else
                    {
                        getPartyRequest.CustomerID = customerID;
                    }
                }

                ViewBag.ActiveParty = partyService.GetParties(getPartyRequest).FirstOrDefault();

                if (PropertyBag.IsGuestOrder())
                {
                    ViewBag.CurrentGuest = partyService.GetPartyGuests(PropertyBag.PartyID, PropertyBag.GuestID).FirstOrDefault();
                }
            }
            #endregion

            return View("Payment", model);
        }

        [HttpPost]
        public ActionResult UseCreditCardOnFile(CreditCardType type)
        {
            var paymentMethod = ExigoDAL.GetCustomerPaymentMethods(new GetCustomerPaymentMethodsRequest
            {
                CustomerID = Identity.Current.CustomerID,
                ExcludeIncompleteMethods = true,
                ExcludeInvalidMethods = true
            }).Where(c => c is CreditCard && ((CreditCard)c).Type == type).FirstOrDefault();

            return UsePaymentMethod(paymentMethod);
        }

        [HttpPost]
        public ActionResult UseBankAccountOnFile(ExigoService.BankAccountType type)
        {
            var paymentMethod = ExigoDAL.GetCustomerPaymentMethods(new GetCustomerPaymentMethodsRequest
            {
                CustomerID = Identity.Current.CustomerID,
                ExcludeIncompleteMethods = true,
                ExcludeInvalidMethods = true
            }).Where(c => c is BankAccount && ((BankAccount)c).Type == type).FirstOrDefault();

            return UsePaymentMethod(paymentMethod);
        }

        [HttpPost]
        public ActionResult UseCreditCard(CreditCard newCard, bool billingSameAsShipping = false)
        {
            try
            {
                if (billingSameAsShipping)
                {
                    var address = PropertyBag.ShippingAddress;

                    newCard.BillingAddress = new Address
                    {
                        Address1 = address.Address1,
                        Address2 = address.Address2,
                        City = address.City,
                        State = address.State,
                        Zip = address.Zip,
                        Country = address.Country
                    };
                }


                // Verify that the card is valid
                if (!newCard.IsValid)
                {

                    return new JsonNetResult(new
                    {
                        success = false,
                        message = "This card is invalid, please try again"
                    });
                }
                else
                {
                    // Save the credit card to the customer's account if applicable
                    #region Save Card Logic
                    //if (LogicProvider.IsAuthenticated())
                    //{
                    //    var paymentMethodsOnFile = Exigo.GetCustomerPaymentMethods(new GetCustomerPaymentMethodsRequest
                    //    {
                    //        CustomerID = Identity.Current.CustomerID,
                    //        ExcludeIncompleteMethods = true,
                    //        ExcludeInvalidMethods = true
                    //    }).Where(c => c is CreditCard).Select(c => c as CreditCard);

                    //    if (paymentMethodsOnFile.Where(c => c.Type == CreditCardType.Primary).FirstOrDefault() == null)
                    //    {
                    //        Exigo.SetCustomerCreditCard(Identity.Current.CustomerID, newCard, CreditCardType.Primary);
                    //    }
                    //    else if (paymentMethodsOnFile.Where(c => c.Type == CreditCardType.Secondary).FirstOrDefault() == null)
                    //    {
                    //        Exigo.SetCustomerCreditCard(Identity.Current.CustomerID, newCard, CreditCardType.Secondary);
                    //    }
                    //}
                    #endregion

                    return UsePaymentMethod(newCard);
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

        [HttpPost]
        public ActionResult UseBankAccount(BankAccount newBankAccount, bool billingSameAsShipping = false)
        {
            if (billingSameAsShipping)
            {
                var address = PropertyBag.ShippingAddress;

                newBankAccount.BillingAddress = new Address
                {
                    Address1 = address.Address1,
                    Address2 = address.Address2,
                    City = address.City,
                    State = address.State,
                    Zip = address.Zip,
                    Country = address.Country
                };
            }

            // Verify that the card is valid
            if (!newBankAccount.IsValid)
            {
                return new JsonNetResult(new
                {
                    success = false
                });
            }
            else
            {
                // Save the bank account to the customer's account if applicable                
                var paymentMethodsOnFile = ExigoDAL.GetCustomerPaymentMethods(new GetCustomerPaymentMethodsRequest
                {
                    CustomerID = Identity.Current.CustomerID,
                    ExcludeIncompleteMethods = true,
                    ExcludeInvalidMethods = true,
                }).Where(c => c is BankAccount).Select(c => c as BankAccount);

                if (paymentMethodsOnFile.FirstOrDefault() == null)
                {
                    ExigoDAL.SetCustomerBankAccount(Identity.Current.CustomerID, newBankAccount);
                }
            }

            return UsePaymentMethod(newBankAccount);
        }

        [HttpPost]
        public ActionResult UsePaymentMethod(IPaymentMethod paymentMethod)
        {
            try
            {
                PropertyBag.PaymentMethod = paymentMethod;
                ExigoDAL.PropertyBags.Update(PropertyBag);

                return new JsonNetResult(new
                {
                    success = true
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

        #region Review/Checkout
        [Route("checkout/review")]
        public ActionResult Review()
        {
            var logicResult = LogicProvider.CheckLogic();
            if (!logicResult.IsValid) return logicResult.NextAction;

            var model = ShoppingViewModelFactory.Create<OrderReviewViewModel>(PropertyBag);
            int priceTypeID = OrderConfiguration.PriceTypeID;

            #region Parties
            // Party Info Logic
            if (PropertyBag.HasValidParty())
            {
                var partyService = new PartyService(Identity.Current.CustomerID);

                var customerID = Identity.Current.CustomerID;
                var isHostess = Identity.Current.IsHostess();

                var getPartyRequest = new ExigoService.GetPartiesRequest
                {
                    PartyID = PropertyBag.PartyID,
                    IncludeHostessDetails = true,
                    PartyStatusID = (int)PartyStatusTypes.Open
                };


                if (PropertyBag.IsHostessOrder())
                {
                    getPartyRequest.IncludeHostessRewards = true;

                    if (Identity.Current.IsHostess())
                    {
                        getPartyRequest.HostessID = customerID;
                    }
                    else
                    {
                        getPartyRequest.CustomerID = customerID;
                    }
                }

                ViewBag.ActiveParty = partyService.GetParties(getPartyRequest).FirstOrDefault();

                if (PropertyBag.IsGuestOrder())
                {
                    priceTypeID = PriceTypes.Retail;
                    ViewBag.CurrentGuest = partyService.GetPartyGuests(PropertyBag.PartyID, PropertyBag.GuestID).FirstOrDefault();
                }
            }
            #endregion

            #region Order Totals
            var beginningShipMethodID = PropertyBag.ShipMethodID;

            // If this is the first time coming to the page, and the property bag's ship method has not been set, then set it to the default for the configuration
            if (PropertyBag.ShipMethodID == 0)
            {
                PropertyBag.ShipMethodID = OrderConfiguration.DefaultShipMethodID;
                beginningShipMethodID = PropertyBag.ShipMethodID;
                ExigoDAL.PropertyBags.Update(PropertyBag);
            }

            // Set the price type to Retail if a Guest Order, which occurs above
            OrderConfiguration.PriceTypeID = priceTypeID;

            // Get the cart items
            var cartItems = ShoppingCart.Items.ToList();
            model.Items = ExigoDAL.GetItems(cartItems, OrderConfiguration, CurrentLanguage.LanguageID).ToList();



            model.OrderTotals = ExigoDAL.CalculateOrder(new OrderCalculationRequest
            {
                Configuration = OrderConfiguration,
                Items = cartItems,
                Address = PropertyBag.ShippingAddress,
                ShipMethodID = PropertyBag.ShipMethodID,
                ReturnShipMethods = true
            });
            model.ShipMethods = model.OrderTotals.ShipMethods;

            // Set the default ship method
            if (model.ShipMethods.Count() > 0)
            {
                if (model.ShipMethods.Any(c => c.ShipMethodID == PropertyBag.ShipMethodID))
                {
                    // If the property bag ship method ID exists in the results from order calc, set the correct result as selected                
                    model.ShipMethods.First(c => c.ShipMethodID == PropertyBag.ShipMethodID).Selected = true;
                }
                else
                {
                    // If we don't have the ship method we're supposed to select, check the first one, save the selection and recalculate
                    model.ShipMethods.First().Selected = true;

                    // If for some reason the property bag is outdated and the ship method stored in it is not in the list, set the first result as selected and re-set the property bag's value
                    PropertyBag.ShipMethodID = model.ShipMethods.FirstOrDefault().ShipMethodID;
                    ExigoDAL.PropertyBags.Update(PropertyBag);
                }
            }

            // If the original property bag value has changed from the beginning of the call, re-calculate the values
            if (beginningShipMethodID != PropertyBag.ShipMethodID)
            {
                var newCalculationResult = ExigoDAL.CalculateOrder(new OrderCalculationRequest
                {
                    Configuration = OrderConfiguration,
                    Items = cartItems,
                    Address = PropertyBag.ShippingAddress,
                    ShipMethodID = PropertyBag.ShipMethodID,
                    ReturnShipMethods = false,
                    CustomerID = Identity.Current.CustomerID
                });

                model.OrderTotals = newCalculationResult;
            }
            #endregion

            return View(model);
        }

        [HttpPost]
        public ActionResult SubmitCheckout()
        {
            if (!PropertyBag.IsSubmitting)
            {
                PropertyBag.IsSubmitting = true;
                _propertyBag = ExigoDAL.PropertyBags.Update(PropertyBag);

                try
                {
                    var isGuestOrder = PropertyBag.IsGuestOrder();
                    // Price Type and Customer ID for Order Creation, if applicable
                    int priceTypeID = OrderConfiguration.PriceTypeID;
                    int customerID = Identity.Current.CustomerID;
                    Guest partyGuest = new Guest();
                    var partyService = new PartyService(Identity.Current.CustomerID);

                    // Start creating the API requests
                    var details = new List<ApiRequest>();


                    // If we are dealing with a Guest record, ensure we have a Customer ID, else we need to create a new Customer
                    if (isGuestOrder)
                    {
                        // Set the price type while here
                        priceTypeID = PriceTypes.Retail;

                        partyGuest = partyService.GetPartyGuests(PropertyBag.PartyID, PropertyBag.GuestID, true).FirstOrDefault();

                        if (partyGuest.CustomerID > 0)
                        {
                            customerID = partyGuest.CustomerID;
                        }
                        else
                        {
                            // Set CustomerID to 0 so that we know to ignore it when creating the Order
                            customerID = 0;

                            // Since there is not a Customer ID on the Guest record, we need to create one
                            var createCustomerRequest = new CreateCustomerRequest(partyGuest, PropertyBag.ShippingAddress, OrderConfiguration.WarehouseID, Identity.Current.CustomerID, PropertyBag.PartyID);
                            details.Add(createCustomerRequest);
                        }
                    }


                    var orderItems = ShoppingCart.Items.Where(i => i.Type == ShoppingCartItemType.Order);
                    var hasOrder = orderItems.Any();
                    var autoOrderItems = ShoppingCart.Items.Where(i => i.Type == ShoppingCartItemType.AutoOrder);
                    var hasAutoOrder = autoOrderItems.Any();

                    if (hasOrder)
                    {
                        var orderRequest = new CreateOrderRequest(OrderConfiguration, PropertyBag.ShipMethodID, orderItems, PropertyBag.ShippingAddress);
                        orderRequest.PriceType = priceTypeID;

                        if (customerID != 0)
                        {
                            orderRequest.CustomerID = customerID;
                        }
                        details.Add(orderRequest);
                    }
                    if (hasAutoOrder)
                    {
                        var autoOrderRequest = new CreateAutoOrderRequest(AutoOrderConfiguration, ExigoDAL.GetAutoOrderPaymentType(PropertyBag.PaymentMethod), PropertyBag.AutoOrderStartDate, PropertyBag.ShipMethodID, autoOrderItems, PropertyBag.ShippingAddress)
                        {
                            CustomerID = Identity.Current.CustomerID,
                            Frequency = PropertyBag.AutoOrderFrequencyType
                        };
                        details.Add(autoOrderRequest);
                    }

                    // Create the payment request
                    if (PropertyBag.PaymentMethod is CreditCard)
                    {
                        var card = PropertyBag.PaymentMethod as CreditCard;
                        if (card.Type == CreditCardType.New)
                        {
                            if (!card.IsTestCreditCard && !Request.IsLocal)
                            {
                                details.Add(new ChargeCreditCardTokenRequest(card));
                            }
                            else
                            {
                                // Test Credit Card, so no need to charge card
                                ((CreateOrderRequest)details.Where(c => c is CreateOrderRequest).FirstOrDefault()).OrderStatus = GlobalUtilities.GetDefaultOrderStatusType();
                            }
                        }
                        else
                        {
                            details.Add(new ChargeCreditCardTokenOnFileRequest(card));
                        }
                    }

                    // Process the transaction
                    var transactionRequest = new TransactionalRequest();
                    transactionRequest.TransactionRequests = details.ToArray();
                    var transactionResponse = ExigoDAL.WebService().ProcessTransaction(transactionRequest);

                    var newOrderID = 0;
                    var newCustomerID = 0;
                    if (transactionResponse.Result.Status == ResultStatus.Success)
                    {
                        foreach (var response in transactionResponse.TransactionResponses)
                        {
                            if (response is CreateOrderResponse)
                            {
                                var orderResponse = (CreateOrderResponse)response;
                                newOrderID = orderResponse.OrderID;

                                // Add cookie for Order History to show most recently created Order in case it was not yet synced to the Reporting db.
                                // This will only be set up if this is not a Guest Order
                                if (!isGuestOrder)
                                {
                                    // Create a cookie to store our newest Order ID to ensure it shows on the Order History page
                                    var orderIDCookie = new System.Web.HttpCookie("NewOrder_{0}".FormatWith(Identity.Current.CustomerID), newOrderID.ToString());
                                    orderIDCookie.Expires = DateTime.UtcNow.AddMinutes(5);
                                    Response.Cookies.Add(orderIDCookie);
                                }
                            }
                            if (response is CreateCustomerResponse)
                            {
                                var customerResponse = (CreateCustomerResponse)response;
                                newCustomerID = customerResponse.CustomerID;
                            }
                        }
                    }

                    // Update our Party Guest record with new Customer ID if created for the first time.
                    if (isGuestOrder && newCustomerID > 0)
                    {
                        partyGuest.CustomerID = newCustomerID;
                        partyService.UpdateGuest(partyGuest);
                    }

                    PropertyBag.NewOrderID = newOrderID;
                    _propertyBag = ExigoDAL.PropertyBags.Update(PropertyBag);

                    return new JsonNetResult(new
                    {
                        success = true
                    });
                }
                catch (Exception exception)
                {
                    PropertyBag.OrderException = exception.Message;
                    PropertyBag.IsSubmitting = false;
                    _propertyBag = ExigoDAL.PropertyBags.Update(PropertyBag);

                    return new JsonNetResult(new
                    {
                        success = false,
                        message = exception.Message
                    });
                }
            }
            else
            {
                if (PropertyBag.NewOrderID > 0)
                {
                    return new JsonNetResult(new
                    {
                        success = true
                    });
                }
                else
                {
                    return new JsonNetResult(new
                    {
                        success = false,
                        message = Resources.Common.YourOrderIsSubmitting
                    });
                }
            }
        }

        [Route("thank-you")]
        public ActionResult OrderComplete()
        {
            ExigoDAL.PropertyBags.Delete(PropertyBag);
            ExigoDAL.PropertyBags.Delete(ShoppingCart);
            return View();
        }

        [Route("checkout")]
        public ActionResult Checkout()
        {
            return LogicProvider.GetNextAction();
        }
        #endregion

        #region Ajax
        public ActionResult GetCartItemsCount()
        {
            try
            {
                var itemCount = ShoppingCart.Items.Sum(i => i.Quantity);

                return new JsonNetResult(new
                {
                    success = true,
                    itemTotal = itemCount
                });
            }
            catch (Exception ex)
            {
                return new JsonNetResult(new { success = false, message = ex.Message });
            }

        }

        [HttpPost]
        public async Task<JsonNetResult> GetItemList(int categoryID = 0)
        {
            try
            {
                var defaultCategoryID = (categoryID == 0) ? OrderConfiguration.CategoryID : categoryID;

                var categories = ExigoDAL.GetChildWebCategories(new GetChildWebCategoriesRequest
                {
                    TopCategoryID = defaultCategoryID,
                    GetAllChildCategories = true,
                    NestedType = NestedType.UniLevel
                });

                var auxTotalItems = new List<Item>();
                var categoryItemTasks = new List<Task<IEnumerable<Item>>>();
                var orderConfig = OrderConfiguration;
                var languageID = CurrentLanguage.LanguageID;

                if (!categories.Any())
                {
                    var categoryItems = ExigoDAL.GetItems(new ExigoService.GetItemsRequest
                    {
                        Configuration = orderConfig,
                        LanguageID = languageID,
                        IncludeStaticKitChildren = true,
                        IncludeGroupMembers = true,
                        CategoryID = categoryID
                    })?.ToList();

                    categoryItems?.ForEach(i => i.CategoryID = categoryID);

                    if (categoryItems?.Any() == true)
                    {
                        auxTotalItems.AddRange(categoryItems);
                    }
                }
                else
                {
                    categoryItemTasks.AddRange(categories.Select(
                        category => GetItemsByCategory(category, orderConfig, languageID)));

                    var categoriesItems = await Task.WhenAll(categoryItemTasks);

                    if (categoriesItems.Any())
                    {
                        auxTotalItems.AddRange(categoriesItems.SelectMany(item => item.ToList()));
                    }
                }

                // remove duplicate items. Take the item from the highest category available 
                var items = auxTotalItems.GroupBy(i => i.ItemCode)
                    .Select(g => g
                        .ToList()
                        .OrderBy(i => i.CategoryID)
                        .FirstOrDefault())
                    ?.ToList();
                //var html = this.RenderPartialViewToString("Partials/Items/_ShoppingItemList", new ShoppingItemListViewModel
                //{
                //    Items = items,
                //    Xs_Column_Qty = 1,
                //    Sm_Column_Qty = 2,
                //    Md_Column_Qty = 3,
                //    //Lg_Column_Qty = 3,
                //});

                items.ForEach(x => x.ItemUrl = (Url.Action("itemdetail", "shopping", new { itemcode = x.ItemCode })));

                return new JsonNetResult(new
                {
                    success = true,
                    items
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

        private static async Task<IEnumerable<Item>> GetItemsByCategory(WebCategory category, IOrderConfiguration configuration, int languageId)
        {
            return await Task.Run(() =>
            {
                var categoryItems = ExigoDAL.GetItems(new ExigoService.GetItemsRequest
                {
                    Configuration = configuration,
                    LanguageID = languageId,
                    IncludeStaticKitChildren = true,
                    IncludeGroupMembers = true,
                    CategoryID = category.WebCategoryID
                }).ToList();

                categoryItems.ForEach(i => i.CategoryID = category.WebCategoryID);

                return categoryItems;
            });
        }
        //public JsonNetResult GetItemList(int categoryID = 0)
        //{
        //    try
        //    {
        //        int priceTypeID = OrderConfiguration.PriceTypeID;

        //        #region Parties
        //        // If this is a Guest Order, we use an alternate Price Type for the Item calls
        //        if (PropertyBag.HasValidParty())
        //        {
        //            if (PropertyBag.IsGuestOrder())
        //            {
        //                priceTypeID = PriceTypes.Retail;
        //            }
        //        }
        //        #endregion

        //        var items = new List<Item>();
        //        var newItems = new List<Item>();
        //        ExigoService.GetItemsRequest itemsRequest;

        //        var defaultCategoryID = (categoryID == 0) ? OrderConfiguration.CategoryID : categoryID;
        //        var categories = ExigoDAL.GetChildWebCategories(new GetChildWebCategoriesRequest {
        //            TopCategoryID         = defaultCategoryID,
        //            GetAllChildCategories = true,
        //            NestedType            = NestedType.UniLevel
        //        });

        //        if (categories != null && categories.Any())
        //        {
        //            var featuredCategoryID = OrderConfiguration.FeaturedCategoryID.Equals(0) ? OrderConfiguration.CategoryID : OrderConfiguration.FeaturedCategoryID;
        //            var category = categories.Where(c => c.ParentID == featuredCategoryID).FirstOrDefault();

        //            itemsRequest = new ExigoService.GetItemsRequest
        //            {
        //                Configuration = OrderConfiguration,
        //                IncludeAllChildCategories = true,
        //                CategoryID = category.WebCategoryID,
        //                IncludeShortDescriptions = true
        //            };

        //            newItems = ExigoDAL.GetItems(itemsRequest).OrderBy(c => c.SortOrder).ToList();

        //            foreach (var newItem in newItems)
        //            {
        //                if (items.Count(i => i.ItemCode == newItem.ItemCode).Equals(0))
        //                {
        //                    items.Add(newItem);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            itemsRequest = new ExigoService.GetItemsRequest
        //            {
        //                Configuration = OrderConfiguration,
        //                IncludeAllChildCategories = true,
        //                CategoryID = categoryID,
        //                IncludeShortDescriptions = true
        //            };

        //            items = ExigoDAL.GetItems(itemsRequest).OrderBy(c => c.SortOrder).ToList();
        //        }

        //        var html = this.RenderPartialViewToString("Partials/Items/_ShoppingItemList", items);

        //        return new JsonNetResult(new
        //        {
        //            success = true,
        //            html = html
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return new JsonNetResult(new
        //        {
        //            success = false,
        //            message = ex.Message
        //        });
        //    }
        //}

        [HttpPost]
        public JsonNetResult CheckOrderSubmissionStatus()
        {
            if (PropertyBag.NewOrderID > 0)
            {
                return new JsonNetResult(new
                {
                    success = true
                });
            }
            else
            {
                var orderHadException = (PropertyBag.IsSubmitting == false);

                return new JsonNetResult(new
                {
                    success = false,
                    orderHadException = orderHadException,
                    exceptionMessage = PropertyBag.OrderException
                });
            }
        }

        [HttpPost]
        public ActionResult AddItemToCart(Item item)
        {
            ShoppingCart.Items.Add(item);
            ExigoDAL.PropertyBags.Update(ShoppingCart);

            // Return the result
            if (Request.IsAjaxRequest())
            {
                return new JsonNetResult(new
                {
                    success = true
                });
            }
            else
            {
                return RedirectToAction("Cart");
            }
        }

        [HttpPost]
        public ActionResult RemoveItemFromCart(Guid id)
        {
            try
            {
                var item = ShoppingCart.Items.Where(c => c.ID == id).FirstOrDefault();

                ShoppingCart.Items.Remove(id);
                ExigoDAL.PropertyBags.Update(ShoppingCart);

                if (Request.IsAjaxRequest())
                {
                    return new JsonNetResult(new
                    {
                        success = true,
                        item = new
                        {
                            ID = item.ID,
                            Quantity = item.Quantity,
                            Type = item.Type.ToString()
                        },
                        items = ShoppingCart.Items.GetItemsOfType(item.Type),
                        subtotal = (item != null ? this.GetCartSubtotal(item.Type) : (decimal?)null),
                        bvsubtotal = (item != null ? this.GetCartBvSubtotal(item.Type) : (decimal?)null)
                    });
                }
                else
                {
                    return RedirectToAction("Cart");
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = Request.IsLocal ? $"Raw Error for UAT/Local: {ex.Message}" : Resources.Common.UnknownError
                });
            }
        }

        [HttpPost]
        public ActionResult UpdateItemQuantity(Guid id, decimal quantity)
        {
            try
            {
                var item = ShoppingCart.Items.Where(c => c.ID == id).FirstOrDefault();

                ShoppingCart.Items.Update(id, quantity);
                ExigoDAL.PropertyBags.Update(ShoppingCart);

                if (quantity == 0)
                {
                    item.Quantity = 0;
                }

                if (Request.IsAjaxRequest())
                {
                    return new JsonNetResult(new
                    {
                        success = true,
                        item = new
                        {
                            ID = item.ID,
                            Quantity = item.Quantity,
                            Type = item.Type.ToString()
                        },
                        items = ShoppingCart.Items.GetItemsOfType(item.Type),
                        subtotal = (item != null ? this.GetCartSubtotal(item.Type) : (decimal?)null),
                        bvsubtotal = (item != null ? this.GetCartBvSubtotal(item.Type) : (decimal?)null)
                    });
                }
                else
                {
                    return RedirectToAction("Cart");
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = Request.IsLocal ? $"Raw Error for UAT/Local: {ex.Message}" : Resources.Common.UnknownError
                });
            }
        }

        [NonAction]
        private decimal GetCartSubtotal(ShoppingCartItemType type)
        {
            IOrderConfiguration config;
            switch (type)
            {
                case ShoppingCartItemType.AutoOrder:
                    config = AutoOrderConfiguration;
                    break;
                case ShoppingCartItemType.Order:
                default:
                    config = OrderConfiguration;
                    break;
            }
            var cartItems = ShoppingCart.Items.GetItemsOfType(type);
            var items = ExigoDAL.GetItems(cartItems, config, CurrentLanguage.LanguageID);
            var subtotal = items.Sum(i => i.Quantity * i.Price);
            return subtotal;
        }

        [NonAction]
        private decimal GetCartBvSubtotal(ShoppingCartItemType type)
        {
            IOrderConfiguration config;
            switch (type)
            {
                case ShoppingCartItemType.AutoOrder:
                    config = AutoOrderConfiguration;
                    break;
                case ShoppingCartItemType.Order:
                default:
                    config = OrderConfiguration;
                    break;
            }
            var cartItems = ShoppingCart.Items.GetItemsOfType(type);
            var items = ExigoDAL.GetItems(cartItems, config, CurrentLanguage.LanguageID);
            var bvsubtotal = items.Sum(i => i.Quantity * i.BV);
            return bvsubtotal;
        }

        [HttpPost]
        public ActionResult SetShipMethodID(int shipMethodID)
        {
            PropertyBag.ShipMethodID = shipMethodID;
            ExigoDAL.PropertyBags.Update(PropertyBag);

            return RedirectToAction("Review");
        }
        #endregion

        #region Parties
        [Route("hostorder")]
        public ActionResult HostOrder(int hostID, int partyID)
        {
            // Reset our Property Bags
            ExigoDAL.PropertyBags.Delete(PropertyBag);
            ExigoDAL.PropertyBags.Delete(ShoppingCart);

            PropertyBag.PartyID = partyID;
            PropertyBag.HostID = hostID;

            // Clear out our Guest ID just in case
            PropertyBag.GuestID = 0;

            // Reset Property Bag first just to ensure the payment method and shipping address are reset
            PropertyBag.ShippingAddress = null;
            PropertyBag.PaymentMethod = null;

            ExigoDAL.PropertyBags.Update(PropertyBag);


            return RedirectToAction("itemlist");
        }

        public ActionResult StartPartyOrder(int partyID, int guestID)
        {
            // Reset our Property Bags
            ExigoDAL.PropertyBags.Delete(PropertyBag);
            ExigoDAL.PropertyBags.Delete(ShoppingCart);

            PropertyBag.PartyID = partyID;
            PropertyBag.GuestID = guestID;

            // Clear out the Host ID as well just in case
            PropertyBag.HostID = 0;

            // Reset Property Bag first just to ensure the payment method and shipping address are reset
            PropertyBag.ShippingAddress = null;
            PropertyBag.PaymentMethod = null;

            ExigoDAL.PropertyBags.Update(PropertyBag);

            return RedirectToAction("itemlist");
        }

        public JsonNetResult ResetShopping()
        {
            ExigoDAL.PropertyBags.Delete(ShoppingCart);
            ExigoDAL.PropertyBags.Delete(PropertyBag);

            return new JsonNetResult(new { success = true });
        }
        #endregion
    }
}
