using Common.Services;
using ExigoService;
using System;

namespace Backoffice.ViewModels
{
    // View Model used for Kendo Grids that display Orders (ex. Downline Orders or Profile/Orders
    public class OrdersViewModel
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerIDToken { get { return Security.Encrypt(this.CustomerID, Identity.Current.CustomerID); } }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal BusinessVolumeTotal { get; set; }
        public decimal CommissionableVolumeTotal { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public string CurrencyCode { get; set; }
        public string CountryCode { get; set; }
        public DateTime OrderDate { get; set; }

        // Auto Order properties
        public DateTime NextRunDate { get; set; }
        public DateTime LastRunDate { get; set; }

        public string CultureCode
        {
            get
            {
                return Common.GlobalUtilities.GetCultureCodeFormatBasedOnCurrency(CurrencyCode, CountryCode);
            }
        }
    }
}