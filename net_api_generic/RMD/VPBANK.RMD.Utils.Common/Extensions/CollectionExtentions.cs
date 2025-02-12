using System;
using System.Collections.Generic;
using System.Data;

namespace VPBANK.RMD.Utils.Common.Extensions
{
    public static class CollectionExtentions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> values)
        {
            var table = new DataTable();
            table.Columns.Add("Item", typeof(T));
            if (values != null)
            {
                foreach (var value in values)
                {
                    table.Rows.Add(value);
                }
            }
            return table;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static DataTable ToUserDefinedDataTable<T>(this IEnumerable<T> values) where T : class
        {
            var table = new DataTable();

            var properties = typeof(T).GetProperties();
            foreach (var prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            if (values != null)
            {
                foreach (var value in values)
                {
                    var newRow = table.NewRow();
                    foreach (var prop in properties)
                    {
                        if (table.Columns.Contains(prop.Name))
                            newRow[prop.Name] = prop.GetValue(value, null) ?? DBNull.Value;
                    }
                    table.Rows.Add(newRow);
                }
            }
            return table;
        }
    }
}
