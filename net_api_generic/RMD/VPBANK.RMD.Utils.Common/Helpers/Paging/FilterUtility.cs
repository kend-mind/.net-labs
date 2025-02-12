using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VPBANK.RMD.Utils.Common.Extensions;
using static VPBANK.RMD.Utils.Common.Enums;

namespace VPBANK.RMD.Utils.Common.Helpers.Paging
{
    public class FilterUtility
    {
        /// <summary>  
        /// Filter parameters Model Class
        /// </summary>  
        public class FilterParams
        {
            public string ColumnName { get; set; } = string.Empty;
            public string FilterValue { get; set; } = string.Empty;
            public FilterOptions FilterOption { get; set; } = FilterOptions.Contains;
        }

        /// <summary>  
        /// This is generic class responsible for filtering the data  
        /// </summary>  
        /// <typeparam name="T"></typeparam>
        public class Filter<T> where T : class
        {
            public static IEnumerable<T> FilteredData(IEnumerable<FilterParams> filterParams, IEnumerable<T> data)
            {
                IEnumerable<string> distinctColumns = filterParams.Where(x => !string.IsNullOrEmpty(x.ColumnName)).Select(x => x.ColumnName).Distinct();

                foreach (string colName in distinctColumns)
                {
                    var colNameOrigin = colName.ToPascalCaseWithUnderscore();
                    var filterColumn = typeof(T).GetProperty(colNameOrigin, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                    if (filterColumn != null)
                    {
                        IEnumerable<FilterParams> filterValues = filterParams
                            .Where(x => x.ColumnName.ToPascalCaseWithUnderscore().Equals(colNameOrigin, StringComparison.CurrentCultureIgnoreCase))
                            .Select(x => new FilterParams 
                            {
                                ColumnName = x.ColumnName.ToPascalCaseWithUnderscore(),
                                FilterOption = x.FilterOption,
                                FilterValue = x.FilterValue
                            })
                            .Distinct();

                        if (filterValues.Count() > 1)
                        {
                            IEnumerable<T> sameColData = Enumerable.Empty<T>();

                            foreach (var val in filterValues)
                            {
                                sameColData = sameColData.Concat(FilterData(val.FilterOption, data, filterColumn, val.FilterValue));
                            }

                            data = data.Intersect(sameColData);
                        }
                        else
                        {
                            data = FilterData(filterValues.FirstOrDefault().FilterOption, data, filterColumn, filterValues.FirstOrDefault().FilterValue);
                        }
                    }
                }
                return data;
            }

            private static IEnumerable<T> FilterData(FilterOptions filterOption, IEnumerable<T> data, PropertyInfo filterColumn, string filterValue)
            {
                int outValue;
                DateTime dateValue;
                switch (filterOption)
                {
                    #region Number, Datetime

                    case FilterOptions.Equals:
                        if (filterValue == string.Empty)
                        {
                            data = data.Where(x => filterColumn.GetValue(x, null) == null || (filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString().ToLower() == string.Empty));
                        }
                        else
                        {
                            if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                            {
                                data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) == outValue);
                            }
                            else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                            {
                                data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) == dateValue);
                                break;
                            }
                            else
                            {
                                data = data.Where(x => filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString().ToLower() == filterValue.ToLower());
                            }
                        }
                        break;

                    case FilterOptions.DoesNotEquals:
                        if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                        {
                            data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) != outValue);
                        }
                        else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                        {
                            data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) != dateValue);
                            break;
                        }
                        else
                        {
                            data = data.Where(x => filterColumn.GetValue(x, null) == null ||
                                             (filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString().ToLower() != filterValue.ToLower()));
                        }
                        break;

                    case FilterOptions.LessThan:
                        if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                        {
                            data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) < outValue);
                        }
                        else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                        {
                            data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) < dateValue);
                            break;
                        }
                        break;

                    case FilterOptions.LessThanOrEquals:
                        if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                        {
                            data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) <= outValue);
                        }
                        else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                        {
                            data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) <= dateValue);
                            break;
                        }
                        break;

                    case FilterOptions.GreaterThan:
                        if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                        {
                            data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) > outValue);
                        }
                        else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                        {
                            data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) > dateValue);

                        }
                        break;

                    case FilterOptions.GreaterThanOrEquals:
                        if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                        {
                            data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) >= outValue);
                        }
                        else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                        {
                            data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) >= dateValue);
                            break;
                        }
                        break;

                    #endregion

                    #region String Data Type

                    case FilterOptions.Contains:
                        data = data.Where(x => filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString().ToLower().Contains(filterValue.ToString().ToLower()));
                        break;

                    case FilterOptions.DoesNotContain:
                        data = data.Where(x => filterColumn.GetValue(x, null) == null || (filterColumn.GetValue(x, null) != null && !filterColumn.GetValue(x, null).ToString().ToLower().Contains(filterValue.ToString().ToLower())));
                        break;

                    case FilterOptions.StartsWith:
                        data = data.Where(x => filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString().ToLower().StartsWith(filterValue.ToString().ToLower()));
                        break;

                    case FilterOptions.EndsWith:
                        data = data.Where(x => filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString().ToLower().EndsWith(filterValue.ToString().ToLower()));
                        break;

                    case FilterOptions.IsEmpty:
                        data = data.Where(x => filterColumn.GetValue(x, null) == null || (filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString() == string.Empty));
                        break;

                    case FilterOptions.IsNotEmpty:
                        data = data.Where(x => filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString() != string.Empty);
                        break;

                    #endregion
                }
                return data;
            }
        }
    }
}
