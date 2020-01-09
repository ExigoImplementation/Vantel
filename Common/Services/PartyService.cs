using Common.Models;
using Common.Api.ExigoWebService;
using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Dapper;
using Serilog;

// NOTE: This object requires that System.Windows.Forms is referenced in your project.
namespace Common.Services
{
    public class PartyService
    {
        #region Constructors
        public PartyService(int customerID)
        {
            CustomerID = customerID;
        }

        public PartyService(string webalias, int customerID)
        {
            WebAlias = webalias;
            CustomerID = customerID;
        }
        #endregion

        #region Properties
        public string WebAlias { get; set; }
        public int CustomerID { get; set; }

        List<int> PartyGuestCustomerTypes = new List<int> { CustomerTypes.RetailCustomer };
        #endregion

        #region Parties
        /// <summary>
        /// Get one or more parties based on a distributor ID.
        /// </summary>
        /// <param name="customerID">Owner of site</param>
        /// <param name="partyID">Specific party you would like to pull</param>
        /// <param name="includeHostDetails">Pull the host's information as well as the party details</param>
        /// <param name="hostID">Get specific parties based on a particular Host</param>
        /// <returns></returns>
        public List<Party> GetParties(ExigoService.GetPartiesRequest request)
        {
            var list = new List<Party>();

            try
            {
                var apirequest = new Api.ExigoWebService.GetPartiesRequest();

                if (request.CustomerID > 0)
                {
                    apirequest.DistributorID = request.CustomerID;
                }

                if (request.PartyID > 0)
                {
                    apirequest.PartyID = request.PartyID;
                }

                if (request.HostessID > 0)
                {
                    apirequest.HostID = request.HostessID;
                }

                if (request.PartyStatusID > 0)
                {
                    apirequest.PartyStatusType = request.PartyStatusID;
                }

                var response = ExigoDAL.WebService().GetParties(apirequest);


                // If we don't get any parties back, then we simply return an empty list - Mike M.
                if (response.Parties.Count() == 0)
                    return list;

                var responseParties = response.Parties.ToList();


                if (request.ExcludeFutureParties)
                {
                    responseParties.RemoveAll(p => p.StartDate.BeginningOfDay() > DateTime.Now.BeginningOfDay());
                }

                // Exlude Parties that have a Close date value that is less than DateTime.Now
                if (request.ExcludeClosedParties)
                {
                    responseParties.RemoveAll(p => p.CloseDate != null && p.CloseDate < DateTime.Now);
                }


                // Get our Current Party Totals
                var partyTotalCollection = new Dictionary<int, decimal>();
                var partyIDs = responseParties.Select(c => c.PartyID);

                List<Party> totals = new List<Party>();

                using (var context = ExigoDAL.Sql())
                {
                    totals = context.Query<Party>(@"
                            select 
                                PartyID = p.PartyID,
                                CustomerID = o.CustomerID,
                                CurrentSales = isnull(SUM(o.CommissionableVolumeTotal), 0)
                            from Parties p
                            left join Orders o
                            on p.PartyID = o.PartyID
                            where p.PartyID in @partyIDs
                            and o.OrderStatusID not in (2, 3, 4)
                            group by p.PartyID, o.CustomerID
                                ", new { partyIDs }).ToList();
                }

                // Get a collection of Host orders to ensure the parties that have them already placed
                List<Order> hostOrders = new List<Order>();
                if (request.CheckForHostessOrder)
                {
                    using (var context = ExigoDAL.Sql())
                    {
                        hostOrders = context.Query<Order>(@"
                                select 
                                    OrderID = o.OrderID,
                                    PartyID = o.PartyID,
                                    CustomerID = o.CustomerID
                                from Orders o
                                where o.PartyID in @partyIDs
                                and o.OrderStatusID not in (2, 3, 4)
                                and o.CustomerID in @hostIDs
                                    ", new { partyIDs, hostIDs = responseParties.Select(p => p.HostID) }).ToList();
                    }
                }



                // Adapt the response to our objects
                responseParties.ForEach(p =>
                {
                    var party = (Party)p;
                    party.SetPartyUrl(WebAlias);

                    // Run method to set up the Address model on the Party record, if applicable
                    party.PopulateAddress();

                    // Find our total match and sync it up
                    var total = totals.Where(t => t.PartyID == p.PartyID);
                    if (total.Count() > 0)
                    {
                        party.CurrentSales = total.Sum(s => s.CurrentSales);
                    }

                    // Here if we are needing to check if the final host order has been placed, we check the host orders list above
                    if (request.CheckForHostessOrder)
                    {
                        var partyHostOrder = hostOrders.FirstOrDefault(h => h.CustomerID == party.HostID && h.PartyID == party.PartyID);
                        if (partyHostOrder != null)
                        {
                            party.HostOrderIsPlaced = true;
                        }
                    }


                    list.Add(party);
                });

                var rewards = new List<ExigoService.HostessReward>();
                if (request.IncludeHostessRewards)
                {
                    rewards = GetHostessRewards(request.CustomerID, parties: list);

                    list.ForEach(p =>
                    {
                        var reward = rewards.FirstOrDefault(r => r.PartyID == p.PartyID);
                        if (reward != null)
                        {
                            p.HostessRewards = reward;
                        }
                    });
                }

                // If we need to include any host details with this call, here is where that is done
                if (request.IncludeHostessDetails)
                {
                    var hostIDList = list.Select(p => p.HostID).Distinct().ToList();
                    var hosts = ExigoDAL.GetCustomers(hostIDList);

                    if (hosts.Count() > 0)
                    {
                        list.ForEach(p =>
                        {
                            var host = hosts.Where(c => c.CustomerID == p.HostID).FirstOrDefault();
                            if (host != null)
                            {
                                p.HostFirstName = host.FirstName;
                                p.HostLastName = host.LastName;
                                p.HostPhone = host.MobilePhone;

                                if (host.MainAddress.IsComplete)
                                {
                                    p.HostAddress = host.MainAddress;
                                }
                                else if (host.MailingAddress.IsComplete)
                                {
                                    p.HostAddress = host.MailingAddress;
                                }
                            }
                        });
                    }
                }


                // If we need to include the Party creator's details with this call, we will get that similar to the Hostess detail call above
                if (request.IncludeOwnerDetails)
                {
                    var customerIDList = list.Select(p => p.CustomerID).Distinct().ToList();
                    var customers = ExigoDAL.GetCustomers(customerIDList);

                    if (customers.Count() > 0)
                    {
                        list.ForEach(p =>
                        {
                            var customer = customers.Where(c => c.CustomerID == p.CustomerID).FirstOrDefault();
                            if (customer != null)
                            {
                                p.PartyOwner = customer;
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("PartyService.Get Parties failed: ", ex);
            }

            return list;
        }

        public List<Party> SearchParties(PartySearchRequest request)
        {
            var list = new List<Party>();

            try
            {
                var whereClause = "";

                if (request.FirstName.IsNotNullOrEmpty())
                {
                    whereClause = "where c.FirstName = '{0}'".FormatWith(request.FirstName);
                }

                if (request.LastName.IsNotNullOrEmpty())
                {
                    if (whereClause.Length > 0)
                    {
                        whereClause += " and ";
                    }
                    else
                    {
                        whereClause = "where ";
                    }
                    whereClause += " c.LastName = '{0}'".FormatWith(request.LastName);
                }

                if (request.ZipCode.IsNotNullOrEmpty())
                {
                    if (whereClause.Length > 0)
                    {
                        whereClause += " and ";
                    }
                    else
                    {
                        whereClause = "where ";
                    }
                    whereClause += " p.ZipCode = '{0}'".FormatWith(request.ZipCode);
                }

                // We filter this one with a year, month and day comparison only to not filter out specific times the user sets when creating the party - Mike M.
                if (request.EventStart != null)
                {
                    var partyStartDate = Convert.ToDateTime(request.EventStart);

                    if (whereClause.Length > 0)
                    {
                        whereClause += " and ";
                    }
                    else
                    {
                        whereClause = "where ";
                    }
                    whereClause += " p.EventStart = '{0}'".FormatWith(partyStartDate);

                    //query = query.Where(p => p.StartDate.Year == partyStartDate.Year && p.StartDate.Month == partyStartDate.Month && p.StartDate.Day == partyStartDate.Day);
                }

                if (request.ShowOpenPartiesOnly)
                {
                    if (whereClause.Length > 0)
                    {
                        whereClause += " and ";
                    }
                    else
                    {
                        whereClause = "where ";
                    }
                    whereClause += " p.PartyStatusID = {0}".FormatWith(PartyStatusTypes.Open);
                }

                if (request.PartyID > 0)
                {
                    if (whereClause.Length > 0)
                    {
                        whereClause += " and ";
                    }
                    else
                    {
                        whereClause = "where ";
                    }
                    whereClause += " p.PartyID = {0}".FormatWith(request.PartyID);
                }

                if (request.CustomerID > 0)
                {
                    if (whereClause.Length > 0)
                    {
                        whereClause += " and ";
                    }
                    else
                    {
                        whereClause = "where ";
                    }
                    whereClause += " p.DistributorID = {0}".FormatWith(request.CustomerID);
                }

                using (var context = ExigoDAL.Sql())
                {
                    list = context.Query<Party>(@"
                            select
                            p.*,
                            CurrentSales = case when p.Field1 <> '' then p.Field1 else 0 end,
                            HostID = c.CustomerID,
                            HostFirstName = c.FirstName,
                            HostLastName = c.LastName,
                            HostPhone = coalesce(c.Phone, c.MobilePhone)
                            from Parties p
                            left join Customers c 
                                on p.HostID = c.CustomerID
                            " + whereClause).ToList();
                }

                // Run a method on each party so it will populate the Address property correctly
                list.ForEach(p => { p.PopulateAddress(); });

            }
            catch (Exception ex)
            {
                throw new Exception("PartyService.SearchParties failed: " + ex.Message);
            }

            return list;
        }

        public List<WeekSalesTotalNode> GetSalesTotals(List<Party> parties, int customerID)
        {
            var list = new List<SalesTotalNode>();
            var startOfMonth = DateTime.Now.BeginningOfMonth();
            var endOfMonth = startOfMonth.AddMonths(1).AddMinutes(-1);
            var weeklySalesNodeList = new List<WeekSalesTotalNode> 
            {
                new WeekSalesTotalNode { Week = 1, StartDate = startOfMonth, EndDate = startOfMonth.AddDays(7) },
                new WeekSalesTotalNode { Week = 2, StartDate = startOfMonth.AddDays(7), EndDate = startOfMonth.AddDays(14) },
                new WeekSalesTotalNode { Week = 3, StartDate = startOfMonth.AddDays(14), EndDate = startOfMonth.AddDays(21) },
                new WeekSalesTotalNode { Week = 4, StartDate = startOfMonth.AddDays(21), EndDate = endOfMonth }
            };


            var partyIDs = parties.Select(c => c.PartyID);

            using (var context = ExigoDAL.Sql())
            {
                list = context.Query<SalesTotalNode>(@"
                        select CustomerID = o.CustomerID,
                            PartyID    = p.PartyID,
                            OrderID    = o.OrderID,
                            OrderDate  = o.OrderDate,
                            Total      = o.Total,
                            PartyStatusID = p.PartyStatusID                        
                        from Orders o
                        inner join Parties p
                            on p.DistributorID = @customerID
                            and o.PartyID = p.PartyID 
                        where o.OrderDate between dateadd(D, -1, @startDate) and dateadd(D, 1, @endDate)
                        ", new
                        {
                            customerID = customerID,
                            startDate = startOfMonth,
                            endDate = endOfMonth
                        }).ToList();
            }



            if (list.Count > 0)
            {
                weeklySalesNodeList.ForEach(node =>
                {
                    // Set active if this is the current week
                    if (node.StartDate <= DateTime.Now && node.EndDate >= DateTime.Now)
                    {
                        node.IsActiveWeek = true;
                    }

                    var listnode = list.Where(c => c.OrderDate >= node.StartDate && c.OrderDate <= node.EndDate);

                    if (listnode.Count() > 0)
                    {
                        node.Value = listnode.Sum(c => c.Total);
                    }
                });
            }


            return weeklySalesNodeList;
        }

        public Party CreateParty(ExigoService.CreatePartyRequest request)
        {
            // Create the party
            var context = ExigoDAL.WebService();


            var eventStartDate = new DateTime(request.EventStartDate.Year, request.EventStartDate.Month, request.EventStartDate.Day, request.EventStartTime.Hour, request.EventStartTime.Minute, 0);
            var eventEndDate = new DateTime(request.EventEndDate.Year, request.EventEndDate.Month, request.EventEndDate.Day, request.EventEndTime.Hour, request.EventEndTime.Minute, 0);

            var apirequest = new Common.Api.ExigoWebService.CreatePartyRequest(request);

            apirequest.EventStart = eventStartDate;
            apirequest.EventEnd = eventEndDate;

            var partyID = context.CreateParty(apirequest).PartyID;
            var party = GetParties(new ExigoService.GetPartiesRequest { CustomerID = request.CustomerID, PartyID = partyID, IncludeHostessDetails = true }).FirstOrDefault();


            // Return the party
            return party;
        }
        public ExigoService.CreatePartyRequest GetCreatePartyRequest(int partyID, int customerID)
        {
            var request = new ExigoService.CreatePartyRequest();

            try
            {
                var party = GetParties(new ExigoService.GetPartiesRequest { CustomerID = customerID, PartyID = partyID, IncludeHostessDetails = true }).FirstOrDefault();

                request = new ExigoService.CreatePartyRequest(party);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error Creating Party: {Message}", ex.Message);
            }


            return request;
        }

        public Party UpdateParty(ExigoService.CreatePartyRequest request)
        {
            if (request.PartyID == 0)
            {
                return null;
            }

            // Create the party
            var context = ExigoDAL.WebService();


            var eventStartDate = new DateTime(request.EventStartDate.Year, request.EventStartDate.Month, request.EventStartDate.Day, request.EventStartTime.Hour, request.EventStartTime.Minute, 0);
            var eventEndDate = new DateTime(request.EventEndDate.Year, request.EventEndDate.Month, request.EventEndDate.Day, request.EventEndTime.Hour, request.EventEndTime.Minute, 0);

            var apirequest = new Common.Api.ExigoWebService.UpdatePartyRequest(request);

            apirequest.StartDate = eventStartDate;
            apirequest.EventStart = eventStartDate;
            apirequest.CloseDate = eventEndDate;
            apirequest.EventEnd = eventEndDate;

            // Update the Party 
            context.UpdateParty(apirequest);

            var partyID = request.PartyID;
            var party = GetParties(new ExigoService.GetPartiesRequest { CustomerID = request.CustomerID, PartyID = partyID, IncludeHostessDetails = true }).FirstOrDefault();


            // Return the party
            return party;
        }

        public void CloseParty(int partyID)
        {
            try
            {
                var request = new UpdatePartyRequest { PartyID = partyID, CloseDate = DateTime.Now, EventEnd = DateTime.Now, PartyStatusType = PartyStatusTypes.Closed };

                var response = ExigoDAL.WebService().UpdateParty(request);
            }
            catch { }
        }

        public void CancelParty(int partyID)
        {
            try
            {
                var request = new UpdatePartyRequest { PartyID = partyID, CloseDate = DateTime.Now, EventEnd = DateTime.Now, PartyStatusType = PartyStatusTypes.Canceled };

                var response = ExigoDAL.WebService().UpdateParty(request);
            }
            catch { }
        }

        /// <summary>
        /// Get our party discount total based off the total amount of the party order(s). This is used for Hostess Reward calculations
        /// </summary>
        /// <param name="partyTotal">Total value of the party orders that the percentage is based on. This is in BV</param>
        /// <returns></returns>
        public decimal GetPartyDiscountTotal(Party party)
        {
            var partyTotal = party.CurrentSales;

            DateTime? startDate = (party.StartDate != null) ? party.StartDate : DateTime.Today;

            decimal multiplier = 0M;

            int[] tiers = new int[] { 200, 400, 800, 1200 };
            

            if (partyTotal >= tiers[0] && partyTotal < tiers[1])
            {
                multiplier = 0.1M;
            }

            if (partyTotal > tiers[1] && partyTotal < tiers[2])
            {
                multiplier = 0.15M;
            }

            if (partyTotal > tiers[2] && partyTotal < tiers[3])
            {
                multiplier = 0.2M;
            }

            if (partyTotal > tiers[3])
            {
                multiplier = 0.25M;
            }

            return partyTotal * multiplier;
        }
        #endregion

        #region Hosts
        public IQueryable<Host> GetHosts(int customerID)
        {
            var hosts = new List<Host>();

            using (var context = ExigoDAL.Sql())
            {
                hosts = context.Query<Host>(@"
                        select 
                            CustomerID = c.CustomerID,
                            FirstName = c.FirstName,
                            LastName = c.LastName,
                            Address1 = c.MainAddress1,
                            Address2 = c.MainAddress2,
                            City = c.MainCity,
                            State = c.MainState,
                            Zip = c.MainZip,
                            Country = c.MainCountry,
                            Email = c.Email,
                            Phone = c.Phone
                        from Customers c
                        where c.CustomerID = @customerID 
                            or c.EnrollerID = @customerID
                        ", new { customerID }).ToList();
            }


            // Return the hosts
            return hosts.AsQueryable();
        }

        public int GetPartyHostID(int partyID)
        {
            var hostID = 0;
            using (var context = ExigoDAL.Sql())
            {
                hostID = context.Query<int>(@"select HostID from Parties where PartyID = @partyID", new { partyID }).FirstOrDefault();
            }

            return hostID;
        }

        public List<HostessReward> GetHostessRewards(int customerID, int partyID = 0, List<Party> parties = null)
        {
            if (parties == null)
            {
                parties = new List<Party>();
            }

            List<HostessReward> rewards = new List<HostessReward>();

            try
            {
                // If we are not passing parties in, we need to go and get the party or parties that are needed for this call
                if (parties.Count() == 0)
                {
                    var searchRequest = new PartySearchRequest();

                    searchRequest.CustomerID = customerID;

                    if (partyID > 0)
                    {
                        searchRequest.PartyID = partyID;
                    }

                    parties = SearchParties(searchRequest);
                }

                // Add out qualifications 
                parties.ForEach(party =>
                {
                    var matchingQualification = HostessRewardQualifications.Where(q => q.SalesTotal < party.CurrentSales).OrderByDescending(q => q.SalesTotal).FirstOrDefault();

                    if (matchingQualification != null)
                    {
                        matchingQualification.PartyID = party.PartyID;

                        // Here we resolve the Amount Discount vs. Percent off logic
                        if (matchingQualification.FreeProductAmount == 0 && matchingQualification.FreeProductPercentage > 0)
                        {
                            var partyTotal = party.CurrentSales;

                            matchingQualification.FreeProductAmount = matchingQualification.FreeProductPercentage * partyTotal;
                        }

                        rewards.Add(matchingQualification);
                    }
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error Getting Hostess Rewards: {Message}", ex.Message);
            }

            return rewards;
        }

        public List<HostessReward> HostessRewardQualifications
        {
            get
            {
                if (_hostessRewardQualifications == null)
                { 

                using (var context = ExigoDAL.Sql())
                {
                    _hostessRewardQualifications = context.Query<HostessReward>(@"
                                select 
                                    SalesTotal = MinVolume, 
                                    HalfPricedItems = HalfPriceQty,
                                    FreeProductAmount = FlatProductCredit, 
                                    FreeProductPercentage = PercentageProductCredit
                                from OrderCalcContext.HostessRewards
                                ").ToList();
                }

                // Test Qualifications
                //_hostessRewardQualifications = new List<ExigoService.HostessReward> 
                //{
                //    new ExigoService.HostessReward { SalesTotal = 250, FreeProductAmount = 25, HalfPricedItems = 0 },
                //    new ExigoService.HostessReward { SalesTotal = 350, FreeProductAmount = 35, HalfPricedItems = 1 },
                //    new ExigoService.HostessReward { SalesTotal = 450, FreeProductAmount = 60, HalfPricedItems = 1 },
                //    new ExigoService.HostessReward { SalesTotal = 550, FreeProductAmount = 75, HalfPricedItems = 2 },
                //    new ExigoService.HostessReward { SalesTotal = 650, FreeProductAmount = 100, HalfPricedItems = 2 },
                //    new ExigoService.HostessReward { SalesTotal = 750, FreeProductAmount = 125, HalfPricedItems = 3 },
                //    new ExigoService.HostessReward { SalesTotal = 850, FreeProductAmount = 150, HalfPricedItems = 3 },
                //    new ExigoService.HostessReward { SalesTotal = 1000, FreeProductAmount = 200, HalfPricedItems = 4 },
                //    new ExigoService.HostessReward { SalesTotal = 1001, FreeProductAmount = 0, HalfPricedItems = 4, FreeProductPercentage = 0.2m }
                //};
                }

                return _hostessRewardQualifications;
            }
        }
        private List<ExigoService.HostessReward> _hostessRewardQualifications;
        #endregion

        #region Guests
        public Guest CreateGuest(Guest guest)
        {
            // Assemble our Guest creation request and get the new Guest ID
            var request = (Api.ExigoWebService.CreateGuestRequest)guest;

            // Failsafe to ensure that the Host ID is added because for some reason it has to be on the CreateGuest request
            if (request.HostID == 0)
            {
                request.HostID = GetPartyHostID(guest.PartyID);
            }

            var createGuestResponse = ExigoDAL.WebService().CreateGuest(request);

            // Get our Guest ID and add them to the appropriate Party            
            var addPartyGuestRequest = new AddPartyGuestsRequest();
            addPartyGuestRequest.PartyID = guest.PartyID;
            addPartyGuestRequest.GuestIDs = new int[1] { createGuestResponse.GuestID };
            ExigoDAL.WebService().AddPartyGuests(addPartyGuestRequest);

            guest.GuestID = createGuestResponse.GuestID;

            return guest;
        }

        /// <summary>
        /// Create a Guest from our Existing Guests page which takes a real Retail Customer's Customer ID and creates a Guest record along with a Party Guest record, so we can manage their RSVP's per Party.
        /// </summary>
        /// <param name="customerID">Real Customer ID from actual Customer account.</param>
        /// <param name="partyID">Party the resulting Guest record needs to be associated with.</param>
        /// <returns></returns>
        public Guest CreateGuest(int customerID, int partyID)
        {
            // Get our Customer first thing and create their Guest record for this specfic party
            var customer = ExigoDAL.GetCustomer(customerID);
            var guest = new Guest(customer, partyID);

            return CreateGuest(guest);
        }

        public Guest UpdateGuest(Guest guest)
        {
            // Assemble our Guest creation request and get the new Guest ID
            var request = (Common.Api.ExigoWebService.UpdateGuestRequest)guest;

            var updateGuestResponse = ExigoDAL.WebService().UpdateGuest(request);

            return guest;
        }

        // Remove a Guest from a Party, but the Guest record is not deleted
        public void RemoveGuestFromParty(int guestID, int partyID)
        {
            if (guestID > 0)
            {
                var service = ExigoDAL.WebService();

                service.RemovePartyGuests(new RemovePartyGuestsRequest
                {
                    PartyID = partyID,
                    GuestIDs = new int[1] { guestID }
                });
            }
        }

        /// <summary>
        /// Get all Guests currently invited to the specified Party. This uses the GetPartyGuests API call, only accepting partyID. 
        /// If you would like to pass in a specific guestID, we use the GetGuests call to get that user specifically.
        /// </summary>
        /// <param name="partyID">Party ID to call GetPartyGuests with.</param>
        /// <param name="guestID">Guest ID to call GetGuests with, should return only one record.</param>
        /// <param name="getCustomerInfo">Set true to pull actual information from the Customer record if a CustomerID is present on the Guest record.</param>
        /// <returns></returns>
        public List<Guest> GetPartyGuests(int partyID = 0, int guestID = 0, bool getCustomerInfo = false, bool validateHasAttendedParty = false)
        {
            var guests = new List<Guest>();

            try
            {
                if (guestID > 0)
                {
                    var guestResponse = ExigoDAL.WebService().GetGuests(new GetGuestsRequest { GuestID = guestID });
                    // Populate a list of actual Customer's from Exigo if the Guest record contains a Customer ID value
                    List<Customer> guestCustomers = new List<Customer>();
                    if (getCustomerInfo && guestResponse.Guests.Any(c => c.CustomerID > 0))
                    {
                        guestCustomers = ExigoDAL.GetCustomers(guestResponse.Guests.Where(c => c.CustomerID > 0).Select(c => Convert.ToInt32(c.CustomerID)).ToList());
                    }
                    foreach (var guest in guestResponse.Guests)
                    {
                        if (getCustomerInfo && guestCustomers.Any(g => g.CustomerID == guest.CustomerID))
                        {
                            var customer = guestCustomers.Where(g => g.CustomerID == guest.CustomerID).FirstOrDefault();

                            guest.FirstName = customer.FirstName;
                            guest.LastName = customer.LastName;
                            guest.Email = customer.Email;
                            if (customer.MainAddress.IsComplete)
                            {
                                guest.Address1 = customer.MainAddress.Address1;
                                guest.Address2 = customer.MainAddress.Address2;
                                guest.City = customer.MainAddress.City;
                                guest.State = customer.MainAddress.State;
                                guest.Zip = customer.MainAddress.Zip;
                                guest.Country = customer.MainAddress.Country;
                            }
                        }

                        guests.Add(new Guest(guest));
                    }
                }
                else
                {
                    var guestResponse = ExigoDAL.WebService().GetPartyGuests(new GetPartyGuestsRequest { PartyID = partyID });

                    foreach (var guest in guestResponse.Guests)
                    {
                        guests.Add(new Guest(guest));
                    }

                }

                // If we need to check if these Guests have attended or ordered from the Party, we check here
                if (validateHasAttendedParty)
                {
                    var guestCustomerIDs = guests.Select(c => c.CustomerID).Distinct();
                    var orders = new List<Order>();

                    using (var sqlContext = ExigoDAL.Sql())
                    {
                        orders = sqlContext.Query<Order>(@"
                                    select  
                                        OrderID,
                                        CustomerID,
                                        PartyID
                                    from Orders
                                    where PartyID = @partyID
                                    and CustomerID in @guestCustomerIDs
                                    ", new { partyID, guestCustomerIDs }).ToList();
                    }

                    if (orders.Count() > 0)
                    {
                        foreach (var order in orders)
                        {
                            var matchingGuest = guests.FirstOrDefault(g => g.CustomerID == order.CustomerID);

                            if (matchingGuest != null)
                            {
                                matchingGuest.HasAttendedParty = true;
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error Getting Party Guests: {Message}", ex.Message);
            }

            return guests;
        }
        #endregion
    }
        
    // Adapters that are used in the Party Service only
    #region Adapters
    public class PartyOrderNode
    {
        public PartyOrderNode()
        {
            this.Party = new Party();
            this.Orders = new List<Order>();
        }

        public Party Party { get; set; }
        public List<Order> Orders { get; set; }
    }

    public class GuestSearchNode
    {
        public int? CustomerID { get; set; }
        public int? GuestID { get; set; }
    }
    #endregion
}