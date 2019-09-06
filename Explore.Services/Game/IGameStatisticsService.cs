using Explore.Core;
using Explore.Core.Domain.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Game
{
    public partial interface IGameStatisticsService
    {
        /// <summary>
        /// 根据时间获取每日游戏数据
        /// </summary>
        /// <param name="gameType"></param>
        /// <param name="beginDate"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        IPagedList<GameTurnover> GetGameStatisticsByDate(int gameType, DateTime beginDate, DateTime endDate,
            int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
