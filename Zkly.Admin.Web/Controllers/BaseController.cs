using System.Web.Mvc;
using Zkly.Common.Mvc.Controllers;

namespace Zkly.Admin.Web.Controllers
{
    [Authorize]
    public class BaseController : UserController
    {
        protected void AddSuccessMessage(string title, string message)
        {
            TempData["_SuccessTitle"] = title;
            TempData["_SuccessMessage"] = message;
        }

        protected void AddSuccessMessage(string message)
        {
            TempData["_SuccessMessage"] = message;
        }
    }
}