using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using PagedList;

using Zkly.BLL.Repositories;
using Zkly.BLL.Services;
using Zkly.BLL.ViewModels;
using Zkly.Common;
using Zkly.Common.Mvc.Attribute;
using Zkly.Common.Utils;
using Zkly.DAL.Models;

namespace Zkly.Admin.Web.Controllers
{
    public class InvestController : BaseController
    {
        private int pageSize = 30;
        private InvestRepository investRepository = new InvestRepository();

        /// <summary>
        /// 审批列表
        /// </summary>
        [Route("invest-{stage=First}/{state?}")]
        public ActionResult AuditList(int? page, EInvestAuditStage stage, string state, int?grade=null)
        {
            ViewBag.Stage = stage;
            ViewBag.State = state;

            int pageNumber = page ?? 1;

            if (string.IsNullOrEmpty(state))
            {
                return View(investRepository.GetInvestsByStage(stage).ToPagedList(pageNumber, pageSize));
            }

            var invests = new List<Invest>();

            if (state == FilterType.Immediately)
            {
                invests = investRepository.GetImmediatelyInvests(stage);
            }
            else if (state == FilterType.Waiting)
            {
                invests = investRepository.GetWaitingInvest(stage);
            }
            else if (state == FilterType.Temporarily)
            {
                invests = investRepository.GetTemporaryInvests(stage);
            }
            else if (state == FilterType.UnSubmited)
            {
                var model = investRepository.GetInvests(stage, EInvestAuditStatus.NotStarted).ToPagedList(pageNumber, pageSize);
                return View(model);
            }

            //用来筛选一审企业资金级别
            if (stage == EInvestAuditStage.First)
            {
                invests = invests.FilterGrade(grade);
            }

            return View(invests.ToPagedList(pageNumber, pageSize));
        }

        [RestoreModelState]
        [Route("audit-detail-{stage}/{id}", Name = "audit-detail")]
        public async Task<ActionResult> AuditDetails(int id, EInvestAuditStage stage, bool isedit = false)
        {
            var invest = investRepository.GetInvestById(id);

            //如果是刚提交的申请，需要改变它的状态到：审批中
            if (stage < EInvestAuditStage.Roadshow && invest.Stage == stage && invest.Status == EInvestAuditStatus.Submited)
            {
                invest.AuditQuota = null;
                invest.Status = EInvestAuditStatus.Auditing;
                invest.Reason = "查阅审批";
                investRepository.UpdateInvest(invest, UserId);

                var prjName = invest.FirstAuditInfo.ProjectName;
                var stageDesc = invest.Stage.Description();
                var subject = string.Format("项目:{0} 已经进入{1}审批环节", prjName, stageDesc);
                var message = string.Format("尊敬的用户，您好！您的投资申请项目:{0} 已经进入{1}审批环节。请随时关注审批状态变化，谢谢！", prjName, stageDesc);
                await UserManager.SendAllMessagesAsync(invest.UserId, subject, message);
            }

            var model = new InvestAuditViewModel
            {
                Invest = investRepository.GetInvestById(id),
                Stage = stage
            };

            ViewData["isedit"] = isedit;

            return Request.IsAjaxRequest()
             ? View("AuditDetailTab", model)
             : View("AuditDetails", model);
        }

        [HttpPost]
        [SetModelState]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveAudit(AuditResult auditResult)
        {
            var invest = investRepository.GetInvestById(auditResult.InvestId);
            if (!invest.IsAuditable(auditResult.Stage))
            {
                var statusDesc = invest.GetStatusDescription();
                var stageDesc = auditResult.Stage.Description();
                ModelState.AddModelError("", string.Format("该申请当前处于{0}状态，不可做{1}审批。", statusDesc, stageDesc));
            }
            else if (ModelState.IsValid)
            {
                #region 变更状态
                if (auditResult.IsPass)
                {
                    if (auditResult.AuditQuota > 0)
                    {
                        invest.AuditQuota = auditResult.AuditQuota;
                    }

                    invest.Status = EInvestAuditStatus.Accepted;
                    invest.Reason = null;
                }
                else
                {
                    invest.AuditQuota = null;
                    invest.Status = EInvestAuditStatus.Rejected;
                    invest.Reason = auditResult.Reason;
                }
                #endregion

                if (investRepository.UpdateInvest(invest, UserId))
                {
                    #region 发送审批消息
                    var prjName = invest.FirstAuditInfo.ProjectName;
                    var stageDesc = invest.Stage.Description();
                    var statusDesc = invest.Status.Description();
                    var subject = string.Format("项目:{0}，{1}审批{2}", prjName, stageDesc, statusDesc);
                    statusDesc = auditResult.IsPass ? "已经通过了" : "被拒绝了，失败原因：" + auditResult.Reason;
                    var message = string.Format("尊敬的用户，您好！您的投资申请项目:{0}，在{1}审批环节{2}。请随时关注审批状态变化，谢谢！", prjName, stageDesc, statusDesc);
                    await UserManager.SendAllMessagesAsync(invest.UserId, subject, message);
                    #endregion

                    //#region 协议结束，自动进入路演

                    //if (invest.Stage == EInvestAuditStage.Agreement && invest.Status == EInvestAuditStatus.Accepted)
                    //{
                    //    invest.Stage = EInvestAuditStage.Roadshow;
                    //    invest.Status = EInvestAuditStatus.Submited;
                    //    invest.Reason = "自动进入路演阶段";
                    //    investRepository.UpdateInvest(invest, UserId);
                    //}
                    //#endregion
                }
                else
                {
                    ModelState.AddModelError("", "数据保存失败，请检查相关数据是否有效。");
                }
            }

            return ModelState.IsValid
                ? RedirectToAction("AuditList", new { stage = auditResult.Stage, state = auditResult.FilterType })
                : RedirectToAction("AuditDetails", new { invest.Id, auditResult.Stage });
        }

        [AjaxOnly]
        public ActionResult History(int id)
        {
            var model = investRepository.GetInvestHistoryById(id);
            return View("AuditHistoryTab", model);
        }

        [Route("invest-import")]
        public ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveImport(HttpPostedFileBase excelFile)
        {
            var model = new Dictionary<int, Invest>();
            if (ModelState.IsValid)
            {
                if (excelFile == null)
                {
                    ModelState.AddModelError("", "请选择上传的Excel文件！");
                    return View("Import", model);
                }

                //读取上传excel 的数据
                var table = ExcelUtil.ReadTable(excelFile.InputStream, excelFile.FileName.ToLower().EndsWith(".xlsx"));
                if (table == null || table.Rows.Count == 0)
                {
                    ModelState.AddModelError("", "没有相关数据，请上传一个有效的表格。");
                }
                else
                {
                    var users = table.ColumnToList<string>("用户名");
                    var investFirstAuditLists = table.CastToList<InvestFirstAuditInfo>();
                    var recordId = 0;
                    foreach (var userName in users)
                    {
                        var firstInfo = investFirstAuditLists[recordId++];
                        try
                        {
                            //var user = await UserManager.FindByNameAsync(userName); //会导致EF 多个dbcontext异常
                            var user = investRepository.FirstOrDefault<ApplicationUser>(u => u.UserName == userName);
                            if (user == null)
                            {
                                ModelState.AddModelError(string.Format("记录{0}异常：", recordId), string.Format("无法找到用户名为：{0}的相关记录。", userName));
                            }
                            else
                            {
                                var invest = new Invest
                                {
                                    FirstAuditInfo = firstInfo,
                                    UserId = user.Id,
                                    Stage = EInvestAuditStage.First,
                                    Status = EInvestAuditStatus.NotStarted,
                                    Reason = "批量导入",
                                    ApplyTime = DateTime.Now,
                                    UpdateTime = DateTime.Now
                                };

                                if (!investRepository.AddInvest(invest, UserId))
                                {
                                    ModelState.AddModelError(string.Format("记录{0}异常：", recordId), "数据库保存失败，请检查该记录的数据是否合法有效。");
                                }
                                else
                                {
                                    //invest.User = user;
                                    model.Add(recordId, invest);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError(string.Format("记录{0}异常：", recordId), ex.Message);
                        }
                    }

                    AddSuccessMessage("导入完成：", string.Format("已读取{0}条记录，成功导入{1}条一审材料。", table.Rows.Count, model.Count));
                }
            }

            return View("Import", model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (investRepository != null)
                {
                    investRepository.Dispose();
                    investRepository = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}
