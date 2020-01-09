using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Common
{
    public static partial class GlobalUtilities
    {
        /// <summary>
        /// Clear cache for specific cache Key 
        /// </summary>
        /// <param name="cacheKey"></param>
        public static void DeleteFromCache(string cacheKey)
        {
            if (HttpRuntime.Cache[cacheKey] != null)
            {
                HttpRuntime.Cache.Remove(cacheKey);
            }

        }
    }
}