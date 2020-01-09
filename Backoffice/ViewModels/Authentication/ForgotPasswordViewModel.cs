using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Backoffice.ViewModels
{
    public class ForgotPasswordViewModel
    {

        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), Display(Name = "Email", ResourceType = typeof(Common.Resources.Models))]
        public string Email { get; set; }

        public int CustomerID { get; set; }
    }
}