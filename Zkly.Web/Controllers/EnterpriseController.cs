using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;

using PagedList;

using WebGrease.Css.Extensions;

using Zkly.BLL.Repositories;
using Zkly.BLL.ViewModels;

using Zkly.Common;
using Zkly.Common.Dictionary;
using Zkly.Common.Extension;
using Zkly.Common.Log;
using Zkly.Common.Mvc.Attribute;
using Zkly.DAL.Models;

namespace Zkly.Web.Controllers
{
    [Authorize]
    public class EnterpriseController : BaseController
    {
        #region lasy loading repository
        private readonly IInvestAgreementRepository _investAgreementRepository = new InvestAgreementRepository();
        private InvestRepository _investRepository;
        private LoanRepository _loanRepository;
        private MessageRepository _messageRepository;
        private UploadFileInfoRepository _uploadFileInfoRepository;
        private InvestSecondAuditFileReponsitory _investSecondAuditFileReponsitory;

        public InvestRepository InvestRepository
        {
            get { return _investRepository ?? (_investRepository = new InvestRepository()); }
        }

        public LoanRepository LoanRepository
        {
            get { return _loanRepository ?? (_loanRepository = new LoanRepository()); }
        }

        public MessageRepository MessageRepository
        {
            get { return _messageRepository ?? (_messageRepository = new MessageRepository()); }
        }

        public UploadFileInfoRepository UploadFileInfoRepository
        {
            get { return _uploadFileInfoRepository ?? (_uploadFileInfoRepository = new UploadFileInfoRepository()); }
        }

        public InvestSecondAuditFileReponsitory InvestSecondAuditFileReponsitory
        {
            get
            {
                return _investSecondAuditFileReponsitory
                       ?? (_investSecondAuditFileReponsitory = new InvestSecondAuditFileReponsitory());
            }
        }

        #endregion

        #region 个人主页

        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            var model = new EnterpriseViewModel
            {
                Loans = LoanRepository.GetLoansByUserId(userId),
                Invests = InvestRepository.GetInvestsByUserId(userId),
                Roadshows = InvestRepository.GetBusinessRoadshowByUserId(userId),
                CapitalRoadshows = InvestRepository.GetCapitalRoadshowByUserId(userId),
                Message = MessageRepository.GetLatestMessage(userId)
            };

            return View(model);
        }

        #endregion

        #region 申请贷款

        [Route("loan/{id?}")]
        public ActionResult ApplyLoan(long? id)
        {
            Loan model = null;

            if (id.HasValue)
            {
                model = LoanRepository.GetLoanById(id.Value);
                if (model != null)
                {
                this.ViewData["indus"] = new IndustryDropdown(model.Industry).Items;
            }
            else
            {
                model = new Loan { FoundingDate = DateTime.Now };
                this.ViewData["indus"] = new IndustryDropdown().Items;
            }
            }
            else
            {
                model = new Loan { FoundingDate = DateTime.Now };
                this.ViewData["indus"] = new IndustryDropdown().Items;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveLoan(Loan loan)
        {
            this.ViewData["indus"] = new IndustryDropdown(loan.Industry).Items;

            var checkResult = CheckData();
            if (checkResult.Count>0)
            {
                return new JsonResult { Data = new { status = false, message = checkResult.Join(",") } };
            }

            if (loan.TeamAdv == null && loan.ProdAdv == null && loan.TechAdv == null && loan.ScaleAdv == null && loan.SaleAdv == null && loan.IndustryAdv == null && loan.ResourceAdv == null && loan.OtherAdv == null)
            {
                return new JsonResult { Data = new { status = false, message = "公司核心优势至少选填一个" } };
            }

            loan.UserId = User.Identity.GetUserId();
            loan.Status = ELoanAuditStatus.Submited;
            loan.UpdateDate = DateTime.Now;
            if (loan.Id == 0)
            {
                loan.ApplyTime = DateTime.Now;
            }

            var result =new LoanRepository().CreateOrUpdateLoan(loan);

            if (!result)
            {
                return new JsonResult { Data = new { status = false, message = "数据保存失败！" } };
            }

            return new JsonResult { Data = new { status = true, message = "" } };
        }

        [ActionName("loan-success")]
        public ActionResult LoanSuccess()
        {
            ViewBag.Message = "完成，借贷已成功提交。";
            return View("Success_Info", new ResultViewModel { ActionName = "index", ControllerName = "enterprise" });
        }
        #endregion

        #region 申请投资

        [Route("invest/{id}")]
        public ActionResult Invest(long id)
        {
            var invest = InvestRepository.GetInvestById(id);
            if (invest != null && invest.UserId == UserId)
            {
                // 阶段选择
                var stage = invest.IsFinished() ? EInvestAuditStage.First : invest.GetUserStage();
                switch (stage)
                {
                    case EInvestAuditStage.First:
                        return RedirectToAction("FirstAuditInfo");
                    case EInvestAuditStage.Second:
                        return RedirectToAction("SecondAuditInfo");
                    case EInvestAuditStage.Agreement:
                        return RedirectToAction("InvestAgreement");
                    case EInvestAuditStage.Roadshow:
                        return RedirectToAction("RoadshowIndex", "Roadshow", new { id });
                }
            }

            return RedirectToAction("FirstAuditInfo");
        }

        #region 一审
        [Route("invest-frist/{id?}")]
        public ActionResult FirstAuditInfo(long? id)
        {
            if (id == null)
            {
                var data = new InvestFirstAuditInfo { FoundingDate = DateTime.Now };
                this.ViewData["indus"] = new IndustryDropdown(data.Industry).Items;
                ViewBag.IsEditable = true;
                ViewBag.MaxStage = EInvestAuditStage.First;
                return View(data);           
            }
            else
            {
                var invest = InvestRepository.GetInvestById(id);
                if (invest == null || invest.UserId != UserId)
                {
                    return HttpNotFound();
                }

                this.ViewData["indus"] = new IndustryDropdown(invest.FirstAuditInfo.Industry).Items;
                ViewBag.IsEditable = invest.IsUserEditable(EInvestAuditStage.First);
                ViewBag.MaxStage = invest.GetUserStage();

                return View(invest.FirstAuditInfo);
            }            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("invest/submit-first-audit")]
        public ActionResult SaveFirstAuditInfo(InvestFirstAuditInfo info)
        {
            if (info.LegalPhone == null && info.LegalCellPhone == null)
            {
                ModelState.AddModelError("LegalCellPhone", "电话和手机必填一个");
            }

            if (info.ContactPhone == null && info.ContactCellPhone == null)
            {
                ModelState.AddModelError("ContactCellPhone", "电话和手机必填一个");
            }

            if (info.TeamAdv == null && info.ProdAdv == null && info.TechAdv == null && info.ScaleAdv == null && info.SaleAdv == null && info.IndustryAdv == null && info.ResourceAdv == null && info.OtherAdv == null)
            {
                ModelState.AddModelError("ResourceAdv", "公司核心优势必须至少填写一个");
            }

            //save data
            if (ModelState.IsValid)
            {
                if (info.Id == 0)
                {
                    var invest = new Invest
                    {
                        FirstAuditInfo = info,
                        UserId = UserId,
                        Stage = EInvestAuditStage.First,
                        Status = EInvestAuditStatus.Submited,
                        Reason = "提交初审",
                        ApplyTime = DateTime.Now,
                        UpdateTime = DateTime.Now
                    };

                    if (!InvestRepository.AddInvest(invest))
                    {
                        ModelState.AddModelError(string.Empty, "无法保存该申请，请检查有关数据是否合法有效。");
                    }
                }
                else
                {
                    //状态为审核中的申请,不能修改
                    var invest = InvestRepository.GetInvestById(info.Id);
                    if (invest.IsUserEditable(EInvestAuditStage.First))
                    {
                        invest.Stage = EInvestAuditStage.First;
                        invest.Status = EInvestAuditStatus.Submited;
                        invest.Reason = null;
                        invest.FirstAuditInfo = info;

                        if (!InvestRepository.UpdateInvest(invest))
                        {
                            ModelState.AddModelError(string.Empty, "无法保存该申请，请检查有关数据是否合法有效。");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "状态为审核中或审核完成的申请，不能修改。");
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                this.ViewData["indus"] = new IndustryDropdown(info.Industry).Items;
                return View("FirstAuditInfo", info);
            }

            ViewBag.Message = "完成，初审已成功提交。";
            return View("Success_Info", new ResultViewModel { ActionName = "index", ControllerName = "enterprise" });
        }

        #endregion

        #region 二审
        [Route("invest-second/{id}")]
        public ActionResult SecondAuditInfo(long id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("invest/submit-second")]
        public JsonResult SaveSecondAuditInfo(EnterpriseSecondAudit model)
        {
            string errorMessage = null;
            var checkResult = CheckData();
            if (checkResult.Count == 0)
            {
                try
                {
                    var invest = new InvestRepository().GetInvestById(model.InvestId);
                    if (invest.SecondAuditInfo == null || invest.IsUserEditable(EInvestAuditStage.Second))
                    {
                        var secondAuditInfo = invest.SecondAuditInfo ?? new InvestSecondAuditInfo();

                        //复制二审信息，返回上传文件信息
                        List<InvestSecondAuditFile> listInvestSecondAuditFiles = CopySecondAuditInfo(model, secondAuditInfo);
                        invest.Stage = EInvestAuditStage.Second;
                        invest.Status = EInvestAuditStatus.Submited;
                        invest.Reason = "提交二审";
                        invest.SecondAuditInfo = secondAuditInfo;

                        //二审上传相关表的操作
                        string result = new InvestRepository().UpdateInvestInfoAndOperateInvestSecondAuditFile(invest, listInvestSecondAuditFiles);

                        if (string.IsNullOrEmpty(result))
                        {
                            return new JsonResult { Data = new { status = true, message = "数据保存成功！" } };
                        }
                        else
                        {
                            return new JsonResult { Data = new { status = false, message = result } };
                        }
                    }
                    else
                    {
                        //状态为审核中的申请,不能修改
                        errorMessage = "状态为审核中或审核完成的申请，不能修改。";
                    }
                }
                catch (Exception ex)
                {
                    errorMessage = "数据保存时发生错误！";
                    Logger.Error(ex.ToString());
                }
            }
            else
            {
                //检验不通过
                errorMessage = checkResult.Join(",");
            }

            return new JsonResult { Data = new { status = false, message = errorMessage } };
        }

        //数据验证
        public IList<string> CheckData()
        {
            List<string> listMessage = new List<string>();
            var showModelStateError = new List<ShowModelStateError>();

            //找到出错的字段以及出错信息
            var errorFieldsAndMsgs = ModelState.Where(m => m.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });

            foreach (var item in errorFieldsAndMsgs)
            {
                //获取键
                var fieldKey = item.Key;

                //获取键对应的错误信息
                var fieldErrors = item.Errors.Select(e => new ShowModelStateError(fieldKey, e.ErrorMessage));
                showModelStateError.AddRange(fieldErrors);
            }

            listMessage.AddRange(showModelStateError.Select(p => p.Message)); 
            return listMessage;
        }

        //复制二审信息,返回上传文件Id
        public List<InvestSecondAuditFile> CopySecondAuditInfo(EnterpriseSecondAudit model, InvestSecondAuditInfo secondAuditInfo)
        {
            List<InvestSecondAuditFile> investSecondAuditFileList = new List<InvestSecondAuditFile>();

            //初始化SecondAuditInfo
            string patentInventor = string.Empty;
            string patentOwner = string.Empty;

            if (!string.IsNullOrEmpty(model.PatentInventor))
            {
                var list = model.PatentInventor.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
                patentInventor = string.Join(",", list);
            }

            if (!string.IsNullOrEmpty(model.PatentOwner))
            {
                var list = model.PatentOwner.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
                patentOwner = string.Join(",", list);
            }

            //项目来源
            secondAuditInfo.ProjectSource = !string.IsNullOrEmpty(model.ProjectSourceOther) ? model.ProjectSourceOther : model.ProjectSource;
            secondAuditInfo.Address = model.Address;
            secondAuditInfo.RegisteredCapital = model.RegisteredCapital;
            secondAuditInfo.CompanyStage = model.CompanyStage;
            secondAuditInfo.Introduction = model.Introduction;
            secondAuditInfo.ProjectStage = model.ProjectStage;
            secondAuditInfo.ProjectIntroduction = model.ProjectIntroduction;
            secondAuditInfo.Inferiority = model.Inferiority;
            secondAuditInfo.IsHasIPR = model.IsHasIPR;
            secondAuditInfo.PatentStatus = model.PatentStatus;
            secondAuditInfo.PatentNumber = model.PatentNumber;

            //知识产权形式
            secondAuditInfo.IprForm = !string.IsNullOrEmpty(model.IPRformOther) ? model.IPRformOther : model.IPRform;
            secondAuditInfo.PatentInventor = patentInventor;
            secondAuditInfo.PatentOwner = patentOwner;
            secondAuditInfo.Plan = model.Plan;
            secondAuditInfo.RiskPrevention = model.RiskPrevention;
            secondAuditInfo.Debt = model.Debt;
            secondAuditInfo.DebtAmount = model.DebtAmount;

            //将临时文件剪切到正式文件夹中，并保存数据到 UploadFileInfoe
            List<string> listFileInfos = new List<string>();
            List<string> fileInfoId = new List<string>();
            UploadFileInfoRepository upload = new UploadFileInfoRepository();
            InvestSecondAuditFile investSecondAuditFile = null;
            UploadFileInfo uploadFileInfo = null;

            //去空
            model.FileInfos.ForEach(p =>
            {
                    if (!string.IsNullOrEmpty(p))
                    {
                        listFileInfos.Add(p);
                    }
             });

            model.FileInfoId.ForEach(p =>
            {
                   if (!string.IsNullOrEmpty(p))
                   {
                        fileInfoId.Add(p);
                   }
            });

            int index = 0;
            foreach (var item in listFileInfos)
            {
                investSecondAuditFile = new InvestSecondAuditFile();
                uploadFileInfo = upload.AddUploadFileInfo((int)Enums.FolderPath.InvestSecondAuditFiles, item);

                if (uploadFileInfo != null)
                {
                    investSecondAuditFile.UploadFileInfoesId = uploadFileInfo.Id;
                    investSecondAuditFile.FileTypeId = Convert.ToInt64(fileInfoId[index]);
                    investSecondAuditFile.CreateTime = DateTime.Now;
                    investSecondAuditFileList.Add(investSecondAuditFile);
                }

                index++;
            }

            return investSecondAuditFileList;
       }

        //获取文件名
        public string GetUploadFileInfo(long investSecondAuditId, long fileTypeId)
        {
            long? id = InvestSecondAuditFileReponsitory.GetInvestSecondAuditFile(investSecondAuditId, fileTypeId);

            if (id.HasValue)
            {
                var model = UploadFileInfoRepository.GetUploadFileInfoById(id.Value);

                if (model != null)
                {
                    return model.FileName;
                }
            }

            return string.Empty;
        }

        //显示图片,下载文件
        [AllowAnonymous]
        public ActionResult ShowUploadFileInfo(long investSecondAuditId, long fileTypeId)
        {
            long? id = InvestSecondAuditFileReponsitory.GetInvestSecondAuditFile(investSecondAuditId, fileTypeId);

            if (id.HasValue)
            {
                int businessPlan = (int)Enums.InvestSecondAuditFilesUpload.BusinessPlan;
                int manual = (int)Enums.InvestSecondAuditFilesUpload.Manual;
                int accessory1FileId = (int)Enums.InvestSecondAuditFilesUpload.Accessory1FileId;
                int accessory2FileId = (int)Enums.InvestSecondAuditFilesUpload.Accessory2FileId;
                int accessory3FileId = (int)Enums.InvestSecondAuditFilesUpload.Accessory3FileId;

                bool isImage = fileTypeId != businessPlan && fileTypeId != manual && fileTypeId != accessory1FileId
                              && fileTypeId != accessory2FileId && fileTypeId != accessory3FileId;

                if (isImage)
                {
                    return RedirectToAction("FileDisplayById", "File", new { id = id.Value });
                }
                else
                {
                    return RedirectToAction("FileDownLoad", "File", new { id = id.Value }); 
                }
            }
            else
            {
                return null;
            }
        }

        [ActionName("upload-success")]
        public ActionResult SecondAuditUploadSuccess()
        {
            ViewBag.Message = "完成，二审已成功提交。";
            return View("Success_Info", new ResultViewModel { ActionName = "index", ControllerName = "enterprise" });
        }

        #endregion

        #endregion

        #region 协议签订

        [Route("invest-sign-agreement/{id}")]
        [RestoreModelState]
        public ActionResult InvestAgreement(long id)
        {
            Invest invest = InvestRepository.GetInvestById(id);
            ViewBag.IsEditable = invest.IsUserEditable(EInvestAuditStage.Agreement);
            ViewBag.MaxStage = invest.GetUserStage();

            var model = new InvestAgreementViewModel
            {
                InvestId = id,
                CashOptions = _investAgreementRepository.GetCashOptions(),
                StockOptions = _investAgreementRepository.GetStockOptions(),
                CombineOptions = _investAgreementRepository.GetCombineOptions(),
                ErrorInfo = string.Empty
            };

            if (!ModelState.IsValid)
            {
                model.ErrorInfo = "错误";
                return View(model);
            }

            if (invest.UserId != User.Identity.GetUserId())
            {
                ModelState.AddModelError(string.Empty, "无权访问该投资申请记录。");
                model.ErrorInfo = "无权访问该投资申请记录";
                return View(model);
            }

            model.InvestAgreement = _investAgreementRepository.GetInvestAgreement(model.InvestId);
            return View(model);
        }

        [HttpPost]
        [Route("invest/sign-agreement")]
        [ValidateAntiForgeryToken]
        [SetModelState]
        public ActionResult SaveInvestAgreement(InvestAgreementPostModel model, HttpPostedFileBase investAgreement)
        {
            ViewBag.IsEditable = true;

            var viewModel = new InvestAgreementViewModel
            {
                InvestId = model.InvestId,
                CashOptions = _investAgreementRepository.GetCashOptions(),
                StockOptions = _investAgreementRepository.GetStockOptions(),
                CombineOptions = _investAgreementRepository.GetCombineOptions(),
                ErrorInfo = string.Empty
            };

            if (!ModelState.IsValid)
            {
                return this.View("InvestAgreement", viewModel);
            }

            var getResult = _investAgreementRepository.SaveAgreement(
                model.InvestId,
                model.LockMonth,
                model.AgencyCommissionId,
                investAgreement);

            if (!string.IsNullOrEmpty(getResult))
            {
                ModelState.AddModelError(string.Empty, getResult);
                viewModel.ErrorInfo = getResult;
                return this.View("InvestAgreement", viewModel);
            }

            ViewBag.Message = "完成，协议已成功提交。";
            return View("Success_Info", new ResultViewModel { ActionName = "index", ControllerName = "enterprise" });
        }

        [HttpPost]
        [Route("invest/upload-agreement")]
        [ValidateAntiForgeryToken]
        public ActionResult UploadInvestAgreement(InvestAgreementPostModel model, HttpPostedFileBase investAgreement)
        {
            string getInfo = _investAgreementRepository.UpdateAgreement(model.InvestId, investAgreement);

            if (!string.IsNullOrEmpty(getInfo))
            {
                ModelState.AddModelError(string.Empty, "协议保存失败。");
                return RedirectToAction("InvestAgreement", new { id = model.InvestId });
            }

            return RedirectToAction("Index");
        }

        #endregion

        #region 企业消息
        [Authorize]
        [Route("message-center")]
        public ActionResult MessageCenter(bool? state, int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;

            var model = new EnterpriseViewModel();
            model.MessageState = state;

            List<Message> listMessage = MessageRepository.GetMessages().OrderByDescending(p => p.UpdateTime).Where(p => p.UserId == User.Identity.GetUserId()).ToList();

                if (state != null)
                {
                    model.ListMessage = listMessage.Where(p => p.State == state).ToPagedList(pageNumber, pageSize);
                }
                else
                {
                    model.ListMessage = listMessage.ToPagedList(pageNumber, pageSize);
                }

            return View(model);
        }

        public JsonResult UpdateMessageState(int id, string state)
        {
            bool getState = state == "False" ? true : false;

            Message message = MessageRepository.GetMessages(id);
            message.State = getState;
            message.UpdateTime = DateTime.Now;

            bool result = MessageRepository.UpdateMessage(message);

            return new JsonResult { Data = new { status = result, message = result ? "数据保存成功！" : "数据保存时发生错误！" } };
        }

        public JsonResult DeleteMessage(string id)
        {
            if (id == string.Empty)
            {
                return new JsonResult { Data = new { status = false, message = "请选择删除的信息!" } };
            }

            var list = id.Split(',').Select(a => Convert.ToInt32(a)).ToList();
            bool result = MessageRepository.DeleteMessage(list);
            return new JsonResult { Data = new { status = result, message = result ? "数据删除成功！" : "数据删除时发生错误！" } };
        }

        #endregion
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_investRepository != null)
                {
                    _investRepository.Dispose();
                    _investRepository = null;
                }

                if (_loanRepository != null)
                {
                    _loanRepository.Dispose();
                    _loanRepository = null;
                }

                if (_uploadFileInfoRepository != null)
                {
                    _uploadFileInfoRepository.Dispose();
                    _uploadFileInfoRepository = null;
                }
            }

            base.Dispose(disposing);
        }
    }

    /*显示模型验证错误信息*/
    public class ShowModelStateError
    {
        public ShowModelStateError(string key, string message)
        {
            Key = key;
            Message = message;
        }

        public string Key { get; set; }

        public string Message { get; set; }
    }
}
