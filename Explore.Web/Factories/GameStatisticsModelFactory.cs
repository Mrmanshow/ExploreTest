using Explore.Core.Domain.Game;
using Explore.Web.Models.Game;
using Explore.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Explore.Services.Localization;
using Explore.Core;

namespace Explore.Web.Factories
{
    public partial class GameStatisticsModelFactory: IGameStatisticsModelFactory
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public GameStatisticsModelFactory(ILocalizationService localizationService,
                                        IWorkContext workContext)
        {
            this._localizationService = localizationService;
            this._workContext = workContext;
        }

        #endregion

        #region Methods

        public virtual IList<GameDailyStatisticsModel> PrepareGameDailyStatisticsListModel(IList<GameTurnover> gameTurnovers)
        {
            var list = gameTurnovers.Select(g =>
            {
                var model = g.ToModel();
                model.GameType = g.GameType.GetLocalizedEnum(_localizationService, _workContext);

                return model;
            });

            return list.ToList();
        }

        #endregion
    }
}