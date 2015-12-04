using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;

using Zkly.BLL.Repositories;
using Zkly.BLL.ViewModels;
using Zkly.Common.Dictionary;
using Zkly.Common.Log;
using Zkly.DAL.Models;

namespace Zkly.Web.Controllers
{
    public class HomeController : BaseController
    {
        private InvestRepository _investRepository;

        public HomeController(InvestRepository investRepository)
        {
            _investRepository = investRepository;
        }

        public InvestRepository InvestRepository
        {
            get { return _investRepository; }
        }

        public ActionResult Index()
        {
            HomeIndexViewModel model = new HomeIndexViewModel();
            model.Invests = _investRepository.GetHomePageInvestTop4(EInvestAuditStage.Agreement);
            return View(model);
        }

        [Route("more-project")]
        public ActionResult MoreProject()
        {
            HomeIndexViewModel model = new HomeIndexViewModel();
            model.Invests = _investRepository.GetHomePageInvests(EInvestAuditStage.Agreement);
            return View(model);
        }

        [Route("seemore")]
        public ActionResult SeeMore() 
        {
            return View();
        }

        [Route("home-invest/{id}")]
        public ActionResult Invest(long? id)
        {
            var invest = InvestRepository.GetInvestById(id);
            if (invest != null)
            {
                // 阶段选择
                var stage = invest.IsFinished() ? EInvestAuditStage.First : invest.GetUserStage();
                switch (stage)
                {
                    case EInvestAuditStage.First:
                        return RedirectToAction("FirstAuditInfo", new { id = id });
                    case EInvestAuditStage.Second:
                        return RedirectToAction("SecondAuditInfo");
                }
            }

            return RedirectToAction("FirstAuditInfo");
        }

        [Route("home-invest-frist/{id?}")]
        public ActionResult FirstAuditInfo(long? id)
        {
                var invest = InvestRepository.GetInvestById(id);
                if (invest == null)
                {
                    return HttpNotFound();
                }

                this.ViewData["indus"] = new IndustryDropdown(invest.FirstAuditInfo.Industry).Items;
                ViewBag.IsEditable = invest.IsUserEditable(EInvestAuditStage.First);
                ViewBag.MaxStage = invest.GetUserStage();

                return View(invest.FirstAuditInfo);
        }

        [Route("home-invest-second/{id}")]
        public ActionResult SecondAuditInfo(long? id)
        {
            var invest = InvestRepository.GetInvestById(id);
            ViewBag.IsEditable = invest.IsUserEditable(EInvestAuditStage.Second);
            ViewBag.MaxStage = invest.GetUserStage();
            var secondAuditInfo = invest.SecondAuditInfo;
            var model = new EnterpriseSecondAudit
            {
                InvestId = invest.Id,
                IsHasIPR = true,
                InvestSecondAuditFilesUpload = EnumService.GetInvestSecondAuditFilesUpload()
            };
            if (secondAuditInfo == null)
            {
                return View("SecondAuditInfo", model);
            }

            model.SecondAuditInfoId = secondAuditInfo.Id;
            model.Address = secondAuditInfo.Address;
            model.RegisteredCapital = secondAuditInfo.RegisteredCapital;
            model.CompanyStage = secondAuditInfo.CompanyStage;
            model.Introduction = secondAuditInfo.Introduction;
            model.ProjectStage = secondAuditInfo.ProjectStage;
            model.ProjectIntroduction = secondAuditInfo.ProjectIntroduction;
            model.Inferiority = secondAuditInfo.Inferiority;
            model.IsHasIPR = secondAuditInfo.IsHasIPR;
            model.PatentStatus = secondAuditInfo.PatentStatus;
            model.PatentNumber = secondAuditInfo.PatentNumber;
            model.PatentInventor = secondAuditInfo.PatentInventor;
            model.PatentOwner = secondAuditInfo.PatentOwner;
            model.Plan = secondAuditInfo.Plan;
            model.RiskPrevention = secondAuditInfo.RiskPrevention;
            model.Debt = secondAuditInfo.Debt;
            model.DebtAmount = secondAuditInfo.DebtAmount;

            //项目来源
            var source = secondAuditInfo.ProjectSource;
            if (source == "0" || source == "1" || source == "2" || source == "3" || source == "4" || source == "5")
            {
                model.ProjectSource = source;
                model.ProjectSourceOther = null;
            }
            else
            {
                model.ProjectSourceOther = source;
            }

            //知识产权形式
            var form = secondAuditInfo.IprForm;
            if (form == "0" || form == "1" || form == "2" || form == "3")
            {
                model.IPRform = secondAuditInfo.IprForm;
                model.IPRformOther = null;
            }
            else
            {
                model.IPRformOther = form;
            }

            return View("SecondAuditInfo", model);
        }

        [Authorize]
        [Route("about")]
        public ActionResult About()
        {
            return View();
        }

        [Route("contract")]
        public ActionResult Contact()
        {
            return View();
        }

        [Route("hotline")]
        public ActionResult HotLine()
        {
            return View("Contact");
        }

        //判断用户以机构账号登录且偏好未设置 点击立即加入 进入对应的页面
        public ActionResult Setup()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Investor"))
                {
                    //查询是否已经进行偏好设置
                    InvestRepository invest = new InvestRepository();
                    InvestorPreference i = invest.Preference(User.Identity.GetUserId());
                    if (i == null)
                    {
                        //未进行了偏好设计  提醒用户
                        return RedirectToAction("SetPreferencesErro", "Error");
                    }
                    else
                    {
                        return RedirectToAction("PromptErro", "Error");
                    }
                }

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Register", "Account");
        }

        [Route("mianze")]
        public ActionResult MianZe()
        {
            return View();
        }

        //建设中的页面
        [Route("build")]
        public ActionResult Build()
        {
            return View();
        }

        //新闻报道页面
        [Route("report")]
        public ActionResult Report()
        {
            return View();
        }

        //数据字典表用法
        public ActionResult DataTest()
        {
            ViewBag.DataDictionaries =
                new DataDictionaryReponsitory().GetDataDictionaryList(Enums.DataDictionaryType.Industries.ToString());
            return View();
        }

        public ActionResult FileTest()
        {
            ViewBag.InvestSecondAuditFilesUpload = EnumService.GetInvestSecondAuditFilesUpload();
            ViewBag.Path = (int)Enums.FolderPath.InvestSecondAuditFiles;
            return View();
        }

        [HttpPost]
        public ActionResult FileTest(FormCollection form)
        {
            UploadFileInfoRepository upload = new UploadFileInfoRepository();
            UploadFileInfo uploadFile = null;
            List<UploadFileInfo> list = new List<UploadFileInfo>();
            int count = Request.Files.Count;
            string[] strs = Request["fileType"].Split(',');
            for (int i = 0; i < count; i++)
            {
                if (Request.Files[i] != null)
                {
                    uploadFile = upload.GetFileInfo(Request.Files[i], (int)Zkly.Common.Dictionary.Enums.FolderPath.Business);
                    list.Add(uploadFile);
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult FileTest1(EnterpriseSecondAudit model)
        {
            string files = Request["files"];
            if (string.IsNullOrEmpty(files)==false)
            {
                UploadFileInfoRepository upload = new UploadFileInfoRepository();
                string[] strs = files.Trim(',').Split(',');
                foreach (var item in strs)
                {
                    upload.AddUploadFileInfo((int)Enums.FolderPath.InvestSecondAuditFiles, item);
                }
            }

            return RedirectToAction("FileTest");
        }
    }
}