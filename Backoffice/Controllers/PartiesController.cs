using Common.Services;
using System;
using System.Web.Mvc;
using ExigoService;
using Backoffice.ViewModels;
using System.Linq;
using Common;
using System.Threading.Tasks;
using System.Collections.Generic;
using Dapper;
using ExigoWeb.Kendo;
using Serilog;

namespace Backoffice.Controllers
{
    public class PartiesController : Controller
    {
        #region Properties
        public string OwnerWebAlias
        {
            get
            {
                if (_ownerWebAlias == null)
                {
                    _ownerWebAlias = GlobalUtilities.GetWebAlias(Identity.Current.CustomerID);
                }

                return _ownerWebAlias;
            }
        }
        private string _ownerWebAlias;
        public PartyService PartyService
        {
            get
            {
                if (_partyService == null)
                {
                    _partyService = new PartyService(OwnerWebAlias, Identity.Current.CustomerID);
                }

                return _partyService;
            }
        }
        private PartyService _partyService;
        #endregion

        #region Party Create/Edit
        [Route("createparty")]
        public ActionResult CreateParty(int partyID = 0)
        {
            var model = new CreatePartyViewModel();
            var service = PartyService;


            model.ParentPartyID = partyID;

            if (partyID > 0)
            {
                var parentParty = service.GetParties(new GetPartiesRequest { PartyID = partyID }).FirstOrDefault();
                if (parentParty != null)
                {
                    model.ParentPartyDescription = parentParty.Description;
                }
            }

            return View(model);
        }

        [HttpPost]
        [Route("createparty")]
        public JsonNetResult CreateParty(CreatePartyRequest request)
        {
            var service = PartyService;

            try
            {
                // Validate if email is already taken, if so return the matching customer for the user
                if (request.HostEmail.IsNotNullOrEmpty() && request.HostID == 0)
                {
                    var emailMatches = ExigoDAL.GetCustomersFromEmail(request.HostEmail);

                    // We found some matches, so we need to ensure that the customer can choose a host from the results
                    if (emailMatches.Count() > 0)
                    {
                        return new JsonNetResult(new
                        {
                            success = false,
                            message = "email taken",
                            error = Resources.Common.EmailAlreadyInUse
                        });
                    }
                }

                // Make sure we either have a Host ID or the required Host fields are filled out in full
                if (request.HostID == 0 && request.HostFirstName.IsNullOrEmpty() && request.HostLastName.IsNullOrEmpty())
                {
                    throw new Exception("This party can't be created until you select a Hostess. Either find an existing Hostess with the Hostess Search or Create a new Hostess in the Choose a Hostess section.");
                }

                var customerID = Identity.Current.CustomerID;
                request.CustomerID = customerID;

                // We are going to send this password out later, either by pulling their current password or generating a new one for the user
                var password = "";

                // If we have a new host, we create them here
                var hostID = request.HostID;
                if (hostID == 0)
                {
                    password = System.Web.Security.Membership.GeneratePassword(8, 2);

                    var createCustomerRequest = new Common.Api.ExigoWebService.CreateCustomerRequest
                    {
                        FirstName = request.HostFirstName,
                        LastName = request.HostLastName,
                        MainAddress1 = request.HostAddress1,
                        MainAddress2 = request.HostAddress2,
                        MainCity = request.HostCity,
                        MainState = request.HostState,
                        MainCountry = request.HostCountry,
                        MainZip = request.HostZip,
                        Phone = request.HostPhone,
                        CustomerStatus = CustomerStatusTypes.Active,
                        CustomerType = (int)CustomerTypes.RetailCustomer,
                        InsertEnrollerTree = true,
                        EnrollerID = Identity.Current.CustomerID,
                        InsertUnilevelTree = true,
                        SponsorID = Identity.Current.CustomerID
                    };

                    if (!request.HostEmail.IsNullOrEmpty())
                    {
                        createCustomerRequest.CanLogin = true;
                        createCustomerRequest.LoginName = request.HostEmail;
                        createCustomerRequest.LoginPassword = password;
                        createCustomerRequest.Email = request.HostEmail;
                    }

                    var customer = ExigoDAL.WebService().CreateCustomer(createCustomerRequest);

                    hostID = customer.CustomerID;
                    request.HostID = hostID;
                }
                else
                {
                    password = ExigoDAL.GetCustomerPassword(hostID);

                    // If for some reason the Host's password is empty, they may not have been set up in the traditional form. In this rare case, we need to create a new login and password for them.
                    if (password.IsNullOrEmpty() && Settings.GeneratePasswordForExistingHostess)
                    {
                        var hostFirstName = "";
                        var hostLastName = "";

                        if (!request.HostFirstName.IsNullOrEmpty())
                        {
                            hostFirstName = request.HostFirstName;
                            hostLastName = request.HostLastName;
                        }
                        else
                        {
                            var customer = Customers.GetCustomer(request.HostID);

                            hostFirstName = customer.FirstName;
                            hostLastName = customer.LastName;
                        }

                        var newUserName = (hostFirstName + "_" + hostLastName + hostID).Replace(" ", "");
                        password = System.Web.Security.Membership.GeneratePassword(8, 2);

                        ExigoDAL.UpdateCustomer(new Common.Api.ExigoWebService.UpdateCustomerRequest { CustomerID = hostID, LoginName = newUserName, LoginPassword = password, CanLogin = true });
                    }
                }


                // Create the party
                var party = service.CreateParty(request);

                var hostEmail = !request.HostEmail.IsNullOrEmpty() ? request.HostEmail : ExigoDAL.GetCustomerEmail(hostID);


                // Now lets send out our Hostess Email
                if (!hostEmail.IsNullOrEmpty())
                {
                    var webAlias = ExigoDAL.GetCustomerSite(Identity.Current.CustomerID).WebAlias;

                    SendHostessEmail(party, webAlias, hostEmail, password);
                }

                // Return the result
                return new JsonNetResult(new
                {
                    success = true,
                    party = party,
                    partyID = party.PartyID
                });
            }
            catch (Exception ex)
            {
                return new JsonNetResult(new
                {
                    success = false,
                    error = ex.Message,
                    request = request
                });
            }
        }

        [Route("editparty")]
        public ActionResult EditParty(int partyID = 0)
        {
            var model = new CreatePartyViewModel();
            var service = PartyService;

            model.CreatePartyRequest = service.GetCreatePartyRequest(partyID, Identity.Current.CustomerID);

            model.PartyID = partyID;

            if (model.CreatePartyRequest.Address1.IsNullOrEmpty() && model.CreatePartyRequest.Zip.IsNullOrEmpty())
            {
                model.CreatePartyRequest.IsVirtualOnly = true;
            }

            return View(model);
        }

        [HttpPost]
        [Route("editparty")]
        public JsonNetResult EditParty(CreatePartyRequest request)
        {
            var service = PartyService;

            try
            {
                // Validate if email is already taken, if so return the matching customer for the user
                if (request.HostEmail.IsNotNullOrEmpty() && request.HostID == 0)
                {
                    var emailMatches = ExigoDAL.GetCustomersFromEmail(request.HostEmail);

                    // We found some matches, so we need to ensure that the customer can choose a host from the results
                    if (emailMatches.Count() > 0)
                    {
                        return new JsonNetResult(new
                        {
                            success = false,
                            message = "email taken",
                            error = Resources.Common.EmailAlreadyInUse
                        });
                    }
                }

                // Make sure we either have a Host ID or the required Host fields are filled out in full
                if (request.HostID == 0 && request.HostFirstName.IsNullOrEmpty() && request.HostLastName.IsNullOrEmpty() && request.HostAddress1.IsNullOrEmpty() && request.City.IsNullOrEmpty())
                {
                    throw new Exception("This party can't be created until you select a Hostess. Either find an existing Hostess with the Hostess Search or Create a new Hostess in the Choose a Hostess section.");
                }

                var customerID = Identity.Current.CustomerID;
                request.CustomerID = customerID;
                var hasHostChanged = false;
                var email = request.HostEmail;
                var loginName = "";
                var password = request.HostID > 0 ? ExigoDAL.GetCustomerPassword(request.HostID) : System.Web.Security.Membership.GeneratePassword(8, 2);

                var currentPartyHostID = service.GetParties(new GetPartiesRequest { PartyID = request.PartyID }).FirstOrDefault().HostID;

                // If we have a new host, we create them here
                var hostID = request.HostID;
                if (hostID == 0)
                {
                    hasHostChanged = true;
                    loginName = request.HostEmail;

                    var createCustomerRequest = new Common.Api.ExigoWebService.CreateCustomerRequest
                    {
                        FirstName = request.HostFirstName,
                        LastName = request.HostLastName,
                        MainAddress1 = request.HostAddress1,
                        MainAddress2 = request.HostAddress2,
                        MainCity = request.HostCity,
                        MainState = request.HostState,
                        MainCountry = request.HostCountry,
                        MainZip = request.HostZip,
                        Phone = request.HostPhone,
                        CustomerStatus = CustomerStatusTypes.Active,
                        CustomerType = (int)CustomerTypes.RetailCustomer,
                        InsertEnrollerTree = true,
                        EnrollerID = Identity.Current.CustomerID,
                        InsertUnilevelTree = true,
                        SponsorID = Identity.Current.CustomerID
                    };

                    if (request.HostEmail.IsNotNullOrEmpty())
                    {
                        email = request.HostEmail;
                        createCustomerRequest.Email = request.HostEmail;
                        createCustomerRequest.CanLogin = true;
                        createCustomerRequest.LoginName = request.HostEmail;
                        createCustomerRequest.LoginPassword = password;
                    }

                    var customer = ExigoDAL.WebService().CreateCustomer(createCustomerRequest);

                    hostID = customer.CustomerID;
                    request.HostID = hostID;
                }
                else
                {
                    var customer = ExigoDAL.GetCustomers(new List<int> { request.HostID }).FirstOrDefault();
                    loginName = customer.LoginName;
                    email = customer.Email;

                    // We need to ensure there is a login name, else we generate one. This will only change if the email is present.
                    if (loginName.IsNullOrEmpty() && email.IsNotNullOrEmpty())
                    {
                        loginName = (customer.FirstName + "_" + customer.LastName + hostID).Replace(" ", "");

                        ExigoDAL.UpdateCustomer(new Common.Api.ExigoWebService.UpdateCustomerRequest
                        {
                            CustomerID = hostID,
                            LoginName = loginName,
                            LoginPassword = password,
                            CanLogin = true
                        });
                    }

                    if (hostID != currentPartyHostID && email.IsNotNullOrEmpty())
                    {
                        hasHostChanged = true;
                    }
                }

                // Update the party
                var party = service.UpdateParty(request);

                if (hasHostChanged && email.IsNotNullOrEmpty())
                {
                    SendHostessEmail(party, OwnerWebAlias, email, password);
                }

                // Return the result
                return new JsonNetResult(new
                {
                    success = true,
                    party = party,
                    partyID = party.PartyID
                });
            }
            catch (Exception ex)
            {
                return new JsonNetResult(new
                {
                    success = false,
                    error = ex.Message,
                    request = request
                });
            }
        }
        #endregion

        #region Party Dashboard
        [Route("parties")]
        public ActionResult Index()
        {
            var model = new PartyDashboardViewModel();
            var service = PartyService;
            var customerID = Identity.Current.CustomerID;
            var isHostess = Identity.Current.IsHostess();

            var getPartiesRequest = new GetPartiesRequest { IncludeHostessDetails = true };

            if (isHostess)
            {
                getPartiesRequest.HostessID = customerID;
            }
            else
            {
                getPartiesRequest.CustomerID = customerID;
            }

            model.Parties = service.GetParties(getPartiesRequest);

            if (!isHostess)
            {

                model.WeeklySalesTotals = service.GetSalesTotals(model.Parties, customerID);
                model.MonthlyTotalSales = model.WeeklySalesTotals.Sum(c => c.Value);
            }

            return View(model);
        }

        [Route("open-parties")]
        public ActionResult OpenParties()
        {
            var model = new PartyListViewModel(PartyStatusTypes.Open);
            var service = PartyService;
            var customerID = Identity.Current.CustomerID;

            try
            {
                var getPartiesRequest = new ExigoService.GetPartiesRequest
                {
                    CheckForHostessOrder = true,
                    IncludeHostessDetails = true,
                    PartyStatusID = (int)PartyStatusTypes.Open,
                    IncludeHostessRewards = true,
                    IncludeOwnerDetails = true
                };

                if (Identity.Current.IsHostess())
                {
                    getPartiesRequest.HostessID = customerID;
                }
                else
                {
                    getPartiesRequest.CustomerID = customerID;
                }

                var parties = service.GetParties(getPartiesRequest);

                model.Parties.AddRange(parties);
            }
            catch { }


            return View("PartiesList", model);
        }

        [Route("closed-parties")]
        public ActionResult ClosedParties()
        {
            var model = new PartyListViewModel(PartyStatusTypes.Closed);
            var service = PartyService;
            var customerID = Identity.Current.CustomerID;


            try
            {
                var getPartiesRequest = new ExigoService.GetPartiesRequest { IncludeHostessDetails = true, IncludeHostessRewards = true, IncludeOwnerDetails = true, PartyStatusID = (int)PartyStatusTypes.Closed };

                if (Identity.Current.IsHostess())
                {
                    getPartiesRequest.HostessID = customerID;
                }
                else
                {
                    getPartiesRequest.CustomerID = customerID;
                }

                var parties = service.GetParties(getPartiesRequest);

                model.Parties.AddRange(parties.Where(c => c.EventEnd < DateTime.Now.BeginningOfDay() || c.PartyStatusType == (int)PartyStatusTypes.Closed).OrderByDescending(c => c.PartyID));
            }
            catch { }

            return View("PartiesList", model);
        }

        [Route("summary/{id}")]
        public ActionResult Summary(int id)
        {
            var service = PartyService;
            var model = new PartySummaryViewModel();
            var customerID = Identity.Current.CustomerID;


            var getPartiesRequest = new ExigoService.GetPartiesRequest
            {
                IncludeHostessDetails = true,
                PartyID = id,
                IncludeHostessRewards = true,
                IncludeOwnerDetails = true
            };

            if (Identity.Current.IsHostess())
            {
                getPartiesRequest.HostessID = customerID;
            }
            else
            {
                getPartiesRequest.CustomerID = customerID;
            }

            model.Party = service.GetParties(getPartiesRequest).FirstOrDefault();


            // Get the order info
            var orders = ExigoDAL.GetCustomerOrders(new GetCustomerOrdersRequest { PartyID = id, IncludePayments = true }).ToList();
            // Filter out the cancelled orders
            model.Orders = orders.Where(o => o.OrderStatusID != (int)OrderStatuses.Cancelled).ToList();
            model.OrderCount = orders.Count();
            model.OrderTotal = orders.Sum(c => c.Total);

            // Get party guests
            var guests = service.GetPartyGuests(id, validateHasAttendedParty: true);
            model.Guests = guests;

            // If we have a booked party this party came from, lets pull that parties information so we can display for the user
            if (model.Party.BookingPartyID > 0)
            {
                var bookedParentParty = service.GetParties(new GetPartiesRequest { PartyID = model.Party.BookingPartyID, IncludeHostessDetails = true }).FirstOrDefault();

                if (bookedParentParty != null)
                {
                    model.ParentBookingParty = bookedParentParty;
                }
            }

            return View(model);
        }
        #endregion

        #region Guests
        [Route("newguest/{partyID}")]
        public ActionResult NewGuest(int partyID)
        {
            var model = new ManageGuestsViewModel(partyID);
            var service = new PartyService(Identity.Current.CustomerID);
            var party = service.GetParties(new GetPartiesRequest { CustomerID = Identity.Current.CustomerID, PartyID = partyID }).FirstOrDefault();
            model.Party = party;
            model.HostessID = party.HostID;

            return View(model);
        }

        public ActionResult CreateNewGuest(ManageGuestsViewModel model)
        {
            try
            {
                var service = new PartyService(Identity.Current.CustomerID);

                // Validate our email address just to make sure it is not taken already
                var isHostessEmail = ExigoDAL.IsEmailCustomersEmail(model.Guest.Email, model.HostessID);

                if (!isHostessEmail)
                {
                    var guest = model.Guest;


                    // Add a few values we will need to associate the guest correctly
                    guest.CreatedByID = Identity.Current.CustomerID;
                    guest.HostID = Identity.Current.CustomerID;
                    guest.CreatedDate = DateTime.Now;
                    guest.PartyID = model.PartyID;

                    service.CreateGuest(guest);
                    return RedirectToAction("Summary", new { id = model.PartyID, msg = "guestcreated" });
                }
                else
                {
                    // Can't use Hostess Email to create guest
                    throw new Exception("You can't use the Party Hostess email for new guests.");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("newguest", new { partyID = model.PartyID, error = ex.Message });
            }
        }

        //[Route("existingguests/{partyID}")]
        //public ActionResult ExistingGuestList(int partyID, KendoGridRequest request = null)
        //{
        //    if (Request.HttpMethod.ToUpper() == "GET")
        //    {
        //        return View(partyID);
        //    }
        //    else
        //    {
        //        var sortByClause = KendoUtilitiesCustom.GetSqlOrderByClause(request.SortObjects, new SortObject("CreatedDate", "desc"));
        //        var whereClause = KendoUtilitiesCustom.GetSqlWhereClause(request.FilterObjectWrapper.FilterObjects);
        //        dynamic guestList = null;

        //        var customerTypes = new List<int> { (int)CustomerTypes.RetailCustomer };
        //            // TEMP: Get our current Party Guests and filter them out of the list, this should be done within the SQL but is waiting on the Party Guests and Guests to be added to Replicated SQL
        //            var service = new PartyService(Identity.Current.CustomerID);
        //            var currentGuests = service.GetPartyGuests(partyID).Where(g => g.CustomerID > 0);
        //            var additionalWhereClause = currentGuests.Count() > 0 ? " and c.CustomerID not in @existingCustomerIDs" : "";
        //            var existingCustomerIDs = new List<int>();

        //            if (currentGuests.Count() > 0)
        //            {
        //                existingCustomerIDs.AddRange(currentGuests.Select(c => c.CustomerID));
        //            }
        //            try
        //            {
        //                using (var context = Exigo.Sql())
        //                {
        //                    guestList = context.Query(@"                    
        //                    Declare @customer TABLE(
        //                    CustomerID int)

        //                    INSERT @customer
        //                    SELECT customerid 
        //                    FROM   customers 
        //                    WHERE  enrollerid = @CustomerID 
        //                    and customertypeid = 1
        //                    -- Personally Enrolled 

        //                    INSERT @customer
        //                    SELECT po.customerid 
        //                    -- Anyone who shopped at my party 
        //                    FROM   parties p 
        //                            JOIN orders po 
        //                            ON p.partyid = po.partyid 
        //                    WHERE  @CustomerID = distributorid 

        //                    INSERT @customer
        //                    SELECT customerid 
        //                    FROM   orders 
        //                    WHERE  other12 = Cast(@CustomerID AS VARCHAR(200))


        //                    SELECT CustomerID = c.customerid, 
        //                            FirstName = c.firstname, 
        //                            LastName = c.lastname,
        //                            CreatedDate = Cast(c.createddate AS DATE), 
        //                            Email = c.email, 
        //                            EmailOptIn = CASE 
        //                                            WHEN c.field2 = '1' THEN 'true' 
        //                                            WHEN c.field2 = '0' THEN 'false' 
        //                                            ELSE 'false' 
        //                                        END, 
        //                            MainAddress1 = c.mainaddress1 + ' ' + c.mainaddress2, 
        //                            MainCity = c.maincity, 
        //                            MainState =  c.mainstate, 
        //                            MainZip = c.mainzip, 
        //                            MainCountry = c.maincountry, 
        //                            Phone = c.phone, 
        //                            MobilePhone =  c.mobilephone 
        //                    FROM   customers c 
        //                    WHERE  c.customerid IN(SELECT DISTINCT customerid from @customer) 
        //                    " + whereClause + @"
        //                    " + additionalWhereClause + @"
        //                    Order By
        //                         " + sortByClause + @"
        //                        OFFSET @skip ROWS
        //                        FETCH NEXT @take ROWS ONLY
        //                    ", new
        //                    {
        //                        existingCustomerIDs = existingCustomerIDs,
        //                        skip = request.Skip,
        //                        take = request.Take,
        //                        CustomerID = Identity.Current.CustomerID
        //                     });
        //                }

        //                if (request.Total == 0 || request.FilterObjectWrapper.FilterObjects.Count() > 0)
        //                {
        //                    using (var context = Exigo.Sql())
        //                    {
        //                        request.Total = context.Query<int>(@"
        //                        Declare @customer TABLE(CustomerID int)

        //                        INSERT @customer
        //                        SELECT customerid 
        //                        FROM   customers 
        //                        WHERE  enrollerid = @customerid 
        //                        and customertypeid = 1

        //                        INSERT @customer
        //                        SELECT po.customerid 
        //                        -- Anyone who shopped at my party 
        //                        FROM   parties p 
        //                                JOIN orders po 
        //                                ON p.partyid = po.partyid 
        //                        WHERE  @customerid = distributorid 

        //                        INSERT @customer
        //                        SELECT customerid 
        //                        FROM   orders 
        //                        WHERE  other12 = Cast(@customerid AS VARCHAR(200))
        //                        select Count(*) from
        //                        (
        //                        SELECT CustomerID = c.customerid, 
        //                         FirstName = c.firstname, 
        //                         LastName = c.lastname, 
        //                         CreatedDate = Cast(c.createddate AS DATE), 
        //                         Email = c.email, 
        //                         EmailOptIn = CASE 
        //                             WHEN c.field2 = '1' THEN 'true' 
        //                             WHEN c.field2 = '0' THEN 'false' 
        //                             ELSE 'false' 
        //                            END, 
        //                         MainAddress1 = c.mainaddress1 + ' ' + c.mainaddress2, 
        //                         MainCity = c.maincity, 
        //                         MainState = c.mainstate, 
        //                         MainZip = c.mainzip, 
        //                         MainCountry = c.maincountry, 
        //                         Phone = c.phone, 
        //                         MobilePhone = c.mobilephone 
        //                        FROM Customers c
        //                        WHERE c.CustomerID in (SELECT DISTINCT CustomerID from @customer) 
        //                        " + whereClause + @"
        //                        " + additionalWhereClause + @"
        //                        ) as report
        //                        ", new
        //                        {
        //                            existingCustomerIDs = existingCustomerIDs,
        //                            CustomerID = Identity.Current.CustomerID
        //                        }).FirstOrDefault();
        //                    }
        //                }

        //                // If Additional Logic needs to be applied to the results, use the following..
        //                var results = new List<dynamic>();
        //                foreach (var item in guestList)
        //                {
        //                    var newItem = item as IDictionary<String, object>;

        //                    var value = newItem["CustomerID"];
        //                    var token = Security.Encrypt(value, Identity.Current.CustomerID);

        //                    newItem = DynamicHelper.AddProperty(item, "CustomerID" + "Token", token);


        //                    results.Add(newItem);
        //                }
        //                guestList = results;

        //                return new JsonNetResult(new
        //                {
        //                    data = guestList,
        //                    total = request.Total
        //                });

        //            }
        //            catch (Exception exception)
        //            {
        //                return new JsonNetResult(new
        //                {
        //                    success = false,
        //                    message = exception.Message
        //                });
        //            }
        //    }
        //}

        #region OLD Existing Guest List
        [Route("existingguests/{partyID}")]
        public ActionResult ExistingGuestList(int partyID, KendoGridRequest request = null)
        {
            if (Request.HttpMethod.ToUpper() == "GET")
            {
                return View(partyID);
            }
            else
            {
                var customerTypes = new List<int> { (int)CustomerTypes.RetailCustomer };
                using (var context = new KendoGridDataContext(ExigoDAL.Sql()))
                {
                    // TEMP: Get our current Party Guests and filter them out of the list, this should be done within the SQL but is waiting on the Party Guests and Guests to be added to Replicated SQL
                    var service = new PartyService(Identity.Current.CustomerID);
                    var currentGuests = service.GetPartyGuests(partyID).Where(g => g.CustomerID > 0);
                    var additionalWhereClause = currentGuests.Count() > 0 ? " and ed.CustomerID not in @existingCustomerIDs" : "";
                    var existingCustomerIDs = new List<int>();

                    if (currentGuests.Count() > 0)
                    {
                        existingCustomerIDs.AddRange(currentGuests.Select(c => c.CustomerID));
                    }

                    return context.Query<RetailCustomerViewModel>(request, @"
                        select 
	                        CustomerID = c.CustomerID,
	                        FirstName  = c.FirstName,
	                        LastName    = c.LastName,
                            Name = c.FirstName + ' ' + c.LastName,
	                        CreatedDate     = CAST(CreatedDate as date),
	                        Email           = c.Email,
	                        Phone           = c.Phone

                        from EnrollerDownline ed
                        inner join Customers c 
	                        on c.CustomerID = ed.CustomerID
                            and c.CustomerTypeID in @customerTypes

                        where ed.DownlineCustomerID = @customerID
                        " + additionalWhereClause, new
                    {
                        customerTypes = customerTypes,
                        customerID = Identity.Current.CustomerID,
                        existingCustomerIDs = existingCustomerIDs
                    });

                }
            }
        }
        #endregion End OLD Existing Guest List

        [HttpPost]
        public JsonNetResult AddExistingGuest(string customerToken, int partyID)
        {
            try
            {
                var customerID = Convert.ToInt32(Security.Decrypt(customerToken, Identity.Current.CustomerID));
                var service = new PartyService(Identity.Current.CustomerID);

                var guest = service.CreateGuest(customerID, partyID);

                return new JsonNetResult(new
                {
                    success = true,
                    message = guest.FullName + " has been added to your party."
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

        [Route("editguest/{partyID}/{guestID}")]
        public ActionResult EditGuest(int partyID, int guestID)
        {
            var model = new ManageGuestsViewModel(partyID);

            var service = new PartyService(Identity.Current.CustomerID);
            model.HostessID = service.GetPartyHostID(partyID);
            model.Guest = service.GetPartyGuests(guestID: guestID).FirstOrDefault();


            return View(model);
        }

        public ActionResult UpdateGuest(ManageGuestsViewModel model)
        {
            try
            {
                // Validate our email address just to make sure it is not taken already
                var isHostessEmail = ExigoDAL.IsEmailCustomersEmail(model.Guest.Email, model.HostessID);

                if (!isHostessEmail)
                {
                    var guest = model.Guest;
                    var service = new PartyService(Identity.Current.CustomerID);

                    // Add a few values we will need to associate the guest correctly
                    guest.CreatedByID = Identity.Current.CustomerID;


                    service.UpdateGuest(guest);

                    return RedirectToAction("summary", new { id = model.PartyID, msg = "guestupdated" });

                }
                else
                {
                    // Can't use Hostess Email to create guest
                    throw new Exception("You can't use the Party Hostess email for new guests.");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("editguest", new { partyID = model.PartyID, guestID = model.Guest.GuestID, error = ex.Message });
            }
        }

        [HttpPost]
        public JsonNetResult DeleteGuest(int guestID, int partyID)
        {
            try
            {
                if (guestID > 0)
                {
                    var service = new PartyService(Identity.Current.CustomerID);

                    service.RemoveGuestFromParty(guestID, partyID);

                    return new JsonNetResult(new
                    {
                        success = true
                    });
                }
                else
                {
                    throw new Exception("No Guest ID provided GuestID: {0}".FormatWith(guestID));
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

        #region Guest/Hostess Invitations
        [HttpPost]
        public JsonNetResult GetInviteModal(int partyID)
        {
            try
            {
                var customerID = Identity.Current.CustomerID;
                var service = new PartyService(customerID);

                var getPartiesRequest = new ExigoService.GetPartiesRequest { PartyID = partyID };

                if (Identity.Current.IsHostess())
                {
                    getPartiesRequest.HostessID = customerID;
                }
                else
                {
                    getPartiesRequest.CustomerID = customerID;
                }

                var party = service.GetParties(getPartiesRequest).FirstOrDefault();
                var model = new PartyInviteViewModel(party);

                model.Guests = service.GetPartyGuests(partyID);

                var html = this.RenderPartialViewToString("partials/invitemodal", model);

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

        [HttpPost]
        public JsonNetResult SendPartyInvitations(int partyID)
        {
            try
            {
                var customerID = Identity.Current.CustomerID;
                var service = new PartyService(customerID);
                var getPartiesRequest = new ExigoService.GetPartiesRequest { PartyID = partyID };

                if (Identity.Current.IsHostess())
                {
                    getPartiesRequest.HostessID = customerID;
                }
                else
                {
                    getPartiesRequest.CustomerID = customerID;
                }

                var party = service.GetParties(getPartiesRequest).FirstOrDefault();

                var webAlias = ExigoDAL.GetCustomerSite(party.CustomerID).WebAlias;
                var inviteResponse = SendPartyInvitations(partyID, webAlias);

                if (inviteResponse.FailedEmails.Count > 0)
                {
                    var exceptionContent = string.Join(", ", inviteResponse.FailedEmails);

                    throw new Exception("Some of the guests you attempted to invite had problems sending the email: <br /> {0}".FormatWith(exceptionContent));
                }

                var model = new PartySummaryViewModel();
                model.Guests = service.GetPartyGuests(partyID);
                model.Party = new Party(partyID);

                var html = this.RenderPartialViewToString("partials/partyguesttable", model);

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

        /// <summary>
        /// Send the Party Invite emails to the Guests that have yet to be invited. This also updates their Guest record with today's date.
        /// </summary>
        /// <param name="partyID">Party ID to pull in all the appropriate Guests for the emails.</param>
        /// <param name="ownerWebAlias">Party creator's web alias for formatting the RSVP url.</param>
        /// <returns></returns>
        public SendPartyInvitationResponse SendPartyInvitations(int partyID, string ownerWebAlias)
        {
            var tasks = new List<Task>();
            var response = new SendPartyInvitationResponse(partyID);
            var service = new PartyService(Identity.Current.CustomerID);

            var party = service.GetParties(new ExigoService.GetPartiesRequest { PartyID = partyID, IncludeHostessDetails = true }).FirstOrDefault();
            var guests = service.GetPartyGuests(partyID).Where(g => g.AllowEmail && !g.HasSentInvite && g.Email.IsNotNullOrEmpty());

            if (guests.Count() > 0)
            {
                foreach (var guest in guests)
                {
                    var email = guest.Email;
                    var emailResponse = SendInviteEmail(party, ownerWebAlias, email, guest, partyID);

                    if (!emailResponse)
                    {
                        response.FailedEmails.Add(email);
                    }
                }
            }

            return response;
        }

        public bool SendInviteEmail(Party party, string ownerWebAlias, string email, Guest guest, int partyID)
        {
            var success = false;

            try
            {
                string encryptedValues = Security.Encrypt(new
                {
                    GuestID = guest.GuestID,
                    PartyID = partyID
                });

                var rsvpUrl = GlobalSettings.ReplicatedSites.GetPartyRSVPUrl(ownerWebAlias, partyID) + "?token=" + encryptedValues;

                // Get Host Email address
                var host = Customers.GetCustomer(party.HostID);

                var model = new RSVPModel(partyID);
                model.Party = party;
                model.Guest = guest;
                model.PartyRSVPUrl = rsvpUrl;
                model.OwnerWebAlias = ownerWebAlias;

                model.HostEmail = host.Email;
                model.HostPhone = host.PrimaryPhone;

                var body = this.RenderPartialViewToString("partials/rsvpemail", model);

                // Send the email
                ExigoDAL.SendEmail(new SendEmailRequest
                {
                    To = new[] { email },
                    From = GlobalSettings.Emails.NoReplyEmail,
                    ReplyTo = new[] { GlobalSettings.Emails.NoReplyEmail },
                    Subject = "{0} - You are invited!".FormatWith(guest.FullName),
                    Body = body
                });

                // The email has been sent, so now we need to update their Guest record to ensure they are marked as Invitation Sent
                ExigoDAL.WebService().UpdateGuest(new Common.Api.ExigoWebService.UpdateGuestRequest { GuestID = guest.GuestID, Field2 = DateTime.Now.ToLongDateString() });

                success = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error Sending Party Invite Email: {Message}", ex.Message);
            }

            return success;
        }

        /// <summary>
        /// Send the Party Hostess Email. If they are a new Customer as well, it will contain their password to log into the back office.
        /// </summary>
        /// <param name="partyID"></param>
        /// <param name="ownerWebAlias"></param>
        /// <returns></returns>
        public SendPartyInvitationResponse SendHostessEmail(Party party, string ownerWebAlias, string email, string password)
        {
            var response = new SendPartyInvitationResponse(party.PartyID);
            var service = new PartyService(Identity.Current.CustomerID);

            try
            {
                var model = new HostessEmailModel(party.PartyID);
                model.Party = party;
                model.Email = email;
                model.Password = password;

                var token = Security.Encrypt("{0}|{1}".FormatWith(party.HostID, DateTime.Now.AddHours(1)), GlobalSettings.EncryptionKeys.SilentLogins.Key);
                model.HostessLoginUrl = GlobalSettings.Backoffices.SilentLogins.DistributorBackofficeUrl.FormatWith(token);

                // Get the Replicated Site home page URL
                if (ownerWebAlias.IsNullOrEmpty())
                {
                    ownerWebAlias = GlobalSettings.ReplicatedSites.DefaultWebAlias;
                }

                model.ReplicatedSiteHomePage = GlobalSettings.ReplicatedSites.FormattedBaseUrl.FormatWith(ownerWebAlias);

                var body = this.RenderPartialViewToString("hostessinviteemail", model);

                // Send the email
                ExigoDAL.SendEmail(new SendEmailRequest
                {
                    To = new[] { email },
                    From = GlobalSettings.Emails.NoReplyEmail,
                    ReplyTo = new[] { GlobalSettings.Emails.NoReplyEmail },
                    UseExigoApi = true,
                    Subject = "{0} - You are invited!".FormatWith(party.HostName),
                    Body = body
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to send hostess email: {Message}", ex.Message);
                response.FailedEmails.Add(email);
            }

            return response;
        }

        [Route("hostess-email")]
        public ActionResult HostessInviteEmail(Party party, string email, string password)
        {
            // TESTING
            //var service = new PartyService(Identity.Current.CustomerID);
            //party = service.GetParties(new GetPartiesRequest { PartyID = 10, IncludeHostessDetails = true }).FirstOrDefault();
            //email = "testman@mailinator.com";
            //password = "30u2h3lksgjo";

            var model = new HostessEmailModel(party.PartyID);
            model.Party = party;
            model.Email = email;
            model.Password = password;

            // Get the Replicated Site home page URL
            model.ReplicatedSiteHomePage = GlobalSettings.ReplicatedSites.FormattedBaseUrl.FormatWith("test");

            // Assemble our silent login link
            var token = Security.Encrypt("{0}|{1}".FormatWith(party.HostID, DateTime.Now.AddHours(1)));
            model.HostessLoginUrl = GlobalSettings.Backoffices.SilentLogins.DistributorBackofficeUrl.FormatWith(token);

            return View(model);
        }
        #endregion

        #region Ajax
        // Ajax Calls
        public JsonNetResult GetParties(int hostID = 0)
        {
            try
            {
                var service = PartyService;
                hostID = (hostID == 0) ? Identity.Current.CustomerID : hostID;

                var parties = service.GetParties(new ExigoService.GetPartiesRequest { CustomerID = Identity.Current.CustomerID, HostessID = hostID });

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

        public JsonNetResult GetPotentialHostesses(string query = "")
        {
            try
            {
                using (var context = ExigoDAL.Sql())
                {
                    var data = context.Query<Host>(@"
                            SELECT
                                CustomerID = c.CustomerID 
                                ,FirstName = c.FirstName 
                                ,LastName = c.LastName 
                                ,CreatedDate = c.CreatedDate 
                                ,c.Phone
                                ,c.MobilePhone
                                ,Email = c.Email 
                                ,Address = c.MainAddress1
                                ,Address2 = c.MainAddress2 
                                ,City = c.MainCity 
                                ,State = c.MainState 
                                ,Zip = c.MainZip 
                                ,Country = c.MainCountry
                            FROM Customers c
                            WHERE c.EnrollerID = @ownerCustomerID 
                                --AND c.CustomerTypeID = @customertypeid
                                AND (c.FirstName LIKE @queryValue OR c.LastName LIKE @queryValue OR c.FirstName + ' ' + c.LastName LIKE @queryValue)
                            ", new
                    {
                        queryValue = "%" + query + "%",
                        customertypeid = CustomerTypes.Distributor,
                        ownerCustomerID = Identity.Current.CustomerID
                    }).ToList();

                    return new JsonNetResult(new
                    {
                        success = true,
                        hosts = data
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

        public ActionResult GetClosePartyModal(int partyID)
        {
            try
            {
                var service = new PartyService(Identity.Current.CustomerID);
                var party = service.GetParties(new ExigoService.GetPartiesRequest { PartyID = partyID, IncludeHostessDetails = true }).FirstOrDefault();

                var html = this.RenderPartialViewToString("Partials/CancelPartyModal", party);

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

        public ActionResult CloseParty(int partyid)
        {
            try
            {
                var service = PartyService;

                service.CloseParty(partyid);

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

        [HttpPost]
        public JsonNetResult CancelOrder(int orderID)
        {
            try
            {
                if (orderID > 0)
                {
                    ExigoDAL.WebService().ChangeOrderStatus(new Common.Api.ExigoWebService.ChangeOrderStatusRequest { OrderID = orderID, OrderStatus = Common.Api.ExigoWebService.OrderStatusType.Canceled });

                    return new JsonNetResult(new
                    {
                        success = true
                    });
                }
                else
                {
                    throw new Exception("No Order ID provided {0}".FormatWith(orderID));
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

        // A 'virtual' order is created to mark a customer as having attended a Party, this will also create a customer account and update the guest record if one does not exist already
        [HttpPost]
        public JsonNetResult CreateAttendanceRecord(int partyID, int guestID)
        {
            try
            {
                Common.Api.ExigoWebService.CreateOrderResponse createOrderResponse = new Common.Api.ExigoWebService.CreateOrderResponse();
                var attendanceItemCode = "ATT-PTY";

                var guest = PartyService.GetPartyGuests(partyID, guestID, true, true).FirstOrDefault();

                if (guest == null)
                {
                    throw new Exception("Guest could not be found");
                }



                // Set up our Create Order Request and the defaults
                var createOrderRequest = new Common.Api.ExigoWebService.CreateOrderRequest();
                createOrderRequest.CurrencyCode = "usd";
                createOrderRequest.Details = new List<Common.Api.ExigoWebService.OrderDetailRequest> { new Common.Api.ExigoWebService.OrderDetailRequest { ItemCode = attendanceItemCode, Quantity = 1, PriceEachOverride = 0, ShippingPriceEachOverride = 0 } }.ToArray();
                createOrderRequest.FirstName = guest.FirstName;
                createOrderRequest.LastName = guest.LastName;
                createOrderRequest.OrderDate = DateTime.Now;
                createOrderRequest.OrderStatus = Common.Api.ExigoWebService.OrderStatusType.Accepted;
                createOrderRequest.OrderType = Common.Api.ExigoWebService.OrderType.APIOrder;
                createOrderRequest.PartyID = partyID;
                createOrderRequest.PriceType = (int)PriceTypes.Retail;
                createOrderRequest.ShipMethodID = 1;
                createOrderRequest.ShippingAmountOverride = 0;
                createOrderRequest.WarehouseID = 1;

                createOrderRequest.State = GlobalSettings.Company.Address.State;
                createOrderRequest.Country = GlobalSettings.Company.Address.Country;

                // Check if we have a Customer ID, else we need to create an account at the same time as the Order, via a Transaction 
                if (guest.CustomerID > 0)
                {
                    createOrderRequest.CustomerID = guest.CustomerID;

                    createOrderResponse = ExigoDAL.WebService().CreateOrder(createOrderRequest);
                }
                else
                {
                    // Create our list of API Requests
                    List<Common.Api.ExigoWebService.ApiRequest> details = new List<Common.Api.ExigoWebService.ApiRequest>();



                    // Now we assemble our Create Customer Request, based off of the Guest record
                    Common.Api.ExigoWebService.CreateCustomerRequest createCustomerRequest = new Common.Api.ExigoWebService.CreateCustomerRequest();
                    createCustomerRequest.FirstName = guest.FirstName;
                    createCustomerRequest.LastName = guest.LastName;
                    createCustomerRequest.InsertEnrollerTree = true;
                    createCustomerRequest.EnrollerID = Identity.Current.CustomerID;
                    createCustomerRequest.CurrencyCode = createOrderRequest.CurrencyCode;
                    createCustomerRequest.CustomerStatus = (int)CustomerStatuses.Active;
                    createCustomerRequest.CustomerType = (int)CustomerTypes.RetailCustomer;

                    createCustomerRequest.EntryDate = DateTime.Now;
                    createCustomerRequest.Notes = "Customer Created from Guest account # {0} via the Back Office of Customer # {1}.".FormatWith(guestID, Identity.Current.CustomerID);

                    if (guest.Email.IsNotNullOrEmpty())
                    {
                        createCustomerRequest.Email = guest.Email;

                    }
                    if (guest.Phone.IsNotNullOrEmpty())
                    {
                        createCustomerRequest.Phone = guest.Phone;
                    }

                    details.Add(createCustomerRequest);

                    // Now add the Create Order Request
                    details.Add(createOrderRequest);

                    var transactionResponse = ExigoDAL.WebService().ProcessTransaction(new Common.Api.ExigoWebService.TransactionalRequest { TransactionRequests = details.ToArray() });

                    // If the transaction was successful, lets update our Guest record with the new Customer ID 
                    if (transactionResponse.Result.Status == Common.Api.ExigoWebService.ResultStatus.Success)
                    {
                        var newCustomerID = 0;

                        foreach (var response in transactionResponse.TransactionResponses)
                        {
                            if (response is Common.Api.ExigoWebService.CreateCustomerResponse) newCustomerID = ((Common.Api.ExigoWebService.CreateCustomerResponse)response).CustomerID;
                        }

                        guest.CustomerID = newCustomerID;
                        PartyService.UpdateGuest(guest);
                    }
                }

                // If we have made it this far, we need to fetch the Guests again so we can overwrite the html of the Guest table
                var vModel = new PartySummaryViewModel();
                vModel.Guests = PartyService.GetPartyGuests(partyID, validateHasAttendedParty: true);

                // Make sure that our Party Guest we just set is showing as attended, in case there is any delay in the SQL to pull their Orders from this Party
                if (vModel.Guests.FirstOrDefault(g => g.GuestID == guestID).HasAttendedParty == false)
                {
                    vModel.Guests.FirstOrDefault(g => g.GuestID == guestID).HasAttendedParty = true;
                }

                vModel.Party = PartyService.GetParties(new GetPartiesRequest { CustomerID = Identity.Current.CustomerID, PartyID = partyID }).FirstOrDefault();

                var html = this.RenderPartialViewToString("partials/partyguesttable", vModel);


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
