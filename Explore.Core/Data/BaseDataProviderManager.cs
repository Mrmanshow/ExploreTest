using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Data
{
    /// <summary>
    /// 数据提供程序管理基类
    /// </summary>
    public abstract class BaseDataProviderManager
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="settings">Data settings</param>
        protected BaseDataProviderManager(DataSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            this.Settings = settings;
        }

        /// <summary>
        /// 获取或更改设置
        /// </summary>
        protected DataSettings Settings { get; private set; }

        /// <summary>
        /// 加载数据提供对象
        /// </summary>
        /// <returns>数据提供对象</returns>
        public abstract IDataProvider LoadDataProvider();
    }
}
