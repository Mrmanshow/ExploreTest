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
    /// Redis连接包装接口
    /// </summary>
    public interface IRedisConnectionWrapper : IDisposable
    {
        /// <summary>
        /// 获取与Redis内数据库的交互连接
        /// </summary>
        /// <param name="db">数据库编号；传递空值以使用默认值</param>
        /// <returns>Redis缓存数据库</returns>
        IDatabase GetDatabase(int? db = null);

        /// <summary>
        /// 获取单个服务器的配置API
        /// </summary>
        /// <param name="endPoint">网络节点</param>
        /// <returns>Redis服务</returns>
        IServer GetServer(EndPoint endPoint);

        /// <summary>
        /// 获取服务器上定义的所有节点
        /// </summary>
        /// <returns>节点数组</returns>
        EndPoint[] GetEndPoints();

        /// <summary>
        /// 删除数据库的所有键
        /// </summary>
        /// <param name="db">数据库编号；传递空值以使用默认值</param>
        void FlushDatabase(int? db = null);

        /// <summary>
        /// 对redis分布式锁执行一些操作
        /// </summary>
        /// <param name="resource">我们锁定的东西</param>
        /// <param name="expirationTime">Redis自动到期锁的时间</param>
        /// <param name="action">锁定时要执行的操作</param>
        /// <returns>如果获取锁并执行操作，则为true；否则为false</returns>
        bool PerformActionWithLock(string resource, TimeSpan expirationTime, Action action);
    }
}
