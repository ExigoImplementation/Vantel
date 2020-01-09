using System.ComponentModel.DataAnnotations;
using Common;
using ExigoService;

namespace ReplicatedSite.ViewModels
{
    public class ContactViewModel
    {
        public string Name { get; set; }
        public string Phone { get; set; }

        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), DataType(DataType.EmailAddress), RegularExpression(GlobalSettings.RegularExpressions.EmailAddresses, ErrorMessageResourceName = "IncorrectEmail", ErrorMessageResourceType = typeof(Common.Resources.Models)), Display(Name = "Email", ResourceType = typeof(Common.Resources.Models))]
        public string Email { get; set; }
        public string Notes { get; set; }
        public string SiteOwnerNotes { get; set; }
    }
}