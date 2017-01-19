using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HZ.Framework
{
    public static class CSVFileHelper
    {
        public static string SaveCSV<T>(List<T> list, string full_path)
        {
            FileInfo file = new FileInfo(full_path);
            if (!file.Directory.Exists)
                file.Directory.Create();

            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();

            int dtColCount = properties.Length;

            StringBuilder data = new StringBuilder();

            //写列名称
            for (int i = 0; i < properties.Length; i++)
            {
                data.Append(properties[i].Name);
                if (i < dtColCount - 1)
                    data.Append(",");
            }
            data.Append("\r\n");

            //sw.WriteLine(data);
            //写行数据
            for (int i = 0; i < list.Count; i++)
            {
                //data.Clear();
                for (int j = 0; j < properties.Length; j++)
                {
                    PropertyInfo property = properties[j];
                    T item = list[i];

                    object value = property.GetValue(item);

                    string str = string.Empty;

                    if (value != null)
                        str = property.GetValue(item).ToString().Replace("\"", "\"\"");

                    if (str.Contains(',') || str.Contains('"') || str.Contains('\r') || str.Contains('\n'))
                        str = string.Format("\"{0}\"", str);

                    data.Append(str);
                    if (j < dtColCount - 1)
                        data.Append(",");
                }

                data.Append("\r\n");
                //sw.WriteLine(data);
            }
            //        sw.Close();
            //    }
            //    fs.Close();
            //}

            return data.ToString();
        }

        #region 01将DataTable的数据写入CSV文件+static void SaveCSV(DataTable dt, string full_path)
        /// <summary>
        /// 将DataTable的数据写入CSV文件
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="full_path"></param>
        public static void SaveCSV(DataTable dt, string full_path)
        {
            FileInfo file = new FileInfo(full_path);
            if (!file.Directory.Exists)
                file.Directory.Create();

            using (FileStream fs = new FileStream(full_path, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                {
                    StringBuilder data = new StringBuilder();
                    int dtColCount = dt.Columns.Count;
                    //写列名称
                    for (int i = 0; i < dtColCount; i++)
                    {
                        data.Append(dt.Columns[i].ColumnName.ToString());
                        if (i < dtColCount - 1)
                            data.Append(",");
                    }
                    sw.WriteLine(data);
                    //写行数据
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        data.Clear();
                        for (int j = 0; j < dtColCount; j++)
                        {
                            string str = dt.Rows[i][j].ToString().Replace("\"", "\"\"");
                            if (str.Contains(',') || str.Contains('"') || str.Contains('\r') || str.Contains('\n'))
                                str = string.Format("\"{0}\"", str);

                            data.Append(str);
                            if (j < dtColCount - 1)
                                data.Append(",");
                        }
                        sw.WriteLine(data);
                    }
                    sw.Close();
                }
                fs.Close();
            }
        }
        #endregion

        #region 02将CSV文件中的数据读取到DataTable+DataTable OpenCSV(string file_path)
        /// <summary>
        /// 将CSV文件中的数据读取到DataTable
        /// </summary>
        /// <param name="file_path"></param>
        /// <returns></returns>
        public static DataTable OpenCSV(string file_path)
        {
            Encoding encoding = EncodingTypeHelper.GetType(file_path);
            DataTable dt = new DataTable();

            using (FileStream fs = new FileStream(file_path, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs, encoding))
                {
                    string strLine = string.Empty;
                    string[] arryLine = null;
                    string[] tableHead = null;
                    //标识列数
                    int columnCount = 0;
                    //标识是否读第一行
                    bool isFirst = true;
                    //逐行读取CSV中的数据
                    while ((strLine = sr.ReadLine()) != null)
                    {
                        if (isFirst)
                        {
                            tableHead = strLine.Split(',');
                            isFirst = false;
                            columnCount = tableHead.Length;
                            //创建列
                            for (int i = 0; i < columnCount; i++)
                            {
                                DataColumn column = new DataColumn(tableHead[i]);
                                dt.Columns.Add(column);
                            }
                        }
                        else
                        {
                            arryLine = strLine.Split(',');
                            DataRow dr = dt.NewRow();
                            for (int j = 0; j < columnCount; j++)
                            {
                                dr[j] = arryLine[j];
                            }
                            dt.Rows.Add(dr);
                        }
                    }

                    sr.Close();
                }
                fs.Close();
            }
            return dt;
        }
        #endregion

        public static Dictionary<int, string[]> ReadCSV(string file_path)
        {
            Encoding encoding = EncodingTypeHelper.GetType(file_path);
            Dictionary<int, string[]> dic = new Dictionary<int, string[]>();
            //int[] intArray = new int[11];

            using (FileStream fs = new FileStream(file_path, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs, encoding))
                {
                    string strLine = string.Empty;
                    string[] arryLine = null;
                    string[] tableHead = null;
                    //标识列数
                    int columnCount = 0;
                    //标识是否读第一行
                    bool isFirst = true;
                    //逐行读取CSV中的数据
                    while ((strLine = sr.ReadLine()) != null)
                    {
                        if (isFirst)
                        {
                            tableHead = strLine.Split(',');
                            isFirst = false;
                            columnCount = tableHead.Length;
                        }
                        else
                        {
                            arryLine = strLine.Split(',');
                            int key = Convert.ToInt32(arryLine[0]);
                            if (dic.ContainsKey(key)) continue;
                            dic.Add(key, arryLine);
                        }
                    }

                    sr.Close();
                }
                fs.Close();
            }
            return dic;
        }
    }
}
