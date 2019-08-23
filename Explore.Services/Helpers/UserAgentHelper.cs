using Explore.Core;
using Explore.Core.Configuration;
using Explore.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Explore.Services.Helpers
{
    /// <summary>
    /// 用户代理帮助类
    /// </summary>
    public partial class UserAgentHelper : IUserAgentHelper
    {
        private readonly NopConfig _config;
        private readonly HttpContextBase _httpContext;
        private static readonly object _locker = new object();

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="config">Config</param>
        /// <param name="httpContext">HTTP context</param>
        public UserAgentHelper(NopConfig config, HttpContextBase httpContext)
        {
            this._config = config;
            this._httpContext = httpContext;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        protected virtual BrowscapXmlHelper GetBrowscapXmlHelper()
        {
            if (Singleton<BrowscapXmlHelper>.Instance != null)
                return Singleton<BrowscapXmlHelper>.Instance;

            //数据配置文件不存在
            if (String.IsNullOrEmpty(_config.UserAgentStringsPath))
                return null;

            //防止多重加载数据
            lock (_locker)
            {
                //我们可以在等待时加载数据
                if (Singleton<BrowscapXmlHelper>.Instance != null)
                    return Singleton<BrowscapXmlHelper>.Instance;

                var userAgentStringsPath = CommonHelper.MapPath(_config.UserAgentStringsPath);
                var crawlerOnlyUserAgentStringsPath = string.IsNullOrEmpty(_config.CrawlerOnlyUserAgentStringsPath) ? string.Empty : CommonHelper.MapPath(_config.CrawlerOnlyUserAgentStringsPath);

                var browscapXmlHelper = new BrowscapXmlHelper(userAgentStringsPath, crawlerOnlyUserAgentStringsPath);
                Singleton<BrowscapXmlHelper>.Instance = browscapXmlHelper;

                return Singleton<BrowscapXmlHelper>.Instance;
            }
        }

        /// <summary>
        /// 获取一个值，该值指示请求是否由搜索引擎（Web爬虫程序）发出。
        /// </summary>
        /// <returns>结果</returns>
        public virtual bool IsSearchEngine()
        {
            if (_httpContext == null)
                return false;

            //we put required logic in try-catch block
            //more info: http://www.nopcommerce.com/boards/t/17711/unhandled-exception-request-is-not-available-in-this-context.aspx
            try
            {
                var bowscapXmlHelper = GetBrowscapXmlHelper();

                //我们无法加载分析器
                if (bowscapXmlHelper == null)
                    return false;

                var userAgent = _httpContext.Request.UserAgent;
                return bowscapXmlHelper.IsCrawler(userAgent);
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
            }

            return false;
        }
    }
}
