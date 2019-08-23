using Explore.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Data
{
    public static class Extensions
    {
        /// <summary>
        /// 获取未经处理的实体类型
        /// </summary>
        /// <remarks> 如果实体框架上下文启用了代理，则运行时将创建实体的代理实例，即动态生成的类，该类继承自实体类，
        /// 并通过插入用于跟踪更改和延迟的特定代码来重写其虚拟属性。
        /// </remarks>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Type GetUnproxiedEntityType(this BaseEntity entity)
        {
            var userType = ObjectContext.GetObjectType(entity.GetType());
            return userType;
        }
    }
}
