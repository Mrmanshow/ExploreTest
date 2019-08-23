using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Plugins
{
    /// <summary>
    /// 表示加载插件的模式
    /// </summary>
    public enum LoadPluginsMode
    {
        /// <summary>
        /// 全部（已安装和未安装）
        /// </summary>
        All = 0,
        /// <summary>
        /// 仅安装
        /// </summary>
        InstalledOnly = 10,
        /// <summary>
        /// 未安装
        /// </summary>
        NotInstalledOnly = 20
    }
}
