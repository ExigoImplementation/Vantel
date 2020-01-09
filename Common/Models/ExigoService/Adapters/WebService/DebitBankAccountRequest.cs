using ExigoService;

namespace Common.Api.ExigoWebService
{
    public partial class DebitBankAccountRequest
    {
        public DebitBankAccountRequest() { }
        public DebitBankAccountRequest(BankAccount account)
        {
            NameOnAccount     = account.NameOnAccount;
            BankName          = account.BankName;
            BankAccountNumber = account.AccountNumber;
            BankRoutingNumber = account.RoutingNumber;
            BankAccountType   = ExigoWebService.BankAccountType.CheckingPersonal;

            BillingAddress = account.BillingAddress.AddressDisplay;
            BillingCity    = account.BillingAddress.City;
            BillingState   = account.BillingAddress.State;
            BillingZip     = account.BillingAddress.Zip;
            BillingCountry = account.BillingAddress.Country;
        }

        public static explicit operator DebitBankAccountRequest(ExigoService.BankAccount account)
        {
            var model = new DebitBankAccountRequest();
            if (account == null) return model;

            model.NameOnAccount     = account.NameOnAccount;
            model.BankName          = account.BankName;
            model.BankAccountNumber = account.AccountNumber;
            model.BankRoutingNumber = account.RoutingNumber;
            model.BankAccountType   = ExigoWebService.BankAccountType.CheckingPersonal;

            model.BillingAddress    = account.BillingAddress.AddressDisplay;
            model.BillingCity       = account.BillingAddress.City;
            model.BillingState      = account.BillingAddress.State;
            model.BillingZip        = account.BillingAddress.Zip;
            model.BillingCountry    = account.BillingAddress.Country;

            return model;
        }
    }
}