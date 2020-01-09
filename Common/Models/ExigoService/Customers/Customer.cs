using Common;
using Common.Filters;
using Common.ModelBinders;
using MVCValidations.Filters.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ExigoService
{
    public class Customer : ICustomer
    {
        public Customer()
        {
            this.MainAddress = new Address() { AddressType = AddressType.Main };
            this.MailingAddress = new Address() { AddressType = AddressType.Mailing };
            this.OtherAddress = new Address() { AddressType = AddressType.Other };
            this.HighestAchievedRank = new Rank();
        }
        

        public int CustomerID { get; set; }

        [Required(ErrorMessageResourceName = "FirstNameRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), Display(Name = "FirstName", ResourceType = typeof(Common.Resources.Models))]
        public string FirstName { get; set; }

        [Display(Name = "MiddleName", ResourceType = typeof(Common.Resources.Models))]
        public string MiddleName { get; set; }

        [Required(ErrorMessageResourceName = "LastNameRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), Display(Name = "LastName", ResourceType = typeof(Common.Resources.Models))]
        public string LastName { get; set; }

        [Display(Name = "Company", ResourceType = typeof(Common.Resources.Models))]
        public string Company { get; set; }

        public int CustomerTypeID { get; set; }
        public int CustomerStatusID { get; set; }
        public int DefaultWarehouseID { get; set; }

        [DataType("Languages"), Display(Name = "PreferredLanguage", ResourceType = typeof(Common.Resources.Models))]
        public int LanguageID { get; set; }
        public DateTime CreatedDate { get; set; }

        [PropertyBinder(typeof(DateModelBinder)), DataType("BirthDate"), Display(Name = "Birthday", ResourceType = typeof(Common.Resources.Models))]
        public DateTime BirthDate { get; set; }

        [Display(Name = "TaxID", ResourceType = typeof(Common.Resources.Models)), DataType("TaxID")]
        public string TaxID { get; set; }

        [Required(ErrorMessageResourceName = "FieldIsRequired", ErrorMessageResourceType = typeof(Common.Resources.Models))]
        public string PayableToName { get; set; }

        [Required]
        public int PayableTypeID { get; set; }

        public bool IsOptedIn { get; set; }
        public bool IsSMSSubscribed { get; set; }

        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), DataType(DataType.EmailAddress), RegularExpression(GlobalSettings.RegularExpressions.EmailAddresses, ErrorMessageResourceName = "IncorrectEmail", ErrorMessageResourceType = typeof(Common.Resources.Models)), Display(Name = "Email", ResourceType = typeof(Common.Resources.Models))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "PrimaryPhoneRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), DataType(DataType.PhoneNumber), Display(Name = "PrimaryPhone", ResourceType = typeof(Common.Resources.Models))]
        public string PrimaryPhone { get; set; }

        [DataType(DataType.PhoneNumber), Display(Name = "SecondaryPhone", ResourceType = typeof(Common.Resources.Models))]
        public string SecondaryPhone { get; set; }

        [DataType(DataType.PhoneNumber), Display(Name = "MobilePhone", ResourceType = typeof(Common.Resources.Models))]
        public string MobilePhone { get; set; }

        [DataType(DataType.PhoneNumber), Display(Name = "FaxNumber", ResourceType = typeof(Common.Resources.Models))]
        public string Fax { get; set; }

        [DataType("Address")]
        public Address MainAddress { get; set; }
        public ShippingAddress MainShippingAddress
        {
            get
            {
                return new ShippingAddress(MainAddress)
                {
                    FirstName = FirstName,
                    MiddleName = MiddleName,
                    LastName = LastName,
                    Company = Company,
                    Email = Email,
                    Phone = PrimaryPhone
                };
            }
        }

        [DataType("Address")]
        public Address MailingAddress { get; set; }
        public ShippingAddress MailingShippingAddress
        {
            get
            {
                return new ShippingAddress(MailingAddress)
                {
                    FirstName = FirstName,
                    MiddleName = MiddleName,
                    LastName = LastName,
                    Company = Company,
                    Email = Email,
                    Phone = PrimaryPhone
                };
            }
        }

        [DataType("Address")]
        public Address OtherAddress { get; set; }
        public ShippingAddress OtherShippingAddress
        {
            get
            {
                return new ShippingAddress(OtherAddress)
                {
                    FirstName = FirstName,
                    MiddleName = MiddleName,
                    LastName = LastName,
                    Company = Company,
                    Email = Email,
                    Phone = PrimaryPhone
                };
            }
        }

        public List<Address> Addresses
        {
            get
            {
                var collection = new List<Address>();
                if (this.MainAddress != null && this.MainAddress.IsComplete) collection.Add(this.MainAddress);
                if (this.MailingAddress != null && this.MailingAddress.IsComplete) collection.Add(this.MailingAddress);
                if (this.OtherAddress != null && this.OtherAddress.IsComplete) collection.Add(this.OtherAddress);
                return collection;
            }
            set { }
        }

        public List<ShippingAddress> ShippingAddresses
        {
            get
            {
                var collection = new List<ShippingAddress>();
                if (this.MainShippingAddress != null && this.MainShippingAddress.IsComplete) collection.Add(this.MainShippingAddress);
                if (this.MailingShippingAddress != null && this.MailingShippingAddress.IsComplete) collection.Add(this.MailingShippingAddress);
                if (this.OtherShippingAddress != null && this.OtherShippingAddress.IsComplete) collection.Add(this.OtherShippingAddress);
                return collection;
            }
        }

        [Required(ErrorMessageResourceName = "UsernameRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), Remote("IsLoginNameAvailable", "App", ErrorMessageResourceName = "UsernameUnavailable", ErrorMessageResourceType = typeof(Common.Resources.Models)), RegularExpression(GlobalSettings.RegularExpressions.LoginName, ErrorMessageResourceName = "InvalidUsername", ErrorMessageResourceType = typeof(Common.Resources.Models)), Display(Name = "Username", ResourceType = typeof(Common.Resources.Models))]
        public string LoginName { get; set; }

        [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), RegularExpression(GlobalSettings.RegularExpressions.Password, ErrorMessageResourceName = "InvalidPassword", ErrorMessageResourceType = typeof(Common.Resources.Models)), Display(Name = "Password", ResourceType = typeof(Common.Resources.Models)), DataType("NewPassword")]
        public string Password { get; set; }

        public int? EnrollerID { get; set; }
        public int? SponsorID { get; set; }
        public Rank HighestAchievedRank { get; set; }

        public string CurrencyCode { get; set; }

        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }
        public string Field6 { get; set; }
        public string Field7 { get; set; }
        public string Field8 { get; set; }
        public string Field9 { get; set; }
        public string Field10 { get; set; }
        public string Field11 { get; set; }
        public string Field12 { get; set; }
        public string Field13 { get; set; }
        public string Field14 { get; set; }
        public string Field15 { get; set; }

        public DateTime? Date1 { get; set; }
        public DateTime? Date2 { get; set; }
        public DateTime? Date3 { get; set; }
        public DateTime? Date4 { get; set; }
        public DateTime? Date5 { get; set; }

        public CustomerType CustomerType { get; set; }
        public CustomerStatus CustomerStatus { get; set; }
        public Customer Enroller { get; set; }
        public Customer Sponsor { get; set; }

        public string FullName
        {
            get { return string.Join(" ", this.FirstName, this.LastName); }
        }
        public string AvatarUrl
        {
            get
            {
                return GlobalUtilities.GetCustomerAvatarUrl(this.CustomerID);
            }
        }


        // SQL Only Variables
        public string CustomerTypeDescription { get; set; }
        public string CustomerStatusDescription { get; set; }
        public int RankID { get; set; }
    }
}