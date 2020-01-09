using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace ExigoService
{
    public static partial class ExigoDAL
    {
        public static IEnumerable<PointAccount> GetPointAccounts()
        {
            var pointAccounts = new List<PointAccount>();
            using (var context = ExigoDAL.Sql())
            {
                pointAccounts = context.Query<PointAccount>(@"
                                SELECT PointAccountID
                                      , PointAccountDescription
                                      , CurrencyCode
                                FROM PointAccounts
                    ").ToList();
            }

            if (pointAccounts == null) return null;

            return pointAccounts;

        }
        public static PointAccount GetPointAccount(int pointAccountID)
        {
            var pointAccount = GetPointAccounts()
                .Where(c => c.PointAccountID == pointAccountID)
                .FirstOrDefault();
            if (pointAccount == null) return null;

            return (PointAccount)pointAccount;
        }

        public static IEnumerable<CustomerPointAccount> GetCustomerPointAccounts(int customerID)
        {
            var pointAccounts = new List<CustomerPointAccount>();
            using (var context = ExigoDAL.Sql())
            {
                pointAccounts = context.Query<CustomerPointAccount>(@"
                                SELECT cpa.PointAccountID
                                      , cpa.CustomerID
                                      , cpa.PointBalance
	                                  , pa.PointAccountDescription
                                      , pa.CurrencyCode
                                FROM CustomerPointAccounts cpa
                                 LEFT JOIN PointAccounts pa
	                                ON cpa.PointAccountID = pa.PointAccountID
                                WHERE cpa.CustomerID = @CustomerID
                    ", new 
                     {
                         CustomerID = customerID
                     }).ToList();
            }

            if (pointAccounts == null) return null;

            return pointAccounts;
        }
        public static CustomerPointAccount GetCustomerPointAccount(int customerID, int pointAccountID)
        {
            var pointAccount = new CustomerPointAccount();
            using (var context = ExigoDAL.Sql())
            {
                pointAccount = context.Query<CustomerPointAccount>(@"
                                SELECT cpa.PointAccountID
                                      , cpa.CustomerID
                                      , cpa.PointBalance
	                                  , pa.PointAccountDescription
                                      , pa.CurrencyCode
                                FROM CustomerPointAccounts cpa
                                 LEFT JOIN PointAccounts pa
	                                ON cpa.PointAccountID = pa.PointAccountID
                                WHERE cpa.CustomerID = @CustomerID
                                    AND cpa.PointAccountID = @PointAccountID
                    ", new
                     {
                         CustomerID = customerID,
                         PointAccountID = pointAccountID
                     }).FirstOrDefault();
            }

            if (pointAccount == null) return null;

            return pointAccount;

        }
        public static bool ValidateCustomerHasPointAmount(int customerID, int pointAccountID, decimal pointAmount)
        {
            var pointAccount = GetCustomerPointAccount(customerID, pointAccountID);
            if (pointAccount == null) return false;

            return pointAccount.Balance >= pointAmount;
        }
    }
}