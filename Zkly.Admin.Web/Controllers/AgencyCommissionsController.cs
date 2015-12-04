using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Zkly.DAL.Context;
using Zkly.DAL.Models;

namespace Zkly.Admin.Web.Controllers
{
    public class AgencyCommissionsController : BaseController
    {
        // GET: AgencyCommissions
        [Route("agency-commissions")]
        public ActionResult Index()
        {
            using (var db = new UserDbContext())
            {
                return View(db.AgencyCommissions.ToList());
            }
        }

        // GET: AgencyCommissions/Details/5
        [Route("agency-commission-detail/{id?}")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new UserDbContext())
            {
                AgencyCommission agencyCommission = db.AgencyCommissions.Find(id);
                if (agencyCommission == null)
                {
                    return HttpNotFound();
                }

                return View(agencyCommission);
            }
        }

        // GET: AgencyCommissions/Create
        [Route("agency-commission-create")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: AgencyCommissions/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCreate([Bind(Include = "Id,CashPercent,StockPercent,Description")] AgencyCommission agencyCommission)
        {
            if (ModelState.IsValid)
            {
                using (var db = new UserDbContext())
                {
                    db.AgencyCommissions.Add(agencyCommission);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return View(agencyCommission);
        }

        // GET: AgencyCommissions/Edit/5
        [Route("agency-commission-edit/{id?}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new UserDbContext())
            {
                AgencyCommission agencyCommission = db.AgencyCommissions.Find(id);
                if (agencyCommission == null)
                {
                    return HttpNotFound();
                }

                return View(agencyCommission);
            }
        }

        // POST: AgencyCommissions/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveEdit([Bind(Include = "Id,CashPercent,StockPercent,Description")] AgencyCommission agencyCommission)
        {
            if (ModelState.IsValid)
            {
                using (var db = new UserDbContext())
                {
                    db.Entry(agencyCommission).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return View(agencyCommission);
        }

        // GET: AgencyCommissions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {   
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new UserDbContext())
            {
                AgencyCommission agencyCommission = db.AgencyCommissions.Find(id);
                if (agencyCommission == null)
                {
                    return HttpNotFound();
                }

                return View(agencyCommission);
            }
        }

        // POST: AgencyCommissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var db = new UserDbContext())
            {
                AgencyCommission agencyCommission = db.AgencyCommissions.Find(id);
                AddSuccessMessage("成功：", string.Format("股权比例（{0}） 已成功删除。", agencyCommission.StockPercent));
                db.AgencyCommissions.Remove(agencyCommission);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
