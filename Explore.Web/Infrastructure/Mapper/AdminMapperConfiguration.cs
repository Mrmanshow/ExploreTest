using AutoMapper;
using Explore.Core.Domain.Content;
using Explore.Core.Domain.Customers;
using Explore.Core.Domain.Game;
using Explore.Core.Domain.Game.Laba;
using Explore.Core.Domain.Users;
using Explore.Core.Infrastructure.Mapper;
using Explore.Services.Cms;
using Explore.Web.Models.Cms;
using Explore.Web.Models.Content;
using Explore.Web.Models.Game;
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

                cfg.CreateMap<Banner, BannerModel>()
                    .ForMember(dest => dest.Id, mo => mo.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Theme, mo => mo.MapFrom(src => src.Theme))
                    .ForMember(dest => dest.BannerLink, mo => mo.MapFrom(src => src.BannerLink))
                    .ForMember(dest => dest.BannerOrder, mo => mo.MapFrom(src => src.BannerOrder))
                    .ForMember(dest => dest.LinkType, mo => mo.MapFrom(src => Convert.ToBoolean(src.LinkType)))
                    .ForMember(dest => dest.ShowBeginDate, mo => mo.MapFrom(src => src.ShowBeginDate))
                    .ForMember(dest => dest.ShowEndDate, mo => mo.MapFrom(src => src.ShowEndDate))
                    .ForMember(dest => dest.CreateTime, mo => mo.MapFrom(src => src.CreateTime.ToString("yyyy-MM-dd HH:mmm:ss")))
                    .AfterMap((src, dest) => dest.PictureModel.PictureId = src.Pictures.Count > 0 ? src.Pictures.First().PictureId : 0);
                cfg.CreateMap<BannerModel, Banner>()
                    .ForMember(dest => dest.Id, mo => mo.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Theme, mo => mo.MapFrom(src => src.Theme))
                    .ForMember(dest => dest.BannerLink, mo => mo.MapFrom(src => src.BannerLink))
                    .ForMember(dest => dest.BannerOrder, mo => mo.MapFrom(src => src.BannerOrder))
                    .ForMember(dest => dest.LinkType, mo => mo.MapFrom(src => Convert.ToBoolean(src.LinkType)))
                    .ForMember(dest => dest.Status, mo => mo.MapFrom(src => src.Status))
                    .ForMember(dest => dest.ShowBeginDate, mo => mo.MapFrom(src => src.ShowBeginDate))
                    .ForMember(dest => dest.ShowEndDate, mo => mo.MapFrom(src => src.ShowEndDate))
                    .ForMember(dest => dest.CreateTime, mo => mo.Ignore())
                    .ForMember(dest => dest.Status, mo => mo.MapFrom(src => Int32.Parse(src.Status)))
                    .ForMember(dest => dest.Type, mo => mo.MapFrom(src => Int32.Parse(src.Type)));

                cfg.CreateMap<GameTurnover, GameDailyStatisticsModel>()
                    .ForMember(dest => dest.GameCount, mo => mo.MapFrom(src => src.GameCount))
                    .ForMember(dest => dest.GameUser, mo => mo.MapFrom(src => src.GameUser))
                    .ForMember(dest => dest.GameWin, mo => mo.MapFrom(src => src.GameWin))
                    .ForMember(dest => dest.GameFail, mo => mo.MapFrom(src => src.GameFail))
                    .ForMember(dest => dest.CreateTime, mo => mo.MapFrom(src => src.CreateTime));

                cfg.CreateMap<LabaWinRoute, GameLabaRouteModel>()
                    .ForMember(dest => dest.Id, mo => mo.MapFrom(src => src.Id))
                    .ForMember(dest => dest.X1, mo => mo.MapFrom(src => src.X1))
                    .ForMember(dest => dest.X2, mo => mo.MapFrom(src => src.X2))
                    .ForMember(dest => dest.X3, mo => mo.MapFrom(src => src.X3))
                    .ForMember(dest => dest.X4, mo => mo.MapFrom(src => src.X4))
                    .ForMember(dest => dest.X5, mo => mo.MapFrom(src => src.X5))
                    .ForMember(dest => dest.Y1, mo => mo.MapFrom(src => src.Y1))
                    .ForMember(dest => dest.Y2, mo => mo.MapFrom(src => src.Y2))
                    .ForMember(dest => dest.Y3, mo => mo.MapFrom(src => src.Y3))
                    .ForMember(dest => dest.Y4, mo => mo.MapFrom(src => src.Y4))
                    .ForMember(dest => dest.Y5, mo => mo.MapFrom(src => src.Y5))
                    .ForMember(dest => dest.Status, mo => mo.MapFrom(src => src.Status));
                cfg.CreateMap<LabaWinRouteNew, GameLabaRouteModel>()
                    .ForMember(dest => dest.Id, mo => mo.MapFrom(src => src.Id))
                    .ForMember(dest => dest.X1, mo => mo.MapFrom(src => src.X1))
                    .ForMember(dest => dest.X2, mo => mo.MapFrom(src => src.X2))
                    .ForMember(dest => dest.X3, mo => mo.MapFrom(src => src.X3))
                    .ForMember(dest => dest.X4, mo => mo.MapFrom(src => src.X4))
                    .ForMember(dest => dest.X5, mo => mo.MapFrom(src => src.X5))
                    .ForMember(dest => dest.Y1, mo => mo.MapFrom(src => src.Y1))
                    .ForMember(dest => dest.Y2, mo => mo.MapFrom(src => src.Y2))
                    .ForMember(dest => dest.Y3, mo => mo.MapFrom(src => src.Y3))
                    .ForMember(dest => dest.Y4, mo => mo.MapFrom(src => src.Y4))
                    .ForMember(dest => dest.Y5, mo => mo.MapFrom(src => src.Y5))
                    .ForMember(dest => dest.Status, mo => mo.MapFrom(src => src.Status));
                cfg.CreateMap<GameLabaRouteModel, LabaWinRoute>()
                    .ForMember(dest => dest.Id, mo => mo.MapFrom(src => src.Id))
                    .ForMember(dest => dest.X1, mo => mo.MapFrom(src => src.X1))
                    .ForMember(dest => dest.X2, mo => mo.MapFrom(src => src.X2))
                    .ForMember(dest => dest.X3, mo => mo.MapFrom(src => src.X3))
                    .ForMember(dest => dest.X4, mo => mo.MapFrom(src => src.X4))
                    .ForMember(dest => dest.X5, mo => mo.MapFrom(src => src.X5))
                    .ForMember(dest => dest.Y1, mo => mo.MapFrom(src => src.Y1))
                    .ForMember(dest => dest.Y2, mo => mo.MapFrom(src => src.Y2))
                    .ForMember(dest => dest.Y3, mo => mo.MapFrom(src => src.Y3))
                    .ForMember(dest => dest.Y4, mo => mo.MapFrom(src => src.Y4))
                    .ForMember(dest => dest.Y5, mo => mo.MapFrom(src => src.Y5))
                    .ForMember(dest => dest.Status, mo => mo.MapFrom(src => Int32.Parse(src.Status)))
                    .ForMember(dest => dest.CreateTime, mo => mo.Ignore());
                cfg.CreateMap<GameLabaRouteModel, LabaWinRouteNew>()
                    .ForMember(dest => dest.Id, mo => mo.MapFrom(src => src.Id))
                    .ForMember(dest => dest.X1, mo => mo.MapFrom(src => src.X1))
                    .ForMember(dest => dest.X2, mo => mo.MapFrom(src => src.X2))
                    .ForMember(dest => dest.X3, mo => mo.MapFrom(src => src.X3))
                    .ForMember(dest => dest.X4, mo => mo.MapFrom(src => src.X4))
                    .ForMember(dest => dest.X5, mo => mo.MapFrom(src => src.X5))
                    .ForMember(dest => dest.Y1, mo => mo.MapFrom(src => src.Y1))
                    .ForMember(dest => dest.Y2, mo => mo.MapFrom(src => src.Y2))
                    .ForMember(dest => dest.Y3, mo => mo.MapFrom(src => src.Y3))
                    .ForMember(dest => dest.Y4, mo => mo.MapFrom(src => src.Y4))
                    .ForMember(dest => dest.Y5, mo => mo.MapFrom(src => src.Y5))
                    .ForMember(dest => dest.Status, mo => mo.MapFrom(src => Int32.Parse(src.Status)))
                    .ForMember(dest => dest.CreateTime, mo => mo.Ignore());

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