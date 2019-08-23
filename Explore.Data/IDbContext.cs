using Explore.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Data
{
    public interface IDbContext
    {
        /// <summary>
        /// 获取数据集
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <returns>结果集</returns>
        IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;

        /// <summary>
        /// Save changes
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        /// <summary>
        /// 执行存储过程并在末尾加载实体列表
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="commandText">命令</param>
        /// <param name="parameters">参数</param>
        /// <returns>结果集</returns>
        IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters)
            where TEntity : BaseEntity, new();

        /// <summary>
        /// 创建一个原始SQL查询，返回给定泛型类型的元素。类型可以是具有与查询返回的列名称匹配的属性的任何类型，
        /// 也可以是简单的基元类型。类型不必是实体类型。即使返回的对象类型是实体类型，上下文也不会跟踪此查询的结果。
        /// </summary>
        /// <typeparam name="TElement">通过查询返回的类型</typeparam>
        /// <param name="sql">查询字符串</param>
        /// <param name="parameters">要应用于SQL查询字符串的参数。</param>
        /// <returns>结果</returns>
        IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters);

        /// <summary>
        /// 对数据库执行给定的ddl/dml命令。
        /// </summary>
        /// <param name="sql">命令字符串</param>
        /// <param name="doNotEnsureTransaction">false - 不使用事务创建; true - 使用事务创建.</param>
        /// <param name="timeout">超时值，以秒为单位。空值表示将使用基础提供程序的默认值</param>
        /// <param name="parameters">要应用于命令字符串的参数。</param>
        /// <returns>执行命令后数据库返回的结果。</returns>
        int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters);

        /// <summary>
        /// 从上下文中分离实体
        /// </summary>
        /// <param name="entity">实体</param>
        void Detach(object entity);

        /// <summary>
        /// 获取或设置一个值，该值指示是否启用了代理创建设置（在EF中使用）
        /// </summary>
        bool ProxyCreationEnabled { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否启用自动检测更改设置（在EF中使用）
        /// </summary>
        bool AutoDetectChangesEnabled { get; set; }
    }
}
