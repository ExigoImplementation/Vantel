using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.Api.ExigoWebService;
using Common;

namespace ExigoService
{
    public class Party
    {
        public Party()
        {
            this.HostAddress = new Address();
        }
        public Party(int partyID)
        {
            this.PartyID = partyID;
            this.HostAddress = new Address();
        }

        public int PartyID { get; set; }
        public int PartyType { get; set; }
        public int PartyStatusType { get; set; }
        public int CustomerID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public string Description { get; set; }
        public DateTime? EventStart { get; set; }
        public DateTime? EventEnd { get; set; }
        public int? LanguageID { get; set; }
        public string Information { get; set; }

        // Field 3 on the Party record
        public int MasterOrderID { get; set; }
        
        // Booking Party ID
        public int BookingPartyID { get; set; }

        // Party Address
        public Address Address { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public void PopulateAddress()
        {
            if (Address1.IsNotNullOrEmpty() && City.IsNotNullOrEmpty() && Country.IsNotNullOrEmpty())
            {
                this.Address = new Address
                {
                    Address1 = Address1,
                    Address2 = Address2,
                    City = City,
                    State = State,
                    Zip = Zip,
                    Country = Country
                };
            }
        }


        public bool HasFreeShipping { get { return this.CurrentSales >= 500; } }

        public string PartyUrl { get; set; }
        public void SetPartyUrl(string webalias)
        {
            var url = GlobalSettings.ReplicatedSites.GetPartyUrl(webalias, PartyID); // This is for party shopping in replicated - Mike M.
            PartyUrl = url;
        }

        // Host Info
        public int HostID { get; set; }
        public string HostName { get { return this.HostFirstName + " " + this.HostLastName; } }
        public string HostFirstName { get; set; }
        public string HostLastName { get; set; }
        public string HostPhone { get; set; }
        public string HostDisplayName 
        {
            get 
            {
                if (this.HostFirstName.IsNullOrEmpty())
                {
                    return "";
                }
                else
                {
                    var lastName = this.HostLastName;
                    if (lastName.Length > 1)
                    {
                        lastName = this.HostLastName.Substring(0, 1);
                    }
                    return this.HostFirstName + " " + lastName + "."; 
                }
            } 
        }
        public Address HostAddress { get; set; }
        public bool HostOrderIsPlaced { get; set; }

        public List<Customer> CurrentCustomers { get; set; }
        // goals and rewards
        public decimal SalesGoal { get; set; }
        public decimal CurrentSales { get; set; }
        public decimal HalfPricedItems { get; set; }
        public decimal FreeProductAmount { get; set; }
        public decimal FreeProductPercentage { get; set; }

        // dynamic properties
        public decimal SalesGoalPercentage
        {
            get
            {
                if (SalesGoal != 0) { return (CurrentSales / SalesGoal) * 100; }
                else { return 0; }
            }
        }
        public string SalesGoalPercentageDisplay { 
            get 
            {
                return SalesGoalPercentage.ToString("N0"); 
            } 
        }
        public string GetStartDate
        {
            get
            {
                if (EventStart != null){ return Convert.ToDateTime(EventStart).ToShortDateString(); }
                else{ return string.Empty; }
            }
        }
        public string GetStartTime
        {
            get
            {
                if (EventStart != null) { return Convert.ToDateTime(EventStart).ToShortTimeString(); }
                else { return string.Empty; }
            }
        }

        public HostessReward HostessRewards { get; set; }
        

        // Used for shopping in Replicated Site, to determine which Guest record needs to have a Customer record tied to it
        public Guest CurrentGuest { get; set; }

        public Customer PartyOwner { get; set; }
    }
}