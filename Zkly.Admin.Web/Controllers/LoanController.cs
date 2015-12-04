using System;
using System.Threading.Tasks;
using System.Web.Mvc;

using PagedList;

using Zkly.BLL.Repositories;
using Zkly.Common.Mvc.Attribute;
using Zkly.DAL.Models;

namespace Zkly.Admin.Web.Controllers
{
    public class LoanController : BaseController
    {
        private LoanRepository loanRepository = new LoanRepository();
        private int pageSize = 30;

        [Route("loans")]
        public ActionResult LoanList(int? page)
        {
            var loans = loanRepository.GetLoans();

            return View(loans.ToPagedList(page ?? 1, pageSize));
        }

        [RestoreModelState]
        [Route("loan-detail/{id}")]
        public async Task<ActionResult> AuditDetails(long id)
        {
            var loan = loanRepository.GetLoanById(id);

            if (loan.Status == ELoanAuditStatus.Submited)
            {
                loan.Status = ELoanAuditStatus.Auditing;
                var result = loanRepository.CreateOrUpdateLoan(loan);

                var prjName = loan.ProjectName;
                var subject = string.Format("贷款: {0} 已经进入审批环节", prjName);
                var message = string.Format("尊敬的用户，您好！您的投资申请项目:{0} 已经进入审批环节。请随时关注审批状态变化，谢谢！", prjName);
                await UserManager.SendAllMessagesAsync(loan.UserId, subject, message);
            }

            ViewData["user"] = loan.User;

            return View(loan);
        }

        [HttpPost]
        public async Task<ActionResult> SaveAudit(FormCollection form)
        {
            var id = form.GetValue("Id").ConvertTo(typeof(long));
            var isPassObj = form.GetValue("IsPass").ConvertTo(typeof(bool));
            var remark = form.GetValue("Remark").ConvertTo(typeof(string));

            var isPass = bool.Parse(isPassObj.ToString());

            var loan = loanRepository.GetLoanById(long.Parse(id.ToString()));

            loan.Status = isPass ? ELoanAuditStatus.Accepted : ELoanAuditStatus.Rejected;
            loan.UpdateDate = DateTime.Now;
            loan.FailReason = isPass ? string.Empty : remark.ToString();

            var result = loanRepository.CreateOrUpdateLoan(loan);

            if (!result)
            {
                ModelState.AddModelError(string.Empty, "审批数据保存失败");
            }

            if (!ModelState.IsValid)
            {
                return View("AuditDetails", loan);
            }

            var prjName = loan.ProjectName;
            var statusDesc = loan.Status.Description();
            var subject = string.Format("贷款: {0}，审批{1}", prjName, statusDesc);
            statusDesc = isPass ? "已经通过了" : "被拒绝了，失败原因：" + remark.ToString();
            var message = string.Format("尊敬的用户，您好！您的贷款申请项目:{0}，审批{1}。请随时关注审批状态变化，谢谢！", prjName, statusDesc);
            await UserManager.SendAllMessagesAsync(loan.UserId, subject, message);

            return RedirectToAction("LoanList");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (loanRepository != null)
                {
                    loanRepository.Dispose();
                    loanRepository = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}