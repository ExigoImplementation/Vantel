using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public class CreatePartyPaymentRequest
    {
        public CreatePartyPaymentRequest()
        {
            this.Payments = new List<PartyPaymentRequest>();
        }

        public CreatePartyPaymentRequest(int partyID)
        {
            this.PartyID = partyID;
        }

        public int PartyID { get; set; }
        public CreditCard NewCard { get; set; }
        public string PaymentType { get; set; }
        public int MasterOrderID { get; set; }

        public List<PartyPaymentRequest> Payments { get; set; }

        public bool IsStagedPayment()
        {
            return this.PaymentType.Contains("staged");
        }
        public bool IsCardOnFilePayment()
        {
            return this.PaymentType.Contains("cardonfile");
        }
        public bool IsNewCardPayment()
        {
            return !this.IsCardOnFilePayment() && !this.IsStagedPayment();
        }

        public int StagedPartyPaymentID {
            get 
            {
                if (this.IsStagedPayment())
                {
                    return Convert.ToInt32(this.PaymentType.Replace("staged-", ""));
                }
                else
                {
                    return 0;
                }
            }
        }
    }

    public class PartyPaymentRequest
    {
        public int OrderID { get; set; }
        public decimal Amount { get; set; }
        public bool IsHostessOrder { get; set; }
    }
}