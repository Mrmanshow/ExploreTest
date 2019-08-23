using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Helpers
{
    /// <summary>
    /// 用户代理帮助接口
    /// </summary>
    public partial interface IUserAgentHelper
    {
        /// <summary>
        /// 获取一个值，该值指示请求是否由搜索引擎（Web爬虫程序）发出。
        /// </summary>
        /// <returns>Result</returns>
        bool IsSearchEngine();
    }
}
