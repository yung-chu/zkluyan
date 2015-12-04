using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Zkly.Web.Controllers
{
    public class HelpBaseController : Controller
    {
        public string IndexName 
        { 
            set { ViewBag.IndexName = value; }
        }

        public string PageName
        {
            set { ViewBag.PageName = value; }
        }
    }
}