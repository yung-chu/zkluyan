using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;

namespace Zkly.Web.Controllers
{
    public class ErrorController : BaseController
    {
        // GET: /Error/InternalServerError
        public ActionResult InternalServerError()
        {
            return View("Error");
        }

        // GET: /Error/PageNotFound
        public ActionResult PageNotFound()
        {
            return View("404");
        }

        // GET: /Error/Forbidden
        public ActionResult Forbidden()
        {
            return View("403");
        }

        #region
        public ActionResult LogsUsersErro()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult RoadshowErro()
        {
            return View();
        }

        public ActionResult RoadshowLoagErro()
        {
            return View();
        }

        [Route("set-preference")]
        public ActionResult SetPreferencesErro()
        {
            return View();
        }

        public ActionResult PromptErro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Register", "Account");
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        #endregion
    }
}