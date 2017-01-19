using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace HZ.Framework
{
    public class CacheHelper
    {
        private readonly string _region;
        private readonly ObjectCache _cache;

        public string Region
        {
            get{return _region;}
        }

        public CacheHelper(string region)
        {
            this._region = region;
            _cache = MemoryCache.Default;          
        }

        /// <summary>
        /// 从缓存中获取数据
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <returns>返回值</returns>
        public object Get(string key)
        {
            key.CheckNotNull("key");
            string cacheKey = GetCacheKey(key);
            object value = _cache.Get(cacheKey);
            if(value==null) 
                return null;

            DictionaryEntry entry=(DictionaryEntry)value;
            if(!key.Equals(entry.Key))
                return null;

            return entry.Value;
        }

        /// <summary>
        ///  从缓存中获取强类型数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">缓存key</param>
        /// <returns>返回值</returns>
        public T Get<T>(string key)
        {
            return (T)Get(key);
        }


        /// <summary>
        /// 获取当前缓存中的所有数据
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetAll()
        {
            string token=string.Concat(_region,":");
            return _cache.Where(p=>p.Key.StartsWith(token)).Select(p=>p.Value).Cast<DictionaryEntry>().Select(p=>p.Value);
        }


        /// <summary>
        /// 获取当前缓存中的所有强类型数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetAll<T>()
        {
            return GetAll().Cast<T>();
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">要缓存的数据</param>
        public void Set(string key, object value)
        {
            key.CheckNotNull("key");
            value.CheckNotNull("value");
            string cacheKey = GetCacheKey(key);
            DictionaryEntry entry = new DictionaryEntry(key, value);
            CacheItemPolicy policy = new CacheItemPolicy();
            _cache.Set(cacheKey,entry, policy);
        }

        /// <summary>
        /// 设置缓存并设置失效时间(DateTime)
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">要缓存的数据</param>
        /// <param name="expiry">过期时间</param>
        public void Set(string key, object value, DateTime expiry = default(DateTime))
        {
            key.CheckNotNull("key");
            value.CheckNotNull("value");
            string cacheKey = GetCacheKey(key);
            DictionaryEntry entry = new DictionaryEntry(key, value);
            CacheItemPolicy policy = new CacheItemPolicy() { AbsoluteExpiration = expiry };
            _cache.Set(cacheKey, entry, policy);

        }

        /// <summary>
        ///设置缓存并设置失效时间(TimeSpan)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        public void Set(string key, object value, TimeSpan expiry = default(TimeSpan))
        {
            key.CheckNotNull("key");
            value.CheckNotNull("value");
            string cacheKey=GetCacheKey(key);
            DictionaryEntry entry = new DictionaryEntry(key, value);
            CacheItemPolicy policy = new CacheItemPolicy() { SlidingExpiration = expiry };
            _cache.Set(cacheKey, entry, policy);
        }

        /// <summary>
        /// 移除指定key的缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        public void Remove(string key)
        {
            key.CheckNotNull(key);
            string cacheKey = GetCacheKey(key);
            _cache.Remove(cacheKey);
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        public void Clear()
        {
            string token = string.Concat(_region, ":");
            List<string> cacheKeys = _cache.Where(p => p.Key.StartsWith(token)).Select(p=>p.Key).ToList();
            foreach (string cacheKey in cacheKeys)
            {
                _cache.Remove(cacheKey);
            }
        }

        private string GetCacheKey(string key)
        {
            return string.Concat(_region, ":", key, "@", key.GetHashCode());
        }
    }
}
