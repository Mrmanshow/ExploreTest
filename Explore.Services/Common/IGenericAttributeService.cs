using Explore.Core;
using Explore.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Common
{
    /// <summary>
    /// 通用属性服务接口
    /// </summary>
    public partial interface IGenericAttributeService
    {
        /// <summary>
        /// Gets an attribute
        /// </summary>
        /// <param name="attributeId">Attribute identifier</param>
        /// <returns>An attribute</returns>
        GenericAttribute GetAttributeById(int attributeId);


        /// <summary>
        /// 获取属性列表
        /// </summary>
        /// <param name="entityId">实体标识符</param>
        /// <param name="keyGroup">键组</param>
        /// <returns>获取属性列表</returns>
        IList<GenericAttribute> GetAttributesForEntity(int entityId, string keyGroup);

        /// <summary>
        /// Save attribute value
        /// </summary>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="storeId">Store identifier; pass 0 if this attribute will be available for all stores</param>
        void SaveAttribute<TPropType>(BaseEntity entity, string key, TPropType value, int storeId = 0);
    }
}
