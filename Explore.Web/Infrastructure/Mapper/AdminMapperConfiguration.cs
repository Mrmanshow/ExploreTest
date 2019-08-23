using AutoMapper;
using Explore.Core.Domain.Customers;
using Explore.Core.Domain.Users;
using Explore.Core.Infrastructure.Mapper;
using Explore.Services.Cms;
using Explore.Web.Models.Cms;
using Explore.Web.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Explore.Web.Infrastructure.Mapper
{
    /// <summary>
    /// admin area模型的AutoMaper配置
    /// </summary>
    public class AdminMapperConfiguration : IMapperConfiguration
    {
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <returns>映射器配置方法</returns>
        public Action<IMapperConfigurationExpression> GetConfiguration()
        {
            //TODO remove 'CreatedOnUtc' ignore mappings because now presentation layer models have 'CreatedOn' property and core entities have 'CreatedOnUtc' property (distinct names)

            Action<IMapperConfigurationExpression> action = cfg =>
            {
                //widgets
                cfg.CreateMap<IWidgetPlugin, WidgetModel>()
                    .ForMember(dest => dest.FriendlyName, mo => mo.MapFrom(src => src.PluginDescriptor.FriendlyName))
                    .ForMember(dest => dest.SystemName, mo => mo.MapFrom(src => src.PluginDescriptor.SystemName))
                    .ForMember(dest => dest.DisplayOrder, mo => mo.MapFrom(src => src.PluginDescriptor.DisplayOrder))
                    .ForMember(dest => dest.IsActive, mo => mo.Ignore())
                    .ForMember(dest => dest.ConfigurationActionName, mo => mo.Ignore())
                    .ForMember(dest => dest.ConfigurationControllerName, mo => mo.Ignore())
                    .ForMember(dest => dest.ConfigurationRouteValues, mo => mo.Ignore())
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());

                cfg.CreateMap<User, UserModel>()
                    .ForMember(dest => dest.Id, mo => mo.MapFrom(src => src.Id))
                    .ForMember(dest => dest.GoldCoin, mo => mo.MapFrom(src => src.UserAssets.GoldCoin))
                    .ForMember(dest => dest.UserName, mo => mo.MapFrom(src => src.UserName))
                    .ForMember(dest => dest.NickName, mo => mo.MapFrom(src => src.NickName))
                    .ForMember(dest => dest.LoginType, mo => mo.Ignore())
                    .ForMember(dest => dest.CreateTime, mo => mo.MapFrom(src => src.CreateTime.ToString("yyyy-MM-dd HH:mmm:ss")));
            };
            return action;
        }


        /// <summary>
        /// 此映射器实现的顺序
        /// </summary>
        public int Order
        {
            get { return 0; }
        }
    }
}