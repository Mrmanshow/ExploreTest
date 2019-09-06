using Explore.Core;
using Explore.Core.Caching;
using Explore.Core.Data;
using Explore.Core.Domain.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Game
{
    public partial class GameStatisticsService : IGameStatisticsService
    {
        #region Constants

        /// <summary>
        /// 游戏每日数据缓存键
        /// </summary>
        /// <remarks>
        /// {0} : 游戏类型
        /// {1} : 查询开始时间
        /// {2} : 查询结束时间
        /// {3} : 页数
        /// {4} : 一页数量
        /// </remarks>
        public const string GAME_STATISTICS_BY_DATE_KEY = "Explore.gamestatistics.date-{0}-{1}-{2}-{3}-{4}";

        #endregion

        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IRepository<ScratchCard> _scratchCardRepository;
        private readonly IRepository<Sea> _seaRepository;
        private readonly IRepository<LabaOrder> _labaOrderRepository;
        private readonly IRepository<LuckEggOrder> _luckEggOrderRepository;
        private readonly IRepository<LuckEggOrderDetail> _luckEggOrderDetailRepository;

        #endregion

        #region Ctor

        public GameStatisticsService(ICacheManager cacheManager,
            IRepository<ScratchCard> scratchCardRepository, 
            IRepository<Sea> seaRepository,
            IRepository<LabaOrder> labaOrderRepository,
            IRepository<LuckEggOrder> luckEggOrderRepository,
            IRepository<LuckEggOrderDetail> luckEggOrderDetailRepository)
        {
            this._cacheManager = cacheManager;
            this._scratchCardRepository = scratchCardRepository;
            this._seaRepository = seaRepository;
            this._labaOrderRepository = labaOrderRepository;
            this._luckEggOrderRepository = luckEggOrderRepository;
            this._luckEggOrderDetailRepository = luckEggOrderDetailRepository;
        }

        #endregion

        #region Methods

        public virtual IPagedList<GameTurnover> GetGameStatisticsByDate(int gameType, DateTime beginDate, DateTime endDate,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            string key = string.Format(GAME_STATISTICS_BY_DATE_KEY, gameType, beginDate, endDate, pageIndex, pageSize);

            return _cacheManager.Get(key, () =>
            {
                if (gameType == 0)
                    return null;


                var query = from s in _scratchCardRepository.Table
                            where beginDate < s.ScratchTime && s.ScratchTime < endDate && s.Status == 2
                            group s by new { s.ScratchTime.Year, s.ScratchTime.Month, s.ScratchTime.Day } into g
                            select new GameTurnover
                            {
                                GameUser = g.Select(c => c.UserId).Distinct().Count(),
                                GameCount = g.Count(),
                                GameWin = g.Sum(a => a.BuyAmount),
                                GameFail = g.Sum(a => a.Amount),
                                CreateTime = g.Key.Year + "-" + g.Key.Month + "-" + g.Key.Day
                            };

                switch (gameType)
                {
                    case 2:
                        query = from s in _seaRepository.Table
                                where beginDate < s.date && s.date < endDate
                                group s by new { s.date.Year, s.date.Month, s.date.Day } into g
                                select new GameTurnover
                                {
                                    GameUser = g.Select(c => c.UserId).Distinct().Count(),
                                    GameCount = g.Count(),
                                    GameWin = g.Sum(a => a.BetSum),
                                    GameFail = g.Sum(a => a.BetSum + a.WinSum),
                                    CreateTime = g.Key.Year + "-" + g.Key.Month + "-" + g.Key.Day
                                };
                        break;
                    case 3:
                        query = from s in _luckEggOrderRepository.Table
                                join d in _luckEggOrderDetailRepository.Table
                                on s.Id equals d.OrderId
                                where beginDate < d.CreateTime && d.CreateTime < endDate && s.Status == 1
                                group new { s, d } by new { d.CreateTime.Year, d.CreateTime.Month, d.CreateTime.Day } into g
                                select new GameTurnover
                                {
                                    GameUser = g.Select(c => c.s.UserId).Distinct().Count(),
                                    GameCount = g.Select(c => c.s.Id).Distinct().Count(),
                                    GameWin = g.Select(s => new { s.s.Id, s.s.Amount }).Distinct().Sum(a => a.Amount),
                                    GameFail = g.Select(s => new { s.s.Id, s.s.WinAmount }).Distinct().Sum(a => a.WinAmount),
                                    CreateTime = g.Key.Year + "-" + g.Key.Month + "-" + g.Key.Day
                                };
                        break;
                    case 4:
                        query = from s in _labaOrderRepository.Table
                                where beginDate < s.CreateTime && s.CreateTime < endDate && s.Status == 1
                                group s by new { s.CreateTime.Year, s.CreateTime.Month, s.CreateTime.Day } into g
                                select new GameTurnover
                                {
                                    GameUser = g.Select(c => c.UserId).Distinct().Count(),
                                    GameCount = g.Count(),
                                    GameWin = g.Sum(a => a.Amount),
                                    GameFail = g.Sum(a => a.WinAmount),
                                    CreateTime = g.Key.Year + "-" + g.Key.Month + "-" + g.Key.Day
                                };
                        break;
                }



                query = query.OrderByDescending(q => q.CreateTime);

                return new PagedList<GameTurnover>(query, pageIndex, pageSize);

            });
        }

        #endregion
    }
}
