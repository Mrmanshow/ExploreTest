using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Caching
{
    /// <summary>
    /// 缓存管理接口
    /// </summary>
    public interface ICacheManager : IDisposable
    {
        /// <summary>
        /// 获取或设置与指定键关联的值。
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">根据键获得值。</param>
        /// <returns>键对应的值</returns>
        T Get<T>(string key);

        /// <summary>
        /// 将指定的键和对象添加到缓存中。
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="data">对象</param>
        /// <param name="cacheTime">缓存时间</param>
        void Set(string key, object data, int cacheTime);

        /// <summary>
        /// 获取一个值，该值指示与指定键关联的值是否被缓存。
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>结果</returns>
        bool IsSet(string key);

        /// <summary>
        /// 从缓存中删除具有指定键的值
        /// </summary>
        /// <param name="key">键</param>
        void Remove(string key);

        /// <summary>
        /// 删除相同类型的缓存
        /// </summary>
        /// <param name="pattern">类型</param>
        void RemoveByPattern(string pattern);

        /// <summary>
        /// 删除所有缓存
        /// </summary>
        void Clear();
    }
}
