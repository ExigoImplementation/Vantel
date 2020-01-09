using Common;
using System.ComponentModel.DataAnnotations;

namespace ExigoService
{
    public class ShippingAddress : Address
    {
        public ShippingAddress() { }
        public ShippingAddress(Address address)
        {
            AddressType = address.AddressType;
            Address1    = address.Address1;
            Address2    = address.Address2;
            City        = address.City;
            State       = address.State;
            Zip         = address.Zip;
            Country     = address.Country;
        }
        public ShippingAddress(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        [Required(ErrorMessageResourceName = "FirstNameRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), /*Display(Name = "FirstName", ResourceType = typeof(Common.Resources.Models))*/]
        public string FirstName { get; set; }

        [Display(Name = "MiddleName", ResourceType = typeof(Common.Resources.Models))]
        public string MiddleName { get; set; }

        [Required(ErrorMessageResourceName = "LastNameRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), /*Display(Name = "LastName", ResourceType = typeof(Common.Resources.Models))*/]
        public string LastName { get; set; }

        [Display(Name = "Company", ResourceType = typeof(Common.Resources.Models))]
        public string Company { get; set; }

        [Required(ErrorMessageResourceName = "PhoneNumberRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), DataType(DataType.PhoneNumber), Display(Name = "PhoneNumber", ResourceType = typeof(Common.Resources.Models))]
        [RegularExpression(GlobalSettings.RegularExpressions.PhoneNumber, ErrorMessageResourceName = "IncorrectPhone", ErrorMessageResourceType = typeof(Common.Resources.Models))]
        public string Phone { get; set; }

        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), DataType(DataType.EmailAddress), RegularExpression(GlobalSettings.RegularExpressions.EmailAddresses, ErrorMessageResourceName = "IncorrectEmail", ErrorMessageResourceType = typeof(Common.Resources.Models)), Display(Name = "Email", ResourceType = typeof(Common.Resources.Models))]
        public string Email { get; set; }

        public string Notes { get; set; }

        public string FullName
        {
            get { return string.Join(" ", this.FirstName, this.LastName); }
        }
    }
}