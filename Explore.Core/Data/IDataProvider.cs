using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Data
{
    /// <summary>
    /// 数据提供程序接口
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// 初始化连接工厂
        /// </summary>
        void InitConnectionFactory();

        /// <summary>
        /// 设置数据库初始值设定项
        /// </summary>
        void SetDatabaseInitializer();

        /// <summary>
        /// 初始化数据库
        /// </summary>
        void InitDatabase();

        /// <summary>
        /// 一个值，指示此数据提供程序是否支持存储过程
        /// </summary>
        bool StoredProceduredSupported { get; }

        /// <summary>
        /// 一个值，指示此数据提供程序是否支持备份
        /// </summary>
        bool BackupSupported { get; }

        /// <summary>
        /// 获取支持数据库参数对象（由存储过程使用）
        /// </summary>
        /// <returns>Parameter</returns>
        DbParameter GetParameter();

        /// <summary>
        /// HASHBYTES函数的最大数据长度
        /// 如果不支持HASHBYTES函数，则返回0
        /// </summary>
        /// <returns>HASHBYTES函数的数据长度</returns>
        int SupportedLengthOfBinaryHash();
    }
}
