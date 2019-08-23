using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Infrastructure.Mapper
{
    /// <summary>
    /// 映射配置注册接口
    /// </summary>
    public interface IMapperConfiguration
    {
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <returns>映射器配置方法</returns>
        Action<IMapperConfigurationExpression> GetConfiguration();

        /// <summary>
        /// 此映射器实现的顺序
        /// </summary>
        int Order { get; }
    }
}
