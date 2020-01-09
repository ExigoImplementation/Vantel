using System;
using System.Collections.Generic;
using System.Linq;

namespace ExigoService
{
    public static partial class ExigoDAL
    {
        private static Dictionary<Common.Api.ExigoWebService.PaymentType, int> PaymentTypeBindings
        {
            get
            {
                return new Dictionary<Common.Api.ExigoWebService.PaymentType, int>()
                {
                    { Common.Api.ExigoWebService.PaymentType.Cash, 0 },
                    { Common.Api.ExigoWebService.PaymentType.CreditCard, 1 },
                    { Common.Api.ExigoWebService.PaymentType.Check, 2 },
                    { Common.Api.ExigoWebService.PaymentType.CreditVoucher, 3 },
                    { Common.Api.ExigoWebService.PaymentType.Net30, 4 },
                    { Common.Api.ExigoWebService.PaymentType.Net60, 5 },
                    { Common.Api.ExigoWebService.PaymentType.UseCredit, 6 },
                    { Common.Api.ExigoWebService.PaymentType.ACHDebit, 7 },
                    { Common.Api.ExigoWebService.PaymentType.BankDraft, 8 },
                    { Common.Api.ExigoWebService.PaymentType.BankWire, 9 },
                    { Common.Api.ExigoWebService.PaymentType.PointRedemtion, 10 },
                    { Common.Api.ExigoWebService.PaymentType.COD, 11 },
                    { Common.Api.ExigoWebService.PaymentType.MoneyOrder, 12 },
                    { Common.Api.ExigoWebService.PaymentType.BankDeposit, 13 },
                    { Common.Api.ExigoWebService.PaymentType.Other1, 14 },
                    { Common.Api.ExigoWebService.PaymentType.Other2, 15 },
                    { Common.Api.ExigoWebService.PaymentType.Other3, 16 },
                    { Common.Api.ExigoWebService.PaymentType.Wallet, 17 }
                };
            }
        }

        public static int GetPaymentTypeID(Common.Api.ExigoWebService.PaymentType paymentType)
        {
            try
            {
                return PaymentTypeBindings.Where(c => c.Key == paymentType).FirstOrDefault().Value;
            }
            catch
            {
                throw new Exception("Corresponding int not found for PaymentType {0}.".FormatWith(paymentType.ToString()));
            }
        }

        // Based on the IPaymentMethod type, we return the appropriate ExigoWebService PaymentType int value
        public static int GetPaymentTypeID(IPaymentMethod paymentMethod)
        {
            var paymentMethodType = 0;

            if (paymentMethod.CanBeParsedAs<CreditCard>())
            {
                // Common.Api.ExigoWebService.PaymentType.CreditCard
                paymentMethodType = PaymentTypeBindings.Where(c => c.Key == Common.Api.ExigoWebService.PaymentType.CreditCard).FirstOrDefault().Value; 
            }

            if (paymentMethod.CanBeParsedAs<BankAccount>())
            {
                // Common.Api.ExigoWebService.PaymentType.ACHDebit
                paymentMethodType = PaymentTypeBindings.Where(c => c.Key == Common.Api.ExigoWebService.PaymentType.ACHDebit).FirstOrDefault().Value;
            }

            return paymentMethodType;
        }
        public static Common.Api.ExigoWebService.PaymentType GetPaymentType(int paymentTypeID)
        {
            try
            {
                return PaymentTypeBindings.Where(c => c.Value == paymentTypeID).FirstOrDefault().Key;
            }
            catch
            {
                throw new Exception("Corresponding PaymentType not found for int {0}.".FormatWith(paymentTypeID));
            }
        }
    }
}
