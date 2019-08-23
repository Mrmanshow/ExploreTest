using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Web.Framework.Security
{
    public enum SslRequirement
    {
        /// <summary>
        /// 页面应该是安全的
        /// </summary>
        Yes,
        /// <summary>
        /// 不应保护页面
        /// </summary>
        No,
        /// <summary>
        /// 没关系（对于要求）
        /// </summary>
        NoMatter,
    }
}
