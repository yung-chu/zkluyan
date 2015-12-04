using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Zkly.Web.Controllers
{
    public class FundsController : Controller
    {
        [Route("funds")]
        public ActionResult List()
        {
            return View();
        }
    }
}