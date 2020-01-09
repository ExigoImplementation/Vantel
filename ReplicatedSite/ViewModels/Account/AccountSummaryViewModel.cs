using Common;
using ExigoService;
using System.ComponentModel.DataAnnotations;

namespace ReplicatedSite.ViewModels
{
    public class AccountSummaryViewModel
    {
        public AccountSummaryViewModel()
        {
            this.Customer = new Customer();
        }

        public Customer Customer { get; set; }        

        [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), Display(Name = "Password", ResourceType = typeof(Common.Resources.Models)), RegularExpression(GlobalSettings.RegularExpressions.Password, ErrorMessageResourceName = "InvalidPassword", ErrorMessageResourceType = typeof(Common.Resources.Models)), DataType("NewPassword")]
        public string Password { get; set; }

        [Required(ErrorMessageResourceName = "ConfirmPasswordRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), Display(Name = "ConfirmPassword", ResourceType = typeof(Common.Resources.Models)), Compare("Password", ErrorMessageResourceName = "PasswordsDoNotMatch", ErrorMessageResourceType = typeof(Common.Resources.Models)), DataType("Password")]
        public string ConfirmPassword { get; set; }           
    }
}