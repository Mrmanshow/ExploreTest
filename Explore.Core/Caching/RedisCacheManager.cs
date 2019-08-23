using Explore.Core.Configuration;
using Explore.Core.Infrastructure;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Caching
{
    /// <summary>
    /// 表示用于在Redis存储中缓存的管理器（http://redis.io/）.
    /// 主要用于在Web场景或Azure中运行。
    /// 当然，它也可以在任何服务器或环境中使用。
    /// </summary>
    public partial class RedisCacheManager : ICacheManager
    {
        #region Fields
        private readonly IRedisConnectionWrapper _connectionWrapper;
        private readonly IDatabase _db;
        private readonly ICacheManager _perRequestCacheManager;

        #endregion

        #region Ctor

        public RedisCacheManager(NopConfig config, IRedisConnectionWrapper connectionWrapper)
        {
            if (String.IsNullOrEmpty(config.RedisCachingConnectionString))
                throw new Exception("Redis connection string is empty");

            // ConnectionMultiplexer.Connect 只应调用一次并在调用方之间共享
            this._connectionWrapper = connectionWrapper;

            this._db = _connectionWrapper.GetDatabase();
            this._perRequestCacheManager = EngineContext.Current.Resolve<ICacheManager>();
        }

        #endregion

        #region Utilities

        protected virtual byte[] Serialize(object item)
        {
            var jsonString = JsonConvert.SerializeObject(item);
            return Encoding.UTF8.GetBytes(jsonString);
        }
        protected virtual T Deserialize<T>(byte[] serializedObject)
        {
            if (serializedObject == null)
                return default(T);

            var jsonString = Encoding.UTF8.GetString(serializedObject);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        #endregion

        #region Methods

        /// <summary>
        /// 获取或设置与指定键关联的值。
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">根据键获得值。</param>
        /// <returns>键对应的值</returns>
        public virtual T Get<T>(string key)
        {
            //提高性能:
            //我们使用“PerRequestCacheManager”将加载的对象缓存到当前HTTP请求的内存中。
            //这样，我们就不会在每个HTTP请求中连接到Redis服务器500（例如，每次加载一个区域设置或设置时）
            if (_perRequestCacheManager.IsSet(key))
                return _perRequestCacheManager.Get<T>(key);

            var rValue = _db.StringGet(key);
            if (!rValue.HasValue)
                return default(T);
            var result = Deserialize<T>(rValue);

            _perRequestCacheManager.Set(key, result, 0);
            return result;
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

            var entryBytes = Serialize(data);
            var expiresIn = TimeSpan.FromMinutes(cacheTime);

            _db.StringSet(key, entryBytes, expiresIn);
        }

        /// <summary>
        /// 获取一个值，该值指示与指定键关联的值是否被缓存。
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>结果</returns>
        public virtual bool IsSet(string key)
        {
            //提高性能:
            //我们使用“PerRequestCacheManager”将加载的对象缓存到当前HTTP请求的内存中。
            //这样，我们就不会在每个HTTP请求中连接到Redis服务器500（例如，每次加载一个区域设置或设置时）
            if (_perRequestCacheManager.IsSet(key))
                return true;

            return _db.KeyExists(key);
        }

        /// <summary>
        /// 从缓存中删除具有指定键的值
        /// </summary>
        /// <param name="key">键</param>
        public virtual void Remove(string key)
        {
            _db.KeyDelete(key);
            _perRequestCacheManager.Remove(key);
        }

        /// <summary>
        /// 删除相同类型的缓存
        /// </summary>
        /// <param name="pattern">类型</param>
        public virtual void RemoveByPattern(string pattern)
        {
            foreach (var ep in _connectionWrapper.GetEndPoints())
            {
                var server = _connectionWrapper.GetServer(ep);
                var keys = server.Keys(database: _db.Database, pattern: "*" + pattern + "*");
                foreach (var key in keys)
                    Remove(key);
            }
        }

        /// <summary>
        /// 清除所有缓存数据
        /// </summary>
        public virtual void Clear()
        {
            foreach (var ep in _connectionWrapper.GetEndPoints())
            {
                var server = _connectionWrapper.GetServer(ep);
                //我们可以使用下面的代码（注释）
                //但它需要管理权限-“，allowadmin=true”
                //server.FlushDatabase();

                //这就是为什么我们现在只需要通过所有元素进行交互
                var keys = server.Keys(database: _db.Database);
                foreach (var key in keys)
                    Remove(key);
            }
        }

        /// <summary>
        /// 释放
        /// </summary>
        public virtual void Dispose()
        {
            //if (_connectionWrapper != null)
            //    _connectionWrapper.Dispose();
        }

        #endregion

    }
}
