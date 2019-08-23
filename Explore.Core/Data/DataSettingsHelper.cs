using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Data
{
    /// <summary>
    /// 数据设置帮助程序
    /// </summary>
    public partial class DataSettingsHelper
    {
        private static bool? _databaseIsInstalled;

        /// <summary>
        /// 返回一个值，该值指示是否已安装数据库
        /// </summary>
        /// <returns></returns>
        public static bool DatabaseIsInstalled()
        {
            if (!_databaseIsInstalled.HasValue)
            {
                var manager = new DataSettingsManager();
                var settings = manager.LoadSettings();
                _databaseIsInstalled = settings != null && !String.IsNullOrEmpty(settings.DataConnectionString);
            }
            return _databaseIsInstalled.Value;
        }

        //Reset information cached in the "DatabaseIsInstalled" method
        public static void ResetCache()
        {
            _databaseIsInstalled = null;
        }
    }
}
