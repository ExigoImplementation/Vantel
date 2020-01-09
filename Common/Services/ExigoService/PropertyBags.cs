using System;
using System.Web;
using Newtonsoft.Json;
using Common;
using Caching;

namespace ExigoService
{
    public static partial class ExigoDAL
    {
        public static class PropertyBags
        {
            private static readonly int DbExpire = GlobalSettings.Exigo.UserSession.DbExpireSessionTaskMilliSecDelay;

            private static void SetSessionData(string sessionID, string data)
            {
                Cache.Set(sessionID, TimeSpan.FromMilliseconds(DbExpire), data);
            }

            private static string GetSessionData(string sessionID)
            {
                var sessionData = Cache.Get<string>(sessionID);
                return sessionData;
            }

            public static T Get<T>(string description) where T : IPropertyBag
            {
                // Attempt to load the bag from the cookie
                var cookie = HttpContext.Current.Request.Cookies[description];
                if (cookie == null)
                {
                    return Create<T>(description);
                }

                string sessionData = "";

                sessionData = GetSessionData(cookie.Value);

                if (string.IsNullOrEmpty(sessionData))
                {
                    return Create<T>(description);
                }

                try
                {
                    // Deserialize the session data and get our bag.
                    dynamic bag = Deserialize<T>(sessionData);

                    // If the customer ID in the bag doesn't match the current customer ID, stop here.
                    if (!bag.IsValid())
                    {
                        return Create<T>(description);
                    }

                    // If we got here, we have a valid property bag. Populate it into the current object.
                    return bag;
                }
                catch
                {
                    return Create<T>(description);
                }
            }
            public static T Create<T>(string description) where T : IPropertyBag
            {
                dynamic bag = Activator.CreateInstance(typeof(T));

                bag.SessionID = HttpContext.Current.Server.UrlEncode(Guid.NewGuid().ToString());
                bag.Description = description;
                bag.CreatedDate = DateTime.Now;
                bag = bag.OnBeforeUpdate(bag);

                Update<T>(bag);

                return bag;
            }
            public static T Update<T>(T propertyBag) where T : IPropertyBag
            {
                // Set the session
                SetSessionData(propertyBag.SessionID, Serialize<T>(propertyBag));

                // Set the cookie
                var cookie = new HttpCookie(propertyBag.Description, propertyBag.SessionID);
                if (propertyBag.Expires > 0)
                {
                    cookie.Expires = DateTime.Now.AddMinutes(propertyBag.Expires);
                }
                HttpContext.Current.Response.Cookies.Add(cookie);


                return propertyBag;
            }
            public static T Delete<T>(T propertyBag) where T : IPropertyBag
            {
                var bag = Create<T>(propertyBag.Description);
                return bag;
            }
            public static string Serialize<T>(T propertyBag) where T : IPropertyBag
            {
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects
                };

                return JsonConvert.SerializeObject(propertyBag, settings);
            }
            public static T Deserialize<T>(string sessionData) where T : IPropertyBag
            {
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects
                };

                return (T)JsonConvert.DeserializeObject(sessionData, typeof(T), settings);
            }
        }
    }
}