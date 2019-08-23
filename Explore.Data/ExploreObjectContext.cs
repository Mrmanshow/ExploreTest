using Explore.Core;
using Explore.Data.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Data
{
    /// <summary>
    /// 对象上下文
    /// </summary>
    public class ExploreObjectContext: DbContext, IDbContext
    {
        #region 构造函数

        public ExploreObjectContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            //((IObjectContextAdapter) this).ObjectContext.ContextOptions.LazyLoadingEnabled = true;
        }
        
        #endregion

        #region Utilities

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //dynamically load all configuration
            //System.Type configType = typeof(LanguageMap);   //any of your configuration classes here
            //var typesToRegister = Assembly.GetAssembly(configType).GetTypes()

            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => !String.IsNullOrEmpty(type.Namespace))
            .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
                type.BaseType.GetGenericTypeDefinition() == typeof(ExploreEntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
            //...or do it manually below. For example,
            //modelBuilder.Configurations.Add(new LanguageMap());



            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// 将实体附加到上下文或返回已附加的实体（如果已附加）
        /// </summary>
        /// <typeparam name="TEntity">TEntity</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>Attached entity</returns>
        protected virtual TEntity AttachEntityToContext<TEntity>(TEntity entity) where TEntity : BaseEntity, new()
        {
            //除非实体框架真正支持存储过程，否则在实体附加到上下文之前，不会加载已加载实体的导航属性。
            var alreadyAttached = Set<TEntity>().Local.FirstOrDefault(x => x.Id == entity.Id);
            if (alreadyAttached == null)
            {
                //附加新的实体
                Set<TEntity>().Attach(entity);
                return entity;
            }

            //实体已加载好
            return alreadyAttached;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 创建数据本脚本
        /// </summary>
        /// <returns>要生成数据库的SQL</returns>
        public string CreateDatabaseScript()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateDatabaseScript();
        }

        /// <summary>
        /// 获取数据集
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <returns>结果集</returns>
        public new IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }
        
        /// <summary>
        /// 执行存储过程并在末尾加载实体列表
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="commandText">命令</param>
        /// <param name="parameters">参数</param>
        /// <returns>结果集</returns>
        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : BaseEntity, new()
        {
            //向命令添加参数
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    var p = parameters[i] as DbParameter;
                    if (p == null)
                        throw new Exception("Not support parameter type");

                    commandText += i == 0 ? " " : ", ";

                    commandText += "@" + p.ParameterName;
                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                    {
                        //output parameter
                        commandText += " output";
                    }
                }
            }

            var result = this.Database.SqlQuery<TEntity>(commandText, parameters).ToList();

            //performance hack applied as described here - http://www.nopcommerce.com/boards/t/25483/fix-very-important-speed-improvement.aspx
            bool acd = this.Configuration.AutoDetectChangesEnabled;
            try
            {
                this.Configuration.AutoDetectChangesEnabled = false;

                for (int i = 0; i < result.Count; i++)
                    result[i] = AttachEntityToContext(result[i]);
            }
            finally
            {
                this.Configuration.AutoDetectChangesEnabled = acd;
            }

            return result;
        }

        /// <summary>
        /// 创建一个原始SQL查询，返回给定泛型类型的元素。类型可以是具有与查询返回的列名称匹配的属性的任何类型，
        /// 也可以是简单的基元类型。类型不必是实体类型。即使返回的对象类型是实体类型，上下文也不会跟踪此查询的结果。
        /// </summary>
        /// <typeparam name="TElement">通过查询返回的类型</typeparam>
        /// <param name="sql">查询字符串</param>
        /// <param name="parameters">要应用于SQL查询字符串的参数。</param>
        /// <returns>结果</returns>
        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return this.Database.SqlQuery<TElement>(sql, parameters);
        }
    
        /// <summary>
        /// 对数据库执行给定的ddl/dml命令。
        /// </summary>
        /// <param name="sql">命令字符串</param>
        /// <param name="doNotEnsureTransaction">false - 不使用事务创建; true - 使用事务创建.</param>
        /// <param name="timeout">超时值，以秒为单位。空值表示将使用基础提供程序的默认值</param>
        /// <param name="parameters">要应用于命令字符串的参数。</param>
        /// <returns>执行命令后数据库返回的结果。</returns>
        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            int? previousTimeout = null;
            if (timeout.HasValue)
            {
                //store previous timeout
                previousTimeout = ((IObjectContextAdapter) this).ObjectContext.CommandTimeout;
                ((IObjectContextAdapter) this).ObjectContext.CommandTimeout = timeout;
            }

            var transactionalBehavior = doNotEnsureTransaction
                ? TransactionalBehavior.DoNotEnsureTransaction
                : TransactionalBehavior.EnsureTransaction;
            var result = this.Database.ExecuteSqlCommand(transactionalBehavior, sql, parameters);

            if (timeout.HasValue)
            {
                //设置之前的超时时间
                ((IObjectContextAdapter) this).ObjectContext.CommandTimeout = previousTimeout;
            }

            return result;
        }

        /// <summary>
        /// 从上下文中分离实体
        /// </summary>
        /// <param name="entity">实体</param>
        public void Detach(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            ((IObjectContextAdapter)this).ObjectContext.Detach(entity);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 获取或设置一个值，该值指示是否启用了代理创建设置（在EF中使用）
        /// </summary>
        public virtual bool ProxyCreationEnabled
        {
            get
            {
                return this.Configuration.ProxyCreationEnabled;
            }
            set
            {
                this.Configuration.ProxyCreationEnabled = value;
            }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示是否启用自动检测更改设置（在EF中使用）
        /// </summary>
        public virtual bool AutoDetectChangesEnabled
        {
            get
            {
                return this.Configuration.AutoDetectChangesEnabled;
            }
            set
            {
                this.Configuration.AutoDetectChangesEnabled = value;
            }
        }

        #endregion
    }
}
