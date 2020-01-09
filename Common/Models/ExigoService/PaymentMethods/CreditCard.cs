using Common;
using Common.Api.ExigoWebService;
using Common.Filters;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ExigoService
{
    public class CreditCard : ICreditCard
    {
        public CreditCard()
        {
            Type = CreditCardType.New;
            BillingAddress = new Address();
            ExpirationMonth = DateTime.Now.Month;
            ExpirationYear = DateTime.Now.Year;
            AutoOrderIDs = new int[0];
        }
        public CreditCard(CreditCardType type)
        {
            Type = type;
            BillingAddress = new Address();
            ExpirationMonth = DateTime.Now.Month;
            ExpirationYear = DateTime.Now.Year;
        }
        public CreditCard(Address billingAddress)
        {
            Type = CreditCardType.New;
            ExpirationMonth = DateTime.Now.Month;
            ExpirationYear = DateTime.Now.Year;
            AutoOrderIDs = new int[0];
            BillingAddress = billingAddress;
        }

        public bool ShippingAndBilingAddressAreTheSame { get; set; }

        public CreditCardType Type { get; set; }

        [Required(ErrorMessageResourceName = "NameOnCardRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), Display(Name = "NameOnCard", ResourceType = typeof(Common.Resources.Models))]
        public string NameOnCard { get; set; }
        
        [Required(ErrorMessageResourceName = "CardNumberRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), Display(Name = "CardNumber", ResourceType = typeof(Common.Resources.Models)), Luhn(ErrorMessageResourceName = "InvalidCardNumber", ErrorMessageResourceType = typeof(Common.Resources.Models))]
        // Added 3 to each of these to account for - in mask
        [MaxLength(19, ErrorMessageResourceName = "CardNumberTooLong", ErrorMessageResourceType = typeof(Common.Resources.Models))]
        [MinLength(16, ErrorMessageResourceName = "CardNumberTooShort", ErrorMessageResourceType = typeof(Common.Resources.Models))]
        public string CardNumber { get; set; }

        [Required(ErrorMessageResourceName = "ExpirationMonthRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), Display(Name = "ExpirationMonth", ResourceType = typeof(Common.Resources.Models))]        
        public int ExpirationMonth { get; set; }

        [Required(ErrorMessageResourceName = "ExpirationYearRequired", ErrorMessageResourceType = typeof(Common.Resources.Models)), Display(Name = "ExpirationYear", ResourceType = typeof(Common.Resources.Models))]        
        public int ExpirationYear { get; set; }

        public string Token { get; set; }
        public string Display { get; set; }



        public string GetToken()
        {
            if (!IsComplete) return string.Empty;

            // Credit Card Tokens should be retrieved via javascript using the exigopayments.js method, not on the server side.
            if (Token.IsNullOrEmpty())
            {
                throw new Exception("NO TOKEN PRESENT: Token should be retrieved on front end, review the logic that is getting the Credit Card information since the Token is not currently populated.");
            }
            else
            {
                return Token;
            }

            //return Exigo.Payments().FetchCreditCardToken(
            //    this.CardNumber,
            //    this.ExpirationMonth,
            //    this.ExpirationYear);
        }

        [Required, Display(Name = "CVV", ResourceType = typeof(Common.Resources.Models)), RegularExpression(GlobalSettings.RegularExpressions.CVV, ErrorMessageResourceName = "IncorrectCVV", ErrorMessageResourceType = typeof(Common.Resources.Models))]
        public string CVV { get; set; }

        [Required, DataType("Address")]
        public Address BillingAddress { get; set; }

        public int[] AutoOrderIDs { get; set; }

        public DateTime ExpirationDate
        {
            get { return new DateTime(this.ExpirationYear, this.ExpirationMonth, DateTime.DaysInMonth(this.ExpirationYear, this.ExpirationMonth)); }
        }

        public bool IsExpired
        {
            get { return this.ExpirationDate < DateTime.Now; }
        }
        public bool IsComplete
        {
            get
            {
                if (string.IsNullOrEmpty(NameOnCard)) return false;
                if (ExpirationMonth == 0) return false;
                if (ExpirationYear == 0) return false;
                if (!BillingAddress.IsComplete) return false;

                return true;
            }
        }
        public bool IsValid
        {
            get
            {
                if (!IsComplete) return false;
                if (IsExpired) return false;
                // This can't work because Card Number does not belong on the server side, the Token is retrieved before the Credit Card is passed to the back end.
                //if (!IsTestCreditCard && !GlobalUtilities.ValidateCreditCard(CardNumber)) return false;

                return true;
            }
        }
        public bool IsUsedInAutoOrders
        {
            get { return this.AutoOrderIDs.Length > 0; }
        }
        public bool IsTestCreditCard
        {
            get { return this.Display == "9696" || this.CardNumber == "9696969696969696"; }
        }

        public AutoOrderPaymentType AutoOrderPaymentType
        {
            get
            {
                switch(this.Type)
                {
                    case CreditCardType.Primary:
                    default: return Common.Api.ExigoWebService.AutoOrderPaymentType.PrimaryCreditCard;

                    case CreditCardType.Secondary: return Common.Api.ExigoWebService.AutoOrderPaymentType.SecondaryCreditCard;
                }
            }
        }
    }
}