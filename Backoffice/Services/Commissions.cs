using Backoffice.Models.CommissionPayout;
using Common.Api.ExigoWebService;
using ExigoService;
using System.Web.Mvc;

namespace Backoffice.Services
{
    public class Commissions
    {
        public static CommissionPayout GetDirectDeposit()
        {
            var account = new CommissionPayout();

            try
            {
                var result = ExigoDAL.WebService().GetAccountDirectDeposit(new GetAccountDirectDepositRequest
                {
                    CustomerID = Identity.Current.CustomerID
                });

                if (result.Result.Status == ResultStatus.Success)
                {
                    account.AccountNumber = result.BankAccountNumberDisplay;
                    account.NameOnAccount = result.NameOnAccount;
                    account.BankName = result.BankName;
                    account.RoutingNumber = result.BankRoutingNumber;

                    //account.BillingAddress.Address1 = result.BankAddress;
                    //account.BillingAddress.City = result.BankCity;
                    //account.BillingAddress.State = result.BankState;
                    //account.BillingAddress.Country = result.BankCountry;
                    //account.BillingAddress.Zip = result.BankZip;
                }
            }
            catch
            {

            }

            return account;
        }

        public static bool SetDirectDeposit(CommissionPayout account)
        {
            try
            {
                var result = ExigoDAL.WebService().SetAccountDirectDeposit(new SetAccountDirectDepositRequest
                {
                    CustomerID = Identity.Current.CustomerID,
                    NameOnAccount = account.NameOnAccount,
                    BankName = account.BankName,
                    BankRoutingNumber = account.RoutingNumber,
                    BankAccountNumber = account.AccountNumber,
                    DepositAccountType = DepositAccountType.Checking,

                    //BankAddress = account.BillingAddress.Address1,
                    //BankCity = account.BillingAddress.City,
                    //BankState = account.BillingAddress.State,
                    //BankCountry = account.BillingAddress.Country,
                    //BankZip = account.BillingAddress.Zip
                });
            }
            catch
            {
                return false;
            }

            //Adds payable type and bank information to user defined fields as done in client's past
            try
            {
                var updateCustomerRequest = ExigoDAL.WebService().UpdateCustomer(new UpdateCustomerRequest
                {
                    CustomerID = Identity.Current.CustomerID,
                    PayableType = PayableType.DirectDeposit,
                });

            }
            catch
            {

                return false;
            }

            return true;
        }
    }
}