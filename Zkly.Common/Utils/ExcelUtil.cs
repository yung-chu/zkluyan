using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using Excel;

namespace Zkly.Common.Utils
{
    public static class ExcelUtil
    {
        /// <summary>
        /// Reading from a binary Excel file ('97-2003 format; *.xls) or OpenXml Excel file (2007 format; *.xlsx)
        /// </summary>
        public static DataSet ReadDataSet(Stream stream, bool isFirstRowAsColumnNames = true, bool isOpenXmlFormat = true)
        {
            using (
                var excelReader = isOpenXmlFormat
                    ? ExcelReaderFactory.CreateOpenXmlReader(stream)
                    : ExcelReaderFactory.CreateBinaryReader(stream))
            {
                excelReader.IsFirstRowAsColumnNames = isFirstRowAsColumnNames;
                var result = excelReader.AsDataSet();

                //6. Free resources (IExcelDataReader is IDisposable)
                excelReader.Close();

                return result;
            }
        }

        /// <summary>
        /// Reading first table from a binary Excel file ('97-2003 format; *.xls) or OpenXml Excel file (2007 format; *.xlsx)
        /// </summary>
        public static DataTable ReadTable(Stream stream, bool isFirstRowAsColumnNames = true, bool isOpenXmlFormat = true)
        {
            var dataSet = ReadDataSet(stream, isFirstRowAsColumnNames, isOpenXmlFormat);
            return dataSet!=null?(dataSet.Tables.Count > 0 ? dataSet.Tables[0] : null):null;
        }

        /// <summary>
        /// 需要在类型<see cref="T"/>上标记ColumnAttribute
        /// </summary>
        public static IList<T> ReadList<T>(Stream stream, bool isOpenXmlFormat = true) where T : class
        {
            var table = ReadTable(stream, true, isOpenXmlFormat);
            return table.CastToList<T>();
        }

        /// <summary>
        /// 类型<see cref="T"/>为当前column的类型，如string
        /// </summary>
        public static IList<T> ColumnToList<T>(this DataTable table, string columnName)
        {
            if (table == null)
            {
                return new List<T>();
            }

            var itemType = typeof(T);
            return (from DataRow row in table.Rows
                    select (T)Convert.ChangeType(row[columnName], itemType)).ToList();
        }

        /// <summary>
        /// 需要在类型<see cref="T"/>上标记ColumnAttribute
        /// </summary>
        public static IList<T> CastToList<T>(this DataTable table) where T : class
        {
            var targetItemType = typeof(T);
            var activator = targetItemType.GetDefaultActivator<T>();
            var result = new List<T>();
            if (table == null)
            {
                return result;
            }

            var properties = (from p in targetItemType.GetProperties()
                              where p.CanWrite
                              let disp = p.GetCustomAttribute<DisplayAttribute>()
                              let name =disp == null ? p.Name : disp.Name
                              where table.Columns.Contains(name)
                              select new { Name = name, Setter = p.GetPropertySetter(), Type = p.PropertyType }).ToList();

            foreach (DataRow row in table.Rows)
            {
                var item = activator();
                foreach (var property in properties)
                {
                    var value = Convert.ChangeType(row[property.Name], property.Type);
                    property.Setter(item, value);
                }

                result.Add(item);
            }

            return result;
        }
    }
}
