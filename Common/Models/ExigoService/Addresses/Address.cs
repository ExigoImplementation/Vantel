using Common.Services;
using System.ComponentModel.DataAnnotations;

namespace ExigoService
{
    public class Address : IAddress
    {
        public Address()
        {
            this.AddressType = AddressType.New;
        }
        public Address(string country, string state) : this()
        {
            this.Country = country;
            this.State = state;
        }

        public Address(ShippingAddress shippingAddress)
        {
            AddressType = shippingAddress.AddressType;
            Address1 = shippingAddress.Address1;
            Address2 = shippingAddress.Address2;
            Address3 = shippingAddress.Address3;
            City = shippingAddress.City;
            State = shippingAddress.State;
            Zip = shippingAddress.Zip;
            County = shippingAddress.County;
            Country = shippingAddress.Country;
        }

        [Required]
        public AddressType AddressType { get; set; }

        [Required(ErrorMessageResourceName = "AddressOneRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), Display(Name = "AddressOne", ResourceType = typeof(Common.Resources.Models))]
        public string Address1 { get; set; }

        [Display(Name = "AddressTwo", ResourceType = typeof(Common.Resources.Models))]
        public string Address2 { get; set; }

        public string Address3 { get; set; }

        [Required(ErrorMessageResourceName = "CityRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), Display(Name = "City", ResourceType = typeof(Common.Resources.Models))]
        public string City { get; set; }

        [Required(ErrorMessageResourceName = "StateRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), Display(Name = "State", ResourceType = typeof(Common.Resources.Models))]
        public string State { get; set; }

        [Required(ErrorMessageResourceName = "ZipRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), Display(Name = "Zip", ResourceType = typeof(Common.Resources.Models))]
        public string Zip { get; set; }

        [Required(ErrorMessageResourceName = "CountryRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), Display(Name = "Country", ResourceType = typeof(Common.Resources.Models))]
        public string Country { get; set; }

        public string County { get; set; }

        public string AddressDisplay
        {
            get { return this.Address1 + ((!this.Address2.IsEmpty()) ? " {0}".FormatWith(this.Address2) : ""); }
        }
        public bool IsComplete
        {
            get
            {
                return
                    !string.IsNullOrEmpty(Address1) &&
                    !string.IsNullOrEmpty(City) &&
                    !string.IsNullOrEmpty(State) &&
                    !string.IsNullOrEmpty(Zip) &&
                    !string.IsNullOrEmpty(Country);
            }
        }

        public string GetHash()
        {
            return Security.GetHashString(string.Format("{0}|{1}|{2}|{3}|{4}",
                this.AddressDisplay.Trim(),
                this.City.Trim(),
                this.State.Trim(),
                this.Zip.Trim(),
                this.Country.Trim()));
        }

        public override string ToString()
        {
            return this.Address1 + " " + (this.Address2.IsNotNullOrEmpty() ? this.Address2 + " " : string.Empty) + this.City + " " + this.State + " " + this.Zip;
        }
        public override bool Equals(object obj)
        {
            try
            {
                var hasha = this.GetHash();
                var hashb = ((Address)obj).GetHash();
                return hasha.Equals(hashb);
            }
            catch
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}