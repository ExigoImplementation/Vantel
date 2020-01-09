using System;
using System.Collections.Generic;

namespace ExigoService
{
    public interface ICustomer
    {
        int CustomerID { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Company { get; set; }

        int CustomerTypeID { get; set; }
        CustomerType CustomerType { get; set; }

        int CustomerStatusID { get; set; }
        CustomerStatus CustomerStatus { get; set; }

        string Email { get; set; }
        string PrimaryPhone { get; set; }
        string SecondaryPhone { get; set; }
        string MobilePhone { get; set; }
        string Fax { get; set; }

        Address MainAddress { get; set; }
        Address MailingAddress { get; set; }
        Address OtherAddress { get; set; }

        ShippingAddress MainShippingAddress { get; }
        ShippingAddress MailingShippingAddress { get; }
        ShippingAddress OtherShippingAddress { get; }

        List<Address> Addresses { get; }
        List<ShippingAddress> ShippingAddresses { get; }

        string LoginName { get; set; }

        int DefaultWarehouseID { get; set; }
        int LanguageID { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime BirthDate { get; set; }
        bool IsOptedIn { get; set; }
        bool IsSMSSubscribed { get; set; }

        int? EnrollerID { get; set; }
        int? SponsorID { get; set; }
        Rank HighestAchievedRank { get; set; }

        string Field1 { get; set; }
        string Field2 { get; set; }
        string Field3 { get; set; }
        string Field4 { get; set; }
        string Field5 { get; set; }
        string Field6 { get; set; }
        string Field7 { get; set; }
        string Field8 { get; set; }
        string Field9 { get; set; }
        string Field10 { get; set; }
        string Field11 { get; set; }
        string Field12 { get; set; }
        string Field13 { get; set; }
        string Field14 { get; set; }
        string Field15 { get; set; }

        DateTime? Date1 { get; set; }
        DateTime? Date2 { get; set; }
        DateTime? Date3 { get; set; }
        DateTime? Date4 { get; set; }
        DateTime? Date5 { get; set; }
    }
}