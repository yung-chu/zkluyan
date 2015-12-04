using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using Zkly.Common.Extension;

namespace Zkly.DAL.Models
{
    public class Invest
    {
        public Invest()
        {
            this.Histories = new List<InvestHistory>();
        }

        public long Id { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }

        public EInvestAuditStage Stage { get; set; }

        public EInvestAuditStatus Status { get; set; }

        public int? AuditQuota { get; set; }

        [MaxLength(500, ErrorMessage = "原因长度不能大于500！")]
        public string Reason { get; set; }

        public DateTime ApplyTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public virtual InvestFirstAuditInfo FirstAuditInfo { get; set; }

        public virtual InvestSecondAuditInfo SecondAuditInfo { get; set; }

        public virtual InvestAgreement Agreement { get; set; }

        public virtual ICollection<InvestHistory> Histories { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Roadshow Roadshow { get; set; }

        public virtual CapitalRoadshow CapitalRoadshow { get; set; }
    }

    public enum EInvestAuditStage
    {
        [Display(Name = "一审")]
        First = 1,
        [Display(Name = "二审")]
        Second = 2,
        [Display(Name = "协议签订")]
        Agreement = 3,
        [Display(Name = "路演")]
        Roadshow = 4
    }

    public enum EInvestAuditStatus
    {
        //草稿/未开始/未提交等
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

    public static class InvestExtension
    {
        public static bool IsUserEditable(this Invest invest, EInvestAuditStage stage)
        {
            if (stage == invest.Stage)
            {
                return invest.Status.EqualsAny(EInvestAuditStatus.NotStarted, EInvestAuditStatus.Submited, EInvestAuditStatus.Rejected);
            }

            return invest.Status == EInvestAuditStatus.Accepted && stage == (invest.Stage + 1);
        }

        public static EInvestAuditStage GetUserStage(this Invest invest)
        {
            if (invest.Stage != EInvestAuditStage.Roadshow && invest.Status == EInvestAuditStatus.Accepted)
            {
                return invest.Stage + 1;
            }

            return invest.Stage;
        }

        public static bool IsFinished(this Invest invest)
        {
            return invest.Stage == EInvestAuditStage.Roadshow && invest.Status == EInvestAuditStatus.Accepted;
        }

        public static EInvestAuditStatus GetStageStatus(this Invest invest, EInvestAuditStage stage)
        {
            if (stage == invest.Stage)
            {
                return invest.Status;
            }

            return stage < invest.Stage ? EInvestAuditStatus.Accepted : EInvestAuditStatus.NotStarted;
        }

        public static bool IsAuditable(this Invest invest)
        {
            return invest.Status == EInvestAuditStatus.Submited || invest.Status == EInvestAuditStatus.Auditing;
        }

        public static bool IsAuditable(this Invest invest, EInvestAuditStage stage)
        {
            return stage == invest.Stage && invest.IsAuditable();
        }

        //去年的财政收入为准
        //百万级企业:(去年收入>=100万<=1000万)
        //千万级企业:(去年收入>=1000万)
        public static List<Invest> FilterGrade(this List<Invest> listInvest, int? grade = null)
        {
            var newListInvest = new List<Invest>();

            if (grade.HasValue)
            {
                newListInvest.AddRange(listInvest.Where(item => item.FirstAuditInfo.Data2.HasValue));

                if (grade == (int)EInvestGrade.Million)
                {
                    return newListInvest.Where(a => a.FirstAuditInfo.Data2 >= 100 && a.FirstAuditInfo.Data2 <= 1000).ToList();
                }
                else
                {
                    return newListInvest.Where(a => a.FirstAuditInfo.Data2 >= 1000).ToList();
                }
            }

            return listInvest;
        }

        #region Description
        public static string GetStatusDescription(this Invest invest)
        {
            return invest.Stage.Description(invest.Status);
        }

        public static string Description(this EInvestAuditStatus value)
        {
            switch (value)
            {
                case EInvestAuditStatus.NotStarted:
                    return "未提交";
                case EInvestAuditStatus.Submited:
                    return "已提交";
                case EInvestAuditStatus.Auditing:
                    return "审批中";
                case EInvestAuditStatus.Rejected:
                    return "已拒绝";
                case EInvestAuditStatus.Accepted:
                    return "已通过";
                default:
                    return "未知";
            }
        }

        public static string Description(this EInvestAuditStage value)
        {
            switch (value)
            {
                case EInvestAuditStage.First:
                    return "一审";
                case EInvestAuditStage.Second:
                    return "二审";
                case EInvestAuditStage.Agreement:
                    return "协议签订";
                case EInvestAuditStage.Roadshow:
                    return "路演";
                default:
                    return "未知";
            }
        }

        public static string Description(this EInvestAuditStage stage, EInvestAuditStatus status)
        {
            if (stage == EInvestAuditStage.Roadshow)
            {
                return status == EInvestAuditStatus.Accepted ? "已完成" : "路演中";
            }

            return stage.Description() + status.Description();
        }
        #endregion
    }

    public enum EInvestGrade
    {
        [Display(Name = "百万级企业")]
        Million = 100,

        [Display(Name = "千万级企业")]
        TenMillion = 1000
    }
}
