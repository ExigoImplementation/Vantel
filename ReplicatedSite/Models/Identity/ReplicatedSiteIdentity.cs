using System;
using System.Web;
using System.Security.Principal;
using ExigoService;
using Common;
using Common.Api.ExigoWebService;
using System.Collections.Generic;

namespace ReplicatedSite
{
    public class ReplicatedSiteIdentity : IIdentity
    {
        #region Constructors
        public ReplicatedSiteIdentity()
        {
        }
        public ReplicatedSiteIdentity(GetCustomerSiteResponse customerSite)
        {
            CustomerID = customerSite.CustomerID;
            FirstName  = customerSite.FirstName;
            LastName   = customerSite.LastName;
            Company    = customerSite.Company;
            Email      = customerSite.Email;
            WebAlias   = customerSite.WebAlias;
            Phone      = customerSite.Phone;
            Phone2     = customerSite.Phone2;
        }
        #endregion

        #region IIdentity Settings
        string IIdentity.AuthenticationType
        {
            get { return "Custom"; }
        }
        bool IIdentity.IsAuthenticated
        {
            get { return true; }
        }
        public string Name { get; set; }
        #endregion

        #region Properties
        public int CustomerID { get; set; }
        public int CustomerTypeID { get; set; }
        public int CustomerStatusID { get; set; }
        public int WarehouseID { get; set; }
        public int HighestAchievedRankID { get; set; }
        public DateTime CreatedDate { get; set; }

        public string WebAlias { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public string Phone2 { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }

        public string FacebookUrl { get; set; }
        public string GooglePlusUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string BlogUrl { get; set; }
        public string LinkedInUrl { get; set; }
        public string MySpaceUrl { get; set; }
        public string YouTubeUrl { get; set; }
        public string PinterestUrl { get; set; }
        public string InstagramUrl { get; set; }

        public List<KeyValuePair<string, string>> SocialMediaUrls {
            get {
                var item = new List<KeyValuePair<string, string>>()                
                 {
                    new KeyValuePair<string, string> ( "Facebook", FacebookUrl ),
                    new KeyValuePair<string, string> ( "Google-plus", GooglePlusUrl ),
                    new KeyValuePair<string, string> ( "Twitter",  TwitterUrl ),
                    new KeyValuePair<string, string> ( "Blogger", BlogUrl ),
                    new KeyValuePair<string, string> ( "Linkedin", LinkedInUrl ),
                    //new KeyValuePair<string, string>( "myspace", MySpaceUrl ),
                    new KeyValuePair<string, string>( "Youtube", YouTubeUrl ),
                    new KeyValuePair<string, string>( "Pinterest", PinterestUrl ),
                    new KeyValuePair<string, string>( "Instagram", InstagramUrl )
                };
                return item;
            }
        }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string County { get; set; }
        public string Country { get; set; }

        public Address Address { get {
                return new Address()
                {
                    AddressType = AddressType.Main,
                    Address1 = Address1,
                    Address2 = Address2,
                    Address3 = Address3,
                    City = City,
                    State = State,
                    Zip = Zip,
                    County = County,
                    Country = Country
                };
            }
        }

        public string Notes1 { get; set; }
        public string Notes2 { get; set; }
        public string Notes3 { get; set; }
        public string Notes4 { get; set; }

        public string FullName
        {
            get { return this.FirstName + " " + this.LastName; }
        }
        public string DisplayName
        {
            get { return GlobalUtilities.Coalesce(this.Company, this.FirstName + " " + this.LastName); }
        }
        public Market Market
        {
            get { return GlobalUtilities.GetMarket(this.Country); }
        }
        public bool IsOrphan
        {
            get
            {
                return GlobalSettings.ReplicatedSites.DefaultWebAlias.Equals(this.WebAlias, StringComparison.InvariantCultureIgnoreCase);
            }
        }
        #endregion

        #region Private Methods
        private string GetBrowsersDefaultCultureCode()
        {
            string[] languages = HttpContext.Current.Request.UserLanguages;

            if (languages == null || languages.Length == 0)
                return "en-US";
            try
            {
                string language = languages[0].Trim();
                return language;
            }

            catch (ArgumentException)
            {
                return "en-US";
            }
        }
        #endregion
    }
}