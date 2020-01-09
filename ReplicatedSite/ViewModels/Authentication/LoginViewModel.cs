using System.ComponentModel.DataAnnotations;

namespace ReplicatedSite.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceName = "UsernameRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), Display(Name = "Username", ResourceType = typeof(Common.Resources.Models))]
        public string LoginName { get; set; }

        [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), Display(Name = "Password", ResourceType = typeof(Common.Resources.Models)), DataType("Password")]
        public string LoginPassword { get; set; }
    }
}