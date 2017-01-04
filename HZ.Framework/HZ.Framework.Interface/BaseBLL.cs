using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZ.Framework.Interface
{
    public class BaseBLL<T> where T:class,new ()
    {
        public IBaseDal<T> dal = HZ.Framework.DbUtility.DataAccess<IBaseDal<T>>.CreateDal();

        public virtual bool Add(T model)
        {
            return dal.Add(model);
        }


        public virtual bool Update(T model)
        {
            return dal.Update(model);
        }

        public virtual bool Delete(int id)
        {
            return dal.Delete(id);
        }

        public virtual T GetModel(int id)
        {
            return dal.GetModel(id);
        }
    }
}
