using Common.Models;
using System;
using System.Web;

namespace Common.Helpers
{
    public static class IdentityCacheHelper
    {
        public static T Get<T>(string key, int customerID, int expiration = 15) where T : IIdentityCacheable
        {
            key += customerID.ToString();
            var type = typeof(T);
            var model = HttpContext.Current.Cache[key];
            if (model == null)
            {
                model = (T)Activator.CreateInstance(typeof(T));
                ((T)model).CacheKey = key;
                ((T)model).Initialize(customerID);

                HttpContext.Current.Cache.Insert(key,
                        model,
                        null,
                        DateTime.Now.AddMinutes(expiration),
                        System.Web.Caching.Cache.NoSlidingExpiration,
                        System.Web.Caching.CacheItemPriority.Normal,
                        null);
            }
            dynamic typedModel = Convert.ChangeType(model, type);
            return typedModel;
        }
    }
}