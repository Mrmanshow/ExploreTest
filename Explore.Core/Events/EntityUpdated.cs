using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Events
{
    /// <summary>
    /// 用于更新实体的容器。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityUpdated<T> where T : BaseEntity
    {
        public EntityUpdated(T entity)
        {
            this.Entity = entity;
        }

        public T Entity { get; private set; }
    }
}
