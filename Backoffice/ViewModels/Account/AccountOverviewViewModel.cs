using Common;
using ExigoService;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backoffice.ViewModels
{
    public class AccountOverviewViewModel
    {
        public AccountOverviewViewModel()
        {
            this.CustomerSite = new CustomerSite { Address = new Address() };
            this.Customer = new Customer();
        }

        public Customer Customer { get; set; }    

        [Required(ErrorMessageResourceName = "WebAliasRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), Display(Name = "WebAlias", ResourceType = typeof(Common.Resources.Models)), System.Web.Mvc.Remote("IsValidWebAlias", "Account")]
        public string WebAlias { get; set; }

        [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), Display(Name = "Password", ResourceType = typeof(Common.Resources.Models)), RegularExpression(GlobalSettings.RegularExpressions.Password, ErrorMessageResourceName = "InvalidPassword", ErrorMessageResourceType = typeof(Common.Resources.Models)), DataType("NewPassword")]
        public string Password { get; set; }

        [Required(ErrorMessageResourceName = "ConfirmPasswordRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), Display(Name = "ConfirmPassword", ResourceType = typeof(Common.Resources.Models)), Compare("Password", ErrorMessageResourceName = "PasswordsDoNotMatch", ErrorMessageResourceType = typeof(Common.Resources.Models)), DataType("Password")]
        public string ConfirmPassword { get; set; }

        public CustomerSite CustomerSite { get; set; }        

        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string YouTube { get; set; }
        public string Blog { get; set; }
        public string GooglePlus { get; set; }
        public string LinkedIn { get; set; }
        public string MySpace { get; set; }
        public string Pinterest { get; set; }
        public string Instagram { get; set; }
    }
}