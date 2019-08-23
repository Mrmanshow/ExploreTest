using Explore.Core.Infrastructure;
using Explore.Core.Plugins;
using Explore.Services.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explore.Services.Logging;

namespace Explore.Services.Events
{
    /// <summary>
    /// 事件发布
    /// </summary>
    public class EventPublisher : IEventPublisher
    {
        private readonly ISubscriptionService _subscriptionService;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="subscriptionService"></param>
        public EventPublisher(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        /// <summary>
        /// 发布给消费者
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="x">事件消费者</param>
        /// <param name="eventMessage">事件消息</param>
        protected virtual void PublishToConsumer<T>(IConsumer<T> x, T eventMessage)
        {
            //忽略未安装的插件
            var plugin = FindPlugin(x.GetType());
            if (plugin != null && !plugin.Installed)
                return;

            try
            {
                x.HandleEvent(eventMessage);
            }
            catch (Exception exc)
            {
                //记录错误
                var logger = EngineContext.Current.Resolve<ILogger>();
                //我们放入嵌套的try catch以防止可能的循环（如果发生错误）
                try
                {
                    logger.Error(exc.Message, exc);
                }
                catch (Exception)
                {
                    //do nothing
                }
            }
        }

        /// <summary>
        /// 找到某个类型的插件描述符，该类型位于它的程序集中
        /// </summary>
        /// <param name="providerType">Provider type</param>
        /// <returns>插件描述</returns>
        protected virtual PluginDescriptor FindPlugin(Type providerType)
        {
            if (providerType == null)
                throw new ArgumentNullException("providerType");

            if (PluginManager.ReferencedPlugins == null)
                return null;

            foreach (var plugin in PluginManager.ReferencedPlugins)
            {
                if (plugin.ReferencedAssembly == null)
                    continue;

                if (plugin.ReferencedAssembly.FullName == providerType.Assembly.FullName)
                    return plugin;
            }

            return null;
        }

        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="eventMessage">事件消息</param>
        public virtual void Publish<T>(T eventMessage)
        {
            var subscriptions = _subscriptionService.GetSubscriptions<T>();
            subscriptions.ToList().ForEach(x => PublishToConsumer(x, eventMessage));
        }

    }
}
