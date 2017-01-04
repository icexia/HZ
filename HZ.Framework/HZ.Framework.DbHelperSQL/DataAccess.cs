using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZ.Framework.DbUtility
{
    public static class DataAccess<T> where T:class
    {
        private static readonly string AssemblyName = ConfigurationManager.AppSettings["程序集名称"];
        private static readonly string db = ConfigurationManager.AppSettings["HZDB"];

        public static T CreateDal()
        {
            try
            {
                return (T)System.Reflection.Assembly.Load(AssemblyName).CreateInstance(db);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
    }
}
