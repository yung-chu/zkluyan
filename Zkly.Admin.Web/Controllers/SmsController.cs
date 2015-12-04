using System.Web.Mvc;
using Zkly.Common.SMS;

namespace Zkly.Admin.Web.Controllers
{
    [Authorize]
    public class SmsController : Controller
    {
        // GET: Sms/Register
        public ActionResult Register()
        {
            return View("Register");
        }

        // POST: Sms/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(string userName, string mobile, string phone, string fax, string email, string postcode, string company, string address)
        {
            if (!ModelState.IsValid)
            {
                return View("Register");
            }

            var client = new SmsClient();
            var model = client.Register(userName, mobile, phone, fax, email, postcode, company, address);
            return View("Result", model);
        }

        // GET: Sms/Balance
        public ActionResult Balance()
        {
            var client = new SmsClient();
            var model = client.QueryAccount();
            return View("Result", model);
        }

        // GET: Sms/Report
        public ActionResult Report()
        {
            var client = new SmsClient();
            var model = client.QueryReport();
            return View("Result", model);
        }

        // GET: Sms/UpdatePassword
        public ActionResult UpdatePassword()
        {
            return View("ResetPassword");
        }

        // POST: Sms/UpdatePassword
        [HttpPost]
        public ActionResult UpdatePassword(string password)
        {
            var client = new SmsClient();
            var model = client.UpdatePassword(password);
            return View("Result", model);
        }
    }
}