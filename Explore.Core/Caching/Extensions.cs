using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Explore.Core.Caching
{
    /// <summary>
    /// 扩展
    /// </summary>
    public static class CacheExtensions
    {
        /// <summary>
        /// 获取缓存项。如果它还不在缓存中，则加载并缓存它
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="cacheManager">缓存管理</param>
        /// <param name="key">缓存键</param>
        /// <param name="acquire">函数加载项（如果它还不在缓存中）</param>
        /// <returns>缓存内容</returns>
        public static T Get<T>(this ICacheManager cacheManager, string key, Func<T> acquire)
        {
            return Get(cacheManager, key, 60, acquire);
        }

        /// <summary>
        /// 获取缓存项。如果它还不在缓存中，则加载并缓存它
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="cacheManager">缓存管理</param>
        /// <param name="key">缓存键</param>
        /// <param name="cacheTime">缓存时间（分钟）（0-不缓存）</param>
        /// <param name="acquire">函数加载项（如果它还不在缓存中）</param>
        /// <returns>缓存内容</returns>
        public static T Get<T>(this ICacheManager cacheManager, string key, int cacheTime, Func<T> acquire)
        {
            if (cacheManager.IsSet(key))
            {
                return cacheManager.Get<T>(key);
            }

            var result = acquire();
            if (cacheTime > 0)
                cacheManager.Set(key, result, cacheTime);
            return result;
        }

        /// <summary>
        /// 移除所有项
        /// </summary>
        /// <param name="cacheManager">缓存管理</param>
        /// <param name="pattern">正则表达式</param>
        /// <param name="keys">缓存里的所有键</param>
        public static void RemoveByPattern(this ICacheManager cacheManager, string pattern, IEnumerable<string> keys)
        {
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (var key in keys.Where(p => regex.IsMatch(p.ToString())).ToList())
                cacheManager.Remove(key);
        }
    }
}
