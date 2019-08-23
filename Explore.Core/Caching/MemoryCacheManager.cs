using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Caching
{
    /// <summary>
    /// 表示用于在HTTP请求之间进行缓存的管理器（长期缓存）
    /// </summary>
    public partial class MemoryCacheManager : ICacheManager
    {
        /// <summary>
        /// 缓存对象
        /// </summary>
        protected ObjectCache Cache
        {
            get
            {
                return MemoryCache.Default;
            }
        }

        /// <summary>
        /// 获取或设置与指定键关联的值。
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">根据键获得值。</param>
        /// <returns>键对应的值</returns>
        public virtual T Get<T>(string key)
        {
            return (T)Cache[key];
        }

        /// <summary>
        /// 将指定的键和对象添加到缓存中。
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="data">对象</param>
        /// <param name="cacheTime">缓存时间</param>
        public virtual void Set(string key, object data, int cacheTime)
        {
            if (data == null)
                return;

            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime);
            Cache.Add(new CacheItem(key, data), policy);
        }

        /// <summary>
        /// 获取一个值，该值指示与指定键关联的值是否被缓存。
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>结果</returns>
        public virtual bool IsSet(string key)
        {
            return (Cache.Contains(key));
        }

        /// <summary>
        /// 从缓存中删除具有指定键的值
        /// </summary>
        /// <param name="key">键</param>
        public virtual void Remove(string key)
        {
            Cache.Remove(key);
        }

        /// <summary>
        /// 删除相同类型的缓存
        /// </summary>
        /// <param name="pattern">类型</param>
        public virtual void RemoveByPattern(string pattern)
        {
            this.RemoveByPattern(pattern, Cache.Select(p => p.Key));
        }

        /// <summary>
        /// 删除所有缓存
        /// </summary>
        public virtual void Clear()
        {
            foreach (var item in Cache)
                Remove(item.Key);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose()
        {
        }
    }
}
