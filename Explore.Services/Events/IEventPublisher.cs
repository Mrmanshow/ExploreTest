using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Events
{
    /// <summary>
    /// 事件发布
    /// </summary>
    public interface IEventPublisher
    {
        /// <summary>
        /// 事件发布
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="eventMessage">事件信息</param>
        void Publish<T>(T eventMessage);
    }
}
