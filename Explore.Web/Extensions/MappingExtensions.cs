using Explore.Core.Domain.Content;
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

namespace Explore.Web.Extensions
{
    public static class MappingExtensions
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return AutoMapperConfiguration.Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return AutoMapperConfiguration.Mapper.Map(source, destination);
        }

        #region Widgets

        public static WidgetModel ToModel(this IWidgetPlugin entity)
        {
            return entity.MapTo<IWidgetPlugin, WidgetModel>();
        }

        #endregion

        #region User

        public static UserModel ToModel(this User entity)
        {
            return entity.MapTo<User, UserModel>();
        }

        #endregion

        #region Content

        public static BannerModel ToModel(this Banner entity)
        {
            return entity.MapTo<Banner, BannerModel>();
        }

        public static Banner ToEntity(this BannerModel model)
        {
            return model.MapTo<BannerModel, Banner>();
        }

        public static Banner ToEntity(this BannerModel model, Banner destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Game

        public static GameLabaRouteModel ToModel(this LabaWinRoute entity)
        {
            return entity.MapTo<LabaWinRoute, GameLabaRouteModel>();
        }

        public static GameLabaRouteModel ToModel(this LabaWinRouteNew entity)
        {
            return entity.MapTo<LabaWinRouteNew, GameLabaRouteModel>();
        }

        public static LabaWinRoute ToEntity(this GameLabaRouteModel model, LabaWinRoute destination)
        {
            return model.MapTo(destination);
        }

        public static LabaWinRouteNew ToEntity(this GameLabaRouteModel model, LabaWinRouteNew destination)
        {
            return model.MapTo(destination);
        }

        public static GameDailyStatisticsModel ToModel(this GameTurnover entity)
        {
            return entity.MapTo<GameTurnover, GameDailyStatisticsModel>();
        }

        #endregion
    }
}