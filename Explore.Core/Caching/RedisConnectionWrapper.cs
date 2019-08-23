using Explore.Core.Configuration;
using RedLock;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Caching
{
    /// <summary>
    /// Redis connection wrapper implementation
    /// </summary>
    public class RedisConnectionWrapper : IRedisConnectionWrapper
    {
        #region Fields

        private readonly NopConfig _config;
        private readonly Lazy<string> _connectionString;

        private volatile ConnectionMultiplexer _connection;
        private volatile RedisLockFactory _redisLockFactory;
        private readonly object _lock = new object();

        #endregion

        #region Ctor

        public RedisConnectionWrapper(NopConfig config)
        {
            this._config = config;
            this._connectionString = new Lazy<string>(GetConnectionString);
            this._redisLockFactory = CreateRedisLockFactory();
        }

        #endregion

        #region Utilities

        /// <summary>
        /// 从配置获取Redis缓存的连接字符串
        /// </summary>
        /// <returns></returns>
        protected string GetConnectionString()
        {
            return _config.RedisCachingConnectionString;
        }

        /// <summary>
        /// 获取与Redis服务器的连接
        /// </summary>
        /// <returns></returns>
        protected ConnectionMultiplexer GetConnection()
        {
            if (_connection != null && _connection.IsConnected) return _connection;

            lock (_lock)
            {
                if (_connection != null && _connection.IsConnected) return _connection;

                if (_connection != null)
                {
                    //连接断开。正在释放连接…
                    _connection.Dispose();
                }

                //新建redis连接实例
                _connection = ConnectionMultiplexer.Connect(_connectionString.Value);
            }

            return _connection;
        }

        /// <summary>
        /// 创建RedisLockFactory实例
        /// </summary>
        /// <returns>RedisLockFactory</returns>
        protected RedisLockFactory CreateRedisLockFactory()
        {
            //从连接字符串获取密码和值是否使用SSL
            var password = string.Empty;
            var useSsl = false;
            foreach (var option in GetConnectionString().Split(',').Where(option => option.Contains('=')))
            {
                switch (option.Substring(0, option.IndexOf('=')).Trim().ToLowerInvariant())
                {
                    case "password":
                        password = option.Substring(option.IndexOf('=') + 1).Trim();
                        break;
                    case "ssl":
                        bool.TryParse(option.Substring(option.IndexOf('=') + 1).Trim(), out useSsl);
                        break;
                }
            }

            //为使用Redlock分布式锁算法创建RedisLockFactory
            return new RedisLockFactory(GetEndPoints().Select(endPoint => new RedisLockEndPoint
            {
                EndPoint = endPoint,
                Password = password,
                Ssl = useSsl
            }));
        }

        #endregion

        #region Methods

        /// <summary>
        /// 获取与Redis内数据库的交互连接
        /// </summary>
        /// <param name="db">数据库编号；传递空值以使用默认值</param>
        /// <returns>Redis缓存数据库</returns>
        public IDatabase GetDatabase(int? db = null)
        {
            return GetConnection().GetDatabase(db ?? -1); //_settings.DefaultDb);
        }

        /// <summary>
        /// 获取单个服务器的配置API
        /// </summary>
        /// <param name="endPoint">网络节点</param>
        /// <returns>Redis服务</returns>
        public IServer GetServer(EndPoint endPoint)
        {
            return GetConnection().GetServer(endPoint);
        }

        /// <summary>
        /// 获取服务器上定义的所有节点
        /// </summary>
        /// <returns>节点数组</returns>
        public EndPoint[] GetEndPoints()
        {
            return GetConnection().GetEndPoints();
        }

        /// <summary>
        /// 删除数据库的所有键
        /// </summary>
        /// <param name="db">数据库编号；传递空值以使用默认值</param>
        public void FlushDatabase(int? db = null)
        {
            var endPoints = GetEndPoints();

            foreach (var endPoint in endPoints)
            {
                GetServer(endPoint).FlushDatabase(db ?? -1); //_settings.DefaultDb);
            }
        }

        /// <summary>
        /// 对redis分布式锁执行一些操作
        /// </summary>
        /// <param name="resource">我们锁定的东西</param>
        /// <param name="expirationTime">Redis自动到期锁的时间</param>
        /// <param name="action">锁定时要执行的操作</param>
        /// <returns>如果获取锁并执行操作，则为true；否则为false</returns>
        public bool PerformActionWithLock(string resource, TimeSpan expirationTime, Action action)
        {
            //use RedLock library
            using (var redisLock = _redisLockFactory.Create(resource, expirationTime))
            {
                //ensure that lock is acquired
                if (!redisLock.IsAcquired)
                    return false;

                //perform action
                action();
                return true;
            }
        }

        /// <summary>
        /// 释放与此对象关联的所有资源
        /// </summary>
        public void Dispose()
        {
            //释放 ConnectionMultiplexer
            if (_connection != null)
                _connection.Dispose();

            //释放 RedisLockFactory
            if (_redisLockFactory != null)
                _redisLockFactory.Dispose();
        }

        #endregion
    }
}
