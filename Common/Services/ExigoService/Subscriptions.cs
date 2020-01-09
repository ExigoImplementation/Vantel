using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace ExigoService
{
    public static partial class ExigoDAL
    {
        public static IEnumerable<Subscription> GetSubscriptions()
        {
            var subscriptions = new List<Subscription>();
            using (var context = ExigoDAL.Sql())
            {
                subscriptions = context.Query<Subscription>(@"
                                SELECT 
                                    SubscriptionID
                                    , SubscriptionDescription
                                FROM Subscriptions
                    ").ToList();
            }

            return subscriptions;

        }
        public static Subscription GetSubscription(int subscriptionID)
        {
            var subscriptions = new Subscription();
            using (var context = ExigoDAL.Sql())
            {
                subscriptions = context.Query<Subscription>(@"
                                SELECT
                                    SubscriptionID
                                    , SubscriptionDescription
                                FROM Subscriptions
                                WHERE SubscriptionID = @SubscriptionID
                    ", new
                     {
                         SubscriptionID = subscriptionID
                     }).FirstOrDefault();
            }

            return subscriptions;
        }

        public static IEnumerable<CustomerSubscription> GetCustomerSubscriptions(int customerID)
        {
            var subscriptions = new List<CustomerSubscription>();
            using (var context = ExigoDAL.Sql())
            {
                subscriptions = context.Query<CustomerSubscription>(@"
                                SELECT cs.SubscriptionID
                                      , cs.CustomerID
                                      , cs.IsActive
                                      , cs.StartDate
                                      , cs.ExpireDate
                                FROM CustomerSubscriptions cs
	                                LEFT JOIN Subscriptions s
		                                ON cs.SubscriptionID = s.SubscriptionID
                                WHERE cs.CustomerID = @CustomerID
                    ", new
                     {
                         CustomerID = customerID
                     }).ToList();
            }

            return subscriptions;
        }
        public static CustomerSubscription GetCustomerSubscription(int customerID, int subscriptionID)
        {
            var subscription = new CustomerSubscription();
            using (var context = ExigoDAL.Sql())
            {
                subscription = context.Query<CustomerSubscription>(@"
                                SELECT cs.SubscriptionID
                                      , cs.CustomerID
                                      , cs.IsActive
                                      , cs.StartDate
                                      , cs.ExpireDate
                                FROM CustomerSubscriptions cs
	                                LEFT JOIN Subscriptions s
		                                ON cs.SubscriptionID = s.SubscriptionID
                                WHERE cs.CustomerID = @CustomerID
	                                AND cs.SubscriptionID = @SubscriptionID
                    ", new
                     {
                         SubscriptionID = subscriptionID,
                         CustomerID = customerID
                     }).FirstOrDefault();
            }
            if (subscription == null) return null;

            return subscription;
        }
    }
}
