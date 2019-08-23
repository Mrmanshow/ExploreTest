﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Events
{
    /// <summary>
    /// 事件订阅服务
    /// </summary>
    public interface ISubscriptionService
    {
        /// <summary>
        /// 获取订阅
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns>事件消费者</returns>
        IList<IConsumer<T>> GetSubscriptions<T>();
    }
}
