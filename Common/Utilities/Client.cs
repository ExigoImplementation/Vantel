using System;
using System.Web;

namespace Common
{
    public static partial class GlobalUtilities
    {
        /// <summary>
        /// Returns the client's IP address, or (localhost) if there isn't one.
        /// </summary>
        /// <returns>The cleint's IP address</returns>
        public static string GetClientIP()
        {
            var ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (ip.Equals("::1", StringComparison.InvariantCultureIgnoreCase)) ip = "127.0.0.1";

            return ip;
        }
    }
}