using System.Web.Mvc;

namespace Common.HtmlHelpers
{
    public static class ImageHtmlHelpers
    {
        /// <summary>
        /// Gets the URL of the provided customer's avatar photo. Defaults to the current backoffice owner if no CustomerID is provided.
        /// </summary>
        /// <param name="helper">The UrlHelper object</param>
        /// <param name="CustomerID">The customer ID of the desired avatar URL.</param>
        /// <returns>The avatar photo's URL.</returns>
        public static string Avatar(this UrlHelper helper, int customerID, bool cache = true)
        {
            return Avatar(helper, customerID, AvatarType.Default, cache);
        }

        /// <summary>
        /// Gets the URL of the provided customer's avatar photo. Defaults to the current backoffice owner if no CustomerID is provided.
        /// </summary>
        /// <param name="helper">The UrlHelper object</param>
        /// <param name="CustomerID">The customer ID of the desired avatar URL.</param>
        /// <returns>The avatar photo's URL.</returns>
        public static string Avatar(this UrlHelper helper, int customerID, AvatarType type, bool cache = true)
        {
            return helper.Action("Avatar", "App", new { id = customerID, type = type, cache = cache });
        }


        /// <summary>
        /// Never Cached, so only use on Manage Avatar page. Gets the URL of the provided customer's avatar photo. Defaults to the current backoffice owner if no CustomerID is provided.
        /// </summary>
        /// <param name="helper">The UrlHelper object</param>
        /// <param name="CustomerID">The customer ID of the desired avatar URL.</param>
        /// <returns>The avatar photo's URL.</returns>
        public static string RealtimeAvatar(this UrlHelper helper, int customerID, AvatarType type, bool cache = true)
        {
            return helper.Action("RealtimeAvatar", "App", new { id = customerID, type = type, cache = cache });
        }
    }
}