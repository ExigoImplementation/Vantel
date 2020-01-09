using Common;
using Common.Api.ExigoWebService;
using System.ComponentModel.DataAnnotations;

namespace ExigoService
{
    public class BankAccount : IBankAccount
    {
        public BankAccount()
        {
            this.Type = BankAccountType.New;
            this.BillingAddress = new Address();
            this.AutoOrderIDs = new int[0];
        }
        public BankAccount(BankAccountType type)
        {
            Type = type;
            BillingAddress = new Address();
        }

        [Required]
        public BankAccountType Type { get; set; }

        [Required(ErrorMessageResourceName = "BankAccountNameRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), Display(Name = "NameOnAccount", ResourceType = typeof(Common.Resources.Models))]
        public string NameOnAccount { get; set; }

        [Required(ErrorMessageResourceName = "BankNameRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), Display(Name = "BankName", ResourceType = typeof(Common.Resources.Models))]
        public string BankName { get; set; }

        [Required(ErrorMessageResourceName = "AccountNumberRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), Display(Name = "AccountNumber", ResourceType = typeof(Common.Resources.Models)), RegularExpression(GlobalSettings.RegularExpressions.BankAccountNumber, ErrorMessageResourceName = "IncorrectAccountNumber", ErrorMessageResourceType = typeof(Common.Resources.Models))]
        public string AccountNumber { get; set; }

        [Required(ErrorMessageResourceName = "RoutingNumberRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), Display(Name = "RoutingNumber", ResourceType = typeof(Common.Resources.Models)), RegularExpression(GlobalSettings.RegularExpressions.BankRoutingNumber, ErrorMessageResourceName = "IncorrectRoutingNumber", ErrorMessageResourceType = typeof(Common.Resources.Models))]        
        public string RoutingNumber { get; set; }

        [Required, DataType("Address")]
        public Address BillingAddress { get; set; }

        public int[] AutoOrderIDs { get; set; }

        public bool IsComplete
        {
            get
            {
                if (string.IsNullOrEmpty(NameOnAccount)) return false;
                if (string.IsNullOrEmpty(BankName)) return false;
                if (string.IsNullOrEmpty(AccountNumber)) return false;
                if (string.IsNullOrEmpty(RoutingNumber)) return false;
                if (!BillingAddress.IsComplete) return false;

                return true;
            }
        }
        public bool IsValid
        {
            get
            {
                if (!IsComplete) return false;

                return true;
            }
        }
        public bool IsUsedInAutoOrders
        {
            get { return this.AutoOrderIDs.Length > 0; }
        }

        public AutoOrderPaymentType AutoOrderPaymentType
        {
            get
            {
                switch (this.Type)
                {
                    case BankAccountType.Primary:
                    default: return Common.Api.ExigoWebService.AutoOrderPaymentType.CheckingAccount;
                }
            }
        }
    }
}