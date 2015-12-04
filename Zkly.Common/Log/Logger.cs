using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;

using log4net;
using log4net.Config;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = @"log4net.config", Watch = true)]

namespace Zkly.Common.Log
{    
    public class Logger
    {
        private static ILog info = LogManager.GetLogger("logInfo");
        private static ILog error = LogManager.GetLogger("logError");

        public static void Info(string message)
        {
            if (info.IsInfoEnabled)
            {
                info.Info(message);
            }
        }

        public static void Info(string message, Exception e)
        {
            if (info.IsInfoEnabled)
            {
                info.Info(message, e);
            }
        }

        public static void Error(string message)
        {
            if (error.IsErrorEnabled)
            {
                error.Error(message);
            }
        }

        public static void Error(string message, Exception e)
        {
            if (error.IsErrorEnabled)
            {
                error.Error(message, e);
            }
        }
    }
}