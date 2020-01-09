/******************************************************
 * Version 1.0
 * 
 * General purpose caching utilities
 * 
 * Supports both redis and sql in-memory table based
 * caches.
 ******************************************************/
using System;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Caching
{
    public class RedisCacheProvider : ICacheProvider
    {
        string _connectionString;
        ConnectionMultiplexer _redis;
        public RedisCacheProvider(string connectionString)
        {
            _connectionString = connectionString;
            _redis = ConnectionMultiplexer.Connect(_connectionString);
        }

        public void Initialize()
        {

        }

        public void Purge()
        {
            //do nothing for now...      
        }

        class CacheEntry<T>
        {
            public DateTime EntryDate { get; set; }

            public T Value { get; set; }

        }

        public void Set<T>(string key, TimeSpan expiry, T o)
        {
            //we want to be clever here, we want it to expire greater than 
            var db = _redis.GetDatabase();

            var wrapper = new CacheEntry<T>
            {
                EntryDate = DateTime.UtcNow,
                Value = o
            };
            //tripple the actual time to live on backend so we have something to grab and return while
            //it gets updated in background

            db.StringSet(key, JsonConvert.SerializeObject(wrapper), expiry.Add(TimeSpan.FromSeconds(expiry.TotalSeconds * 3)));
        }

        public bool TryGet<T>(string key, out DateTime entryDate, out DateTime now, out T o)
        {
            now = DateTime.UtcNow;

            var db = _redis.GetDatabase();

            var ret = db.StringGet(key);

            if (ret.IsNullOrEmpty)
            {
                entryDate = default(DateTime);

                o = default(T);
                return false;
            }

            var wrapper = JsonConvert.DeserializeObject<CacheEntry<T>>(ret);

            entryDate = wrapper.EntryDate;
            o = wrapper.Value;
            return true;
        }
    }

}