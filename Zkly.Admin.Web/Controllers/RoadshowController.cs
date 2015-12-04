using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PagedList;

using Zkly.BLL.Repositories;

namespace Zkly.Admin.Web.Controllers
{
    public class RoadshowController : Controller
    {
        private RoadshowRepository roadshowRepository = new RoadshowRepository();

        [Route("business-roadshow/{page?}")]
        public ActionResult Business(int? page)
        {
            var model = roadshowRepository.GetAllRoadshows();

            return View("BusinessRoadshow", model.ToPagedList(page ?? 1, 30));
        }

        [HttpPost]
        public ActionResult SavePriority(int id, int priority)
        {
            var result = roadshowRepository.SaveRoadshowPriority(id, priority);

            return new JsonResult { Data = result };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (roadshowRepository != null)
                {
                    roadshowRepository.Dispose();
                    roadshowRepository = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}
