using System;
using System.Collections.Concurrent;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CaptchaMvc.Infrastructure;
using CaptchaMvc.Interface;
using Zkly.BLL.FileSync;
using Zkly.Common.Config;
using Zkly.Common.Log;
using Zkly.DAL.Migrations;

namespace Zkly.Web
{
    public class MvcApplication : HttpApplication
    {
        #region Fields

        public const string MultipleParameterKey = "_multiple_";

        private static readonly ConcurrentDictionary<int, ICaptchaManager> CaptchaManagers = new ConcurrentDictionary<int, ICaptchaManager>();

        #endregion

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MigrateConfig.RegisterContexts();
            CaptchaUtils.CaptchaManager.StorageProvider = new SessionStorageProvider();
            if (!AppSettings.FileWatcherDisable)
            {
                FileWatcher.Register();
            }         
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //在出现未处理的错误时运行的代码
            Exception exception = Server.GetLastError();
            var msg = string.Format("客户IP:{0}\r\n请求的URL:{1}\r\n异常信息:{2}", Request.UserHostAddress, Request.Url, exception.Message);
            Logger.Error(msg, exception);
        }
    }
}
