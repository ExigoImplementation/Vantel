using Common.Api.ExigoWebService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExigoService
{
    public static partial class ExigoDAL
    {
        private static Dictionary<PayableType, int> PayableTypeBindings
        {
            get
            {
                return new Dictionary<PayableType, int>()
                {
                    { PayableType.Check, 1 },
                    { PayableType.WireTransfer, 2 },
                    { PayableType.PaymentCard, 5 },
                    { PayableType.DirectDeposit, 8 },
                    { PayableType.OnHold, 10 },
                    { PayableType.BankWire, 11 },
                    { PayableType.DebitCardHold, 15 }
                };
            }
        }

        public static int GetPayableTypeID(PayableType payableType)
        {
            try
            {
                return PayableTypeBindings.Where(c => c.Key == payableType).FirstOrDefault().Value;
            }
            catch
            {
                throw new Exception("Corresponding int not found for PayableType {0}.".FormatWith(payableType.ToString()));
            }
        }
        public static PayableType GetPayableType(int payableTypeID)
        {
            try
            {
                return PayableTypeBindings.Where(c => c.Value == payableTypeID).FirstOrDefault().Key;
            }
            catch
            {
                throw new Exception("Corresponding PayableType not found for int {0}.".FormatWith(payableTypeID));
            }
        }
    }
}
