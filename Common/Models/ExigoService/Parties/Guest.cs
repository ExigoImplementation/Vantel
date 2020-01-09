using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ExigoService
{
    public class Guest
    {
        public Guest()
        {
            CreatedDate = DateTime.Now;
        }
        public Guest(Common.Api.ExigoWebService.GuestResponse guest)
        {
            GuestID       = guest.GuestID;
            CustomerID    = (guest.CustomerID.CanBeParsedAs<int>()) ? Convert.ToInt32(guest.CustomerID) : 0;
            CreatedDate   = guest.CreatedDate;
            FirstName     = guest.FirstName;
            LastName      = guest.LastName;
            Email         = guest.Email;
            Phone         = guest.Phone;
            WorkPhone     = guest.Phone2;
            CellPhone     = guest.MobilePhone;
            
            Address1      = guest.Address1;
            Address2      = guest.Address2;
            City          = guest.City;
            State         = guest.State;
            Zip           = guest.Zip;
            Country       = guest.Country;            

            AllowEmail    = (!guest.Field1.IsNullOrEmpty()) ? Convert.ToBoolean(guest.Field1) : false;
            HasSentInvite = (!guest.Field2.IsNullOrEmpty() && guest.Field2.CanBeParsedAs<DateTime>()) ? true : false;
            HasRSVPd      = (!guest.Field3.IsNullOrEmpty() && guest.Field3.CanBeParsedAs<DateTime>()) ? true : false;

            
            if (HasRSVPd)
            {
                RSVPResponseDate = Convert.ToDateTime(guest.Field2);
            }
        }
        public Guest(Customer customer, int partyID)
        {
            CustomerID = customer.CustomerID;
            CreatedDate = DateTime.Now;
            FirstName   = customer.FirstName;
            LastName    = customer.LastName;
            Email       = customer.Email;
            Phone       = customer.PrimaryPhone;
            WorkPhone   = customer.SecondaryPhone;
            CellPhone   = customer.MobilePhone;

            if(customer.MainAddress != null && customer.MainAddress.IsComplete)
            {
                Address1 = customer.MainAddress.Address1;
                Address2 = customer.MainAddress.Address2;
                City     = customer.MainAddress.City;
                State    = customer.MainAddress.State;
                Zip      = customer.MainAddress.Zip;
                Country  = customer.MainAddress.Country;
            }

            // Specify which party this Customer needs to be created under
            PartyID = partyID;

            //AllowEmail = customer.IsOptedIn;
        }

        [Display(Name = "Guest ID")]
        public int GuestID { get; set; }

        public int CreatedByID { get; set; }
        public int PartyID { get; set; }
        public int HostID { get; set; }

        public DateTime CreatedDate { get; set; }

        [Display(Name = "ID")]
        public int CustomerID { get; set; }

        [Required]
        [Display(Name = "Status")]
        public int CustomerStatusID { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string FullName
        {
            get { return string.Join(" ", this.FirstName, this.LastName); }
        }

        [RegularExpression(GlobalSettings.RegularExpressions.EmailAddresses)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        //[Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        //[System.Web.Mvc.Remote("IsEmailAvailable_Guest", "App", ErrorMessage = "This email already exists in our records - try another one.", AdditionalFields = "CustomerID")]
        public string Email { get; set; }

        public bool AllowEmail { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Home Phone")]
        public string Phone { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Work Phone")]
        public string WorkPhone { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Cell Phone")]
        public string CellPhone { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Fax")]
        public string Fax { get; set; }
        
        [Display(Name = "Street Address")]
        public string Address1 { get; set; }

        [Display(Name = "Apt/Suite #")]
        public string Address2 { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "Zip")]
        public string Zip { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }
        
        public bool HasSentInvite { get; set; }
        public bool HasRSVPd { get; set; }

        public string Notes { get; set; }
        

        [Display(Name = "RSVP Response Date")]
        public DateTime? RSVPResponseDate { get; set; }

        [Display(Name = "Is Host")]
        public bool IsHost { get; set; }
        public bool IsGuest { get { return GuestID > 0; } }
        public bool HasAttendedParty { get; set; }
    }
}