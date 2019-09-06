using Explore.Core.Domain.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.ExportImport
{
    /// <summary>
    /// Export manager interface
    /// </summary>
    public partial interface IExportManager
    {
        /// <summary>
        /// 将游戏流水列表导出为XML
        /// </summary>
        /// <param name="gameTurnovers">游戏流水</param>
        /// <returns>XML格式的结果</returns>
        string ExportGameDailyStatisticToXml(IList<GameTurnover> gameTurnovers);


        /// <summary>
        /// 将游戏流水列表导出为XLSX
        /// </summary>
        /// <param name="gameTurnovers">游戏流水</param>
        /// <returns></returns>
        byte[] ExportGameDailyStatisticToXlsx(IEnumerable<GameTurnover> gameTurnovers);
    }
}
