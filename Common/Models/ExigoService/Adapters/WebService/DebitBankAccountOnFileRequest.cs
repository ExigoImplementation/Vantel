using ExigoService;

namespace Common.Api.ExigoWebService
{
    public partial class DebitBankAccountOnFileRequest
    {
        public DebitBankAccountOnFileRequest() { }
        public DebitBankAccountOnFileRequest(BankAccount account)
        {
        }

        public static explicit operator DebitBankAccountOnFileRequest(ExigoService.BankAccount account)
        {
            var model = new DebitBankAccountOnFileRequest();
            if (account == null) return model;

            return model;
        }
    }
}