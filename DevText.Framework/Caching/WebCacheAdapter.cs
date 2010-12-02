using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using Castle.Core.Logging;
using DevText.Framework.Logging;

namespace DevText.Framework.Caching
{
    public class WebCacheAdapter:ICache
    {
        private System.Web.Caching.Cache _cache;
        private ILogger _Logger;

        public WebCacheAdapter()
        {
            _Logger=Log4netLogger.Instance;

            if (System.Web.HttpContext.Current != null)
                _cache = System.Web.HttpContext.Current.Cache;
            else
                _Logger.Warn("Not in a web context, unable to use the web cache.");
        }

        public void Add<T>(string cacheKey, DateTime expiry, T dataToAdd) where T : class
        {
            if (dataToAdd != null)
                _cache.Add(cacheKey, dataToAdd, null, expiry, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
        }

        public T Get<T>(string cacheKey) where T : class
        {
            T data = _cache.Get(cacheKey) as T;
            return data;
        }

        public void Add(string cacheKey, DateTime expiry, object dataToAdd)
        {
            Add<object>(cacheKey, expiry, dataToAdd);
        }

        public object Get(string cacheKey)
        {
            return _cache.Get(cacheKey);
        }

        public void InvalidateCacheItem(string cacheKey)
        {
            if (_cache.Get(cacheKey) != null)
                _cache.Remove(cacheKey);
        }
    }
}
