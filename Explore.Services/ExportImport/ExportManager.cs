using Explore.Core;
using Explore.Core.Domain.Game;
using Explore.Services.ExportImport;
using Explore.Services.ExportImport.Help;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Explore.Services.ExportImport
{
    /// <summary>
    /// Export manager
    /// </summary>
    public partial class ExportManager : IExportManager
    {
        #region Fields


        #endregion

        #region Ctor

        public ExportManager()
        {
           
        }

        #endregion

        #region Utilities

        protected virtual void SetCaptionStyle(ExcelStyle style)
        {
            style.Fill.PatternType = ExcelFillStyle.Solid;
            style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
            style.Font.Bold = true;
        }

        /// <summary>
        /// 将对象导出到XLSX
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="properties">类通过对象的属性访问该对象</param>
        /// <param name="itemsToExport">要导出的对象</param>
        /// <returns></returns>
        protected virtual byte[] ExportToXlsx<T>(PropertyByName<T>[] properties, IEnumerable<T> itemsToExport)
        {
            using (var stream = new MemoryStream())
            {
                // 好的，我们现在可以运行样本的真实代码了
                using (var xlPackage = new ExcelPackage(stream))
                {
                    // 如果要将XML写入outputDir，请取消注释此行
                    //xlPackage.DebugMode = true; 

                    // 处理工作表
                    var worksheet = xlPackage.Workbook.Worksheets.Add(typeof(T).Name);
                    var fWorksheet = xlPackage.Workbook.Worksheets.Add("DataForFilters");
                    fWorksheet.Hidden = eWorkSheetHidden.VeryHidden;

                    //创建标题并设置其格式
                    var manager = new PropertyManager<T>(properties.Where(p => !p.Ignore));
                    manager.WriteCaption(worksheet, SetCaptionStyle);

                    var row = 2;
                    foreach (var items in itemsToExport)
                    {
                        manager.CurrentObject = items;
                        manager.WriteToXlsx(worksheet, row++, false, fWorksheet: fWorksheet);
                    }

                    xlPackage.Save();
                }
                return stream.ToArray();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 将游戏流水列表导出为XML
        /// </summary>
        /// <param name="gameTurnovers">游戏流水</param>
        /// <returns>XML格式的结果</returns>
        public virtual string ExportGameDailyStatisticToXml(IList<GameTurnover> gameTurnover)
        {

            var sb = new StringBuilder();
            var stringWriter = new StringWriter(sb);
            var xmlWriter = new XmlTextWriter(stringWriter);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("GameDailyStatistic");
            xmlWriter.WriteAttributeString("Version", ExploreVersion.CurrentVersion);

            foreach (var turnover in gameTurnover)
            {
                xmlWriter.WriteStartElement("Turnove");

                xmlWriter.WriteString("CreateTime", turnover.CreateTime);
                xmlWriter.WriteString("GameUser", turnover.GameUser);
                xmlWriter.WriteString("GameCount", turnover.GameCount);
                xmlWriter.WriteString("GameWin", turnover.GameWin);
                xmlWriter.WriteString("GameFail", turnover.GameFail);

                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            return stringWriter.ToString();
        }

        /// <summary>
        /// 将游戏流水列表导出为XLSX
        /// </summary>
        /// <param name="gameTurnovers">游戏流水</param>
        /// <returns></returns>
        public virtual byte[] ExportGameDailyStatisticToXlsx(IEnumerable<GameTurnover> gameTurnovers)
        {
            //property array
            var properties = new[]
            {
                new PropertyByName<GameTurnover>("日期", p => p.CreateTime),
                new PropertyByName<GameTurnover>("用户人数", p => p.GameUser),
                new PropertyByName<GameTurnover>("游戏次数", p => p.GameCount),
                new PropertyByName<GameTurnover>("消耗", p => p.GameWin),
                new PropertyByName<GameTurnover>("返还", p => p.GameFail)
            };

            return ExportToXlsx(properties, gameTurnovers);
        }

        #endregion
    }
}
