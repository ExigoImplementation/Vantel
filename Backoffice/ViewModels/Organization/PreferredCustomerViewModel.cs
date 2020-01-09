using Common.Services;
using ExigoService;
using System;

namespace Backoffice.ViewModels
{
    public class PreferredCustomerViewModel
    {
        public int CustomerID { get; set; }
        public string CustomerIDToken { get { return Security.Encrypt(this.CustomerID, Identity.Current.CustomerID); } }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int CustomerTypeID { get; set; }
        public string CustomerTypeDescription { get { return CommonResources.CustomerTypes(this.CustomerTypeID, CommonResourceFormat.Short); } }

    }
}