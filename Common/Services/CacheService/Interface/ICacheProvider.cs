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
    public interface ICacheProvider
    {
        void Initialize();
        void Purge();
        bool TryGet<T>(string key, out DateTime entryDate, out DateTime now, out T data);
        void Set<T>(string key, TimeSpan expiry, T data);
    }
}
