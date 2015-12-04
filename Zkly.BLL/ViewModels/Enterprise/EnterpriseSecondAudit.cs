using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Zkly.Common.Dictionary;

namespace Zkly.BLL.ViewModels
{
    public class EnterpriseSecondAudit
    {
        public long InvestId { get; set; }

        public long SecondAuditInfoId { get; set; }

        [Required(ErrorMessage = "公司地址不能为空")]
        [StringLength(100, ErrorMessage = "公司地址字数应该在100以内")]
        public string Address { get; set; }

        [Required(ErrorMessage = "公司注册资本不能为空")]
        [Range(0, 10000000000, ErrorMessage = "不能超过1000000万")]
        [RegularExpression(@"(([1-9][0-9]*\.[0-9][0-9]*)|([0]\.[0-9][0-9]*)|([1-9][0-9]*)|([0]{1}))", ErrorMessage = "公司注册资本格式错误")]
        public long? RegisteredCapital { get; set; }

        [Required(ErrorMessage = "公司所处阶段不能为空")]
        public int CompanyStage { get; set; }

        [Required(ErrorMessage = "股权结构及主要管理者简介不能为空")]
        [StringLength(4000, ErrorMessage = "股权结构及主要管理者简介字数应该在4000以内")]
        public string Introduction { get; set; }

        [Required(ErrorMessage = "项目来源不能为空")]
        public string ProjectSource { get; set; }

        public string ProjectSourceOther { get; set; }

        [Required(ErrorMessage = "项目阶段不能为空")]
        public string ProjectStage { get; set; }

        [Required(ErrorMessage = "项目介绍不能为空")]
        [StringLength(4000, ErrorMessage = "项目介绍字数应该在4000以内")]
        public string ProjectIntroduction { get; set; }

        [Required(ErrorMessage = "公司劣势不能为空")]
        [StringLength(4000, ErrorMessage = "公司劣势字数应该在4000以内")]
        public string Inferiority { get; set; }

        public bool IsHasIPR { get; set; }

        public int PatentStatus { get; set; }

        public string PatentNumber { get; set; }

        public string IPRform { get; set; }

        public string IPRformOther { get; set; }

        public string PatentInventor { get; set; }

        public string PatentOwner { get; set; }

        [Required(ErrorMessage = "融资后的战略规划不能为空")]
        [StringLength(4000, ErrorMessage = "融资后的战略规划字数应该在4000以内")]
        public string Plan { get; set; }

        [Required(ErrorMessage = "目前存在的问题风险及对策不能为空")]
        [StringLength(4000, ErrorMessage = "目前存在的问题风险及对策字数应该在4000以内")]
        public string RiskPrevention { get; set; }

        public bool Debt { get; set; }

        public long DebtAmount { get; set; }

        public Dictionary<int, ZkjlKeyValuePair> InvestSecondAuditFilesUpload { get; set; }

        public List<string> FileInfos { get; set; }

        public List<string> FileInfoId { get; set; }

        public bool IsShowImg { get; set; }
    }
}
