/******************************************************
 * Version 1.0
 * 
 * General purpose caching utilities
 * 
 * Supports both redis and sql in-memory table based
 * caches.
 ******************************************************/
using System;

namespace Caching
{
    public interface ICache
    {
        //T Get<T>(string key, Func<T> command);
        T Get<T>(string key);
        T Get<T>(string key, TimeSpan expiry, Func<T> command);
        void Set<T>(string key, TimeSpan expiry, T data);
    }
}