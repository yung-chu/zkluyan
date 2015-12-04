using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;

using PagedList;

using Zkly.BLL.Repositories;
using Zkly.BLL.ViewModels;
using Zkly.Common.Utils;
using Zkly.DAL.Context;
using Zkly.DAL.Models;

namespace Zkly.Admin.Web.Controllers
{
    [RoutePrefix("tempFolder")]
    public class TempFolderController : Controller
    {
        private InvestTempFolderRepository investTempFolderRepository = new InvestTempFolderRepository();
        private int pageSize = 10;

        [Route("first-audits/{page?}")]
        public ActionResult FirstAudit(int? page)
        {
            int pageNumber = page ?? 1;
            var model = new TempFolderModel
            {
                ListTempFolder = investTempFolderRepository.ListInvestTempFolder(EInvestAuditStage.First).ToPagedList(pageNumber, pageSize)
            };

            return View(model);
        }

        [Route("second-audits/{page?}")]
        public ActionResult SecondAudit(int? page)
        {
            int pageNumber = page ?? 1;
            var model = new TempFolderModel
            {
                ListTempFolder = investTempFolderRepository.ListInvestTempFolder(EInvestAuditStage.Second).ToPagedList(pageNumber, pageSize)
            };
            return View(model);
        }

        //放置暂存文件夹
        public JsonResult PlaceTempFolder(string investId)
        {
            if (string.IsNullOrEmpty(investId))
            {
                return new JsonResult { Data = new { status = true, message = "没有任何选项！" } };
            }

            var listInvestTempFolder = new List<InvestTempFolder>();
            var listIntInvestId = investId.Split(',').Select(a => Convert.ToInt32(a)).ToList();

            for (int i = 0; i < listIntInvestId.Count; i++)
            {
                listInvestTempFolder.Add(new InvestTempFolder
                {
                    InvestId = listIntInvestId[i],
                    UserId = User.Identity.GetUserId(),
                    CreateTime = DateTime.Now
                });
            }

            bool result = investTempFolderRepository.AddOrUpdateMultiple(listInvestTempFolder);

            return new JsonResult { Data = new { status = result, message = result ? "提交成功！" : "提交时发生错误！" } };
        }

        //删除暂存文件夹
        public JsonResult DeleteTempFolder(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new JsonResult { Data = new { status = true, message = "没有任何选项！" } };
            }

            var listIntInvestId = id.Split(',').ToList();
            bool result = investTempFolderRepository.DeleteInvestTempFolder(listIntInvestId);
            return new JsonResult { Data = new { status = result, message = result ? "提交成功！" : "数据提交时发生错误！" } };
        }
    }
}