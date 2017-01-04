using HZ.Framework.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZ.Framework.Interface
{
    public class UserBLL : BaseBLL<Users>
    {
        private UserDAL _dal;
        public UserBLL()
            : base()
        {
            _dal = base.dal as UserDAL;
            if (_dal == null) throw new NullReferenceException();
        }
    }
}
