using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

using PagedList;

using Zkly.BLL.Repositories;
using Zkly.BLL.Vhall;
using Zkly.BLL.ViewModels;
using Zkly.Common;
using Zkly.Common.Mvc.Attribute;
using Zkly.Common.Utils;
using Zkly.DAL.Models;

namespace Zkly.Admin.Web.Controllers
{
    [Authorize]

    public class CapitalRoadshowController : BaseController
    {
        private int pageSize = 30;
        private CapitalRoadshowRepository capitalRoadshowRepository = new CapitalRoadshowRepository();
        private IVhallApi vhallApi = new VhallApi();

        #region //管理资本路演

        [Route("capital")]
        public ActionResult Index(int? page)
        {
            var capitalRoadshows = capitalRoadshowRepository.GetAllCapitalRoadshows();
            var pagingData = capitalRoadshows.ToPagedList(page ?? 1, pageSize);

            foreach (var data in pagingData)
            {
                var status = vhallApi.GetActivityStatus(new ActivityStatus { WebinarId = data.WebinarId });
                data.Status = EnumUtil.Parse<EActivityStatus>(status.Data);
            }

            return View(pagingData);
        }

        [Route("capital-roadshow")]
        public ActionResult List(int? page)
        {
            int pageNumber = page ?? 1;
            InvestRepository investRepository = new InvestRepository();

            return View(investRepository.GetInvestsByStage(EInvestAuditStage.Roadshow).ToPagedList(pageNumber, pageSize));
        }

        [Route("capital-roadshow-create/{id?}")]
        public ActionResult Create(long? id)
        {
            ViewData["invests"] = new InvestDropdownModel().Items;
            return View("Edit", new CapitalRoadshow { Id = id ?? 0, StartDate = DateTime.Now.AddHours(-1), EndDate = DateTime.Now });
        }

        [Route("capital-roadshow-edit/{id?}")]
        public ActionResult Edit(long? id)
        {
            ViewData["invests"] = new InvestDropdownModel().Items;
            var capital = capitalRoadshowRepository.GetCapitalRoadshowById(id);
            return View("Edit", capital);
        }

        [Route("capital-roadshow-delete/{id?}")]
        public ActionResult DeleteConfirm(long? id)
        {
            var model = capitalRoadshowRepository.GetCapitalRoadshowById(id);
            return View("DeleteConfirm", model);
        }

        [HttpPost]
        [SetModelState]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(long? id)
        {
            var result = capitalRoadshowRepository.DeleteRoadshow(id);

            if (!result)
            {
                //return RedirectToAction("Index");
                ModelState.AddModelError("", "数据删除时发生错误！");
            }

            // var model = capitalRoadshowRepository.GetCapitalRoadshowById(id);
            // return View("DeleteConfirm", model);
            return RedirectToAction("AuditDetails", "Invest", new { id = id, stage = EInvestAuditStage.Roadshow });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(CapitalRoadshow roadshow)
        {
            //if (roadshow.Id == 0)
            //{
            //    ModelState.AddModelError("", "请选择一个相对应的投资");
            //}
            var temproadshow = new CapitalRoadshowRepository().GetCapitalRoadshowById(roadshow.Id);
            if (ModelState.IsValid)
            {
                var result = false;

                if ((temproadshow != null && !temproadshow.RecordState) || temproadshow == null)
                {
                    //代表插入操作
                    result = capitalRoadshowRepository.CreateRoadshow(roadshow, temproadshow == null);
                }
                else
                {
                    roadshow.WebinarId = temproadshow.WebinarId;
                    roadshow.FileId = temproadshow.FileId;
                    roadshow.CreateTime = temproadshow.CreateTime;
                    roadshow.RecordState = true;

                    //代表更新操作
                    result = capitalRoadshowRepository.UpdateRoadshow(roadshow);
                }

                if (!result)
                {
                    ModelState.AddModelError("", "数据保存失败，请重新输入！");
                }
                else
                {
                    return RedirectToAction("AuditDetails", "Invest", new { id = roadshow.Id, stage = EInvestAuditStage.Roadshow });
                }
            }

            ViewData["invests"] = new InvestDropdownModel(roadshow.Id.ToString()).Items;
            return View("Edit", roadshow);
        }

        public ActionResult StartActivity(long? id)
        {
            var roadshow = capitalRoadshowRepository.GetCapitalRoadshowById(id);

            var response = vhallApi.StartActivity(new StartActivity
                {
                    WebinarId = roadshow.WebinarId
                });

            var status = vhallApi.GetActivityStatus(new ActivityStatus { WebinarId = roadshow.WebinarId });

            if (response.Success)
            {
                return Redirect(response.Data);
            }

            return RedirectToAction("AuditDetails", "Invest", new { id = id, stage = EInvestAuditStage.Roadshow });
        }

        public ActionResult EndActivity(long? id)
        {
            var roadshow = capitalRoadshowRepository.GetCapitalRoadshowById(id);

            var response = vhallApi.EndActivity(new EndActivity
            {
                WebinarId = roadshow.WebinarId
            });

            return RedirectToAction("AuditDetails", "Invest", new { id = id, stage = EInvestAuditStage.Roadshow });
        }

        public ActionResult RecordParts(long? id)
        {
            string result;
            var parts = capitalRoadshowRepository.GetRecordParts(id, out result);
            var records = capitalRoadshowRepository.GetRecords(id);

            var model = new RecordViewModel
            {
                CapitalId = id,
                Records = records,
                RecordParts = parts,
                OriginData = result
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult GenerateRecord(long? id, MultiRecord record)
        {
            var result = capitalRoadshowRepository.GenerateRecord(id, record);

            return new JsonResult { Data = result };
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (capitalRoadshowRepository != null)
                {
                    capitalRoadshowRepository.Dispose();
                    capitalRoadshowRepository = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}
