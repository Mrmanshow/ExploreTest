using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Explore.Core.Caching
{
    /// <summary>
    /// 表示HTTP请求期间用于缓存的管理器（短期缓存）
    /// </summary>
    public partial class PerRequestCacheManager : ICacheManager
    {
        private readonly HttpContextBase _context;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Context</param>
        public PerRequestCacheManager(HttpContextBase context)
        {
            this._context = context;
        }

        /// <summary>
        /// 创建RequestCache类的新实例
        /// </summary>
        protected virtual IDictionary GetItems()
        {
            if (_context != null)
                return _context.Items;

            return null;
        }

        /// <summary>
        /// 获取或设置与指定键关联的值。
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">根据键获得值。</param>
        /// <returns>键对应的值</returns>
        public virtual T Get<T>(string key)
        {
            var items = GetItems();
            if (items == null)
                return default(T);

            return (T)items[key];
        }

        /// <summary>
        /// 将指定的键和对象添加到缓存中。
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="data">对象</param>
        /// <param name="cacheTime">缓存时间</param>
        public virtual void Set(string key, object data, int cacheTime)
        {
            var items = GetItems();
            if (items == null)
                return;

            if (data != null)
            {
                if (items.Contains(key))
                    items[key] = data;
                else
                    items.Add(key, data);
            }
        }

        /// <summary>
        /// 获取一个值，该值指示与指定键关联的值是否被缓存。
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>结果</returns>
        public virtual bool IsSet(string key)
        {
            var items = GetItems();
            if (items == null)
                return false;

            return (items[key] != null);
        }

        /// <summary>
        /// 从缓存中删除具有指定键的值
        /// </summary>
        /// <param name="key">键</param>
        public virtual void Remove(string key)
        {
            var items = GetItems();
            if (items == null)
                return;

            items.Remove(key);
        }

        /// <summary>
        /// 删除相同类型的缓存
        /// </summary>
        /// <param name="pattern">类型</param>
        public virtual void RemoveByPattern(string pattern)
        {
            var items = GetItems();
            if (items == null)
                return;

            this.RemoveByPattern(pattern, items.Keys.Cast<object>().Select(p => p.ToString()));
        }

        /// <summary>
        /// 删除所有缓存
        /// </summary>
        public virtual void Clear()
        {
            var items = GetItems();
            if (items == null)
                return;

            items.Clear();
        }

        /// <summary>
        /// 释放
        /// </summary>
        public virtual void Dispose()
        {
        }
    }
}
