using Common;
using Common.Api.ExigoWebService;
using Common.Exceptions;
using Common.Providers;
using Common.Services;
using Dapper;
using ExigoService;
using ReplicatedSite.Factories;
using ReplicatedSite.Models;
using ReplicatedSite.Providers;
using ReplicatedSite.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ReplicatedSite.Controllers
{
    [RoutePrefix("{webalias}")]
    public class ShoppingController : BaseController
    {
        #region Properties
        public string ShoppingCartName = GlobalSettings.Globalization.CookieKey + "ReplicatedSiteShopping";

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


        public ShoppingService ShoppingService
        {
            get
            {
                if (_shoppingService == null)
                {
                    _shoppingService = new ShoppingService(OrderConfiguration);
                }
                return _shoppingService;
            }
        }
        private ShoppingService _shoppingService { get; set; }
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
            var partyService = new PartyService(Identity.Owner.CustomerID);

            // Party Info Logic
            if (PropertyBag.HasValidParty())
            {
                ViewBag.ActiveParty = partyService.GetParties(new ExigoService.GetPartiesRequest
                {
                    CustomerID = Identity.Owner.CustomerID,
                    PartyID = PropertyBag.PartyID,
                    IncludeHostessDetails = true,
                    PartyStatusID = (int)PartyStatusTypes.Open,
                    CheckForHostessOrder = true
                }).FirstOrDefault();

                // Validate if party is still able to be shopped under
                if (ViewBag.ActiveParty == null || ViewBag.ActiveParty.HostOrderIsPlaced || (ViewBag.ActiveParty.CloseDate != null && ViewBag.ActiveParty.CloseDate < DateTime.Now))
                {
                    ViewBag.ActiveParty = null;
                    PropertyBag.PartyID = 0;
                    PropertyBag.GuestID = 0;
                    ExigoDAL.PropertyBags.Update(PropertyBag);

                    ViewBag.PartyInvalid = true;
                }
                else
                {
                    if (PropertyBag.IsGuestCheckout())
                    {
                        ViewBag.ActiveParty.CurrentGuest = partyService.GetPartyGuests(PropertyBag.PartyID, PropertyBag.GuestID).FirstOrDefault();
                    }
                }
            }
            else if (!Identity.Owner.IsOrphan)
            {
                ViewBag.OpenParties = partyService.GetParties(new ExigoService.GetPartiesRequest
                {
                    CustomerID = Identity.Owner.CustomerID,
                    IncludeHostessDetails = true,
                    PartyStatusID = (int)PartyStatusTypes.Open,
                    ExcludeFutureParties = true,
                    CheckForHostessOrder = true,
                    ExcludeClosedParties = true
                }).Where(p => p.HostOrderIsPlaced == false).ToList();
            }
            #endregion

            return View(model);
        }

        [Route("new-products")]
        public ActionResult NewItemList()
        {
            return View();
        }

        [Route("product/{itemcode}")]
        public ActionResult ItemDetail(string itemcode)
        {
            var model = ShoppingViewModelFactory.Create<ItemDetailViewModel>(PropertyBag);

            model.Item = ExigoDAL.GetItems(new ExigoService.GetItemsRequest
            {
                Configuration = OrderConfiguration,
                ItemCodes = new List<string> { itemcode },
                LanguageID = CurrentLanguage.LanguageID,
                IncludeShortDescriptions = true,
                IncludeLongDescriptions = true,
                IncludeAllChildCategories = true,
                IncludeGroupMembers = true,
                IncludeStaticKitChildren = true,
                IgnoreCategoryRestrictions = true,
                IncludeDynamicKitChildren = true
            }).FirstOrDefault();

            if (model.Item != null)
            {
                model.Item.Quantity = 1;
            }
            else
            {
                return RedirectToAction("ItemList");
            }

            #region Parties
            var partyService = new PartyService(Identity.Owner.CustomerID);

            // Party Info Logic
            if (PropertyBag.HasValidParty())
            {
                ViewBag.ActiveParty = partyService.GetParties(new ExigoService.GetPartiesRequest
                {
                    CustomerID = Identity.Owner.CustomerID,
                    PartyID = PropertyBag.PartyID,
                    IncludeHostessDetails = true,
                    PartyStatusID = (int)PartyStatusTypes.Open,
                    CheckForHostessOrder = true
                }).FirstOrDefault();

                // Validate if party is still able to be shopped under
                if (ViewBag.ActiveParty == null || ViewBag.ActiveParty.HostOrderIsPlaced || (ViewBag.ActiveParty.CloseDate != null && ViewBag.ActiveParty.CloseDate < DateTime.Now))
                {
                    ViewBag.ActiveParty = null;
                    PropertyBag.PartyID = 0;
                    PropertyBag.GuestID = 0;
                    ExigoDAL.PropertyBags.Update(PropertyBag);

                    ViewBag.PartyInvalid = true;
                }
                else
                {
                    if (PropertyBag.IsGuestCheckout())
                    {
                        ViewBag.ActiveParty.CurrentGuest = partyService.GetPartyGuests(PropertyBag.PartyID, PropertyBag.GuestID).FirstOrDefault();
                    }
                }
            }
            else if (!Identity.Owner.IsOrphan)
            {
                ViewBag.OpenParties = partyService.GetParties(new ExigoService.GetPartiesRequest
                {
                    CustomerID = Identity.Owner.CustomerID,
                    IncludeHostessDetails = true,
                    PartyStatusID = (int)PartyStatusTypes.Open,
                    ExcludeFutureParties = true,
                    CheckForHostessOrder = true,
                    ExcludeClosedParties = true
                }).Where(p => p.HostOrderIsPlaced == false).ToList();
            }
            #endregion

            return View(model);
        }

        [Route("cart")]
        public ActionResult Cart()
        {
            var model = ShoppingViewModelFactory.Create<CartViewModel>(PropertyBag);

            // Get the cart items
            var cartItems = ShoppingCart.Items.ToList();
            if (cartItems.Where(el => el.Type == ShoppingCartItemType.Order).Any())
            {
                model.OrderItems = ExigoDAL.GetItems(new ExigoService.GetItemsRequest
                {
                    ShoppingCartItems = cartItems.Where(i => i.Type == ShoppingCartItemType.Order),
                    Configuration = OrderConfiguration,
                    IgnoreCategoryRestrictions = true,
                    IncludeStaticKitChildren = true,
                    IncludeDynamicKitChildren = true,
                    LanguageID = CurrentLanguage.LanguageID
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
                    ShoppingCartItems = cartItems.Where(i => i.Type == ShoppingCartItemType.AutoOrder),
                    Configuration = AutoOrderConfiguration,
                    IgnoreCategoryRestrictions = true,
                    IncludeStaticKitChildren = true,
                    IncludeDynamicKitChildren = true,
                    LanguageID = CurrentLanguage.LanguageID
                }).ToList();
            }
            else
            {
                model.AutoOrderItems = new List<Item>();
            }
            #region Parties
            var partyService = new PartyService(Identity.Owner.CustomerID);

            // Party Info Logic
            if (PropertyBag.HasValidParty())
            {
                ViewBag.ActiveParty = partyService.GetParties(new ExigoService.GetPartiesRequest
                {
                    CustomerID = Identity.Owner.CustomerID,
                    PartyID = PropertyBag.PartyID,
                    IncludeHostessDetails = true,
                    PartyStatusID = (int)PartyStatusTypes.Open,
                    CheckForHostessOrder = true
                }).FirstOrDefault();

                // Validate if party is still able to be shopped under
                if (ViewBag.ActiveParty == null || ViewBag.ActiveParty.HostOrderIsPlaced || (ViewBag.ActiveParty.CloseDate != null && ViewBag.ActiveParty.CloseDate < DateTime.Now))
                {
                    ViewBag.ActiveParty = null;
                    PropertyBag.PartyID = 0;
                    PropertyBag.GuestID = 0;
                    ExigoDAL.PropertyBags.Update(PropertyBag);

                    ViewBag.PartyInvalid = true;
                }
                else
                {
                    if (PropertyBag.IsGuestCheckout())
                    {
                        ViewBag.ActiveParty.CurrentGuest = partyService.GetPartyGuests(PropertyBag.PartyID, PropertyBag.GuestID).FirstOrDefault();
                    }
                }
            }
            else if (!Identity.Owner.IsOrphan)
            {
                ViewBag.OpenParties = partyService.GetParties(new ExigoService.GetPartiesRequest
                {
                    CustomerID = Identity.Owner.CustomerID,
                    IncludeHostessDetails = true,
                    PartyStatusID = (int)PartyStatusTypes.Open,
                    ExcludeFutureParties = true,
                    CheckForHostessOrder = true,
                    ExcludeClosedParties = true
                }).Where(p => p.HostOrderIsPlaced == false).ToList();
            }
            #endregion

            return View(model);
        }
        [Route("cartview")]
        public ActionResult CartView()
        {
            return View();
        }
        [Route("shopsingle")]
        public ActionResult ShopSingle()
        {
            return View();
        }

        #endregion

        #region Auto Order
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

        #region Shipping
        [Route("checkout/shipping")]
        public ActionResult Shipping()
        {
            var model = ShoppingViewModelFactory.Create<ShippingAddressesViewModel>(PropertyBag);

            if (Identity.Customer != null)
            {
                model.Addresses = Customers.GetCustomerAddresses(Identity.Customer.CustomerID)
                    .Where(c => c.IsComplete)
                    .Select(c => c as ShippingAddress);
            }

            #region Parties
            var partyService = new PartyService(Identity.Owner.CustomerID);

            // Party Info Logic
            if (PropertyBag.HasValidParty())
            {
                ViewBag.ActiveParty = partyService.GetParties(new ExigoService.GetPartiesRequest
                {
                    CustomerID = Identity.Owner.CustomerID,
                    PartyID = PropertyBag.PartyID,
                    IncludeHostessDetails = true,
                    PartyStatusID = (int)PartyStatusTypes.Open,
                    CheckForHostessOrder = true
                }).FirstOrDefault();

                // Validate if party is still able to be shopped under
                if (ViewBag.ActiveParty == null || ViewBag.ActiveParty.HostOrderIsPlaced || (ViewBag.ActiveParty.CloseDate != null && ViewBag.ActiveParty.CloseDate < DateTime.Now))
                {
                    ViewBag.ActiveParty = null;
                    PropertyBag.PartyID = 0;
                    PropertyBag.GuestID = 0;
                    ExigoDAL.PropertyBags.Update(PropertyBag);

                    ViewBag.PartyInvalid = true;
                }
                else
                {
                    if (PropertyBag.IsGuestCheckout())
                    {
                        ViewBag.ActiveParty.CurrentGuest = partyService.GetPartyGuests(PropertyBag.PartyID, PropertyBag.GuestID).FirstOrDefault();
                    }
                }
            }
            else if (!Identity.Owner.IsOrphan)
            {
                ViewBag.OpenParties = partyService.GetParties(new ExigoService.GetPartiesRequest
                {
                    CustomerID = Identity.Owner.CustomerID,
                    IncludeHostessDetails = true,
                    PartyStatusID = (int)PartyStatusTypes.Open,
                    ExcludeFutureParties = true,
                    CheckForHostessOrder = true,
                    ExcludeClosedParties = true
                }).Where(p => p.HostOrderIsPlaced == false).ToList();
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
                ExigoDAL.SetCustomerAddressOnFile(Identity.Customer.CustomerID, address as Address);
            }

            PropertyBag.ShippingAddress = address;
            ExigoDAL.PropertyBags.Update(PropertyBag);

            return LogicProvider.GetNextAction();

        }
        #endregion

        #region Payments
        [Route("checkout/payment")]
        public ActionResult Payment()
        {
            var model = ShoppingViewModelFactory.Create<PaymentMethodsViewModel>(PropertyBag);

            if (Identity.Customer != null)
            {
                model.PaymentMethods = ExigoDAL.GetCustomerPaymentMethods(new GetCustomerPaymentMethodsRequest
                {
                    CustomerID = Identity.Customer.CustomerID,
                    ExcludeIncompleteMethods = true,
                    ExcludeInvalidMethods = true
                });

                model.Addresses = Customers.GetCustomerAddresses(Identity.Customer.CustomerID)
                    .Where(c => c.IsComplete)
                    .Select(c => c as ShippingAddress);
            }

            #region Parties
            var partyService = new PartyService(Identity.Owner.CustomerID);

            // Party Info Logic
            if (PropertyBag.HasValidParty())
            {
                ViewBag.ActiveParty = partyService.GetParties(new ExigoService.GetPartiesRequest
                {
                    CustomerID = Identity.Owner.CustomerID,
                    PartyID = PropertyBag.PartyID,
                    IncludeHostessDetails = true,
                    PartyStatusID = (int)PartyStatusTypes.Open,
                    CheckForHostessOrder = true
                }).FirstOrDefault();

                // Validate if party is still able to be shopped under
                if (ViewBag.ActiveParty == null || ViewBag.ActiveParty.HostOrderIsPlaced || (ViewBag.ActiveParty.CloseDate != null && ViewBag.ActiveParty.CloseDate < DateTime.Now))
                {
                    ViewBag.ActiveParty = null;
                    PropertyBag.PartyID = 0;
                    PropertyBag.GuestID = 0;
                    ExigoDAL.PropertyBags.Update(PropertyBag);

                    ViewBag.PartyInvalid = true;
                }
                else
                {
                    if (PropertyBag.IsGuestCheckout())
                    {
                        ViewBag.ActiveParty.CurrentGuest = partyService.GetPartyGuests(PropertyBag.PartyID, PropertyBag.GuestID).FirstOrDefault();
                    }
                }
            }
            else if (!Identity.Owner.IsOrphan)
            {
                ViewBag.OpenParties = partyService.GetParties(new ExigoService.GetPartiesRequest
                {
                    CustomerID = Identity.Owner.CustomerID,
                    IncludeHostessDetails = true,
                    PartyStatusID = (int)PartyStatusTypes.Open,
                    ExcludeFutureParties = true,
                    CheckForHostessOrder = true,
                    ExcludeClosedParties = true
                }).Where(p => p.HostOrderIsPlaced == false).ToList();
            }
            #endregion

            return View("Payment", model);
        }

        [HttpPost]
        public ActionResult UseCreditCardOnFile(CreditCardType type, string existingcardcvv)
        {
            var paymentMethod = ExigoDAL.GetCustomerPaymentMethods(new GetCustomerPaymentMethodsRequest
            {
                CustomerID = Identity.Customer.CustomerID,
                ExcludeIncompleteMethods = true,
                ExcludeInvalidMethods = true
            }).Where(c => c is CreditCard && ((CreditCard)c).Type == type).FirstOrDefault();

            var card = paymentMethod as CreditCard;

            card.CVV = existingcardcvv;

            return UsePaymentMethod(card);
        }

        [HttpPost]
        public ActionResult UseBankAccountOnFile(ExigoService.BankAccountType type)
        {
            var paymentMethod = ExigoDAL.GetCustomerPaymentMethods(new GetCustomerPaymentMethodsRequest
            {
                CustomerID = Identity.Customer.CustomerID,
                ExcludeIncompleteMethods = true,
                ExcludeInvalidMethods = true
            }).Where(c => c is BankAccount && ((BankAccount)c).Type == type).FirstOrDefault();

            return UsePaymentMethod(paymentMethod);
        }

        [HttpPost]
        public ActionResult UseCreditCard(CreditCard newCard, bool billingSameAsShipping = false)
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
                // Use if you want to have cards auto save on the Customer's account when added
                #region Save Cards if new
                //if (Identity.Customer != null)
                //{
                //    // Save the credit card to the customer's account if applicable                
                //    var paymentMethodsOnFile = Exigo.GetCustomerPaymentMethods(new GetCustomerPaymentMethodsRequest
                //    {
                //        CustomerID = Identity.Customer.CustomerID,
                //        ExcludeIncompleteMethods = true,
                //        ExcludeInvalidMethods = true
                //    }).Where(c => c is CreditCard).Select(c => c as CreditCard);

                //    if (paymentMethodsOnFile.Where(c => c.Type == CreditCardType.Primary).FirstOrDefault() == null)
                //    {
                //        Exigo.SetCustomerCreditCard(Identity.Customer.CustomerID, newCard, CreditCardType.Primary);
                //    }
                //    else if (paymentMethodsOnFile.Where(c => c.Type == CreditCardType.Secondary).FirstOrDefault() == null)
                //    {
                //        Exigo.SetCustomerCreditCard(Identity.Customer.CustomerID, newCard, CreditCardType.Secondary);
                //    }
                //}
                #endregion


                return UsePaymentMethod(newCard);
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
                    CustomerID = Identity.Customer.CustomerID,
                    ExcludeIncompleteMethods = true,
                    ExcludeInvalidMethods = true,
                }).Where(c => c is BankAccount).Select(c => c as BankAccount);

                if (paymentMethodsOnFile.FirstOrDefault() == null)
                {
                    ExigoDAL.SetCustomerBankAccount(Identity.Customer.CustomerID, newBankAccount);
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

            #region Order Totals
            var beginningShipMethodID = PropertyBag.ShipMethodID;

            // If this is the first time coming to the page, and the property bag's ship method has not been set, then set it to the default for the configuration
            if (PropertyBag.ShipMethodID == 0)
            {
                PropertyBag.ShipMethodID = OrderConfiguration.DefaultShipMethodID;
                beginningShipMethodID = PropertyBag.ShipMethodID;
                ExigoDAL.PropertyBags.Update(PropertyBag);
            }


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
                    CustomerID = Identity.Customer.CustomerID
                });

                model.OrderTotals = newCalculationResult;
            }
            #endregion

            #region Parties
            if (!Identity.Owner.IsOrphan)
            {
                var partyService = new PartyService(Identity.Owner.CustomerID);

                if (PropertyBag.HasValidParty())
                {
                    ViewBag.ActiveParty = partyService.GetParties(new ExigoService.GetPartiesRequest
                    {
                        CustomerID = Identity.Owner.CustomerID,
                        PartyID = PropertyBag.PartyID,
                        IncludeHostessDetails = true,
                        PartyStatusID = (int)PartyStatusTypes.Open,
                        CheckForHostessOrder = true
                    }).FirstOrDefault();

                    // Validate if party is still able to be shopped under
                    if (ViewBag.ActiveParty == null || ViewBag.ActiveParty.HostOrderIsPlaced)
                    {
                        ViewBag.ActiveParty = null;
                        PropertyBag.PartyID = 0;
                        PropertyBag.GuestID = 0;
                        ExigoDAL.PropertyBags.Update(PropertyBag);

                        ViewBag.PartyInvalid = true;
                    }
                    else
                    {
                        if (PropertyBag.IsGuestCheckout())
                        {
                            ViewBag.ActiveParty.CurrentGuest = partyService.GetPartyGuests(PropertyBag.PartyID, PropertyBag.GuestID).FirstOrDefault();
                        }
                    }
                }


                if (PropertyBag.PartyID == 0)
                {
                    ViewBag.OpenParties = partyService.GetParties(new ExigoService.GetPartiesRequest
                    {
                        CustomerID = Identity.Owner.CustomerID,
                        IncludeHostessDetails = true,
                        PartyStatusID = (int)PartyStatusTypes.Open,
                        ExcludeFutureParties = true,
                        CheckForHostessOrder = true,
                        ExcludeClosedParties = true
                    }).Where(p => p.HostOrderIsPlaced == false).ToList();
                }
            }
            #endregion

            return View(model);
        }

        [Route("checkout")]
        public ActionResult Checkout()
        {
            return LogicProvider.GetNextAction();
        }
        //public ActionResult Checkout(CheckoutModel model = null)
        //{
        //    if (Request.HttpMethod == "GET")
        //    {
        //        var logicResponse = LogicProvider.CheckLogic();
        //        if (!logicResponse.IsValid)
        //        {
        //            return logicResponse.NextAction;
        //        }

        //        var orderItems = ShoppingCart.Items.Where(i => i.Type == ShoppingCartItemType.Order);
        //        var hasOrderItems = orderItems.Any();
        //        var autoOrderItems = ShoppingCart.Items.Where(i => i.Type == ShoppingCartItemType.AutoOrder);
        //        var hasAutoOrderItems = autoOrderItems.Any();


        //        var vmodel = new CheckoutViewModel(PropertyBag);

        //        #region Order
        //        if (hasOrderItems)
        //        {
        //            vmodel.ShoppingCartItems = ExigoDAL.GetItems(ShoppingCart.Items.Where(i => i.Type == ShoppingCartItemType.Order), OrderConfiguration, CurrentLanguage.LanguageID);
        //            vmodel.OrderCalculationResponse = ExigoDAL.CalculateOrder(new OrderCalculationRequest()
        //            {
        //                Items = orderItems,
        //                Configuration = OrderConfiguration,
        //                Address = PropertyBag.ShippingAddress,
        //                ShipMethodID = PropertyBag.ShipMethodID,
        //                ReturnShipMethods = true,
        //                CustomerID = Identity.Customer != null ? Identity.Customer.CustomerID : 0
        //            });

        //            if (vmodel.OrderCalculationResponse.ShipMethods != null && vmodel.OrderCalculationResponse.ShipMethods.Count() > 0)
        //            {
        //                vmodel.ShipMethods = vmodel.OrderCalculationResponse.ShipMethods.ToList();
        //            }

        //            vmodel.DeliveryType = vmodel.OrderCalculationResponse.ShipMethods != null ? vmodel.OrderCalculationResponse.ShipMethods.FirstOrDefault(s => s.Selected).Price.ToString("C") : Resources.Common.ChooseShippingMethod;
        //        }
        //        #endregion

        //        #region Auto Order
        //        /// TODO: We will need to eventually add logic to utilze Auto Order address if provided
        //        if (hasAutoOrderItems)
        //        {
        //            vmodel.AutoOrderStartDate = PropertyBag.AutoOrderStartDate;
        //            vmodel.AutoOrderShoppingCartItems = ExigoDAL.GetItems(autoOrderItems, OrderConfiguration, CurrentLanguage.LanguageID);
        //            vmodel.AutoOrderCalculationResponse = ExigoDAL.CalculateOrder(new OrderCalculationRequest()
        //            {
        //                Items = autoOrderItems,
        //                Configuration = OrderConfiguration,
        //                Address = PropertyBag.ShippingAddress,
        //                ShipMethodID = PropertyBag.ShipMethodID,
        //                ReturnShipMethods = true,
        //                CustomerID = Identity.Customer != null ? Identity.Customer.CustomerID : 0
        //            });

        //            if (vmodel.OrderCalculationResponse.ShipMethods != null && vmodel.OrderCalculationResponse.ShipMethods.Count() > 0)
        //            {
        //                vmodel.ShipMethods = vmodel.OrderCalculationResponse.ShipMethods.ToList();
        //            }

        //            vmodel.DeliveryType = vmodel.OrderCalculationResponse.ShipMethods != null ? vmodel.OrderCalculationResponse.ShipMethods.FirstOrDefault(s => s.Selected).Price.ToString("C") : Resources.Common.ChooseShippingMethod;
        //        }
        //        #endregion

        //        // Default Country logic for payment method
        //        if (vmodel.CreditCardPayment == null)
        //        {
        //            vmodel.CreditCardPayment = new CreditCard(new Address(CurrentMarket.MainCountry, ""));
        //        }

        //        return View(vmodel);
        //    }

        //    // Saving Information for submission
        //    try
        //    {
        //        // Is Authenticated?
        //        if (Identity.Customer == null)
        //        {
        //            PropertyBag.NewCustomerLoginName = model.Login;
        //            PropertyBag.NewCustomerPassword = model.Password;
        //        }

        //        PropertyBag.ShippingAddress = model.ShippingAddress;
        //        PropertyBag.ShipMethodID = model.ShipMethodID;
        //        PropertyBag.BillingAddressSameAsShipping = model.BillingAddressSameAsShipping;

        //        if (model.BillingAddressSameAsShipping)
        //        {
        //            model.CreditCardPayment.BillingAddress = model.ShippingAddress;
        //        }
        //        PropertyBag.PaymentMethod = model.CreditCardPayment;

        //        ExigoDAL.PropertyBags.Update(PropertyBag);

        //        // Testing
        //        //return Json(new
        //        //{
        //        //    success = true,
        //        //    model,
        //        //    PropertyBag
        //        //});

        //        return SubmitCheckout();
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = ex.Message });
        //    }
        //}

        [HttpPost]
        public ActionResult SubmitCheckout()
        {
            // For Party logic, if applicable
            var partyInvalid = false;

            if (!PropertyBag.IsSubmitting)
            {
                PropertyBag.IsSubmitting = true;
                _propertyBag = ExigoDAL.PropertyBags.Update(PropertyBag);

                try
                {
                    var details = new List<ApiRequest>();
                    var isGuestCheckout = Identity.Customer == null;
                    var isLocal = Request.IsLocal;

                    // Guest Checkout Customer creation, if user not logged in
                    if (Identity.Customer == null)
                    {
                        var address = PropertyBag.ShippingAddress;

                        var createCustomerRequest = new CreateCustomerRequest
                        {
                            FirstName = address.FirstName,
                            LastName = address.LastName,
                            Email = address.Email,
                            Phone = address.Phone,
                            EntryDate = DateTime.Now.ToCST(),
                            MainAddress1 = address.Address1,
                            MainAddress2 = address.Address2,
                            MainCity = address.City,
                            MainState = address.State,
                            MainZip = address.Zip,
                            MainCountry = address.Country,
                            DefaultWarehouseID = OrderConfiguration.WarehouseID,

                            InsertEnrollerTree = true,
                            EnrollerID = Identity.Owner.CustomerID,
                            CustomerStatus = CustomerStatuses.Active,
                            CustomerType = CustomerTypes.RetailCustomer
                        };

                        // If LoginName is provided
                        if (isGuestCheckout && PropertyBag.HasProvidedNewCustomerLoginName)
                        {
                            createCustomerRequest.CanLogin = true;
                            createCustomerRequest.LoginName = PropertyBag.NewCustomerLoginName;
                            createCustomerRequest.LoginPassword = PropertyBag.NewCustomerPassword;
                        }

                        details.Add(createCustomerRequest);
                    }

                    var orderItems = ShoppingCart.Items.Where(i => i.Type == ShoppingCartItemType.Order);
                    var hasOrder = orderItems.Any();
                    var autoOrderItems = ShoppingCart.Items.Where(i => i.Type == ShoppingCartItemType.AutoOrder);
                    var hasAutoOrder = autoOrderItems.Any();

                    if (hasOrder)
                    {
                        var orderRequest = new CreateOrderRequest(OrderConfiguration, PropertyBag.ShipMethodID, orderItems, PropertyBag.ShippingAddress);
                        orderRequest.CustomerID = Identity.Customer?.CustomerID ?? 0;
                        details.Add(orderRequest);
                    }

                    if (hasAutoOrder)
                    {
                        var autoOrderRequest = new CreateAutoOrderRequest(AutoOrderConfiguration, ExigoDAL.GetAutoOrderPaymentType(PropertyBag.PaymentMethod), PropertyBag.AutoOrderStartDate, PropertyBag.ShipMethodID, autoOrderItems, PropertyBag.ShippingAddress)
                        {
                            CustomerID = Identity.Customer?.CustomerID ?? 0,
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
                    if (transactionResponse.Result.Status == ResultStatus.Success)
                    {
                        foreach (var response in transactionResponse.TransactionResponses)
                        {
                            if (response is CreateOrderResponse)
                            {
                                var orderResponse = (CreateOrderResponse)response;
                                newOrderID = orderResponse.OrderID;

                                if (!isGuestCheckout)
                                {
                                    // Create a cookie to store our newest Order ID to ensure it shows on the Order History page
                                    var orderIDCookie = new System.Web.HttpCookie("NewOrder_{0}".FormatWith(Identity.Customer.CustomerID), newOrderID.ToString());
                                    orderIDCookie.Expires = DateTime.UtcNow.AddMinutes(5);
                                    Response.Cookies.Add(orderIDCookie);
                                }
                            }
                        }
                    }

                    ExigoDAL.PropertyBags.Delete(PropertyBag);
                    ExigoDAL.PropertyBags.Delete(ShoppingCart);

                    return new JsonNetResult(new
                    {
                        success = true,
                        redirectUrl = Url.Action("OrderComplete", "Shopping")
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
                        message = exception.Message,
                        partyInvalid
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
            return View();
        }



        #endregion

        #region Ajax
        [HttpPost]
        public async Task<JsonNetResult> GetItemList(int categoryId = 0)
        {
            try
            {
                var defaultCategoryId = (categoryId == 0) ? OrderConfiguration.CategoryID : categoryId;

                var categories = ExigoDAL.GetChildWebCategories(new GetChildWebCategoriesRequest
                {
                    TopCategoryID = defaultCategoryId,
                    GetAllChildCategories = true,
                    NestedType = NestedType.UniLevel
                });

                var auxTotalItems = new List<Item>();
                var categoryItemTasks = new List<Task<IEnumerable<Item>>>();
                var orderConfig = OrderConfiguration;
                var languageId = CurrentLanguage.LanguageID;

                if (!categories.Any())
                {
                    var categoryItems = ExigoDAL.GetItems(new ExigoService.GetItemsRequest
                    {
                        Configuration = orderConfig,
                        LanguageID = languageId,
                        IncludeStaticKitChildren = true,
                        IncludeGroupMembers = true,
                        CategoryID = categoryId
                    })?.ToList();

                    categoryItems?.ForEach(i => i.CategoryID = categoryId);

                    if (categoryItems?.Any() == true)
                    {
                        auxTotalItems.AddRange(categoryItems);
                    }
                }
                else
                {
                    categoryItemTasks.AddRange(categories.Select(
                        category => GetItemsByCategory(category, orderConfig, languageId)));

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

        [HttpPost]
        public JsonNetResult GetQuickViewModal(string itemcode)
        {
            try
            {
                var model = ShoppingViewModelFactory.Create<ItemDetailViewModel>(PropertyBag);

                model.Item = ExigoDAL.GetItems(new ExigoService.GetItemsRequest
                {
                    Configuration = OrderConfiguration,
                    ItemCodes = new List<string> { itemcode },
                    LanguageID = CurrentLanguage.LanguageID,
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

                var html = this.RenderPartialViewToString("Partials/Items/_QuickViewModal", model.Item);

                return new JsonNetResult(new
                {
                    success = true,
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
            try
            {
                // convert children to key/value pairs
                var children = item.Children?.ToDictionary(c => c.ItemCode, c => (int)c.Quantity);
                // validate items/children and convert to cart objects
                var cartItem = this.ShoppingService.GetShoppingCartItem(item.ItemCode, (int)item.Quantity, children);
                if (cartItem == null)
                {
                    throw new Exception($"Could not add item: {Resources.Common.ItemNotFound} ({item.ItemCode})");
                }
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
            // Catch errors where item is not available, or itemcode is not provided
            catch (Exception ex) when (
                ex is ArgumentNullException
                || ex is ArgumentOutOfRangeException)
            {
                return Json(new { success = false, message = "Item not found or not available" }); // TODO: resource text
            }
            // catch errors where item children are not configured correctly
            catch (Exception ex) when (
                ex is InvalidRequestException
                || ex is NullReferenceException)
            {
                return Json(new { success = false, message = "Missing or invalid item configuration" }); // TODO: resource text
            }
            // catch all other errors
            catch (Exception ex)
            {
                return Json(new { success = false, message = Request.IsLocal ? $"Raw Error for UAT/Local: {ex.Message}" : Resources.Common.UnknownError });
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
                        isCartEmpty = !ShoppingCart.Items.Any()
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
                        isCartEmpty = !ShoppingCart.Items.Any()
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


        // Get Ship Methods for Shipping view based on the Address chosen
        public JsonNetResult GetShipMethods(Address address = null)
        {
            try
            {
                if (address == null || !address.IsComplete)
                {
                    address = PropertyBag.ShippingAddress;
                }

                var cartItems = ShoppingCart.Items.ToList();
                var orderTotals = new OrderCalculationResponse();
                List<ShipMethod> shipMethods = new List<ShipMethod>();
                // Calculate the order if applicable
                var orderItems = cartItems.Where(c => c.Type == ShoppingCartItemType.Order).ToList();
                if (orderItems.Count > 0)
                {
                    #region Order Totals
                    var beginningShipMethodID = PropertyBag.ShipMethodID;

                    // If this is the first time coming to the page, and the property bag's ship method has not been set, then set it to the default for the configuration
                    if (PropertyBag.ShipMethodID == 0)
                    {
                        PropertyBag.ShipMethodID = OrderConfiguration.DefaultShipMethodID;
                        beginningShipMethodID = PropertyBag.ShipMethodID;
                        ExigoDAL.PropertyBags.Update(PropertyBag);
                    }

                    orderTotals = ExigoDAL.CalculateOrder(new OrderCalculationRequest
                    {
                        Configuration = OrderConfiguration,
                        Items = orderItems,
                        Address = address,
                        ShipMethodID = PropertyBag.ShipMethodID,
                        ReturnShipMethods = true
                    });
                    shipMethods = orderTotals.ShipMethods.OrderBy(s => s.Price).ToList();

                    // Set the default ship method
                    if (shipMethods.Count() > 0)
                    {
                        if (shipMethods.Any(c => c.ShipMethodID == PropertyBag.ShipMethodID))
                        {
                            // If the property bag ship method ID exists in the results from order calc, set the correct result as selected                
                            shipMethods.First(c => c.ShipMethodID == PropertyBag.ShipMethodID).Selected = true;
                        }
                        else
                        {
                            // If we don't have the ship method we're supposed to select, check the first one, save the selection and recalculate
                            shipMethods.First().Selected = true;

                            // If for some reason the property bag is outdated and the ship method stored in it is not in the list, set the first result as selected and re-set the property bag's value
                            PropertyBag.ShipMethodID = shipMethods.FirstOrDefault().ShipMethodID;
                            ExigoDAL.PropertyBags.Update(PropertyBag);
                        }
                    }

                    #endregion
                }
                else if (cartItems.Any(i => i.Type == ShoppingCartItemType.AutoOrder))
                {
                    /// TODO: Need to add logic for Auto Order specific Ship Method if provided
                    #region Auto Order Totals
                    var beginningShipMethodID = PropertyBag.ShipMethodID;

                    // If this is the first time coming to the page, and the property bag's ship method has not been set, then set it to the default for the configuration
                    if (PropertyBag.ShipMethodID == 0)
                    {
                        PropertyBag.ShipMethodID = OrderConfiguration.DefaultShipMethodID;
                        beginningShipMethodID = PropertyBag.ShipMethodID;
                        ExigoDAL.PropertyBags.Update(PropertyBag);
                    }

                    orderTotals = ExigoDAL.CalculateOrder(new OrderCalculationRequest
                    {
                        Configuration = OrderConfiguration,
                        Items = cartItems.Where(i => i.Type == ShoppingCartItemType.AutoOrder),
                        Address = address,
                        ShipMethodID = PropertyBag.ShipMethodID,
                        ReturnShipMethods = true
                    });
                    shipMethods = orderTotals.ShipMethods.OrderBy(s => s.Price).ToList();

                    // Set the default ship method
                    if (shipMethods.Count() > 0)
                    {
                        if (shipMethods.Any(c => c.ShipMethodID == PropertyBag.ShipMethodID))
                        {
                            // If the property bag ship method ID exists in the results from order calc, set the correct result as selected                
                            shipMethods.First(c => c.ShipMethodID == PropertyBag.ShipMethodID).Selected = true;
                        }
                        else
                        {
                            // If we don't have the ship method we're supposed to select, check the first one, save the selection and recalculate
                            shipMethods.First().Selected = true;

                            // If for some reason the property bag is outdated and the ship method stored in it is not in the list, set the first result as selected and re-set the property bag's value
                            PropertyBag.ShipMethodID = shipMethods.FirstOrDefault().ShipMethodID;
                            ExigoDAL.PropertyBags.Update(PropertyBag);
                        }
                    }

                    #endregion
                }

                return new JsonNetResult(new
                {
                    success = true,
                    shipMethods
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
        public ActionResult SetShipMethodID(int shipMethodID)
        {
            PropertyBag.ShipMethodID = shipMethodID;
            ExigoDAL.PropertyBags.Update(PropertyBag);

            return RedirectToAction("Review");
        }


        /////////////////////Used on the Mock Up Shopping Page//////////////////////////// 

        //[HttpPost]
        //public ActionResult SetShipMethod(int ShipMethodID, ShippingAddress ShippingAddress)
        //{
        //    try
        //    {
        //        // Update the Property Bag
        //        PropertyBag.ShipMethodID = ShipMethodID;
        //        PropertyBag.ShippingAddress = ShippingAddress;
        //        ExigoDAL.PropertyBags.Update(PropertyBag);

            //        // Recalculate the Order with the new Ship Method and Address
            //        var cartItems = ShoppingCart.Items.ToList();
            //        var orderTotals = new OrderCalculationResponse();
            //        // Calculate the order if applicable
            //        var orderItems = cartItems.Where(c => c.Type == ShoppingCartItemType.Order).ToList();
            //        if (orderItems.Count > 0)
            //        {
            //            #region Order Totals

            //            orderTotals = ExigoDAL.CalculateOrder(new OrderCalculationRequest
            //            {
            //                Configuration = OrderConfiguration,
            //                Items = orderItems,
            //                Address = ShippingAddress,
            //                ShipMethodID = ShipMethodID
            //            });

            //            #endregion
            //        }

            //        return new JsonNetResult(new
            //        {
            //            success = true,
            //            shipMethodDescription = orderTotals.Shipping.ToString("C"),
            //            totalDisplay = orderTotals.Total.ToString("C")
            //        });
            //    }
            //    catch (Exception ex)
            //    {
            //        return new JsonNetResult(new { success = false, message = ex.Message });
            //    }
            //}

        [HttpPost]
        public JsonNetResult SetNoReferred()
        {
            try
            {
                PropertyBag.SelectedDistributor = "NoReferred";
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

        #region Parties
        [Route("party/{partyID}/{guestID?}")]
        public ActionResult Party(int partyID, int guestID = 0)
        {
            // Validate our party first to ensure it is still Open and the Hostess Order is not yet created
            var service = new PartyService(Identity.Owner.CustomerID);
            var party = service.GetParties(new ExigoService.GetPartiesRequest { CheckForHostessOrder = true, PartyID = partyID, PartyStatusID = (int)PartyStatusTypes.Open }).FirstOrDefault();

            if (party == null || party.HostOrderIsPlaced)
            {
                return RedirectToAction("itemlist", new { message = "partyinvalid" });
            }
            else
            {
                PropertyBag.PartyID = partyID;

                if (guestID > 0)
                {
                    PropertyBag.GuestID = guestID;
                }

                ExigoDAL.PropertyBags.Update(PropertyBag);

                return RedirectToAction("itemlist");
            }
        }

        [HttpPost]
        public JsonNetResult PartySearch(PartySearchRequest request)
        {
            try
            {
                var results = new List<Party>();
                var ownerCustomerID = Identity.Owner.CustomerID;
                var service = new PartyService(Identity.Owner.CustomerID);

                // Add this filter to ensure no closed parties will be returned - Mike M.
                request.ShowOpenPartiesOnly = true;

                var parties = service.SearchParties(request).ToList();

                if (parties.Count > 0)
                {
                    results = parties;
                }

                var html = this.RenderPartialViewToString("Partials/_PartyHostModalResults", results);

                return new JsonNetResult(new
                {
                    success = true,
                    results,
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

        public JsonNetResult ChoosePartyID(int partyID)
        {
            try
            {
                PropertyBag.PartyID = partyID;
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


        public ActionResult ResetShopping()
        {
            ExigoDAL.PropertyBags.Delete(ShoppingCart);
            ExigoDAL.PropertyBags.Delete(PropertyBag);

            return RedirectToAction("itemlist");
        }
        #endregion

        #region CartPreview
        public ActionResult CartPreview()
        {
            try
            {
                var orderCartItems = ShoppingCart.Items.Where(x => x.Type == ShoppingCartItemType.Order);
                var autoOrderCartItems = ShoppingCart.Items.Where(x => x.Type == ShoppingCartItemType.AutoOrder);
                object orderCart = new { };
                object autoOrderCart = new { };
                var total = 0m;
                var count = 0m;
                if (orderCartItems.Any())
                {
                    var orderItems = ExigoDAL.GetItems(new ExigoService.GetItemsRequest() {
                        Configuration              = OrderConfiguration,
                        ShoppingCartItems          = orderCartItems,
                        IgnoreCategoryRestrictions = true,
                        LanguageID = this.CurrentLanguage.LanguageID
                    });

                    total += orderItems.Sum(i => i.Quantity * i.Price);
                    count += orderItems.Sum(x => x.Quantity);

                    orderCart = orderItems
                        .Select(x => new
                        {
                            Type = x.Type.ToString(),
                            x.ItemCode,
                            x.ItemDescription,
                            x.Quantity,
                            Price = x.Price.ToString("C"),
                            Img = x.LargeImageUrl ?? x.SmallImageUrl ?? x.TinyImageUrl,
                            Url = Url.Action("ItemDetail", "Shopping", new { itemcode = x.GroupMasterItemCode ?? x.ItemCode }),
                        }
                    );
                }

                if (autoOrderCartItems.Any())
                {
                    var autoOrderItems = ExigoDAL.GetItems(new ExigoService.GetItemsRequest()
                    {
                        Configuration = AutoOrderConfiguration,
                        ShoppingCartItems = autoOrderCartItems,
                        IgnoreCategoryRestrictions = true,
                        LanguageID = this.CurrentLanguage.LanguageID
                    });

                    total += autoOrderItems.Sum(i => i.Quantity * i.Price);
                    count += autoOrderItems.Sum(x => x.Quantity);

                    autoOrderCart = autoOrderItems
                        .Select(x => new
                        {
                            Type = x.Type.ToString(),
                            x.ItemCode,
                            x.ItemDescription,
                            x.Quantity,
                            Price = x.Price.ToString("C"),
                            Img = x.LargeImageUrl ?? x.SmallImageUrl ?? x.TinyImageUrl,
                            Url = Url.Action("ItemDetail", "Shopping", new { itemcode = x.GroupMasterItemCode ?? x.ItemCode }),
                        }
                    );
                }

                return new JsonNetResult(new
                {
                    success = true,
                    itemTotal = count,
                    OrdersShoppingCart = orderCart,
                    AutoOrdersShoppingCart = autoOrderCart,
                    hasOrder = orderCartItems.Any(),
                    hasAutoOrder = autoOrderCartItems.Any(),
                    cartEmpty = count == 0,
                    total = total
                });
            }
            catch (Exception ex)
            {
                return new JsonNetResult(new { success = false, message = ex.Message });
            }

        }
        #endregion
    }
}