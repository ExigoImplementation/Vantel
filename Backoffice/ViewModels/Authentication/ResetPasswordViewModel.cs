using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Backoffice.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        public int CustomerID { get; set; }

        [Required]
        public int CustomerType { get; set; }


        [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), RegularExpression(GlobalSettings.RegularExpressions.Password, ErrorMessageResourceName = "InvalidPassword", ErrorMessageResourceType = typeof(Common.Resources.Models)), Display(Name = "Password", ResourceType = typeof(Common.Resources.Models)), DataType("NewPassword")]
        public string Password { get; set; }

        [Display(Name = "ConfirmPassword", ResourceType = typeof(Common.Resources.Models))]
        [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceName = "PasswordsDoNotMatch", ErrorMessageResourceType = typeof(Common.Resources.Models)), DataType("NewPassword")]
        public string ConfirmPassword { get; set; }

        public bool IsExpiredLink { get; set; }


        [Display(Name = "Username", ResourceType = typeof(Common.Resources.Models))]
        [Required(ErrorMessageResourceName = "UsernameRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), Remote("IsValidLoginName", "Account", ErrorMessageResourceName = "UsernameUnavailable", ErrorMessageResourceType = typeof(Common.Resources.Models)), RegularExpression(GlobalSettings.RegularExpressions.LoginName, ErrorMessageResourceName = "InvalidUsername", ErrorMessageResourceType = typeof(Common.Resources.Models))]
        public string LoginName { get; set; }
    }
}