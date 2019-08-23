using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Explore.Core.ComponentModel
{
    /// <summary>
    /// 为实现对资源的锁定访问提供了一种方便的方法。
    /// </summary>
    /// <remarks>
    /// 作为基础设施类。
    /// </remarks>
    public class WriteLockDisposable : IDisposable
    {
        private readonly ReaderWriterLockSlim _rwLock;

        /// <summary>
        /// 初始化的新实例 <see cref="WriteLockDisposable"/>。
        /// </summary>
        /// <param name="rwLock">The rw lock.</param>
        public WriteLockDisposable(ReaderWriterLockSlim rwLock)
        {
            _rwLock = rwLock;
            _rwLock.EnterWriteLock();
        }

        void IDisposable.Dispose()
        {
            _rwLock.ExitWriteLock();
        }
    }
}
