using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Data
{
    /// <summary>
    /// 数据设置（连接字符串信息）
    /// </summary>
    public partial class DataSettings
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DataSettings()
        {
            RawDataSettings = new Dictionary<string, string>();
        }

        /// <summary>
        /// 数据提供程序
        /// </summary>
        public string DataProvider { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string DataConnectionString { get; set; }

        /// <summary>
        /// 原始设置文件
        /// </summary>
        public IDictionary<string, string> RawDataSettings { get; private set; }

        /// <summary>
        /// 指示输入的信息是否有效的值。
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return !String.IsNullOrEmpty(this.DataProvider) && !String.IsNullOrEmpty(this.DataConnectionString);
        }
    }
}
