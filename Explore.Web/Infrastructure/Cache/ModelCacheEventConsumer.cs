using Explore.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Explore.Web.Infrastructure.Cache
{
    /// <summary>
    /// 缓存自定义事件 (用于缓存表示层模型)
    /// </summary>
    public partial class ModelCacheEventConsumer
    {
        #region Cache keys

        /// <summary>
        /// 轮播图信息缓存
        /// </summary>
        /// <remarks>
        /// {0} : 数量
        /// </remarks>
        public const string BANNER_KEY = "Explore.pres.banner-{0}";
        public const string BANNER_PATTERN_KEY = "Explore.pres.banner";

        #endregion
    }
}