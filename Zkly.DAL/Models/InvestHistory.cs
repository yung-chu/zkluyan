using System;
using System.ComponentModel.DataAnnotations;

namespace Zkly.DAL.Models
{
    public class InvestHistory
    {
        public long Id { get; set; }

        public long InvestId { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }

        public EInvestAuditStage Stage { get; set; }

        public EInvestAuditStatus Status { get; set; }

        public long? AuditQuota { get; set; }

        [MaxLength(500, ErrorMessage = "投资备注长度不能大于500！")]
        public string Remark { get; set; }

        public DateTime CreateTime { get; set; }

        public virtual Invest Invest { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
