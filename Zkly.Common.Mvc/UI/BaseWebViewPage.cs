using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Zkly.BLL.Account;

namespace Zkly.Common.Mvc.UI
{
    public abstract class BaseWebViewPage : BaseWebViewPage<object>
    {
    }

    public abstract class BaseWebViewPage<T> : WebViewPage<T>
    {
        private ApplicationUserManager userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return this.userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }

            private set
            {
                this.userManager = value;
            }
        }

        private ApplicationRoleManager roleManager;

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return this.roleManager ?? Request.GetOwinContext().Get<ApplicationRoleManager>();
            }

            private set
            {
                this.roleManager = value;
            }
        }

        public override void InitHelpers()
        {
            base.InitHelpers();
        }
    }
}
