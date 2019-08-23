using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Events
{
    /// <summary>
    /// 用于传递已删除实体的容器。这不适用于通过位列删除logicaly的实体。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityDeleted<T> where T : BaseEntity
    {
        public EntityDeleted(T entity)
        {
            this.Entity = entity;
        }

        public T Entity { get; private set; }
    }
}
