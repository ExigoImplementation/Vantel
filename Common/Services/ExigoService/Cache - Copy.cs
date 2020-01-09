using System;
using Common.Api.ExigoWebService;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace ExigoService
{
    public static partial class Exigo
    {
        /// <summary>
        /// Get an item from the HttpRuntime cache, or run the provided function and cache the results for one hour before returning it.
        /// </summary>
        /// <typeparam name="T">The type of object to return.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <param name="func">The function to execute that returns the object to cache.</param>
        /// <returns>The requested object, either pulled from the cache or created using the provided function.</returns>
        public static T GetCache<T>(string key, Func<T> func)
        {
            return GetCache(key, DateTime.Now.AddHours(1), func);
        }

        /// <summary>
        /// Get an item from the HttpRuntime cache, or run the provided function and cache the results for the provided length of time before returning it.
        /// </summary>
        /// <typeparam name="T">The type of object to return.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <param name="expiration">The amount of time to cache the object.</param>
        /// <param name="func">The function to execute that returns the object to cache.</param>
        /// <returns>The requested object, either pulled from the cache or created using the provided function.</returns>
        public static T GetCache<T>(string key, DateTime expiration, Func<T> func)
        {
            var cache = HttpRuntime.Cache;
            var result = cache[key];

            if (result == null)
            {
                result = func();
                cache.Insert(key, result, null, expiration, Cache.NoSlidingExpiration);
            }

            return (T)result;
        }
    }
}