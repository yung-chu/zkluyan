using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Zkly.BLL.Account;

namespace Zkly.Common.Mvc.Controllers
{
    public class UserController : Controller
    {
        #region properties
        private ApplicationUserManager userManager;
        private ApplicationSignInManager signInManager;
        private ApplicationRoleManager roleManager;

        public string UserId
        {
            get { return User.Identity.GetUserId(); }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return this.userManager ?? (this.userManager = this.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>());
            }

            protected set
            {
                this.userManager = value;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return this.signInManager ?? (this.signInManager = this.HttpContext.GetOwinContext().Get<ApplicationSignInManager>());
            }

            protected set
            {
                this.signInManager = value;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return this.roleManager ?? (this.roleManager = this.HttpContext.GetOwinContext().Get<ApplicationRoleManager>());
            }

            protected set
            {
                this.roleManager = value;
            }
        }
        #endregion

        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (this.Url.IsLocalUrl(returnUrl))
            {
                return this.Redirect(returnUrl);
            }

            return this.RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.userManager != null)
                {
                    this.userManager.Dispose();
                    this.userManager = null;
                }

                if (this.signInManager != null)
                {
                    this.signInManager.Dispose();
                    this.signInManager = null;
                }

                if (this.roleManager != null)
                {
                    this.roleManager.Dispose();
                    this.roleManager = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}