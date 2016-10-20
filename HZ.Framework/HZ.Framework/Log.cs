using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: log4net.Config.DOMConfigurator(ConfigFile = "log4net.config", Watch = true)] 
namespace HZ.Framework
{
    
    public static class Log
    {
        public static log4net.ILog DataAccessLog = log4net.LogManager.GetLogger("DataAccess");
    }
}
