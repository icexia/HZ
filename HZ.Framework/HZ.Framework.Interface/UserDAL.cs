using HZ.Framework.DbHelperSQL;
using HZ.Framework.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZ.Framework.Interface
{
    public class UserDAL:IBaseDal<Users>
    {
        bool IBaseDal<Users>.Add(Users model)
        {
            string sql = "insert into Users values(@UserId,@UserName)";
            SqlParameter[] parameters ={
                                      new SqlParameter("@UserId",model.UserId),
                                      new SqlParameter("@UserName",model.UserName)
                                      };
            return DBHelperSQL.ExecuteSql(sql, parameters)>1?true:false;
        }

        bool IBaseDal<Users>.Delete(int Id)
        {
            throw new NotImplementedException();
        }

        bool IBaseDal<Users>.Update(Users model)
        {
            throw new NotImplementedException();
        }

        Users IBaseDal<Users>.GetModel(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
