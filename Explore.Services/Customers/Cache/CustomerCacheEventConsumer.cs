using Explore.Core.Caching;
using Explore.Core.Domain.Customers;
using Explore.Core.Infrastructure;
using Explore.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Customers.Cache
{
    /// <summary>
    /// 客户缓存事件使用者（用于缓存当前客户密码）
    /// </summary>
    public partial class CustomerCacheEventConsumer : IConsumer<CustomerPasswordChangedEvent>
    {
        #region Constants

        /// <summary>
        /// 当前用户密码生存期的密钥
        /// </summary>
        /// <remarks>
        /// {0} : 用户Id
        /// </remarks>
        public const string CUSTOMER_PASSWORD_LIFETIME = "Nop.customers.passwordlifetime-{0}";

        #endregion

        #region Fields

        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        public CustomerCacheEventConsumer()
        {
            //TODO使用构造函数插入静态缓存管理器
            this._cacheManager = EngineContext.Current.ContainerManager.Resolve<ICacheManager>("nop_cache_static");
        }

        #endregion

        #region Methods

        //密码修改
        public void HandleEvent(CustomerPasswordChangedEvent eventMessage)
        {
            _cacheManager.Remove(string.Format(CUSTOMER_PASSWORD_LIFETIME, eventMessage.Password.CustomerId));
        }

        #endregion
    }
}
