using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Explore.Services.ExportImport.Help
{
    /// <summary>
    /// 用于处理PropertyByName对象列表的类
    /// </summary>
    /// <typeparam name="T">Object type</typeparam>
    public class PropertyManager<T>
    {
        /// <summary>
        /// All properties
        /// </summary>
        private readonly Dictionary<string, PropertyByName<T>> _properties;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="properties">All acsess properties</param>
        public PropertyManager(IEnumerable<PropertyByName<T>> properties)
        {
            _properties = new Dictionary<string, PropertyByName<T>>();

            var poz = 1;
            foreach (var propertyByName in properties)
            {
                propertyByName.PropertyOrderPosition = poz;
                poz++;
                _properties.Add(propertyByName.PropertyName, propertyByName);
            }
        }

        /// <summary>
        /// 当前对象
        /// </summary>
        public T CurrentObject { get; set; }

        /// <summary>
        /// Return properti index
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <returns></returns>
        public int GetIndex(string propertyName)
        {
            if (!_properties.ContainsKey(propertyName))
                return -1;

            return _properties[propertyName].PropertyOrderPosition;
        }

        /// <summary>
        /// Access object by property name
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <returns>Property value</returns>
        public object this[string propertyName]
        {
            get
            {
                return _properties.ContainsKey(propertyName) && CurrentObject != null
                    ? _properties[propertyName].GetProperty(CurrentObject)
                    : null;
            }
        }

        /// <summary>
        /// 将对象数据写入Xlsx工作表
        /// </summary>
        /// <param name="worksheet">数据工作表</param>
        /// <param name="row">行号</param>
        /// <param name="exportImportUseDropdownlistsForAssociatedEntities">指示是否需要创建用于导出的下拉列表</param>
        /// <param name="cellOffset">单元格偏移</param>
        /// <param name="fWorksheet">筛选工作表</param>
        public void WriteToXlsx(ExcelWorksheet worksheet, int row, bool exportImportUseDropdownlistsForAssociatedEntities, int cellOffset = 0, ExcelWorksheet fWorksheet = null)
        {
            if (CurrentObject == null)
                return;

            foreach (var prop in _properties.Values)
            {
                var cell = worksheet.Cells[row, prop.PropertyOrderPosition + cellOffset];
                if (prop.IsDropDownCell)
                {
                    var dropDownElements = prop.GetDropDownElements();
                    if (!dropDownElements.Any())
                    {
                        cell.Value = string.Empty;
                        continue;
                    }

                    cell.Value = prop.GetItemText(prop.GetProperty(CurrentObject));

                    if (!exportImportUseDropdownlistsForAssociatedEntities)
                        continue;

                    var validator = cell.DataValidation.AddListDataValidation();

                    validator.AllowBlank = prop.AllowBlank;

                    if (fWorksheet == null)
                        continue;

                    var fRow = 1;
                    foreach (var dropDownElement in dropDownElements)
                    {
                        var fCell = fWorksheet.Cells[fRow++, prop.PropertyOrderPosition];

                        if (fCell.Value != null && fCell.Value.ToString() == dropDownElement)
                            break;

                        fCell.Value = dropDownElement;
                    }

                    validator.Formula.ExcelFormula = string.Format("{0}!{1}:{2}", fWorksheet.Name, fWorksheet.Cells[1, prop.PropertyOrderPosition].Address, fWorksheet.Cells[dropDownElements.Length, prop.PropertyOrderPosition].Address);
                }
                else
                {
                    cell.Value = prop.GetProperty(CurrentObject);
                }
            }
        }

        /// <summary>
        /// Read object data from XLSX worksheet
        /// </summary>
        /// <param name="worksheet">worksheet</param>
        /// <param name="row">Row index</param>
        /// /// <param name="cellOffset">Cell offset</param>
        public void ReadFromXlsx(ExcelWorksheet worksheet, int row, int cellOffset = 0)
        {
            if (worksheet == null || worksheet.Cells == null)
                return;

            foreach (var prop in _properties.Values)
            {
                prop.PropertyValue = worksheet.Cells[row, prop.PropertyOrderPosition + cellOffset].Value;
            }
        }

        /// <summary>
        /// 将标题（第一行）写入XLSX工作表
        /// </summary>
        /// <param name="worksheet">工作表</param>
        /// <param name="setStyle">单元格阳台是</param>
        /// <param name="row">行号</param>
        /// <param name="cellOffset">单元格偏移</param>
        public void WriteCaption(ExcelWorksheet worksheet, Action<ExcelStyle> setStyle, int row = 1, int cellOffset = 0)
        {
            foreach (var caption in _properties.Values)
            {
                var cell = worksheet.Cells[row, caption.PropertyOrderPosition + cellOffset];
                cell.Value = caption;
                setStyle(cell.Style);
            }
        }

        /// <summary>
        /// Count of properties
        /// </summary>
        public int Count
        {
            get { return _properties.Count; }
        }

        /// <summary>
        /// Get property by name
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public PropertyByName<T> GetProperty(string propertyName)
        {
            return _properties.ContainsKey(propertyName) ? _properties[propertyName] : null;
        }

        /// <summary>
        /// Get property array
        /// </summary>
        public PropertyByName<T>[] GetProperties
        {
            get { return _properties.Values.ToArray(); }
        }


        public void SetSelectList(string propertyName, SelectList list)
        {
            var tempProperty = GetProperty(propertyName);
            if (tempProperty != null)
                tempProperty.DropDownElements = list;
        }

        public bool IsCaption
        {
            get { return _properties.Values.All(p => p.IsCaption); }
        }
    }
}
