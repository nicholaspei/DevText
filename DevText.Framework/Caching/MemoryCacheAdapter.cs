using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Caching;
using DevText.Framework.Logging;
using Castle.Core.Logging;

namespace DevText.Framework.Caching
{
    /// <summary>
    /// In memory cache with no dependencies on the web cache, only runtime dependencies.
    /// ie. Can be used in any type of application,desktop,web,service or otherwise.
    /// </summary>
   public  class MemoryCacheAdapter:ICache
    {
       private MemoryCache _cahce = MemoryCache.Default;
       private ILogger _logger;

       public MemoryCacheAdapter()
       {
           _logger = Log4netLogger.Instance;
       }

       public void Add<T>(string cacheKey,DateTime expiry,T dataToAdd) where T:class
       {
           var policy = new CacheItemPolicy();
           policy.AbsoluteExpiration = new DateTimeOffset(expiry);

           if (dataToAdd != null)
               _cahce.Add(cacheKey, dataToAdd, policy);
       }

       public T Get<T>(string cacheKey) where T:class
       {
           T data = _cahce.Get(cacheKey) as T;
           return data;
       }

       public void Add(string cacheKey,DateTime expiry,object dataToAdd)
       {
           Add<object>(cacheKey, expiry, dataToAdd);
       }

       public object Get(string cacheKey)
       {
           return _cahce.Get(cacheKey);
       }

       public void InvalidateCacheItem(string cacheKey)
       {
           if (_cahce.Contains(cacheKey))
               _cahce.Remove(cacheKey);
       }
    }
}
