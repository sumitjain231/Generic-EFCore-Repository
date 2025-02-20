using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace Utility
{
    public static class ObjectTranslator
    {
        public static ObjectMapper<TSource> From<TSource>(TSource source)
        {
            return new ObjectMapper<TSource>(source);
        }

        public static TReturn[] FromArrayType<TReturn, TParam>(TParam[] arraySource)
            where TParam : class
            where TReturn : class
        {
            List<TReturn> arrayDestination = null!;
            if (arraySource != null)
            {
                arrayDestination = new List<TReturn>();
                foreach (TParam source in arraySource)
                {
                    TReturn newRule = From(source).To<TReturn>();
                    arrayDestination.Add(newRule);
                }
            }
            return arrayDestination?.ToArray()!;
        }

        public static List<TReturn> FromListType<TReturn, TParam>(IEnumerable<TParam> listSource)
           where TParam : class
           where TReturn : class
        {
            List<TReturn> listDestination = null!;
            if (listSource != null)
            {
                listDestination = new List<TReturn>();
                foreach (TParam source in listSource)
                {
                    TReturn newRule = From(source).To<TReturn>();
                    listDestination.Add(newRule);
                }
            }
            return listDestination;
        }

        public static DataTable GetDataTableFromObjectArray<T>(T[] objects, string tableName, string[] NonExportProp)
        {
            if (objects != null)
            {
                Type t = typeof(T);
                DataTable dt = new DataTable(tableName);
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(t);
                foreach (PropertyDescriptor property in properties)
                {
                    if (!NonExportProp.Contains(property.DisplayName))
                        dt.Columns.Add(new DataColumn(property.DisplayName));
                }

                foreach (var o in objects)
                {
                    DataRow dr = dt.NewRow();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (!NonExportProp.Contains(dc.ColumnName))
                        {
                            dr[dc.ColumnName] = o!.GetType().GetProperty(dc.ColumnName.Trim())!.GetValue(o, null);
                            if (string.IsNullOrEmpty(Convert.ToString(dr[dc.ColumnName])))
                            {
                                dr[dc.ColumnName] = " ";
                            }
                        }
                    }
                    dt.Rows.Add(dr);
                }
                return dt;
            }
            return null!;
        }
       
    }
}
