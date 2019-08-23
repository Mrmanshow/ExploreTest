using Explore.Core;
using Explore.Data;
using Explore.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Common
{
    public static class GenericAttributeExtensions
    {
        /// <summary>
        /// 获取实体的属性
        /// </summary>
        /// <typeparam name="TPropType">属性类型</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="key">键</param>
        /// <param name="storeId">加载特定于某个存储的值；传递0以加载为所有存储共享的值</param>
        /// <returns>属性</returns>
        public static TPropType GetAttribute<TPropType>(this BaseEntity entity, string key, int storeId = 0)
        {
            var genericAttributeService = EngineContext.Current.Resolve<IGenericAttributeService>();
            return GetAttribute<TPropType>(entity, key, genericAttributeService, storeId);
        }

        /// <summary>
        /// 获取实体的属性
        /// </summary>
        /// <typeparam name="TPropType">属性类型</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="key">键</param>
        /// <param name="genericAttributeService">GenericAttributeService</param>
        /// <param name="storeId">加载特定于某个存储的值；传递0以加载为所有存储共享的值</param>
        /// <returns>属性</returns>
        public static TPropType GetAttribute<TPropType>(this BaseEntity entity,
            string key, IGenericAttributeService genericAttributeService, int storeId = 0)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            string keyGroup = entity.GetUnproxiedEntityType().Name;

            var props = genericAttributeService.GetAttributesForEntity(entity.Id, keyGroup);
            //little hack here (only for unit testing). we should write ecpect-return rules in unit tests for such cases
            if (props == null)
                return default(TPropType);
            props = props.Where(x => x.StoreId == storeId).ToList();
            if (!props.Any())
                return default(TPropType);

            var prop = props.FirstOrDefault(ga =>
                ga.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)); //should be culture invariant

            if (prop == null || string.IsNullOrEmpty(prop.Value))
                return default(TPropType);

            return CommonHelper.To<TPropType>(prop.Value);
        }
    }
}
