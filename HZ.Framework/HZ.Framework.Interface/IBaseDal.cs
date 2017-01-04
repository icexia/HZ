using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZ.Framework.Interface
{
    public interface IBaseDal<T> where T:class,new ()
    {
        bool Add(T model);
        bool Delete(int Id);

        bool Update(T model);

        T GetModel(int Id);
    }
}
