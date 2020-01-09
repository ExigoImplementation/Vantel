using Common;
using System;

namespace ReplicatedSite
{
    public static partial class Settings
    {
        public static bool RememberLastWebAliasVisited = true;
        public static bool AllowOrphans = true;

        public static string PreferredCustomerSubscriptionItemCode = "TEST1";

        // Triggers modal to ask how a user was referred if they land on the Product list page without an Enroller
        public static bool ShowChooseEnrollerInShopping = false;

        public static string ContactUsEmailAddress = "noreply@exigo.com";
        public static string ContactUsReplyAddress = "noreply@exigo.com";

        // To assist with cache busting on new deployments
        public static int StyleVersionNumber = DateTime.Now.Millisecond;
    }

    public enum EnrollmentType
    {
        Distributor = 1,
        PreferredCustomer = 2
    }

    public enum SearchType
    {
        webaddress = 1,
        distributorID = 2,
        distributorInfo = 3,
        zipcode = 4,
        eventcode = 5,
        eventname = 6
    }

}