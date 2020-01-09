using System.Linq;
using System.Reflection;
using System.Web;

namespace Common.Services
{
    public static class CommonResources
    {
        public static string Countries(string countryCode, CommonResourceFormat format = CommonResourceFormat.Default)
        {
            return GetCommonResource(countryCode, 2, "Country", "Countries", format);
        }
        public static string ResourceTypes(string resourceDescription, CommonResourceFormat format = CommonResourceFormat.Default)
        {
            var key = resourceDescription.Replace("&", "").Replace(" ", "").Replace("/", "").ToLower();
            return GetCommonResource(key, 2, "ResourceType", "ResourceTypes", format, resourceDescription);
        }
        public static string Languages(int languageID, CommonResourceFormat format = CommonResourceFormat.Default)
        {
            return GetCommonResource(languageID, 3, "Language", "Languages", format);
        }
        public static string Bonuses(int periodTypeID, int bonusID, CommonResourceFormat format = CommonResourceFormat.Default)
        {
            var resourceKeyPrefix = "Bonus";
            var padding = 3;

            // Build our resource key
            var resourceKey = "{0}_{1}_{2}".FormatWith(resourceKeyPrefix, periodTypeID, bonusID.ToString().PadLeft(padding, '0'));
            if (format != CommonResourceFormat.Default) resourceKey += "_{0}".FormatWith(format.ToString());

            // Get the resource
            var resource = GetGlobalResource("Common", resourceKey, "{0} {1} {2}".FormatWith(resourceKeyPrefix, periodTypeID, bonusID));

            // If the resource is null and we asked for a specific format, get the default format instead.
            if (resource.IsNullOrEmpty() && format != CommonResourceFormat.Default)
            {
                return Bonuses(periodTypeID, bonusID, CommonResourceFormat.Default);
            }
            else
            {
                return resource;
            }
        }
        public static string CreditCardTypes(int creditCardTypeID, CommonResourceFormat format = CommonResourceFormat.Default)
        {
            return GetCommonResource(creditCardTypeID, 2, "CreditCardType", "CreditCardTypes", format);
        }
        public static string CustomerTypes(int customerTypeID, CommonResourceFormat format = CommonResourceFormat.Default)
        {
            return GetCommonResource(customerTypeID, 2, "CustomerType", "CustomerTypes", format);
        }
        public static string CustomerStatuses(int customerStatusID, CommonResourceFormat format = CommonResourceFormat.Default)
        {
            return GetCommonResource(customerStatusID, 2, "CustomerStatus", "CustomerStatuses", format);
        }
        public static string OrderStatuses(int orderStatusID, CommonResourceFormat format = CommonResourceFormat.Default)
        {
            return GetCommonResource(orderStatusID, 2, "OrderStatus", "OrderStatuses", format);
        }
        public static string Ranks(int rankID, CommonResourceFormat format = CommonResourceFormat.Default, string defaultDescription = "")
        {
            return GetCommonResource(rankID, 2, "Rank", "Ranks", format, defaultDescription);
        }
        public static string Volumes(int volumeID, CommonResourceFormat format = CommonResourceFormat.Default)
        {
            return GetCommonResource(volumeID, 3, "Volume", "Volumes", format);
        }
        public static string VolumeDescriptions(int volumeID, CommonResourceFormat format = CommonResourceFormat.Default)
        {
            return GetCommonResource(volumeID, 3, "Volume_Description", "Volume_Description", format);
        }
        public static string FrequencyTypes(int frequencyTypeID, CommonResourceFormat format = CommonResourceFormat.Default)
        {
            return GetCommonResource(frequencyTypeID, 2, "FrequencyType", "FrequencyTypes", format);
        }
        public static string PaymentTypes(int paymentTypeID, CommonResourceFormat format = CommonResourceFormat.Default)
        {
            return GetCommonResource(paymentTypeID, 2, "PaymentType", "PaymentTypes", format);
        }
        public static string PayableTypes(int payableTypeID, CommonResourceFormat format = CommonResourceFormat.Default)
        {
            return GetCommonResource(payableTypeID, 2, "PayableType", "PayableTypes", format);
        }
        public static string PeriodTypes(int periodTypeID, CommonResourceFormat format = CommonResourceFormat.Default)
        {
            return GetCommonResource(periodTypeID, 3, "PeriodType", "PeriodTypes", format);
        }
        public static string PointAccounts(int pointAccountID, CommonResourceFormat format = CommonResourceFormat.Default)
        {
            return GetCommonResource(pointAccountID, 2, "PointAccount", "PointAccounts", format);
        }
        public static string Subscriptions(int subscriptionID, CommonResourceFormat format = CommonResourceFormat.Default)
        {
            return GetCommonResource(subscriptionID, 2, "Subscription", "Subscriptions", format);
        }


        private static string GetCommonResource(object id, int padding, string resourceKeyPrefix, string classKey, CommonResourceFormat format = CommonResourceFormat.Default, string defaultDescription = "")
        {
            if (id == null) id = string.Empty;

            // Build our resource key
            var resourceKey = "{0}_{1}".FormatWith(resourceKeyPrefix, id.ToString().PadLeft(padding, '0'));

            if (format != CommonResourceFormat.Default)
            {
                resourceKey += "_{0}".FormatWith(format.ToString());
            }

            // Get the resource
            var fallback = (!defaultDescription.IsNullOrEmpty()) ? defaultDescription : (format == CommonResourceFormat.Default) ? "{0} {1}".FormatWith(resourceKeyPrefix, id) : string.Empty;


            var resource = GetReflectionResource(resourceKey, fallback);

            // If the resource is null and we asked for a specific format, get the default format instead.
            if (resource.IsNullOrEmpty() && format != CommonResourceFormat.Default)
            {
                return GetCommonResource(id, padding, resourceKeyPrefix, "Common");
            }
            else
            {
                return resource;
            }
        }

        private static string GetReflectionResource(string key, string fallback)
        {
            var assembly = System.Configuration.ConfigurationManager.AppSettings["ResourceSet.Assembly"];
            System.Type props = null;
            try
            {
                props = Assembly.Load(assembly).GetType("Resources.Common");
            }
            catch { }
            if (props == null) return fallback;

            var item = props.GetProperties().FirstOrDefault(n => n.Name == key);

            if (item != null)
                return item.GetValue(props).ToString().IsNullOrEmpty() ? fallback : (string)item.GetValue(props);

            return fallback;
        }

        private static string GetGlobalResource(string classKey, string resourceKey, string fallback)
        {
            try
            {
                var result = HttpContext.GetGlobalResourceObject(classKey, resourceKey);

                if (result != null) return (string)result;
                else return fallback;
            }
            catch
            {
                return fallback;
            }
        }
    }

    public enum CommonResourceFormat
    {
        Default,
        Short
    }
}