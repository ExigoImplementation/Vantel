using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Api.ExigoWebService;
using Dapper;

namespace ExigoService
{
    public static partial class ExigoDAL
    {
        public static IEnumerable<AutoOrder> GetCustomerAutoOrders(int customerid, int? autoOrderID = null, bool includePaymentMethods = true)
        {
            var autoOrders = new List<AutoOrder>();
            var detailItemCodes = new List<string>();

            var request = new GetAutoOrdersRequest
            {
                CustomerID = customerid,
                AutoOrderStatus = AutoOrderStatusType.Active
            };

            if (autoOrderID != null)
            {
                request.AutoOrderID = (int)autoOrderID;
            }

            var aoResponse = ExigoDAL.WebService().GetAutoOrders(request);

            if (!aoResponse.AutoOrders.Any()) return autoOrders;

            foreach (var aor in aoResponse.AutoOrders)
            {
                autoOrders.Add((AutoOrder)aor);
            }

            detailItemCodes = autoOrders.SelectMany(a => a.Details.Select(d => d.ItemCode)).Distinct().ToList();


            var autoOrderIds = autoOrders.Select(a => a.AutoOrderID).ToList();
            var createdDateNodes = new List<AutoOrderCreatedDate>();
            var aoDetailInfo = new List<AutoOrderDetailInfo>();

            using (var context = ExigoDAL.Sql())
            {
                var nodeResults = context.QueryMultiple(@"
                    SELECT
                        AutoOrderID,
                        CreatedDate
                    FROM
                        AutoOrders
                    WHERE
                        AutoOrderID in @ids

                    SELECT
                        ItemCode,
                        SmallImageName,
                        IsVirtual
                    FROM Items
                    WHERE ItemCode in @itemcodes
                    ",
                    new
                    {
                        ids = autoOrderIds,
                        itemcodes = detailItemCodes
                    });

                createdDateNodes = nodeResults.Read<AutoOrderCreatedDate>().ToList();
                aoDetailInfo = nodeResults.Read<AutoOrderDetailInfo>().ToList();
            }

            foreach (var ao in autoOrders)
            {
                ao.CreatedDate = createdDateNodes.Where(n => n.AutoOrderID == ao.AutoOrderID).Select(n => n.CreatedDate).FirstOrDefault();

                foreach (var detail in ao.Details)
                {
                    var detailInfo = aoDetailInfo.Where(i => i.ItemCode == detail.ItemCode).FirstOrDefault();
                    detail.ImageUrl = GlobalUtilities.GetProductImagePath(detailInfo.ImageUrl);
                    detail.IsVirtual = detailInfo.IsVirtual;
                }
            }

            if (includePaymentMethods)
            {
                // Add payment methods
                var paymentMethods = GetCustomerPaymentMethods(new GetCustomerPaymentMethodsRequest
                {
                    CustomerID = customerid
                });

                foreach (var autoOrder in autoOrders)
                {
                    IPaymentMethod paymentMethod;
                    switch (autoOrder.AutoOrderPaymentTypeID)
                    {
                        case 1: paymentMethod = paymentMethods.Where(c => c is CreditCard && ((CreditCard)c).Type == CreditCardType.Primary).FirstOrDefault(); break;
                        case 2: paymentMethod = paymentMethods.Where(c => c is CreditCard && ((CreditCard)c).Type == CreditCardType.Secondary).FirstOrDefault(); break;
                        case 3: paymentMethod = paymentMethods.Where(c => c is BankAccount && ((BankAccount)c).Type == BankAccountType.Primary).FirstOrDefault(); break;
                        default: paymentMethod = null; break;
                    }
                    autoOrder.PaymentMethod = paymentMethod;
                }
            }

            return autoOrders;
        }                

        public static string GetAutoOrderScheduleSummary(DateTime startDate, int frequencyTypeID)
        {
            return GetAutoOrderScheduleSummary(startDate, GetFrequencyType(frequencyTypeID));
        }
        public static string GetAutoOrderScheduleSummary(DateTime startDate, FrequencyType frequencyType)
        {
            var result = string.Empty;

            switch (frequencyType)
            {
                case FrequencyType.Weekly: result = string.Format("Every {0}", startDate.DayOfWeek); break;
                case FrequencyType.BiWeekly: result = string.Format("Every other {0}", startDate.DayOfWeek); break;
                case FrequencyType.EveryFourWeeks: result = "Every 28 days"; break;
                case FrequencyType.EverySixWeeks: result = "Every 6 weeks"; break;
                case FrequencyType.EveryEightWeeks: result = "Every 8 weeks"; break;
                case FrequencyType.EveryTwelveWeeks: result = "Every 12 weeks"; break;
                case FrequencyType.Monthly: result = string.Format("{0} of each month", startDate.Day.AsOrdinal()); break;
                case FrequencyType.BiMonthly: result = string.Format("{0} of every other month", startDate.Day.AsOrdinal()); break;
                case FrequencyType.Quarterly: result = string.Format("{0} of the month, every 3 months", startDate.Day.AsOrdinal()); break;
                case FrequencyType.SemiYearly: result = string.Format("{0} {1} and {2} {3}", startDate.ToString("MMMM"), startDate.Day.AsOrdinal(), startDate.AddMonths(6).ToString("MMMM"), startDate.AddMonths(6).Day.AsOrdinal()); break;
                case FrequencyType.Yearly: result = string.Format("{0} {1}", startDate.ToString("MMMM"), startDate.Day.AsOrdinal()); break;
                default: break;
            }

            return result;
        }

        public static void DeleteCustomerAutoOrder(int customerID, int autoOrderID)
        {
            // Make sure the autoorder exists           
            if (!IsValidAutoOrderID(customerID, autoOrderID)) return;

            // Cancel the autoorder
            var response = ExigoDAL.WebService().ChangeAutoOrderStatus(new ChangeAutoOrderStatusRequest
            {
                AutoOrderID = autoOrderID,
                AutoOrderStatus = AutoOrderStatusType.Deleted
            });
        }
        public static bool IsValidAutoOrderID(int customerID, int autoOrderID, bool showOnlyActiveAutoOrders = false)
        {
            var includeCancelled = "";

            if (showOnlyActiveAutoOrders)
            {
                includeCancelled = "AND a.AutoOrderStatusID = 0";
            }

            dynamic autoOrder;

            using (var context = ExigoDAL.Sql())
            {
                autoOrder = context.Query<dynamic>(@"
                        SELECT 	                        
	                        a.AutoOrderID

                        FROM
	                        AutoOrders a

                        WHERE
	                        a.CustomerID = @customerid
	                        AND a.AutoOrderID = @autoorderid	
                            " + includeCancelled, new
                              {
                                  customerid = customerID,
                                  autoorderid = autoOrderID
                              }).FirstOrDefault();
            }

            return autoOrder != null;
        }


        public static bool IsValidAutoOrderProcessDate(DateTime date)
        {
            // Ensure we are not returning a day of 28, 29, 30 or 31.
            if (date.Day >= 28)
            {
                return false;
            }

            return true;
        }
        public static DateTime GetNextAvailableAutoOrderStartDate(DateTime date)
        {
            while (!IsValidAutoOrderProcessDate(date))
            {
                date = date.AddDays(1);
            }

            return date.BeginningOfDay();
        }

        private static Dictionary<AutoOrderPaymentType, int> AutoOrderPaymentTypeBindings
        {
            get
            {
                return new Dictionary<AutoOrderPaymentType, int>
                {
                    { AutoOrderPaymentType.PrimaryCreditCard, AutoOrderPaymentTypes.PrimaryCreditCardOnFile },
                    { AutoOrderPaymentType.SecondaryCreditCard, AutoOrderPaymentTypes.SecondaryCreditCardOnFile },
                    { AutoOrderPaymentType.CheckingAccount, AutoOrderPaymentTypes.DebitCheckingAccount },
                    { AutoOrderPaymentType.WillSendPayment, AutoOrderPaymentTypes.CustomerWillSendPayment },
                    { AutoOrderPaymentType.PrimaryWalletAccount, AutoOrderPaymentTypes.PrimaryWalletAccount },
                    { AutoOrderPaymentType.SecondaryWalletAccount, AutoOrderPaymentTypes.SecondaryWalletAccount }
                };
            }
        }
        public static AutoOrderPaymentType GetAutoOrderPaymentType(int autoOrderPaymentTypeID)
        {
            try
            {
                return AutoOrderPaymentTypeBindings.FirstOrDefault(c => c.Value == autoOrderPaymentTypeID).Key;
            }
            catch
            {
                throw new Exception("Corresponding AutoOrderPaymentType not found for int {0}.".FormatWith(autoOrderPaymentTypeID));
            }
        }
        public static AutoOrderPaymentType GetAutoOrderPaymentType(IPaymentMethod paymentMethod)
        {
            if (!(paymentMethod is IAutoOrderPaymentMethod)) throw new Exception("The provided payment method does not implement IAutoOrderPaymentMethod.");

            if (paymentMethod is CreditCard) return ((CreditCard)paymentMethod).AutoOrderPaymentType;
            if (paymentMethod is BankAccount) return ((BankAccount)paymentMethod).AutoOrderPaymentType;

            return AutoOrderPaymentType.WillSendPayment;
        }
        public static int GetAutoOrderPaymentTypeID(AutoOrderPaymentType autoOrderPaymentType)
        {
            try
            {
                return AutoOrderPaymentTypeBindings.FirstOrDefault(c => c.Key == autoOrderPaymentType).Value;
            }
            catch
            {
                throw new Exception("Corresponding int not found for AutoOrderPaymentType {0}.".FormatWith(autoOrderPaymentType.ToString()));
            }
        }

        private static Dictionary<AutoOrderProcessType, int> AutoOrderProcessTypeBindings
        {
            get
            {
                return new Dictionary<AutoOrderProcessType, int>
                {
                    { AutoOrderProcessType.AlwaysProcess, AutoOrderProcessTypes.AlwaysProcess },
                    { AutoOrderProcessType.Conditional, AutoOrderProcessTypes.Conditional }
                };
            }
        }
        public static int GetAutoOrderProcessTypeID(AutoOrderProcessType autoOrderProcessType)
        {
            try
            {
                return AutoOrderProcessTypeBindings.FirstOrDefault(c => c.Key == autoOrderProcessType).Value;
            }
            catch
            {
                throw new Exception("Corresponding int not found for AutoOrderProcessType {0}.".FormatWith(autoOrderProcessType.ToString()));
            }
        }
        public static AutoOrderProcessType GetAutoOrderProcessType(int autoOrderProcessTypeID)
        {
            try
            {
                return AutoOrderProcessTypeBindings.FirstOrDefault(c => c.Value == autoOrderProcessTypeID).Key;
            }
            catch
            {
                throw new Exception("Corresponding AutoOrderProcessType not found for int {0}.".FormatWith(autoOrderProcessTypeID));
            }
        }

        public class AutoOrderCreatedDate
        {
            public int AutoOrderID { get; set; }
            public DateTime CreatedDate { get; set; }
        }

        public class AutoOrderDetailInfo
        {
            public string ItemCode { get; set; }
            public bool IsVirtual { get; set; }
            public string ImageUrl { get; set; }
        }
    }
}