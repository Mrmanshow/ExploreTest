using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Data
{
    public partial interface IRepository<T> where T: BaseEntity
    {
        /// <summary>
        /// 根据标识获得实体
        /// </summary>
        /// <param name="id">标识</param>
        /// <returns>实体</returns>
        T GetById(object id);

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">实体</param>
        void Insert(T entity);

        /// <summary>
        /// 添加实体列表
        /// </summary>
        /// <param name="entities">实体列表</param>
        void Insert(IEnumerable<T> entities);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">Entity</param>
        void Update(T entity);

        /// <summary>
        /// 更新实体列表
        /// </summary>
        /// <param name="entities">实体列表</param>
        void Update(IEnumerable<T> entities);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">实体</param>
        void Delete(T entity);

        /// <summary>
        /// 删除实体列表
        /// </summary>
        /// <param name="entities">实体列表</param>
        void Delete(IEnumerable<T> entities);

        /// <summary>
        /// 获取实体列表
        /// </summary>
        IQueryable<T> Table { get; }

        /// <summary>
        /// 获取一个启用了“no tracking”的表（EF功能），仅当只读操作加载记录时才使用它
        /// </summary>
        IQueryable<T> TableNoTracking { get; }
    }
}
