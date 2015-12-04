using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zkly.DAL.Models
{
    public class InvestSecondAuditInfo
    {
        [Key, ForeignKey("Invest")]
        public long Id { get; set; }

        [Display(Name = "地址")]
        [MaxLength(500, ErrorMessage = "地址长度不能大于500！")]
        public string Address { get; set; }

        [Display(Name = "注册资本")]
        public long? RegisteredCapital { get; set; }

        [Display(Name = "所处阶段")]
        public int CompanyStage { get; set; }  // 0创业期 1发展期 2成熟期

        [Display(Name = "股权结构及主要管理者简介")]
        [MaxLength(500, ErrorMessage = "股权结构及主要管理者简介长度不能大于500！")]
        public string Introduction { get; set; }

        [Display(Name = "项目来源")]
        [MaxLength(500, ErrorMessage = "项目来源长度不能大于500！")]
        public string ProjectSource { get; set; } // 0 --63计划  1 --科技支撑计划  2-- 国家部委计划  3--地方政府计划  4--国际合作 5--企业自主研发  6--其他

        [Display(Name = "项目阶段")]
        [MaxLength(500, ErrorMessage = "项目阶段长度不能大于500！")]
        public string ProjectStage { get; set; } //0 实验室研究成果   1中试成果  2小批量生产成果 3已投入市场

        [Display(Name = "项目介绍")]
        [MaxLength(500, ErrorMessage = "项目介绍长度不能大于500！")]
        public string ProjectIntroduction { get; set; }

        [Display(Name = "公司劣势")]
        [MaxLength(500, ErrorMessage = "公司劣势长度不能大于500！")]
        public string Inferiority { get; set; }

        [Display(Name = "是否有知识产权 ")]
        public bool IsHasIPR { get; set; }

        [Display(Name = "专利状况 ")]
        public int PatentStatus { get; set; } //0未申请专利 1已受理专利 2已授权专利

        [Display(Name = "专利号")]
        [MaxLength(100, ErrorMessage = "专利号长度不能大于100！")]
        public string PatentNumber { get; set; }

        [Display(Name = "知识产权形式")]
        [MaxLength(50, ErrorMessage = "知识产权形式长度不能大于50！")]
        public string IprForm { get; set; } // 0发明专利 1实用新型专利 2外观设计专利 3软件著作权 4其他

        [Display(Name = "专利发明人")]
        [MaxLength(50, ErrorMessage = "专利发明人长度不能大于50！")]
        public string PatentInventor { get; set; }

        [Display(Name = "专利所有人权")]
        [MaxLength(50, ErrorMessage = "专利所有人权长度不能大于50！")]
        public string PatentOwner { get; set; }

        [Display(Name = "战略规划")]
        [MaxLength(500, ErrorMessage = "战略规划长度不能大于500！")]
        public string Plan { get; set; }

        [Display(Name = "目前存在的问题风险及对策")]
        [MaxLength(500, ErrorMessage = "目前存在的问题风险及对策长度不能大于500！")]
        public string RiskPrevention { get; set; }

        [Display(Name = "公司债务")]
        public bool Debt { get; set; }

        public long DebtAmount { get; set; }

        public virtual Invest Invest { get; set; }

        public virtual ICollection<InvestSecondAuditFile> InvestSecondAuditFiles { get; set; }
    }
}
