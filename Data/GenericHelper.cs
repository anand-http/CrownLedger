using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace fintech.Data
{
    public static class DataTableHelper
    {
        public static List<T> ConvertDataTableToList<T>(this DataTable dt)
        {
            try
            {
                List<T> data = new List<T>();
                foreach (DataRow row in dt.Rows)
                {
                    T item = GetItem<T>(row);
                    data.Add(item);
                }
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static T GetItem<T>(DataRow dr)
        {
            string columnName;
            try
            {
                Type temp = typeof(T);
                T obj = Activator.CreateInstance<T>();
                foreach (DataColumn column in dr.Table.Columns)
                {
                    //temp.GetProperty(column.ColumnName).GetValue(dr, null)
                    //PropertyInfo pro = temp.GetProperties().First(w => w.Name.ToLower() == column.ColumnName.ToLower());
                    //pro.GetValue()
                    foreach (PropertyInfo pro in temp.GetProperties().Where(w => w.Name.ToLower() == column.ColumnName.ToLower()))
                    {
                        columnName = column.ColumnName;
                        if (dr[column.ColumnName] != System.DBNull.Value)
                            if (column.DataType.Name == "Byte[]")
                            {
                                pro.SetValue(obj, Convert.ToBase64String((byte[])dr[column.ColumnName]), null);
                            }
                            else if (pro.PropertyType.IsEnum)
                            {
                                pro.SetValue(obj, Enum.Parse(pro.PropertyType, dr[column.ColumnName].ToString()), null);
                            }
                            else
                            {
                                pro.SetValue(obj, dr[column.ColumnName], null);
                            }
                        break;
                    }
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
  }