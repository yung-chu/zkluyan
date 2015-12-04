using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zkly.Common;

namespace Zkly.DAL.Models
{
    public enum ELoanAuditStatus
    {
        [Display(Name = "未开始")]
        NotStarted = -1,
        [Display(Name = "已提交")]
        Submited = 0,
        [Display(Name = "审批中")]
        Auditing = 1,
        [Display(Name = "已拒绝")]
        Rejected = 2,
        [Display(Name = "已通过")]
        Accepted = 3
    }

    /// <summary>
    /// Loan为临时替代品，后面申请贷款页面会有大变动，所以只是Invest一个copy
    /// </summary>
    public class Loan
    {
        public long Id { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }

        [Required(ErrorMessage = "公司名不能为空")]
        [MaxLength(100, ErrorMessage = "公司名长度不能大于100！")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "公司简介不能为空")]
        [MaxLength(4000, ErrorMessage = "公司简介长度不能大于4000！")]
        public string CompanyDescription { get; set; }

        public DateTime ApplyTime { get; set; }

        [Required(ErrorMessage = "申请金额不能为空")]
        [RegularExpression(@"^(([1-9][0-9]*\.[0-9][0-9]*)|([0]\.[0-9][0-9]*)|([1-9][0-9]*)|([0]{1}))$", ErrorMessage = "申请金额必须为数字！")]
        public long? ApplyQuota { get; set; }

        [Required(ErrorMessage = "抵押物估值不能为空")]
        [RegularExpression(@"^(([1-9][0-9]*\.[0-9][0-9]*)|([0]\.[0-9][0-9]*)|([1-9][0-9]*)|([0]{1}))$", ErrorMessage = "抵押物估值必须为数字！")]
        public long? GuaranteeAssessment { get; set; }

        [Required(ErrorMessage = "公司成立日期不能为空")]
        public DateTime FoundingDate { get; set; }

        [Required(ErrorMessage = "所属行业不能为空")]
        [MaxLength(10, ErrorMessage = "所属行业长度不能大于10！")]
        public string Industry { get; set; }

        [Required(ErrorMessage = "所在区域不能为空")]
        [MaxLength(200, ErrorMessage = "所在区域长度不能大于200！")]
        public string Area { get; set; }

        [Required(ErrorMessage = "项目名不能为空")]
        [MaxLength(100, ErrorMessage = "项目名长度不能大于100！")]
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "联系人不能为空")]
        [MaxLength(50, ErrorMessage = "联系人长度不能大于50！")]
        public string Contract { get; set; }

        [Required(ErrorMessage = "电话不能为空")]
        [MaxLength(20, ErrorMessage = "电话长度不能大于20！")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "邮箱不能为空")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "请输入正确的电子邮箱地址！")]
        [MaxLength(50, ErrorMessage = "邮箱长度不能大于50！")]
        public string Email { get; set; }

        public bool IsTeamAdv { get; set; }

        [MaxLength(500, ErrorMessage = "团队优势长度不能大于500！")]
        public string TeamAdv { get; set; }

        public bool IsProdAdv { get; set; }

        [MaxLength(500, ErrorMessage = "产品优势长度不能大于500！")]
        public string ProdAdv { get; set; }

        public bool IsTechAdv { get; set; }

        [MaxLength(500, ErrorMessage = "技术优势长度不能大于500！")]
        public string TechAdv { get; set; }

        public bool IsScaleAdv { get; set; }

        [MaxLength(500, ErrorMessage = "规模优势长度不能大于500！")]
        public string ScaleAdv { get; set; }

        public bool IsSaleAdv { get; set; }

        [MaxLength(500, ErrorMessage = "销售优势长度不能大于500！")]
        public string SaleAdv { get; set; }

        public bool IsIndustryAdv { get; set; }

        [MaxLength(500, ErrorMessage = "产业优势长度不能大于500！")]
        public string IndustryAdv { get; set; }

        public bool IsResourceAdv { get; set; }

        [MaxLength(500, ErrorMessage = "资源优势长度不能大于500！")]
        public string ResourceAdv { get; set; }

        public bool IsOtherAdv { get; set; }

        [MaxLength(500, ErrorMessage = "其他优势长度不能大于500！")]
        public string OtherAdv { get; set; }

        [RegularExpression(@"^(([1-9][0-9]*\.[0-9][0-9]*)|([0]\.[0-9][0-9]*)|([1-9][0-9]*)|([0]{1}))$", ErrorMessage = "财务数据必须为数字！")]
        public long? Data1 { get; set; }

        [RegularExpression(@"^(([1-9][0-9]*\.[0-9][0-9]*)|([0]\.[0-9][0-9]*)|([1-9][0-9]*)|([0]{1}))$", ErrorMessage = "财务数据必须为数字！")]
        public long? Data2 { get; set; }

        [RegularExpression(@"^(([1-9][0-9]*\.[0-9][0-9]*)|([0]\.[0-9][0-9]*)|([1-9][0-9]*)|([0]{1}))$", ErrorMessage = "财务数据必须为数字！")]
        public long? Data3 { get; set; }

        [RegularExpression(@"^(([1-9][0-9]*\.[0-9][0-9]*)|([0]\.[0-9][0-9]*)|([1-9][0-9]*)|([0]{1}))$", ErrorMessage = "财务数据必须为数字！")]
        public long? Data4 { get; set; }

        [RegularExpression(@"^(([1-9][0-9]*\.[0-9][0-9]*)|([0]\.[0-9][0-9]*)|([1-9][0-9]*)|([0]{1}))$", ErrorMessage = "财务数据必须为数字！")]
        public long? Data5 { get; set; }

        public DateTime UpdateDate { get; set; }

        public virtual ApplicationUser User { get; set; }

        public ELoanAuditStatus Status { get; set; }

        [MaxLength(500, ErrorMessage = "原因长度不能大于500！")]
        public string FailReason { get; set; }
    }

    public static class LoanExtention
    {
        public static bool EnableEdit(this Loan loan)
        {
            return loan.Status == ELoanAuditStatus.Submited || loan.Status == ELoanAuditStatus.Rejected;
        }

        public static bool IsAuditable(this Loan loan)
        {
            return loan.Status != ELoanAuditStatus.Accepted && loan.Status != ELoanAuditStatus.Rejected;
        }

        public static string Description(this ELoanAuditStatus value)
        {
            switch (value)
            {
                case ELoanAuditStatus.NotStarted:
                    return "未提交";
                case ELoanAuditStatus.Submited:
                    return "已提交";
                case ELoanAuditStatus.Auditing:
                    return "审批中";
                case ELoanAuditStatus.Rejected:
                    return "已拒绝";
                case ELoanAuditStatus.Accepted:
                    return "已通过";
                default:
                    return "未知";
            }
        }
    }
}
