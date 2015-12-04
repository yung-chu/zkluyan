using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zkly.DAL.Models
{
    public class InvestFirstAuditInfo
    {
        [Key, ForeignKey("Invest")]
        public long Id { get; set; }

        [Display(Name = "公司名称")]
        [MaxLength(100, ErrorMessage = "公司名称长度不能大于100！")]
        public string CompanyName { get; set; }

        [Display(Name = "公司简介")]
        [MaxLength(4000, ErrorMessage = "公司简介长度不能大于4000！")]
        public string CompanyDescription { get; set; }

        [Required(ErrorMessage="申请金额不能为空")]
        [RegularExpression(@"^(([1-9][0-9]*\.[0-9][0-9]*)|([0]\.[0-9][0-9]*)|([1-9][0-9]*)|([0]{1}))$", ErrorMessage = "申请金额必须为数字！")]
        public long ApplyQuota { get; set; }

        public long? CompanyAssessment { get; set; }

        public DateTime? FoundingDate { get; set; }

        //[Required(ErrorMessage = "所属行业不能为空")]
        [MaxLength(10, ErrorMessage = "所属行业长度不能大于10！")]
        public string Industry { get; set; }

        [MaxLength(200, ErrorMessage = "所在地区长度不能大于200！")]
        public string Area { get; set; }

        [Display(Name = "项目名称")]
        [Required(ErrorMessage = "项目名不能为空")]
        [MaxLength(100, ErrorMessage = "项目名称长度不能大于100！")]
        public string ProjectName { get; set; }

        [Display(Name = "电子邮箱")]
        [Required(ErrorMessage = "邮箱不能为空")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "请输入正确的电子邮箱地址！")]
        [MaxLength(50, ErrorMessage = "电子邮箱长度不能大于50！")]
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

        [MaxLength(500, ErrorMessage = "行业优势长度不能大于500！")]
        public string IndustryAdv { get; set; }

        public bool IsResourceAdv { get; set; }

        [MaxLength(500, ErrorMessage = "客户资源优势长度不能大于500！")]
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

        [MaxLength(500, ErrorMessage = "行业地位长度不能大于500！")]
        public string IndustryPosition { get; set; }

        [MaxLength(500, ErrorMessage = "行业竞争情况长度不能大于500！")]
        public string IndustryCompetition { get; set; }

        [MaxLength(500, ErrorMessage = "项目所获奖项长度不能大于500！")]
        public string ProjectAwards { get; set; }

        [MaxLength(10, ErrorMessage = "申请投资长度不能大于10！")]
        public string InvestmentInstitutions { get; set; }

        [MaxLength(50, ErrorMessage = "法定代表人长度不能大于50！")]
        public string LegalPerson { get; set; }

        [MaxLength(20, ErrorMessage = "法定代表人电话长度不能大于20！")]
        public string LegalPhone { get; set; }

        [MaxLength(20, ErrorMessage = "法定代表人手机长度不能大于20！")]
        public string LegalCellPhone { get; set; }

        [Display(Name = "联系人")]
        [Required(ErrorMessage = "联系人不能为空")]
        [MaxLength(50, ErrorMessage = "联系人长度不能大于50！")]
        public string Contact { get; set; }

        [Display(Name = "联系人电话")]
        [MaxLength(20, ErrorMessage = "联系人电话长度不能大于20！")]
        public string ContactPhone { get; set; }

        [Display(Name = "联系人手机")]
        [MaxLength(20, ErrorMessage = "联系人手机不能大于20！")]
        public string ContactCellPhone { get; set; }

        [MaxLength(500, ErrorMessage = "市场及营销长度不能大于500！")]
        public string MarketAndSales { get; set; }

        public virtual Invest Invest { get; set; }
    }
}
