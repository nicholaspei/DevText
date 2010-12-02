using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Core.Logging;
using DevText.Framework.Logging;

namespace DevText.Framework.Caching
{
    /// <summary>
    /// This class acts as a cache provider that will attempt to retrieve items from a cache,and if they do not exist,
    /// execute the passed in delegate to perform a data retrieval,then place the item into the cache before returning it.
    /// Subsequent accesses will get the data from the cahce until it expires.
    /// </summary>
    public class CacheProvider:ICacheProvider
    {

        private ICache _cache;
        private ILogger _logger;

        public CacheProvider(ICache cache)
        {
            _cache = cache;
            _logger = Log4netLogger.Instance;
        }

        public T Get<T>(string cacheKey, DateTime expiryDate, GetDataToCacheDelegate<T> getData) where T : class
        {
            // Get data from cache
            T data = _cache.Get<T>(cacheKey);
            if (data == null)
            {
                // get data from source
                data = getData();

                // only add non null data to the cache.
                if (data != null)
                {
                    _cache.Add<T>(cacheKey, expiryDate, data);
                    _logger.InfoFormat(string.Format("Adding item [{0}] to cache with expiry date/time of [{1}]", cacheKey, expiryDate.ToString()));
                }
            }
            else if (IsNullOrEmptyArray<T>(data))
            {
                // get data from source
                data = getData();

                // only add non null data to the cache
                if (!IsNullOrEmptyArray<T>(data))
                {
                    _cache.Add<T>(cacheKey, expiryDate, data);
                    _logger.InfoFormat(string.Format("Adding item [{0}] to cache with expiry date/time of [{1}].", cacheKey, expiryDate.ToString()));
                }
            }
            else
            {
                _logger.InfoFormat(string.Format("Retrieving item [{0}] from cache.", cacheKey));
            }
            return data;
        }

        public object GetObject(string cacheKey, DateTime expiryDate, GetDataToCacheDelegate<object> getData)
        {
            object data = _cache.Get(cacheKey);
            if (data == null)
            {
                data = getData();
                _cache.Add(cacheKey, expiryDate, data);
                _logger.InfoFormat(string.Format("Adding item [{0}] to cache with expiry date/time of [{1}].", cacheKey, expiryDate.ToString()));
            }
            else
            {
                _logger.InfoFormat(string.Format("Retrieving item [{0}] from cache.", cacheKey));
            }
            return data;
        }

        private bool IsNullOrEmptyArray<T>(T data)
        {
            bool result = false;
            if (data is Array)
            {
                Array dataArray = data as Array;
                if (dataArray == null)
                    result = true;
                else if (dataArray.Length == 0)
                    result = true;
                else
                    result = false;
            }
            return result;
        }

        public void invalidateCacheItem(string cacheKey)
        {
            _cache.InvalidateCacheItem(cacheKey);
        }
    }
}
