using Common.Services;
using ExigoService;
using System;

namespace Backoffice.ViewModels
{
    public class DownlineRankViewModel
    {
        public int CustomerID { get; set; }
        public string CustomerIDToken { get { return Security.Encrypt(this.CustomerID, Identity.Current.CustomerID); } }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RankID { get; set; }
        public string RankDescription { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}