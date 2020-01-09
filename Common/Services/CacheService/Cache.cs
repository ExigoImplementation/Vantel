/******************************************************
 * Version 1.0
 * 
 * General purpose caching utilities
 * 
 * Supports both redis and sql in-memory table based
 * caches.
 ******************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Caching
{
    #region Configuration
    public static class CacheConfig
    {
        private static ICacheProvider _cacheProvider;
        private static TimeSpan? _defaultExpiry;

        public static void RegisterCache(
            ICacheProvider cacheProvider,
            TimeSpan? defaultExpiry = null
            )
        {
            _defaultExpiry = defaultExpiry ?? TimeSpan.FromMinutes(10);
            _cacheProvider = cacheProvider;
            _cacheProvider.Initialize();
        }

        internal static ICacheProvider Provider => _cacheProvider;
        internal static TimeSpan DefaultExpiry => _defaultExpiry ?? TimeSpan.FromMinutes(10);
    }
    #endregion

    #region Service

    /// <summary>
    /// Wrapper for DataCacheService
    /// </summary>
    public static class Cache
    {
        static CacheService _svc = new CacheService();

        public static T Get<T>(string key, TimeSpan expiry, Func<T> command)
        {
            return _svc.Get(key, expiry, command);
        }
        public static void Set<T>(string key, TimeSpan expiry, T data)
        {
            _svc.Set(key, expiry, data);
        }

        public static T Get<T>(string key)
        {
            return _svc.Get<T>(key);
        }
    }

    //we keep this service as an instance class as we may want to use different ones together
    public class CacheService : ICache
    {
        private ICacheProvider _clientCacheProvider;


        public CacheService()
        {
            _clientCacheProvider = CacheConfig.Provider;
        }
        public CacheService(ICacheProvider clientCacheProvider)
        {
            _clientCacheProvider = clientCacheProvider;
        }

        public T Get<T>(string key, long secondsToLive, Func<T> command)
        {
            return Get(key, TimeSpan.FromSeconds(Convert.ToDouble(secondsToLive)), command);
        }

        public T Get<T>(string key, TimeSpan expiry, Func<T> command)
        {
            if (_clientCacheProvider == null)
            {
                command.Invoke();
            }

            DateTime entryDate;
            DateTime serverDate;


            T result = default(T);
            // Try to get the data
            if (_clientCacheProvider.TryGet<T>(key, out entryDate, out serverDate, out result))
            {
                // Looks like we have data!
                // If the data is expired, but we got it back from the server, the Purge process hasn't picked up on it yet.
                // Let's go ahead and return what we have, but silently refetch the data in the background.
                if (entryDate.Add(expiry) < serverDate)
                {
                    Task.Run(() =>
                    {
                        try
                        {

                            _clientCacheProvider.Set<T>(key, expiry, command());
                        }
                        catch (Exception)
                        {
                            //do nothing? 
                            //_logErrorCommand(ex, string.Format("Error putting key {0}", key));
                        }
                    });
                }
            }

            // If we didn't get any data back
            if (EqualityComparer<T>.Default.Equals(result, default(T)))
            {
                result = command();

                _clientCacheProvider.Set<T>(key, expiry, result);
            }

            return result;
        }


        public T Get<T>(string key)
        {
            if (_clientCacheProvider == null)
                throw new Exception("DataCacheProvider is not set. Run DataCacheConfig.Initialize in Application_Start()");

            DateTime entryDate;
            DateTime serverDate;

            T result;
            // Try to get the data
            _clientCacheProvider.TryGet<T>(key, out entryDate, out serverDate, out result);

            return result;
        }

        public void Set<T>(string key, TimeSpan expiry, T data)
        {
            _clientCacheProvider.Set<T>(key, expiry, data);
        }
    }
    #endregion
}